using Chummer.Contracts.Content;
using Chummer.Contracts.Rulesets;

namespace Chummer.Application.Content;

public sealed class DefaultRuntimeLockDiffService : IRuntimeLockDiffService
{
    public RuntimeLockDiffProjection Diff(ResolvedRuntimeLock before, ResolvedRuntimeLock after)
    {
        ArgumentNullException.ThrowIfNull(before);
        ArgumentNullException.ThrowIfNull(after);

        List<RuntimeLockDiffChange> changes = [];

        if (!string.Equals(before.RulesetId, after.RulesetId, StringComparison.Ordinal))
        {
            changes.Add(new RuntimeLockDiffChange(
                RuntimeLockDiffChangeKinds.RulesetChanged,
                "ruleset",
                before.RulesetId,
                after.RulesetId,
                "runtime.diff.ruleset.changed",
                []));
        }

        if (!string.Equals(before.EngineApiVersion, after.EngineApiVersion, StringComparison.Ordinal))
        {
            changes.Add(new RuntimeLockDiffChange(
                RuntimeLockDiffChangeKinds.EngineApiChanged,
                "engine-api",
                before.EngineApiVersion,
                after.EngineApiVersion,
                "runtime.diff.engine-api.changed",
                []));
        }

        AppendBundleDiffs(changes, before, after);
        AppendRulePackDiffs(changes, before, after);
        AppendProviderBindingDiffs(changes, before, after);

        return new RuntimeLockDiffProjection(before.RuntimeFingerprint, after.RuntimeFingerprint, changes);
    }

    private static void AppendBundleDiffs(List<RuntimeLockDiffChange> changes, ResolvedRuntimeLock before, ResolvedRuntimeLock after)
    {
        HashSet<string> beforeBundles = before.ContentBundles
            .Select(static bundle => $"{bundle.BundleId}@{bundle.Version}")
            .ToHashSet(StringComparer.Ordinal);
        HashSet<string> afterBundles = after.ContentBundles
            .Select(static bundle => $"{bundle.BundleId}@{bundle.Version}")
            .ToHashSet(StringComparer.Ordinal);

        foreach (string added in afterBundles.Except(beforeBundles).OrderBy(static value => value, StringComparer.Ordinal))
        {
            changes.Add(new RuntimeLockDiffChange(
                RuntimeLockDiffChangeKinds.ContentBundleAdded,
                added,
                null,
                added,
                "runtime.diff.content-bundle.added",
                []));
        }

        foreach (string removed in beforeBundles.Except(afterBundles).OrderBy(static value => value, StringComparer.Ordinal))
        {
            changes.Add(new RuntimeLockDiffChange(
                RuntimeLockDiffChangeKinds.ContentBundleRemoved,
                removed,
                removed,
                null,
                "runtime.diff.content-bundle.removed",
                []));
        }
    }

    private static void AppendRulePackDiffs(List<RuntimeLockDiffChange> changes, ResolvedRuntimeLock before, ResolvedRuntimeLock after)
    {
        HashSet<string> beforePacks = before.RulePacks
            .Select(static pack => $"{pack.Id}@{pack.Version}")
            .ToHashSet(StringComparer.Ordinal);
        HashSet<string> afterPacks = after.RulePacks
            .Select(static pack => $"{pack.Id}@{pack.Version}")
            .ToHashSet(StringComparer.Ordinal);

        foreach (string added in afterPacks.Except(beforePacks).OrderBy(static value => value, StringComparer.Ordinal))
        {
            changes.Add(new RuntimeLockDiffChange(
                RuntimeLockDiffChangeKinds.RulePackAdded,
                added,
                null,
                added,
                "runtime.diff.rulepack.added",
                []));
        }

        foreach (string removed in beforePacks.Except(afterPacks).OrderBy(static value => value, StringComparer.Ordinal))
        {
            changes.Add(new RuntimeLockDiffChange(
                RuntimeLockDiffChangeKinds.RulePackRemoved,
                removed,
                removed,
                null,
                "runtime.diff.rulepack.removed",
                []));
        }
    }

    private static void AppendProviderBindingDiffs(List<RuntimeLockDiffChange> changes, ResolvedRuntimeLock before, ResolvedRuntimeLock after)
    {
        HashSet<string> keys = before.ProviderBindings.Keys
            .Concat(after.ProviderBindings.Keys)
            .ToHashSet(StringComparer.Ordinal);

        foreach (string key in keys.OrderBy(static candidate => candidate, StringComparer.Ordinal))
        {
            string? beforeBinding = before.ProviderBindings.GetValueOrDefault(key);
            string? afterBinding = after.ProviderBindings.GetValueOrDefault(key);
            if (string.Equals(beforeBinding, afterBinding, StringComparison.Ordinal))
            {
                continue;
            }

            changes.Add(new RuntimeLockDiffChange(
                RuntimeLockDiffChangeKinds.ProviderBindingChanged,
                key,
                beforeBinding,
                afterBinding,
                "runtime.diff.provider-binding.changed",
                [new RulesetExplainParameter("capabilityId", RulesetCapabilityBridge.FromObject(key))]));
        }
    }
}

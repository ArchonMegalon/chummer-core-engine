using Chummer.Contracts.BuildLab;

namespace Chummer.Application.BuildLab;

public sealed class DefaultBuildLabService : IBuildLabService
{
    public IReadOnlyList<BuildVariantProjection> GenerateBuildVariants(string characterId, IReadOnlyList<string> roleTags)
    {
        string seed = Normalize(characterId);
        string[] tags = roleTags
            .Where(static tag => !string.IsNullOrWhiteSpace(tag))
            .Select(static tag => tag.Trim())
            .Distinct(StringComparer.Ordinal)
            .OrderBy(static tag => tag, StringComparer.Ordinal)
            .ToArray();

        if (tags.Length == 0)
        {
            tags = ["generalist"];
        }

        return tags.Select((tag, index) => new BuildVariantProjection(
                VariantId: $"{seed}-{tag}-{index + 1}",
                LabelKey: $"buildlab.variant.{tag}.label",
                RoleTags: [tag],
                Scores:
                [
                    new BuildVariantScore("synergy", 100m - (index * 7m)),
                    new BuildVariantScore("efficiency", 80m - (index * 5m))
                ],
                Constraints: []))
            .ToArray();
    }

    public BuildVariantProjection? ScoreBuildVariant(string characterId, string variantId)
    {
        return GenerateBuildVariants(characterId, [ExtractTag(variantId)])
            .FirstOrDefault(candidate => string.Equals(candidate.VariantId, variantId, StringComparison.Ordinal))
            ?? GenerateBuildVariants(characterId, [ExtractTag(variantId)]).FirstOrDefault();
    }

    public KarmaSpendProjection ProjectKarmaSpend(string characterId, string variantId, IReadOnlyList<int> milestones)
    {
        string tag = ExtractTag(variantId);
        int[] orderedMilestones = milestones
            .Where(static milestone => milestone > 0)
            .Distinct()
            .OrderBy(static milestone => milestone)
            .ToArray();

        if (orderedMilestones.Length == 0)
        {
            orderedMilestones = [25, 50, 100];
        }

        KarmaSpendStep[] steps = orderedMilestones
            .Select(milestone => new KarmaSpendStep(
                KarmaTotal: milestone,
                Scores:
                [
                    new BuildVariantScore("consistency", Math.Max(0m, 100m - (milestone / 3m))),
                    new BuildVariantScore("ceiling", Math.Min(100m, 40m + (milestone / 2m)))
                ],
                AppliedChoiceIds: [$"{milestone}:core", $"{milestone}:{tag}"]))
            .ToArray();

        return new KarmaSpendProjection(
            VariantId: variantId,
            Steps: steps);
    }

    public IReadOnlyList<BuildTrapChoice> DetectTrapChoices(string characterId, string variantId)
    {
        string tag = ExtractTag(variantId);
        return
        [
            new BuildTrapChoice(
                ChoiceId: $"{variantId}:trap:resource-overcommit",
                ReasonKey: $"buildlab.trap.{tag}.resource-overcommit",
                Parameters: ["nuyen", "karma"])
        ];
    }

    public IReadOnlyList<BuildRoleOverlap> DetectRoleOverlap(string characterId, IReadOnlyList<string> variantIds)
    {
        string[] ordered = variantIds
            .Where(static id => !string.IsNullOrWhiteSpace(id))
            .Select(static id => id.Trim())
            .Distinct(StringComparer.Ordinal)
            .OrderBy(static id => id, StringComparer.Ordinal)
            .ToArray();

        List<BuildRoleOverlap> overlaps = [];
        for (int i = 0; i < ordered.Length; i++)
        {
            for (int j = i + 1; j < ordered.Length; j++)
            {
                overlaps.Add(new BuildRoleOverlap(
                    LeftVariantId: ordered[i],
                    RightVariantId: ordered[j],
                    OverlapScore: 0.5m));
            }
        }

        return overlaps;
    }

    public IReadOnlyList<BuildCorePackageSuggestion> SuggestCorePackages(string characterId, string variantId)
    {
        string tag = ExtractTag(variantId);
        return
        [
            new BuildCorePackageSuggestion($"{tag}.core.a", $"buildlab.package.{tag}.core.a", 0.91m),
            new BuildCorePackageSuggestion($"{tag}.core.b", $"buildlab.package.{tag}.core.b", 0.83m)
        ];
    }

    private static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "character";
        }

        return value.Trim().Replace(' ', '-').ToLowerInvariant();
    }

    private static string ExtractTag(string variantId)
    {
        if (string.IsNullOrWhiteSpace(variantId))
        {
            return "generalist";
        }

        string[] parts = variantId.Split('-', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2 ? parts[^2] : "generalist";
    }
}

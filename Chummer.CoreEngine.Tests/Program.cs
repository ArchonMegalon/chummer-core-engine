using Chummer.Application.Content;
using Chummer.Application.Session;
using Chummer.Contracts.Content;
using Chummer.Contracts.Owners;
using Chummer.Contracts.Rulesets;
using Chummer.Contracts.Session;
using Chummer.Rulesets.Hosting;
using Chummer.Rulesets.Sr4;
using Chummer.Rulesets.Sr5;
using Chummer.Rulesets.Sr6;

return CoreEngineTests.Run();

internal static class CoreEngineTests
{
    public static int Run()
    {
        try
        {
            CapabilityDescriptorsEmitLocalizationKeys();
            ExperimentalRulesetsEmitDiagnosticMessageKeys();
            SessionReplayDiagnosticsStayKeyed();
            RuntimeInspectorProjectsCapabilityAndCompatibilityKeys();
            Console.WriteLine("core-engine-tests: ok");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return 1;
        }
    }

    private static void CapabilityDescriptorsEmitLocalizationKeys()
    {
        RulesetCapabilityDescriptor explicitDescriptor = new(
            CapabilityId: RulePackCapabilityIds.SessionQuickActions,
            InvocationKind: RulesetCapabilityInvocationKinds.Script,
            Title: "Session Quick Actions",
            Explainable: true,
            SessionSafe: true,
            DefaultGasBudget: new RulesetGasBudget(1_000, 5_000, 1_024),
            MaximumGasBudget: new RulesetGasBudget(2_000, 10_000, 2_048),
            TitleKey: "ruleset.capability.session.quick-actions.title",
            TitleParameters: []);
        RulesetCapabilityDescriptor fallbackDescriptor = new(
            CapabilityId: RulePackCapabilityIds.DeriveStat,
            InvocationKind: RulesetCapabilityInvocationKinds.Rule,
            Title: "Derived Stat Evaluation",
            Explainable: true,
            SessionSafe: false,
            DefaultGasBudget: new RulesetGasBudget(1_000, 5_000, 1_024));

        AssertEx.Equal(
            "ruleset.capability.session.quick-actions.title",
            RulesetCapabilityDescriptorLocalization.ResolveTitleKey(explicitDescriptor),
            "Explicit capability title keys should be preserved.");
        AssertEx.Equal(
            "ruleset.capability.derive.stat.title",
            RulesetCapabilityDescriptorLocalization.ResolveTitleKey(fallbackDescriptor),
            "Descriptors without an explicit title key should fall back to a capability-scoped key.");
        AssertEx.Equal(
            0,
            RulesetCapabilityDescriptorLocalization.ResolveTitleParameters(explicitDescriptor).Count,
            "Descriptors should expose deterministic title parameter collections.");
    }

    private static void ExperimentalRulesetsEmitDiagnosticMessageKeys()
    {
        Sr4RulesetPlugin sr4 = new();
        Sr6RulesetPlugin sr6 = new();

        RulesetCapabilityInvocationResult sr4Result = sr4.Capabilities
            .InvokeAsync(
                new RulesetCapabilityInvocationRequest(
                    CapabilityId: RulePackCapabilityIds.DeriveStat,
                    InvocationKind: RulesetCapabilityInvocationKinds.Rule,
                    Arguments: []),
                CancellationToken.None)
            .GetAwaiter()
            .GetResult();
        RulesetCapabilityInvocationResult sr6Result = sr6.Capabilities
            .InvokeAsync(
                new RulesetCapabilityInvocationRequest(
                    CapabilityId: RulePackCapabilityIds.SessionQuickActions,
                    InvocationKind: RulesetCapabilityInvocationKinds.Script,
                    Arguments: []),
                CancellationToken.None)
            .GetAwaiter()
            .GetResult();

        AssertEx.Equal("sr4.rule.experimental", sr4Result.Diagnostics[0].MessageKey, "SR4 rule diagnostics should expose localization keys.");
        AssertEx.Equal("sr6.script.experimental", sr6Result.Diagnostics[0].MessageKey, "SR6 script diagnostics should expose localization keys.");
    }

    private static void SessionReplayDiagnosticsStayKeyed()
    {
        DefaultSessionOverlayProjectionService service = new();
        SessionOverlayProjection projection = service.Replay(
            overlayId: "overlay-1",
            characterId: "char-1",
            runtimeFingerprint: "sha256:test",
            events:
            [
                new SessionOverlayEventDto(
                    EventId: "evt-2",
                    Sequence: 2,
                    EventType: SessionOverlayEventKinds.TrackerIncrement,
                    Payload: new Dictionary<string, RulesetCapabilityValue>(StringComparer.Ordinal)
                    {
                        ["absoluteValue"] = RulesetCapabilityBridge.FromObject(2)
                    },
                    CreatedAtUtc: DateTimeOffset.UnixEpoch.AddSeconds(2)),
                new SessionOverlayEventDto(
                    EventId: "evt-1",
                    Sequence: 1,
                    EventType: SessionOverlayEventKinds.TrackerIncrement,
                    Payload: new Dictionary<string, RulesetCapabilityValue>(StringComparer.Ordinal),
                    CreatedAtUtc: DateTimeOffset.UnixEpoch.AddSeconds(1))
            ]);

        AssertEx.True(
            projection.Diagnostics.Any(diagnostic => string.Equals(diagnostic.MessageKey, "session.replay.absolute-write-blocked", StringComparison.Ordinal)),
            "Session replay should keep absolute-write diagnostics keyed.");
        AssertEx.True(
            projection.Diagnostics.Any(diagnostic => string.Equals(diagnostic.MessageKey, "session.replay.tracker.missing-id", StringComparison.Ordinal)),
            "Session replay should keep missing tracker identifiers keyed.");
    }

    private static void RuntimeInspectorProjectsCapabilityAndCompatibilityKeys()
    {
        DefaultRuntimeInspectorService service = new(
            new RulesetPluginRegistry([new Sr5RulesetPlugin(), new Sr6RulesetPlugin()]),
            new RuleProfileRegistryServiceStub(CreateProfile()),
            new RulePackRegistryServiceStub(
            [
                new RulePackRegistryEntry(
                    new RulePackManifest(
                        PackId: "house-rules",
                        Version: "1.0.0",
                        Title: "House Rules",
                        Author: "GM",
                        Description: "Campaign overlay.",
                        Targets: [RulesetDefaults.Sr5],
                        EngineApiVersion: "rulepack-v1",
                        DependsOn: [],
                        ConflictsWith: [],
                        Visibility: ArtifactVisibilityModes.LocalOnly,
                        TrustTier: ArtifactTrustTiers.LocalOnly,
                        Assets: [],
                        Capabilities: [],
                        ExecutionPolicies: []),
                    new RulePackPublicationMetadata(
                        OwnerId: "local-single-user",
                        Visibility: ArtifactVisibilityModes.LocalOnly,
                        PublicationStatus: RulePackPublicationStatuses.Published,
                        Review: new RulePackReviewDecision(RulePackReviewStates.NotRequired),
                        Shares: []),
                    new ArtifactInstallState(ArtifactInstallStates.Installed))
            ]));

        RuntimeInspectorProjection? projection = service.GetProfileProjection(OwnerScope.LocalSingleUser, "official.sr5.core", RulesetDefaults.Sr5);

        AssertEx.NotNull(projection, "Runtime inspector should resolve the seeded profile.");
        RuntimeInspectorProjection resolvedProjection = projection!;
        IReadOnlyList<RuntimeInspectorCapabilityDescriptorProjection> capabilityDescriptors = resolvedProjection.CapabilityDescriptors ?? [];
        AssertEx.True(
            capabilityDescriptors.Any(descriptor =>
                string.Equals(descriptor.CapabilityId, RulePackCapabilityIds.DeriveStat, StringComparison.Ordinal)
                && string.Equals(descriptor.TitleKey, "ruleset.capability.derive.stat.title", StringComparison.Ordinal)),
            "Runtime inspector should surface capability title keys.");
        AssertEx.True(
            resolvedProjection.CompatibilityDiagnostics.Any(diagnostic =>
                string.Equals(diagnostic.MessageKey, "runtime.lock.compatibility.compatible", StringComparison.Ordinal)),
            "Runtime inspector should surface compatibility message keys.");
    }

    private static RuleProfileRegistryEntry CreateProfile()
    {
        return new RuleProfileRegistryEntry(
            new RuleProfileManifest(
                ProfileId: "official.sr5.core",
                Title: "Official SR5 Core",
                Description: "Curated runtime.",
                RulesetId: RulesetDefaults.Sr5,
                Audience: RuleProfileAudienceKinds.General,
                CatalogKind: RuleProfileCatalogKinds.Official,
                RulePacks:
                [
                    new RuleProfilePackSelection(
                        new ArtifactVersionReference("house-rules", "1.0.0"),
                        Required: true,
                        EnabledByDefault: true)
                ],
                DefaultToggles: [],
                RuntimeLock: new ResolvedRuntimeLock(
                    RulesetId: RulesetDefaults.Sr5,
                    ContentBundles:
                    [
                        new ContentBundleDescriptor(
                            BundleId: "official.sr5.base",
                            RulesetId: RulesetDefaults.Sr5,
                            Version: "schema-1",
                            Title: "SR5 Base",
                            Description: "Built-in base content.",
                            AssetPaths: ["data/", "lang/"])
                    ],
                    RulePacks: [new ArtifactVersionReference("house-rules", "1.0.0")],
                    ProviderBindings: new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        [RulePackCapabilityIds.ContentCatalog] = "house-rules/content.catalog"
                    },
                    EngineApiVersion: "rulepack-v1",
                    RuntimeFingerprint: "runtime-lock-sha256"),
                UpdateChannel: RuleProfileUpdateChannels.Stable),
            new RuleProfilePublicationMetadata(
                OwnerId: "local-single-user",
                Visibility: ArtifactVisibilityModes.LocalOnly,
                PublicationStatus: RulePackPublicationStatuses.Published,
                Review: new RulePackReviewDecision(RulePackReviewStates.NotRequired),
                Shares: []),
            new ArtifactInstallState(ArtifactInstallStates.Available),
            RegistryEntrySourceKinds.BuiltInCoreProfile);
    }

    private sealed class RuleProfileRegistryServiceStub : IRuleProfileRegistryService
    {
        private readonly RuleProfileRegistryEntry _entry;

        public RuleProfileRegistryServiceStub(RuleProfileRegistryEntry entry)
        {
            _entry = entry;
        }

        public IReadOnlyList<RuleProfileRegistryEntry> List(OwnerScope owner, string? rulesetId = null) => [_entry];

        public RuleProfileRegistryEntry? Get(OwnerScope owner, string profileId, string? rulesetId = null)
            => string.Equals(profileId, _entry.Manifest.ProfileId, StringComparison.Ordinal) ? _entry : null;
    }

    private sealed class RulePackRegistryServiceStub : IRulePackRegistryService
    {
        private readonly IReadOnlyList<RulePackRegistryEntry> _entries;

        public RulePackRegistryServiceStub(IReadOnlyList<RulePackRegistryEntry> entries)
        {
            _entries = entries;
        }

        public IReadOnlyList<RulePackRegistryEntry> List(OwnerScope owner, string? rulesetId = null) => _entries;

        public RulePackRegistryEntry? Get(OwnerScope owner, string packId, string? rulesetId = null)
            => _entries.FirstOrDefault(entry => string.Equals(entry.Manifest.PackId, packId, StringComparison.Ordinal));
    }
}

internal static class AssertEx
{
    public static void Equal<T>(T expected, T actual, string message)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new InvalidOperationException($"{message} Expected: {expected}. Actual: {actual}.");
        }
    }

    public static void NotNull<T>(T? value, string message) where T : class
    {
        if (value is null)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void True(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}

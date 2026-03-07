using Chummer.Contracts.Seeds;

namespace Chummer.Application.Seeds;

public sealed class DefaultSemanticSeedService : ISemanticSeedService
{
    public CharacterDossierSeed BuildCharacterDossierSeed(string characterId, string rulesetId, string runtimeFingerprint)
    {
        CharacterAestheticDigest digest = new(
            CharacterId: characterId,
            Metatype: "unknown",
            RoleTags: ["generalist"],
            BuildTags: ["balanced"],
            VisibleWareTags: [],
            MagicalStyleTags: [],
            OutfitArchetypeTags: ["street"],
            FactionStyleTags: [],
            MoodTags: ["focused"],
            TraumaTags: [],
            MotifTags: ["neon"]);

        return new CharacterDossierSeed(
            CharacterId: characterId,
            RulesetId: rulesetId,
            RuntimeFingerprint: runtimeFingerprint,
            AestheticDigest: digest,
            Facts: new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["seed.origin"] = "core-engine",
                ["seed.version"] = "v1"
            });
    }

    public NpcDossierSeed BuildNpcDossierSeed(string npcId, string rulesetId)
    {
        return new NpcDossierSeed(
            NpcId: npcId,
            RulesetId: rulesetId,
            ArchetypeTags: ["contact"],
            Facts: new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["seed.origin"] = "core-engine",
                ["seed.version"] = "v1"
            });
    }

    public RunSummarySeed BuildRunSummarySeed(string runId, string rulesetId)
    {
        return new RunSummarySeed(
            RunId: runId,
            RulesetId: rulesetId,
            HeatThresholdKeys: ["heat.low", "heat.medium", "heat.high"],
            ConsequenceTags: ["security", "reputation"]);
    }

    public BuildIdeaSeed BuildBuildIdeaSeed(string ideaId, string rulesetId)
    {
        return new BuildIdeaSeed(
            IdeaId: ideaId,
            RulesetId: rulesetId,
            RoleTags: ["generalist"],
            PackageTags: ["starter"]);
    }

    public ShadowfeedSeed BuildShadowfeedSeed(string feedId, string rulesetId)
    {
        return new ShadowfeedSeed(
            FeedId: feedId,
            RulesetId: rulesetId,
            TopicTags: ["operations"],
            FactionTags: ["independent"]);
    }
}

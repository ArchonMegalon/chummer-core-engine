namespace Chummer.Contracts.BuildLab;

public sealed record BuildVariantConstraint(
    string ConstraintId,
    string ConstraintKey,
    IReadOnlyList<string> Parameters);

public sealed record BuildVariantScore(
    string MetricId,
    decimal Value);

public sealed record BuildVariantProjection(
    string VariantId,
    string LabelKey,
    IReadOnlyList<string> RoleTags,
    IReadOnlyList<BuildVariantScore> Scores,
    IReadOnlyList<BuildVariantConstraint> Constraints);

public sealed record KarmaSpendStep(
    int KarmaTotal,
    IReadOnlyList<BuildVariantScore> Scores,
    IReadOnlyList<string> AppliedChoiceIds);

public sealed record KarmaSpendProjection(
    string VariantId,
    IReadOnlyList<KarmaSpendStep> Steps);

public sealed record BuildTrapChoice(
    string ChoiceId,
    string ReasonKey,
    IReadOnlyList<string> Parameters);

public sealed record BuildRoleOverlap(
    string LeftVariantId,
    string RightVariantId,
    decimal OverlapScore);

public sealed record BuildCorePackageSuggestion(
    string PackageId,
    string LabelKey,
    decimal Rank);

namespace Chummer.Contracts.Rulesets;

public sealed record RulesetGasBudget(
    int ProviderInstructionLimit,
    int RequestInstructionLimit,
    long MemoryBytesLimit,
    TimeSpan? WallClockLimit = null);

public sealed record RulesetExecutionOptions(
    bool Explain = false,
    RulesetGasBudget? GasBudget = null);

public sealed record RulesetGasUsage(
    int ProviderInstructionsConsumed,
    int RequestInstructionsConsumed,
    long PeakMemoryBytes,
    bool ProviderBudgetExceeded = false,
    bool RequestBudgetExceeded = false,
    bool WallClockLimitExceeded = false);

public sealed record RulesetExplainParameter(
    string Name,
    RulesetCapabilityValue Value);

public sealed record RulesetTraceStep(
    string ProviderId,
    string CapabilityId,
    string? PackId,
    string ExplanationKey,
    IReadOnlyList<RulesetExplainParameter> ExplanationParameters,
    string Category,
    decimal? Modifier = null,
    bool? Certain = null);

public sealed record RulesetProviderTrace(
    string ProviderId,
    string CapabilityId,
    string? PackId,
    bool Success,
    IReadOnlyList<RulesetTraceStep> Steps,
    RulesetGasUsage GasUsage);

public sealed record RulesetExplainTrace(
    string TargetKey,
    RulesetCapabilityValue? FinalValue,
    string SummaryKey,
    IReadOnlyList<RulesetExplainParameter> SummaryParameters,
    IReadOnlyList<RulesetProviderTrace> Providers,
    RulesetGasUsage AggregateGasUsage);

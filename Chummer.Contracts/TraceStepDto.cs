using System.Collections.Generic;

namespace Chummer.Contracts;

public record TraceStepDto(
    string ProviderId,
    string SourcePackId,
    string LocalizationKey,
    IReadOnlyDictionary<string, string> LocalizationParameters,
    int ModifierApplied
);

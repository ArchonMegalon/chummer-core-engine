using System.Collections.Generic;

namespace Chummer.Contracts;

public record ExplainEvidencePointerDto(
    string Kind,
    string Pointer,
    string? LabelKey = null,
    IReadOnlyDictionary<string, string>? LabelParameters = null,
    string? ProviderId = null,
    string? PackId = null,
    string? RuleId = null);

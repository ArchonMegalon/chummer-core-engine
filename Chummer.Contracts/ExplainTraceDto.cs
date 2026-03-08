using System.Collections.Generic;

namespace Chummer.Contracts;

public record ExplainTraceDto(
    string TargetKey,
    int FinalValue,
    string SummaryKey,
    IReadOnlyDictionary<string, string> SummaryParameters,
    IReadOnlyList<TraceStepDto> Steps,
    string? RuntimeFingerprint = null,
    string? ProfileId = null,
    IReadOnlyList<ExplainEvidencePointerDto>? Evidence = null
);

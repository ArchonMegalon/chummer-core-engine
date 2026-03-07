using System.Collections.Generic;

namespace Chummer.Contracts.BuildLab;

public interface IBuildLabEngine
{
    IEnumerable<BuildVariantDto> GenerateBuildVariants();
    ProgressionSimulationDto ProjectKarmaSpend();
    IEnumerable<TrapChoiceWarningDto> DetectTrapChoices();
}

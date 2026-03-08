using System;
using System.Collections.Generic;
using System.IO;
using Chummer.Contracts.BuildLab;

namespace Chummer.Backend.BuildLab
{
    public class BuildLabEngine : IBuildLabEngine
    {
        public IEnumerable<BuildVariantDto> GenerateBuildVariants()
        {
            return new List<BuildVariantDto>();
        }

        public ProgressionSimulationDto ProjectKarmaSpend()
        {
            return new ProgressionSimulationDto { ProjectedTotal = 0, RecommendedSpend = 0, Warnings = new List<string>() };
        }

        public IEnumerable<TrapChoiceWarningDto> DetectTrapChoices()
        {
            return new List<TrapChoiceWarningDto>();
        }
    }
}

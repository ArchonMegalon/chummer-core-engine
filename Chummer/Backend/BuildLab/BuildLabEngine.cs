using System.Collections.Generic;
using Chummer.Contracts.BuildLab;

namespace Chummer.Backend.BuildLab
{
    public class BuildLabEngine : IBuildLabEngine
    {
        public IEnumerable<BuildVariantDto> GenerateBuildVariants()
        {
            return new List<BuildVariantDto>
            {
                new BuildVariantDto
                {
                    Id = "VAR-01",
                    Name = "Street Samurai",
                    Description = "Combat focus"
                }
            };
        }

        public KarmaProjectionDto ProjectKarmaSpend()
        {
            return new KarmaProjectionDto
            {
                ProjectedTotal = 0,
                RecommendedSpend = 0,
                Warnings = new List<string>()
            };
        }

        public IEnumerable<TrapChoiceDto> DetectTrapChoices()
        {
            return new List<TrapChoiceDto>
            {
                new TrapChoiceDto
                {
                    ChoiceId = "TRAP-01",
                    Severity = "High",
                    Description = "Suboptimal karma spend detected."
                }
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YouriPortfolio.Models
{
    [Serializable]
    public class CVModel
    {
        public CVModel()
        {
            SkillKeyValues = new Dictionary<string, int>();
        }
        public string ExperienceText { get; set; }
        public string AboutMeText { get; set; }
        public string EducationText { get; set; }
        public Dictionary<string, int> SkillKeyValues { get; set; }
    }
}
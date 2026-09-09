using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantProtectionTechnologist.Models.Recipe
{
    public class RecipeStatusHistory
    {
        public DateOnly date {  get; set; }
        public string? author {  get; set; }
        public string? statusOld { get; set; } 
        public string? statusNew { get; set; } = null!;
        public string? comment {  get; set; }
    }
}

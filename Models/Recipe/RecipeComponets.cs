using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantProtectionTechnologist.Models.Recipe
{
    public class RecipeComponets
    {
        public int id { get; set; }
        public int recipeId { get; set; }
        public int rawMaterialId { get; set; }
        public decimal percentage { get; set; }
        public decimal toleranceMin { get; set; }
        public decimal toleranceMax { get; set; }
        public int loadOrder { get; set; }
    }
}

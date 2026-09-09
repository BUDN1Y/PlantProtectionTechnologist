using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantProtectionTechnologist.Models.Recipe
{
    public class RecipesDataGrid
    {
        public int id {  get; set; }
        public string code { get; set; } = null!;
        public string name { get; set; } = null!;
        public decimal percentage { get; set; }
        public int loadOrder {  get; set; }
        public string tolerance { get; set; }
    }
}

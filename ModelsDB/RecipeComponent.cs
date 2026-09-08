using System;
using System.Collections.Generic;

namespace PlantProtectionTechnologist.Models;

public partial class RecipeComponent
{
    public int id { get; set; }
    public int recipeId { get; set; }
    public int rawMaterialId { get; set; }
    public decimal percentage { get; set; }
    public decimal? toleranceMin { get; set; }
    public decimal? toleranceMax { get; set; }
    public int loadOrder { get; set; }
}

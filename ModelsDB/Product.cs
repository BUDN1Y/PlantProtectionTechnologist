using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PlantProtectionTechnologist.Models;

public partial class Product
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("releaseForm")]
    public string? ReleaseForm { get; set; }

    [JsonPropertyName("statusId")]
    public int StatusId { get; set; }

    [JsonPropertyName("activeRecipeId")]  
    public int? ActiveRecipeId { get; set; }

    [JsonPropertyName("activeTechMapId")]  
    public int? ActiveTechMapId { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ProductionBatch> ProductionBatches { get; set; } = new List<ProductionBatch>();

    public virtual ICollection<ProductionOrder> ProductionOrders { get; set; } = new List<ProductionOrder>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<TechMap> TechMaps { get; set; } = new List<TechMap>();
}

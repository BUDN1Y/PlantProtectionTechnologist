using System;
using System.Collections.Generic;

namespace PlantProtectionTechnologist.Models;

public partial class BatchUsedMaterial
{
    public int Id { get; set; }

    public int BatchId { get; set; }

    public int RawMaterialBatchId { get; set; }

    public decimal QuantityUsed { get; set; }

    public string Unit { get; set; } = null!;

    public virtual ProductionBatch Batch { get; set; } = null!;

    public virtual RawMaterialBatch RawMaterialBatch { get; set; } = null!;
}

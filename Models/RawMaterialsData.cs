using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantProtectionTechnologist.Models
{
    public class RawMaterialsData
    {
        public string code { get; set; } = null!;
        public string name { get; set; } = null!;
        public string? category { get; set; }
        public string unit { get; set; } = null!;
    }
}

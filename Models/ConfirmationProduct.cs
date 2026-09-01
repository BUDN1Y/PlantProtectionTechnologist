using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantProtectionTechnologist.Models
{
    public class ConfirmationProduct
    {
        public string code { get; set; } = null!;
        public string name { get; set; } = null!;
        public string? type { get; set; }
        public string? releaseForm { get; set; } = null!;
        public string comment { get; set; } = null!;
        public int status { get; set; }
       
    }
}

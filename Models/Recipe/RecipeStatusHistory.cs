using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantProtectionTechnologist.Models.Recipe
{
    public class RecipeStatusHistory
    {
        public DateTime? date {  get; set; }
        public string? author {  get; set; }
        public int? statusId { get; set; }
        public string? statusNameOld { get; set; }
        public string? statusColorOld { get; set; }
        public string? statusNameNew { get; set; }
        public string? statusColorNew { get; set; }
        public string? statusOld { get; set; } 
        public string? statusNew { get; set; } = null!;
        public string? comment {  get; set; }
        public string? mainInfo1 { get; set; }
        public string? mainInfo2 { get; set; }
        public string? mainInfo3 { get; set; }
    }
}

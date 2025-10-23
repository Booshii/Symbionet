using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace api.Models
{
    // The 'Table' attribute specifies the database table that this class maps to.
    // 'SdgTargets' is the name of the table in the database.
    [Table("SdgTargets")]
    public class SdgTarget
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now; 
        public string? CreatedByUserId { get; set; }= string.Empty; 
        public int? CompanyId { get; set; }
        
        // Navigation Properties
        public AppUser? CreatedByUser { get; set; }
        public Company? Company { get; set; }
        public List<SdgTargetSubGoal> SubGoals { get; set; } = new List<SdgTargetSubGoal>(); 
        public List<SdgTargetSdgType> SdgTargetSdgTypes { get; set; } = new List<SdgTargetSdgType>();
    }
}
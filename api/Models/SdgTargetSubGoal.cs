using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("SdgTargetSubGoals")]
    public class SdgTargetSubGoal
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
        public DateTime CreationDate { get; set; } = DateTime.Now; 
        public bool IsDone { get; set; }

        public int SdgTargetId {get; set; }
        public SdgTarget? SdgTarget {get; set; }  
    }
}
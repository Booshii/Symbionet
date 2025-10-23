using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("SdgTargetSdgTypes")]
    public class SdgTargetSdgType
    {
        public int SdgTargetId { get; set; } 
        public int SdgTypeId { get; set; } 
        public SdgTarget SdgTarget { get; set; } 
        public SdgType SdgType { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("SdgTypes")]
    public class SdgType
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public List<SdgTargetSdgType> SdgTargetSdgTypes { get; set; } = new List<SdgTargetSdgType>();
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models 
{
    [Table("Industries")]
    public class Industry
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
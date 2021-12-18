using System.ComponentModel.DataAnnotations.Schema;

namespace BlackHoleCMS.Models
{
    [Table("Status", Schema = "lookup")]
    public class Status
    {
        public byte Id { get; set; }
        public string? Name { get; set; }
    }
}
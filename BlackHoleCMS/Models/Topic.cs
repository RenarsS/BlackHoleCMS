using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackHoleCMS.Models
{
    [Table("Topic", Schema = "lookup")]
    public class Topic
    {
        public Topic()
        {
            
        }

        public Topic(byte id, string name, string content, int photo)
        {
            Id = id;
            Name = name;
            Content = content;
            Photo = photo;
        }
        
        public byte Id { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public int? Photo { get; set; }
    }
}
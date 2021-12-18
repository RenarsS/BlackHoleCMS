using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace BlackHoleCMS.Models
{
    public class HttpPost
    {
        public HttpPost()
        {
            
        }
        
        public HttpPost(int id, string? title , string? content, DateTimeOffset? postedAt, int? photoId, byte? topic, byte? status)
        {
            Id = id;
            Title = title;
            Content = content;
            PostedAt = postedAt;
            PhotoId = photoId;
            //Creator = creator;
            Topic = topic;
            Status = status;
        }
        
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTimeOffset? PostedAt { get; set; }
        public IFormFile? Photo { get; set; }
        public int? PhotoId { get; set; }
        public byte? Creator { get; set; }
        public byte? Topic { get; set; }
        public byte? Status { get; set; }
    }
}
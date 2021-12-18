using System;
using System.ComponentModel.DataAnnotations.Schema;
using BlackHoleCMS.Classes;

namespace BlackHoleCMS.Models
{
    [Table("Post", Schema = "post")]
    public class Post
    {
        public Post()
        {
            
        }

        public Post(string? title , string? content, DateTimeOffset? postedAt, int? photo, byte? topic, byte? status)
        {
            Title = title;
            Content = content;
            PostedAt = postedAt;
            Photo = photo;
            //Creator = creator;
            Topic = topic;
            Status = status;
        }
        public Post(int id, string? title , string? content, DateTimeOffset? postedAt, int? photo, byte? topic, byte? status)
        {
            Id = id;
            Title = title;
            Content = content;
            PostedAt = postedAt;
            Photo = photo;
            Topic = topic;
            Status = status;
        }
        
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTimeOffset? PostedAt { get; set; }

        public byte? Topic { get; set; }
        public byte? Status { get; set; }

        public int? Photo { get; set; }

    }
}
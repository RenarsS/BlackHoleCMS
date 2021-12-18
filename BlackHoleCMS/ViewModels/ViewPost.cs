using BlackHoleCMS.Classes;
using BlackHoleCMS.Models;

namespace BlackHoleCMS.ViewModels;

public class ViewPost : BasicPost
{
    public ViewPost(int id, string? title , string? content, DateTimeOffset? postedAt, Photo? photo, byte? topic, byte? status)
    {
        Id = id;
        Title = title;
        Content = content;
        PostedAt = postedAt;
        Photo = photo;
        //Creator = creator;
        Topic = topic;
        Status = status;
    }
    
    public Photo? Photo { get; set; }

}
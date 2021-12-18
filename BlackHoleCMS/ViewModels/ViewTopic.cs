using BlackHoleCMS.Classes;
using BlackHoleCMS.Models;

namespace BlackHoleCMS.ViewModels;

public class ViewTopic : BasicTopic
{
    public ViewTopic(byte id, string? name, string? content, Photo? photo)
    {
        Id = id;
        Name = name;
        Content = content;
        Photo = photo;
    }
    
    public Photo? Photo { get; set; }
    
    
}
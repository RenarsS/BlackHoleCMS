using BlackHoleCMS.Classes;
using BlackHoleCMS.Models;

namespace BlackHoleCMS.HttpModels;

public class HttpTopic : BasicTopic
{
    public HttpTopic()
    {
        
    }
    
    public HttpTopic(byte id, string name, string content, int? photoId)
    {
        Id = id;
        Name = name;
        Content = content;
        PhotoId = photoId;
    }
    
    public IFormFile? Photo { get; set; }
    public int? PhotoId { get; set; }
}
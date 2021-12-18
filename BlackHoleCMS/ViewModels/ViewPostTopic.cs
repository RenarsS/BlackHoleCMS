using BlackHoleCMS.Models;

namespace BlackHoleCMS.ViewModels;

public class ViewPostTopic
{
    public List<ViewTopic> Topics { get; set; }
    public List<ViewPost> Posts { get; set; }
}
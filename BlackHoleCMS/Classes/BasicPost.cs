namespace BlackHoleCMS.Classes;

public class BasicPost
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTimeOffset? PostedAt { get; set; }
    
    public byte? Creator { get; set; }
    
    public byte? Topic { get; set; }
    public byte? Status { get; set; }
}
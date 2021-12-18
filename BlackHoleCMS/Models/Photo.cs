using System.ComponentModel.DataAnnotations.Schema;

namespace BlackHoleCMS.Models;

[Table("File", Schema = "post")]
public class Photo
{
    public Photo()
    {
        
    }
    
    public Photo(string fileName, Byte[] file)
    {
        FileName = fileName;
        File = file;
    }
    
    public int Id { get; set; }
    public string? FileName { get; set; }
    public Byte[]? File { get; set; }
}
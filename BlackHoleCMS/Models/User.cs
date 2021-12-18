using System.ComponentModel.DataAnnotations.Schema;

namespace BlackHoleCMS.Models
{
    [Table("User", Schema = "user")]
    public class User
    {/*
        public User(byte id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }*/
        
        public byte Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
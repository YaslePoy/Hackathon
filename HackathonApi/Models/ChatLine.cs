using System.ComponentModel.DataAnnotations.Schema;
using HackathonApi.Models;

public class ChatLine : DbEntity
{
    public string Key { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    public string Text { get; set; }
}
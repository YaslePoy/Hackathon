using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonApi.Models;

public class Hackathon : DbNamed
{
    public string Description { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    [ForeignKey("Owner")]
    public int OwnerId { get; set; }

    public User Owner { get; set; }
}
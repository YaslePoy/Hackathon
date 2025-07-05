using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonApi.Models;

public class HackathonRequest : DbEntity
{
    [ForeignKey("Team")]
    public int TeamId { get; set; }
    [ForeignKey("Hackathon")]
    public int HackathonId { get; set; }
    public Team Team { get; set; }
    public Hackathon Hackathon { get; set; }
} 
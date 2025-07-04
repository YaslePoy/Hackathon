using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonApi.Models;

public class TeamMember : DbEntity
{
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    [ForeignKey("Team")]
    public int TeamId { get; set; }
    public Team Team { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonApi.Models;

public class HSolution : DbEntity
{
    public string Text { get; set; }
    public Team Team { get; set; }
    public HTask Task { get; set; }
    [ForeignKey("Team")]
    public int TeamId { get; set; }
    [ForeignKey("Task")]
    public int TaskId { get; set; }
}
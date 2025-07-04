using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonApi.Models;

public class HSolution : DbEntity
{
    public string Text { get; set; }
    public Team Team { get; set; }
    public HTask Task { get; set; }
    [ForeignKey("Team")]
    public int TeamId { get; set; }
    [ForeignKey("Tack")]
    public int TaskId { get; set; }
    public string Comment { get; set; }
    public int Score { get; set; }
}
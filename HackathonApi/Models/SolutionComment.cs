using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonApi.Models;

public class SolutionComment : DbEntity
{
    [ForeignKey("Solution")]
    public int SolutionId { get; set; }
    public HSolution Solution { get; set; }
    
    [ForeignKey("Jury")]
    public int JuryId { get; set; }
    public User Jury { get; set; }
    
    public string Comment { get; set; }
    public int Score { get; set; }
} 
using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonApi.Models;

public class HTask :  DbEntity
{
    public string Description { get; set; }
    [ForeignKey("Hackathon")]
    public int HackathonId { get; set; }
    public Hackathon Hackathon { get; set; }
}
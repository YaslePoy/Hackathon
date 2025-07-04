using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HackathonApi.Models;

public class HTask :  DbEntity
{
    public string Description { get; set; }
    [ForeignKey("Hackathon")]
    public int HackathonId { get; set; }
    [JsonIgnore]
    public Hackathon Hackathon { get; set; }
}
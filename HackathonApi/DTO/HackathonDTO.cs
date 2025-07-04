using HackathonApi.Models;

namespace HackathonApi.DTO;

public class HackathonDTO : DbNamed
{
    public string Description { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int OwnerId { get; set; }
}
namespace HackathonApi.Models;

public class User : DbNamed
{
    public string Email { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    public string Surname { get; set; }
}
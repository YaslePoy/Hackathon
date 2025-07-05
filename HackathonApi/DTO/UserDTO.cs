using HackathonApi.Models;

namespace HackathonApi.DTO;

public class UserDTO : DbNamed
{
    public string Email { get; set; }
    public int Role { get; set; }
    public string Surname { get; set; }
}

public class UserCreateDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Role { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
}

public class TeamRequestDTO
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string UserSurname { get; set; }
    public string UserEmail { get; set; }
}
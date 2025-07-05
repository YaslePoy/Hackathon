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

public class HackathonRequestDTO
{
    public int Id { get; set; }
    public string TeamName { get; set; }
    public string TeamDescription { get; set; }
}

public class TeamDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class SolutionCommentDTO
{
    public int Id { get; set; }
    public int SolutionId { get; set; }
    public int JuryId { get; set; }
    public string Comment { get; set; }
    public int Score { get; set; }
    public string? JuryName { get; set; }
}
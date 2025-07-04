using System.ComponentModel.DataAnnotations;

namespace HackathonApi.Models;

public class DbEntity
{
    [Key]
    public int Id { get; set; }
}

public class DbNamed :  DbEntity
{
    public string Name { get; set; }
}
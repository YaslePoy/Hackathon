using HackathonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi;

public class HckContext : DbContext
{
    public HckContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Team> Teams { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<HTask> Tasks { get; set; }
    public DbSet<HSolution> Solutions { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<Hackathon> Hackathons { get; set; }
    public DbSet<TeamRequest>  TeamRequests { get; set; }
    public DbSet<TeamCompetition>  TeamCompetitions { get; set; }
    public DbSet<HackathonRequest>  HackathonRequests { get; set; }
}
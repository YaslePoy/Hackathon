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
    public DbSet<SolutionComment>  SolutionComments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Конфигурация внешних ключей для SolutionComment
        modelBuilder.Entity<SolutionComment>()
            .HasOne(sc => sc.Solution)
            .WithMany()
            .HasForeignKey(sc => sc.SolutionId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<SolutionComment>()
            .HasOne(sc => sc.Jury)
            .WithMany()
            .HasForeignKey(sc => sc.JuryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
using HackathonApi.DTO;
using HackathonApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Controllers;

[ApiController]
[Route("api/team")]
public class TeamApi(HckContext db) : Controller
{
    [HttpGet]
    public async Task<ActionResult<List<Team>>> GetAllTeams()
    {
        return await db.Teams.ToListAsync();
    }
    [HttpPost]
    public async Task<ActionResult> CreateTeam([FromBody] Team team, [FromQuery]int userId)
    {
        if (await db.Teams.AnyAsync(i => i.Name == team.Name))
        {
            return Conflict();
        }
        
        db.Add(team);
        await db.SaveChangesAsync();
        db.Add(new TeamMember { TeamId = team.Id, UserId = userId });
        await db.SaveChangesAsync();
        return Ok(team.Id);
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateTeam([FromBody] Team team)
    {
        if (await db.Teams.FirstOrDefaultAsync(i => i.Id == team.Id) is not { } dbteam)
            return NotFound();

        Utils.TransferData(dbteam, team);
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{teamId}/teammates")]
    public async Task<ActionResult<IReadOnlyList<UserDTO>>> Teammates(int teamId)
    {
        if (!await db.Teams.AnyAsync(i => i.Id == teamId))
            return NotFound();
        var users = db.TeamMembers.Where(i => i.TeamId == teamId).Include(i => i.User).Select(i => i.User).ToList()
            .Select(i => i.To<UserDTO>());
        return Ok(users);
    }
    [HttpGet("{teamId}/hackathons")]
    public async Task<ActionResult<IReadOnlyList<HackathonDTO>>> Hackathons(int teamId)
    {
        if (!await db.TeamCompetitions.AnyAsync(i => i.TeamId == teamId))
            return NotFound();
        var users = db.TeamCompetitions.Where(i => i.TeamId == teamId).Select(i => i.Hackathon).ToList()
            .Select(i => i.To<HackathonDTO>());
        return Ok(users);
    }

    [HttpPost("request")]
    public async Task<ActionResult> PostRequest(int userId, int teamId)
    {
        if (!await db.Teams.AnyAsync(i => i.Id == teamId))
            return NotFound();


        if (db.TeamMembers.Count(i => i.TeamId == teamId) == 5)
        {
            return Conflict();
        }
        
        var req = new TeamRequest { TeamId = teamId, UserId = userId }; 
        db.TeamRequests.Add(req);
        await db.SaveChangesAsync();
        return Ok(req.Id);
    }

    [HttpPost("request/accept")]
    public async Task<ActionResult> Accept(int requestId)
    {
        var req = await db.TeamRequests.FirstOrDefaultAsync(i => i.Id == requestId);
        if (req is null)
            return NotFound();
        db.TeamRequests.Remove(req);
        db.TeamMembers.Add(new TeamMember { TeamId = req.TeamId, UserId = req.UserId });
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{teamId}/register/{hackathonId}")]
    public async Task<ActionResult> RegisterOnHackathon(int teamId, int hackathonId)
    {
        if (db.TeamCompetitions.Any(i => i.TeamId == teamId && i.HackathonId == hackathonId))
        {
            return Conflict();
        }
        
        db.TeamCompetitions.Add(new TeamCompetition { TeamId = teamId,  HackathonId = hackathonId });
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{teamId}/requests")]
    public async Task<ActionResult<IReadOnlyList<TeamRequestDTO>>> GetTeamRequests(int teamId)
    {
        if (!await db.Teams.AnyAsync(i => i.Id == teamId))
            return NotFound();
        
        var requests = await db.TeamRequests
            .Where(r => r.TeamId == teamId)
            .Include(r => r.User)
            .Select(r => new TeamRequestDTO
            {
                Id = r.Id,
                UserName = r.User.Name,
                UserSurname = r.User.Surname,
                UserEmail = r.User.Email
            })
            .ToListAsync();
        
        return Ok(requests);
    }
}
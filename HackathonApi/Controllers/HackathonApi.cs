using HackathonApi.DTO;
using HackathonApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Controllers;

[ApiController]
[Route("api/hackathon")]
public class HackathonApi(HckContext db) : Controller
{
    [HttpGet("all")]
    public async Task<ActionResult> All()
    {
        return Ok(await db.Hackathons.Include(i => i.Owner).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Hackathon>> Get(int id)
    {
        if (await db.Hackathons.Include(i => i.Owner).FirstOrDefaultAsync(i => i.Id == id) is { } hk)
        {
            return Ok(hk);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] HackathonDTO hackathon)
    {
        try
        {
            await db.Hackathons.AddAsync(hackathon.To<Hackathon>());
            await db.SaveChangesAsync();
            return Ok(hackathon.Id);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPatch]
    public async Task<ActionResult> Update([FromBody] HackathonDTO hackathon)
    {
        if (await db.Hackathons.FirstOrDefaultAsync(i => i.Id == hackathon.Id) is { } hk)
        {
            Utils.TransferData(hk, hackathon);
            await db.SaveChangesAsync();
            return Ok();
        }

        return NotFound();
    }

    [HttpGet("{id}/team")]
    public async Task<ActionResult<IReadOnlyList<Team>>> Teams(int id)
    {
        return Ok(await db.TeamCompetitions.Where(i => i.HackathonId == id).Select(i => i.Team).ToListAsync());
    }

    [HttpGet("{id}/tasks")]
    public async Task<ActionResult<IReadOnlyList<HTask>>> Tasks(int id)
    {
        return Ok(await db.Tasks.Where(i => i.HackathonId == id).ToListAsync());
    }

    [HttpPost("request")]
    public async Task<ActionResult> PostTeamRequest(int teamId, int hackathonId)
    {
        if (!await db.Hackathons.AnyAsync(i => i.Id == hackathonId))
            return NotFound();

        if (db.TeamCompetitions.Any(i => i.TeamId == teamId && i.HackathonId == hackathonId) || db.HackathonRequests.Any(i => i.TeamId == teamId && i.HackathonId == hackathonId))
        {
            return Conflict();
        }

        var req = new HackathonRequest { TeamId = teamId, HackathonId = hackathonId }; 
        db.HackathonRequests.Add(req);
        await db.SaveChangesAsync();
        return Ok(req.Id);
    }

    [HttpGet("{hackathonId}/requests")]
    public async Task<ActionResult<IReadOnlyList<HackathonRequestDTO>>> GetHackathonRequests(int hackathonId)
    {
        if (!await db.Hackathons.AnyAsync(i => i.Id == hackathonId))
            return NotFound();
        
        var requests = await db.HackathonRequests
            .Where(r => r.HackathonId == hackathonId)
            .Include(r => r.Team)
            .Select(r => new HackathonRequestDTO
            {
                Id = r.Id,
                TeamName = r.Team.Name,
                TeamDescription = r.Team.Description
            })
            .ToListAsync();
        
        return Ok(requests);
    }

    [HttpPost("request/accept")]
    public async Task<ActionResult> AcceptTeamRequest(int requestId)
    {
        var req = await db.HackathonRequests.FirstOrDefaultAsync(i => i.Id == requestId);
        if (req is null)
            return NotFound();
        
        db.HackathonRequests.Remove(req);
        db.TeamCompetitions.Add(new TeamCompetition { TeamId = req.TeamId, HackathonId = req.HackathonId });
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{hackathonId}/participating-teams")]
    public async Task<ActionResult<IReadOnlyList<TeamDTO>>> GetParticipatingTeams(int hackathonId)
    {
        if (!await db.Hackathons.AnyAsync(i => i.Id == hackathonId))
            return NotFound();
        
        var teams = await db.TeamCompetitions
            .Where(tc => tc.HackathonId == hackathonId)
            .Include(tc => tc.Team)
            .Select(tc => new TeamDTO
            {
                Id = tc.Team.Id,
                Name = tc.Team.Name,
                Description = tc.Team.Description
            })
            .ToListAsync();
        
        return Ok(teams);
    }

    [HttpDelete("{hackathonId}/team/{teamId}")]
    public async Task<ActionResult> RemoveTeamFromHackathon(int hackathonId, int teamId)
    {
        var teamCompetition = await db.TeamCompetitions
            .FirstOrDefaultAsync(tc => tc.HackathonId == hackathonId && tc.TeamId == teamId);
        
        if (teamCompetition == null)
            return NotFound();
        
        db.TeamCompetitions.Remove(teamCompetition);
        await db.SaveChangesAsync();
        return Ok();
    }
}
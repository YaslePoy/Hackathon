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
}
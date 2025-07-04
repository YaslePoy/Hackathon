using HackathonApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Controllers;

[ApiController]
[Route("api/solution")]
public class SolutionApi(HckContext db) : Controller
{
    [HttpGet("task")]
    public async Task<ActionResult<IReadOnlyList<HSolution>>> OfTask(int taskId)
    {
        return await db.Solutions.Where(i => i.TaskId == taskId).ToListAsync();
    }

    [HttpGet]
    public async Task<ActionResult<HSolution?>> Get(int taskId, int teamId)
    {
        return await db.Solutions.FirstOrDefaultAsync(i => i.TaskId == taskId && i.TeamId == teamId);
    }
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] HSolution solution)
    {
        var existing = await db.Solutions.FirstOrDefaultAsync(i => i.Id == solution.Id);
        if (existing is null)
        {
            if (await db.Solutions.FirstOrDefaultAsync(i => i.TaskId == solution.TaskId && i.TeamId == solution.TeamId)
                is
                { } dbSol)
            {
                Utils.TransferData(dbSol, solution);
                await db.SaveChangesAsync();
                return Ok();
            }

            db.Solutions.Add(solution);
            await db.SaveChangesAsync();
            return Ok(solution.Id);
        }

        Utils.TransferData(existing, solution);
        await db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("score")]
    public async Task<ActionResult> SetScore(int solutionId, int score)
    {
        var sol = await db.Solutions.FirstOrDefaultAsync(i => i.Id == solutionId);
        sol.Score = score;
        return Ok();
    }
}
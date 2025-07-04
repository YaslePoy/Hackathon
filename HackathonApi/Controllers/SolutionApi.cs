using HackathonApi.DTO;
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
    public async Task<ActionResult<SolutionDTO?>> Get(int taskId, int teamId)
    {
        
        return Ok((await db.Solutions.FirstOrDefaultAsync(i => i.TaskId == taskId && i.TeamId == teamId)).Text);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] SolutionDTO solution)
    {
        if (await db.Solutions.FirstOrDefaultAsync(i => i.TaskId == solution.TaskId && i.TeamId == solution.TeamId)
            is
            { } dbSol)
        {
            Utils.TransferData(dbSol, solution);
            await db.SaveChangesAsync();
            return Ok();
        }

        var dbsol = new HSolution { Text = solution.Text, TeamId = solution.TeamId, TaskId = solution.TaskId, Comment = ""};
        db.Solutions.Add(dbsol);
        await db.SaveChangesAsync();
        return Ok(dbsol.Id);
    }

    [HttpPost("score")]
    public async Task<ActionResult> SetScore(int solutionId, int score)
    {
        var sol = await db.Solutions.FirstOrDefaultAsync(i => i.Id == solutionId);
        sol.Score = score;
        return Ok();
    }
}
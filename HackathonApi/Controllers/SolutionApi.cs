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

        var dbsol = new HSolution { Text = solution.Text, TeamId = solution.TeamId, TaskId = solution.TaskId};
        db.Solutions.Add(dbsol);
        await db.SaveChangesAsync();
        return Ok(dbsol.Id);
    }

    [HttpPost("comment")]
    public async Task<ActionResult> AddOrUpdateComment([FromBody] SolutionCommentDTO comment)
    {
        // Проверка диапазона оценки
        if (comment.Score < 0 || comment.Score > 10)
        {
            return BadRequest("Оценка должна быть в диапазоне от 0 до 10");
        }
        
        var existingComment = await db.SolutionComments
            .FirstOrDefaultAsync(sc => sc.SolutionId == comment.SolutionId && sc.JuryId == comment.JuryId);
        
        if (existingComment != null)
        {
            // Обновляем существующий комментарий
            existingComment.Comment = comment.Comment;
            existingComment.Score = comment.Score;
        }
        else
        {
            // Создаём новый комментарий
            var newComment = new SolutionComment
            {
                SolutionId = comment.SolutionId,
                JuryId = comment.JuryId,
                Comment = comment.Comment,
                Score = comment.Score
            };
            db.SolutionComments.Add(newComment);
        }
        
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("comment/{solutionId}")]
    public async Task<ActionResult<IReadOnlyList<SolutionCommentDTO>>> GetSolutionComments(int solutionId)
    {
        var comments = await db.SolutionComments
            .Where(sc => sc.SolutionId == solutionId)
            .Include(sc => sc.Jury)
            .Select(sc => new SolutionCommentDTO
            {
                Id = sc.Id,
                SolutionId = sc.SolutionId,
                JuryId = sc.JuryId,
                JuryName = sc.Jury.Name + " " + sc.Jury.Surname,
                Comment = sc.Comment,
                Score = sc.Score
            })
            .ToListAsync();
        
        return Ok(comments);
    }

    [HttpGet("comment/{solutionId}/jury/{juryId}")]
    public async Task<ActionResult<SolutionCommentDTO>> GetJuryComment(int solutionId, int juryId)
    {
        var comment = await db.SolutionComments
            .Where(sc => sc.SolutionId == solutionId && sc.JuryId == juryId)
            .Include(sc => sc.Jury)
            .Select(sc => new SolutionCommentDTO
            {
                Id = sc.Id,
                SolutionId = sc.SolutionId,
                JuryId = sc.JuryId,
                JuryName = sc.Jury.Name + " " + sc.Jury.Surname,
                Comment = sc.Comment,
                Score = sc.Score
            })
            .FirstOrDefaultAsync();
        
        return comment != null ? Ok(comment) : NotFound();
    }
}
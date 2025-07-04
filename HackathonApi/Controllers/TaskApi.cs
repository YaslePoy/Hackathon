using HackathonApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Controllers;

[ApiController]
[Route("api/task")]
public class TaskApi(HckContext db) : Controller
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        if (await db.Tasks.FirstOrDefaultAsync(i => i.Id == id) is HTask api)
        {
            return Ok(api);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] HTask api)
    {
        try
        {
            db.Tasks.Add(api);
            await db.SaveChangesAsync();
            return Ok(api.Id);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPatch]
    public async Task<ActionResult> Update([FromBody] HTask api)
    {
        var task = await db.Tasks.FirstOrDefaultAsync(i => i.Id == api.Id);
        
        if (task is null)
        {
            return NotFound();
        }

        Utils.TransferData(task, api);
        await db.SaveChangesAsync();
        return Ok();
    }
}
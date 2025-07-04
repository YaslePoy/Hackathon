using HackathonApi.DTO;
using HackathonApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackathonApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserApi(HckContext db) : Controller
{
    [HttpGet("auth")]
    public async Task<ActionResult<UserDTO>> Auth(string username, string password)
    {
        if (await db.Users.FirstOrDefaultAsync(i => i.Email == username && i.Password == password) is { } user)
        {
            return Ok(user.To<UserDTO>());
        }

        return Unauthorized();
    }

    [HttpGet]
    public async Task<ActionResult<UserDTO>> Get(int id)
    {
        if (await db.Users.FirstOrDefaultAsync(i => i.Id == id) is { } user)
        {
            return Ok(user.To<UserDTO>());
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> Register(UserCreateDTO user)
    {
        if (await db.Users.FirstOrDefaultAsync(i => i.Email == user.Email) is not null)
        {
            return Conflict();
        }

        var dbuser = user.To<User>();
        db.Add(dbuser);
        await db.SaveChangesAsync();
        return Ok(dbuser.Id);
    }
}
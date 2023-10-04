using Application.LogicInterfaces;
using Domain.CustomExceptions;
using Domain.DTOs;
using FileData;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace WebAPI.Controllers;


[ApiController]
[Route("users")]
public class UserController: ControllerBase
{
    private readonly IUserLogic userLogic;

    public UserController(IUserLogic userLogic)
    {
        this.userLogic = userLogic;
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateAsync(UserCreationDto dto)
    {
        try
        {
            User user = await userLogic.CreateAsync(dto);
            return Created($"/users/{user.Id}", user);
        }
        catch (InvalidUsernameLengthException e)
        {
            Console.WriteLine(e);
            return StatusCode(501, e.Message);
        }
        catch (UnavailableUsernameException e)
        {
            Console.WriteLine(e);
            return StatusCode(502, e.Message);
        }
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAsync([FromQuery] string? username)
    {
        try
        {
            SearchUserParametersDto searchParameters = new(username);
            var users = await userLogic.GetAsync(searchParameters);
            return Ok(users);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}
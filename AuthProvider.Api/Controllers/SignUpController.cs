using AuthProvider.Api.Handlers;
using AuthProvider.Data.Entities;
using AuthProvider.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AuthProvider.Api.Controllers;

[Route("api/auth/[controller]")]
[ApiController]
public class SignUpController(UserManager<UserEntity> userManager, ServiceBusHandler sb) : ControllerBase
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly ServiceBusHandler _sb = sb;

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUp model)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        try 
        { 
            if (!await _userManager.Users.AnyAsync(x => x.Email == model.Email))
            {
                var user = new UserEntity
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var userCreatedMessage = new { model.Email, model.FirstName, model.LastName };
                    await _sb.SendMessageAsync("newuser-queue", userCreatedMessage);

                    return Ok();
                }
            }

            return Conflict();
        }
        catch (Exception ex) 
        {
            Debug.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, "An internal server error occurred.");
        }
    }
}

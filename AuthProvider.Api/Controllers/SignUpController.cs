using AuthProvider.Api.Handlers;
using AuthProvider.Data.Entities;
using AuthProvider.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthProvider.Api.Controllers;

[Route("api/auth/signup")]
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
                await _sb.SendUserCreatedMessageAsync(model);

                return Ok();
            }
            else
                return Conflict();

        }
          
        return BadRequest();
    }
}

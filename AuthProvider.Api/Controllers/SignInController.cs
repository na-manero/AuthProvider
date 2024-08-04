using AuthProvider.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthProvider.Api.Controllers;

[Route("api/auth/signin")]
[ApiController]
public class SignInController(SignInManager<IdentityUser> signInManager) : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;

    [HttpPost]
    public async Task<IActionResult> SignIn(SignIn model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        return result.Succeeded ? Ok() : Unauthorized("Invalid email or password.");
    }
}

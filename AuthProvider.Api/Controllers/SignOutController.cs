using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthProvider.Api.Controllers;

[Route("api/auth/signout")]
[ApiController]
public class SignOutController(SignInManager<IdentityUser> signInManager) : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;

    [HttpGet]
    public async Task<IActionResult> Signout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;

[Route("Manager")]
public class ManagerController : Controller
{
    [AllowAnonymous]
    [HttpGet("Login")]
    public IActionResult Login()
    {
        return Challenge(
            new AuthenticationProperties
            {
                RedirectUri = "/Manager"
            },
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        return View();
    }
}
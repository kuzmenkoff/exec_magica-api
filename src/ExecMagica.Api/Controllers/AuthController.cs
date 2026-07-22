using ExecMagica.Application.Dtos;
using ExecMagica.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExecMagica.Api.Controllers;

/// <summary>Authentication endpoints: register and login.</summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService auth;

    /// <summary>Initializes the controller with the auth service.</summary>
    public AuthController(IAuthService auth)
    {
        this.auth = auth;
    }

    /// <summary>Registers a new account and returns a JWT.</summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var result = await this.auth.RegisterAsync(request);
        return result.Succeeded
            ? this.Ok(result.Response)
            : this.BadRequest(new { errors = result.Errors });
    }

    /// <summary>Logs in with email and password and returns a JWT.</summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var result = await this.auth.LoginAsync(request);
        return result.Succeeded
            ? this.Ok(result.Response)
            : this.Unauthorized(new { errors = result.Errors });
    }
}

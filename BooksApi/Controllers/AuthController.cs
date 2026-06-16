using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IAuthService _authService;
        public AuthController(AppDbContext db, IAuthService tokenService)
        {
            _db = db;
            _authService = tokenService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            await _authService.RegisterAsync(req);
            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            string token = await _authService.LoginAsync(req);
            return Ok(new {token});
        }
    }
}
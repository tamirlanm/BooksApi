
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BooksApi.Models;
using BooksApi.DTOs;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;
    public AuthService(IConfiguration config, AppDbContext db)
    {
        _db = db;    
        _config = config;
    }

    public async Task RegisterAsync(RegisterRequest req)
    {
        bool isUsernameToken = await _db.Users.AnyAsync(u => u.Username == req.Username);
        if (isUsernameToken)
        {
            throw new Exception("Username already exists");
        }
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);
        var newUser = new User
        {
            Username = req.Username,
            PasswordHash = passwordHash,
            Role = req.Role
        };
        await _db.Users.AddAsync(newUser);
        await _db.SaveChangesAsync();   
    }
    
    public async Task<string> LoginAsync(LoginRequest req)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
        if(user == null)
        {
            throw new Exception("Invalid username or password");
        }
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new Exception("Invalid username or password");
        }
        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        var key =new  SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
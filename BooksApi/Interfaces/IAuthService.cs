
using Microsoft.AspNetCore.Identity.Data;
using BooksApi.DTOs;
public interface IAuthService
{
    Task RegisterAsync(RegisterRequest req);
    Task<string> LoginAsync(LoginRequest req);
}
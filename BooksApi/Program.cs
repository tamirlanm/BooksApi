using Microsoft.EntityFrameworkCore;
using FluentValidation;
using BooksApi.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using BooksApi.Repositories;
using BooksApi.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddDbContext<BookContext>(opt => opt.UseSqlite("Data Source=books.db"));
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)) 
    };
});
builder.Services.AddAuthorization();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // 1. Защита от null для самого документа
        if (document is null) return Task.CompletedTask;

        // 2. Инициализируем компоненты и словарь схем, чтобы убрать warning CS8602
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        var jwtBearerScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT Token in the field below."
        };

        // Добавляем схему в компоненты, если её там ещё нет
        if (!document.Components.SecuritySchemes.ContainsKey("Bearer"))
        {
            document.Components.SecuritySchemes.Add("Bearer", jwtBearerScheme);
        }

        // 3. Исправление ошибок CS1950 и CS1503:
        // Используем явный OpenApiSecuritySchemeReference и строго List<string>
        var requirement = new OpenApiSecurityRequirement
        {
            { 
                new OpenApiSecuritySchemeReference("Bearer", document), 
                new List<string>() 
            }
        };

        // 4. Инициализируем список требований безопасности и добавляем наше
        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(requirement);

        return Task.CompletedTask;
    });
});


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IValidator<CreateBookRequest>, CreateBookValidator>();
builder.Services.AddScoped<RequestCounterService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequest>();


var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("BooksApi Api").WithTheme(ScalarTheme.DeepSpace)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}
app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();

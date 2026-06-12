using Microsoft.EntityFrameworkCore;
using BooksApi.Models;
using FluentValidation.AspNetCore;
using FluentValidation;
using BooksApi.DTOs;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.Services.AddDbContext<BookContext>(opt => opt.UseSqlite("Data Source=books.db"));
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IValidator<CreateBookRequest>, CreateBookValidator>();
builder.Services.AddScoped<RequestCounterService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookValidator>();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

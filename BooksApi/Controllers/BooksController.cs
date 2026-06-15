using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksApi.Models;
using FluentValidation;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using BooksApi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        
        public BooksController(IBookService booKService )
        {
            _bookService = booKService;     
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _bookService.GetAllBooksAsync();
            
            return Ok(response);
        }
        [HttpGet("genre/{genreId:int}")]
        public async Task<IActionResult> GetByGenre([FromRoute] int genreId)
        {
            var response = await _bookService.GetBookByGenreAsync(genreId);      
            return Ok(response);
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new BadRequestException($"Search query cannot be empty");
            }
            var response = await _bookService.SearchBookAsync(query);
        
            return Ok(response);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _bookService.GetBookByIdAsync(id);   
            return Ok(response);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookRequest request)
        {
            var created = await _bookService.CreateBookAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id}, created);
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateBookRequest request)
        {
            await _bookService.UpdateBookAsync(id, request);
            
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }

    }
}

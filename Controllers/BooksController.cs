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
        private readonly IValidator<CreateBookRequest> _validator;
    
        public BooksController(IBookService booKService, IValidator<CreateBookRequest> validator)
        {
            _bookService = booKService;
            _validator = validator;        
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _bookService.GetAllBooksAsync();
            /*var response = books.Select(book => new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Price = book.Price,
                IsAvailable = book.IsAvailable,
                GenreName = book.Genre?.Name ?? "Без Жанра"
            });*/
            return Ok(response);
        }
        [HttpGet("genre/{genreId:int}")]
        public async Task<IActionResult> GetByGenre([FromRoute] int genreId)
        {
            var response = await _bookService.GetBookByGenreAsync(genreId);
            /*var response = books.Select(book => new BookResponse{
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Price = book.Price,
                IsAvailable = book.IsAvailable,
                GenreName = book.Genre?.Name ?? "Без Жанра"
            });*/         
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
            /*var response = books.Select(book => new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Price = book.Price,
                IsAvailable = book.IsAvailable,
                GenreName = book.Genre?.Name ?? "Без жанра"
            });*/
            return Ok(response);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _bookService.GetBookByIdAsync(id);   
            /*var response = new BookResponse{
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Price = book.Price,
                IsAvailable = book.IsAvailable,
                GenreName = book.Genre?.Name ?? "Без Жанра"
            };*/
            return Ok(response);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new BadRequestException($"Error validation: {errors}");
            }/*
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                Year = request.Year,
                Price = request.Price,
                IsAvailable = request.IsAvailable,
                GenreId = request.GenreId
            };*/
            var created = await _bookService.CreateBookAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id}, created);
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateBookRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new BadRequestException($"Error validation: {errors}");
            }

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

        /*
        private readonly BookContext _context;

        public BooksController(BookContext context)
        {
            _context = context;
        }

        
        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBook([FromRoute] int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:long}")]
        public async Task<IActionResult> PutBook([FromRoute] long id,[FromBody] Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteBook([FromRoute] long id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        

        private bool BookExists(long id)
        {
            return _context.Books.Any(e => e.Id == id);
        }*/
    }
}

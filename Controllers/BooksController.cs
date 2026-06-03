using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksApi.Models;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooKService _bookService;
        public BooksController(IBooKService booKService) => _bookService = booKService;

        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _bookService.GetAll();
            var response = books.Select(b => new BookResponse{
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Year = b.Year,
                Price = b.Price
            });
            return Ok(response);
        }

        [HttpGet("{id:long}")]
        public IActionResult GetById([FromRoute] long id)
        {
            var book = _bookService.GetById(id);
            if(book == null)
            {
                return NotFound(new {message = $"Книга с ID {id} не найдена."});
            }
            var response = new BookResponse{
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Price = book.Price
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(CreateBookRequest request)
        {
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                Year = request.Year,
                Price = request.Price
            };
            var created = _bookService.Create(book);
            return CreatedAtAction(nameof(GetById), new { id = created.Id}, created);
        }

        [HttpPut("{id:long}")]
        public IActionResult Update([FromRoute] long id, [FromBody] CreateBookRequest request)
        {
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                Year = request.Year,
                Price = request.Price
            };

            var updated = _bookService.Update(id, book);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            var deleted = _bookService.Delete(id);
            if (!deleted)
            {
                return NotFound();
            }
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
        [HttpGet("{id:long}")]
        public async Task<ActionResult<Book>> GetBook([FromRoute] long id)
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

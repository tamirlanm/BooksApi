
using BooksApi.Models;

public class BookService : IBooKService
{
    private readonly List<Book> _books = new();
    private long _nextId = 1;
    private readonly object _lock = new();

    public IEnumerable<Book> GetAll()
    {
        lock (_lock)
        {
            return _books.ToList();
        }
    }

    public Book? GetById(long id)
    {
        lock(_lock)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }
    }

    public Book Create(Book book)
    {
        lock (_lock)
        {
            var newBook = new Book
            {
                Id = _nextId++,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Price = book.Price
            };
            _books.Add(newBook);
            return newBook;
        }
    }

    public bool Update(long id, Book book)
    {
        lock (_lock)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == id);
            if(existingBook == null)
            {
                return false;
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Year = book.Year;
            existingBook.Price = book.Price;

            return true;
        }
    }
    public bool Delete(long id)
    {
        lock (_lock)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == id);
            if(existingBook == null)
            {
                return false;
            }
            _books.Remove(existingBook);
            return true;
        }
    }
}
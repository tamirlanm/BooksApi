using BooksApi.Models;

public interface IBooKService
{
    IEnumerable<Book> GetAll();
    Book GetById(long id);
    Book Create(Book book);
    bool Update(long id, Book book);
    bool Delete(long id);
}

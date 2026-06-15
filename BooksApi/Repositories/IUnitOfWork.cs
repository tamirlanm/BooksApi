
using BooksApi.Repositories;

public interface IUnitOfWork : IDisposable
{
    IBookRepository Books {get;}
    IRepository<Genre> Genres {get;}
    Task<int> SaveChangesAsync();
}
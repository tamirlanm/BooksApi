using BooksApi.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    public IBookRepository Books {get;}
    public IRepository<Genre> Genres {get;}

    public UnitOfWork(AppDbContext db, IBookRepository books, IRepository<Genre> genres)
    {
        _db = db;
        Books = books;
        Genres = genres;
    }

    public async Task<int> SaveChangesAsync() => await _db.SaveChangesAsync();

    public void Dispose() => _db.Dispose();
}
using Microsoft.EntityFrameworkCore;
public class GenreRepository : IRepository<Genre>
{
    private readonly AppDbContext _db;
    public GenreRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Genre>> GetAllAsync() 
        => await _db.Genres.ToListAsync();
    
    public async Task<Genre?> GetByIdAsync(int id)
        => await _db.Genres.FirstOrDefaultAsync(b => b.Id == id);

    public async Task AddAsync(Genre entity) 
        => await _db.Genres.AddAsync(entity);
    public void Update(Genre entity) => _db.Genres.Update(entity);
    public void Delete(Genre entity) => _db.Genres.Remove(entity);
}
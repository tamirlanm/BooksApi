
namespace BooksApi.Models;

public class Book
{
    public int Id {get;set;}
    public string Title {get;set;} = "";
    public string Author {get;set;} = "";
    public int Year {get;set;}
    public decimal Price {get;set;}
    public bool IsAvailable {get;set;}

    public int GenreId {get;set;} //Foreign Key
    public Genre Genre {get;set;} = null!; // Navigation property
}
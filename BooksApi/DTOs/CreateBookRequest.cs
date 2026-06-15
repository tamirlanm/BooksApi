namespace BooksApi.DTOs;
public class CreateBookRequest
{
    public string Title {get;set;} = "";
    public string Author {get;set;} = "";
    public int Year {get;set;}
    public decimal Price {get;set;}
    public bool IsAvailable { get; set;}
    public int GenreId {get;set;}
}
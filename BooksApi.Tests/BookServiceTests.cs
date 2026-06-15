using BooksApi.Models;
using BooksApi.DTOs;
using BooksApi.Repositories;
using BooksApi.Services;
using FluentAssertions;
using Moq;
using Xunit;
using FluentValidation;
using Azure.Core;
namespace BooksApi.Tests;
public class BookServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IBookRepository> _repoMock;
    private readonly Mock<IRepository<Genre>> _genreRepoMock;
    private readonly Mock<IValidator<CreateBookRequest>> _validatorMock;
    private readonly BookService _service;


    public BookServiceTests()
    {
        _repoMock = new Mock<IBookRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _genreRepoMock = new Mock<IRepository<Genre>>();
        _validatorMock = new Mock<IValidator<CreateBookRequest>>();

        _service = new BookService(_uowMock.Object, _validatorMock.Object);

        _uowMock.Setup(u => u.Books).Returns(_repoMock.Object);
        _uowMock.Setup(u => u.Genres).Returns(_genreRepoMock.Object);
        _genreRepoMock.Setup(g => g.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Genre{Id = 1, Name = "Fantasy"});
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateBookRequest>(),It.IsAny<CancellationToken>()))
        .ReturnsAsync(new FluentValidation.Results.ValidationResult());
    }

    [Fact]
    public async Task GetById_WhenBookExists_ReturnBook()
    {
        // Arrange
        var book = new Book {Id = 1, Title = "Clean Code", Author = "Martin"};
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

        // Act
        var result = await _service.GetBookByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Clean Code");
    }

    [Fact]
    public async Task GetById_WhenBookNotFound_ThrowNotFoundException()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Book?)null);

        // Act
        var act = async() => await _service.GetBookByIdAsync(99);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Create_ValidBook_CallsSaveChanges()
    {
        
        // Arrange
        var request = new CreateBookRequest
        {
            Title = "Test", Author = "Author", Year = 2024, Price = 10, IsAvailable = true, GenreId = 4
        };

        // Act
        await _service.CreateBookAsync(request);

        // Assert
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Create_WhenGenreNotFound_ThrowNotFoundException()
    {
        // Arrange
        _genreRepoMock.Setup(g => g.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Genre?)null);
        
        var request = new CreateBookRequest
        {
            Title = "Valid Title",
            Author = "Some Author",
            Year = 1994,
            Price = 4000,
            IsAvailable = true,
            GenreId = 99
        };

        // Act

        var act = async () => await _service.CreateBookAsync(request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task Create_WithEmptyTitle_ThrowBadRequestException(string? title)
    {
        // Arrange
        var failures = new[] { new FluentValidation.Results.ValidationFailure("Title", "Пустое название") };
        var failedResult = new FluentValidation.Results.ValidationResult(failures);

        _validatorMock
        .Setup(v => v.ValidateAsync(It.IsAny<CreateBookRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(failedResult);

        var request = new CreateBookRequest {Title = title!, Author = "A"};
        
        // Act
        var act = async () => await _service.CreateBookAsync(request);
        
        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }
}

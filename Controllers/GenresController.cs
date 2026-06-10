
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksApi.Models;
using FluentValidation;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly IValidator<CreateGenreRequest> _validator;
        public GenresController(IGenreService genreService, IValidator<CreateGenreRequest> validator)
        {
            _genreService = genreService;
            _validator = validator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genreService.GetAllAsync();
            var response = genres.Select(genre=> new GenreResponse
            {
                Id = genre.Id,
                Name = genre.Name
            });
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGenreRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new BadRequestException($"Error validation: {errors}");
            }
            var genre = new Genre
            {
                Name = request.Name
            };
            var created = await _genreService.CreateAsync(genre);
            return Ok(created);
        }
    }
}
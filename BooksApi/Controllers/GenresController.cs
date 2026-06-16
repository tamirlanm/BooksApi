
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksApi.Models;
using FluentValidation;
using BooksApi.DTOs;
using Microsoft.AspNetCore.Authorization;

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
            var genres = await _genreService.GetAllGenresAsync();
            return Ok(genres);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateGenreRequest request)
        {
            var created = await _genreService.CreateGenreAsync(request);
            return Ok(created);
        }
    }
}
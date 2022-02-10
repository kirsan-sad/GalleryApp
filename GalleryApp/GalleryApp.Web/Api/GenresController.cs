using AutoMapper;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Infrastructure;
using GalleryApp.Infrastructure.Entities;
using GalleryApp.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryApp.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository _repository;
        private readonly GalleryContext _context;
        private readonly IMapper _mapper;

        public GenresController(IGenreRepository repository, GalleryContext context, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult Get([FromQuery] QueryStringParameters parameters)
        {
            var genresQuery = from Genre in _context.Genres
                              select new GenreModelAPI
                              {
                                  Index = Genre.Id,
                                  Name = Genre.Name,
                                  Description = Genre.Description,
                                  Photos = Genre.Photos.Select(g => g.Id).ToArray()
                              };

            var genres1 = PagedList<GenreModelAPI>.ToPagedList(genresQuery, parameters.PageNumber, parameters.PageSize);

            //var genres = await genresQuery.ToListAsync();
            if (genres1 == null) return NotFound();

            var metadata = new
            {
                genres1.TotalCount,
                genres1.PageSize,
                genres1.CurrentPage,
                genres1.TotalPages,
                genres1.HasNext,
                genres1.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(genres1);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var genreQuery = from Genre in _context.Genres
                             where Genre.Id == id
                             select new GenreModelAPI
                             {
                                 Index = Genre.Id,
                                 Name = Genre.Name,
                                 Description = Genre.Description,
                                 Photos = Genre.Photos.Select(g => g.Id).ToArray()
                             };

            var genre = await genreQuery.FirstOrDefaultAsync();

            if (genre == null) return NotFound();

            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Genre genre)
        {
            var genreEntityExist = await _context.Genres
                .AnyAsync(genreEntity => genreEntity.Name == genre.Name);

            if (genreEntityExist == true)
                return UnprocessableEntity("Genres with this name alredy exist");

            var entityGenre = _mapper.Map<GenreEntity>(genre);
            _context.Genres.Add(entityGenre);
            await _context.SaveChangesAsync();

            var genreApi = new GenreModelAPI
            {
                Index = entityGenre.Id,
                Name = entityGenre.Name,
                Description = entityGenre.Description,
                Photos = entityGenre.Photos.Select(g => g.Id).ToArray()
            };

            return CreatedAtAction("GetById", new { id = genreApi.Index }, genreApi);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tryDelete = await _repository.TryDeleteAsync(id);

            if (!tryDelete) return BadRequest();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Genre genre)
        {
            var genreEntityExist = await _context.Genres
                .AnyAsync(genreEntity => genreEntity.Id == genre.Index);

            if (!genreEntityExist)
                return NotFound();

            var entityForUpdate = _mapper.Map<GenreEntity>(genre);
            _context.Genres.Update(entityForUpdate);
            await _context.SaveChangesAsync();

            var genreApi = new GenreModelAPI
            {
                Index = entityForUpdate.Id,
                Name = entityForUpdate.Name,
                Description = entityForUpdate.Description,
                Photos = entityForUpdate.Photos.Select(g => g.Id).ToArray()
            };

            return Ok(genreApi);
        }
    }
}

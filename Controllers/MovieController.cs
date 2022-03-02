using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieChatacterAPI.Models;
using MovieChatacterAPI.Models.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieChatacterAPI.Controllers
{
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private readonly MovieCharacterDbContext _context;
        private readonly IMapper _mapper;

        public MovieController(MovieCharacterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetAllMovies()
        {
            var movies = await _context.Movies.ToListAsync();

            var moviesDto = _mapper.Map<List<MovieDTO>>(movies);

            return Ok(moviesDto);
        }

        [HttpGet(template: "{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovieById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDTO>(movie);

            return Ok(movieDto);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie([FromBody] MovieDTO movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);

            try
            {
                _context.Add(movie);

                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var newMovie = _mapper.Map<MovieDTO>(movie);

            return CreatedAtAction("GetById", new { Id = movie.Id }, newMovie);
        }

        [HttpDelete(template: "{id}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut(template: "{id}")]
        public async Task<ActionResult> UpdateMovie(int id, [FromBody] Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

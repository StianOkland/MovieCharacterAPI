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
        public async Task<ActionResult<IEnumerable<MovieReadDTO>>> GetAllMovies()
        {
            var movies = await _context.Movies.ToListAsync();

            var moviesDto = _mapper.Map<List<MovieReadDTO>>(movies);

            return Ok(moviesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieReadDTO>> GetMovieById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieReadDTO>(movie);

            return Ok(movieDto);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie([FromBody] MovieCreateDTO movieDto)
        {
            if (!FranchiseExist(movieDto.FranchiseId ?? -1))
                return BadRequest();

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

            var newMovie = _mapper.Map<MovieCreateDTO>(movie);

            return CreatedAtAction("GetMovieById", new { Id = movie.Id }, newMovie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieEditDTO movieDto)
        {
            if (id != movieDto.Id || !FranchiseExist(movieDto.FranchiseId ?? -1))
            {
                return BadRequest();
            }

            Movie domainMovie = _mapper.Map<Movie>(movieDto);
            _context.Entry(domainMovie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException)
            {
                if(!MovieExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpGet("charactersByMovie/{id}")]
        public async Task<ActionResult<List<int>>> GetCharactersByMovie(int id)
        {
            var movie = await _context.Movies.Include(m => m.Characters).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<int>>(movie.Characters.ToList());
        }

        private bool MovieExist(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        private bool FranchiseExist(int id)
        {
            return _context.Franchises.Any(e => e.Id == id);
        }
    }
}
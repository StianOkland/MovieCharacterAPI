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
        public ActionResult<IEnumerable<MovieDTO>> GetAllMovies()
        {
            var movies = _context.Movies.ToList();

            var moviesDto = _mapper.Map<List<MovieDTO>>(movies);

            return Ok(moviesDto);
        }

        [HttpGet(template: "{id}")]
        public ActionResult<MovieDTO> GetMovieById(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDTO>(movie);

            return Ok(movieDto);
        }

        [HttpPost]
        public ActionResult<Movie> PostMovie([FromBody] MovieDTO movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);

            try
            {
                _context.Add(movie);

                _context.SaveChanges();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var newMovie = _mapper.Map<MovieDTO>(movie);

            return CreatedAtAction("GetById", new { Id = movie.Id }, newMovie);
        }

        [HttpDelete(template: "{id}")]
        public ActionResult DeleteMovie(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Remove(movie);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut(template: "{id}")]
        public ActionResult UpdateMovie(int id, [FromBody] Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }
    }
}

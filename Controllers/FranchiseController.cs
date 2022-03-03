using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class FranchiseController : Controller
    {
        private readonly MovieCharacterDbContext _context;
        private readonly IMapper _mapper;

        public FranchiseController(MovieCharacterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all franchises in database
        /// </summary>
        /// <returns>List of franchises</returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FranchiseReadDTO>>> GetAllFranchises()
        {
            var franchises = await _context.Franchises.ToListAsync();

            var franchisesDto = _mapper.Map<List<FranchiseReadDTO>>(franchises);

            return Ok(franchisesDto);
        }

        /// <summary>
        /// Gets franchise in database by ID
        /// </summary>
        /// <param name="id">Franchise ID</param>
        /// <returns>Franchise</returns>

        [HttpGet("{id}")]
        public async Task<ActionResult<FranchiseReadDTO>> GetFranchiseById(int id)
        {
            var franchise = await _context.Franchises.FindAsync(id);

            if (franchise == null)
            {
                return NotFound();
            }

            var franchiseDto = _mapper.Map<FranchiseReadDTO>(franchise);

            return Ok(franchiseDto);
        }

        /// <summary>
        /// Adds franchise to database
        /// </summary>
        /// <param name="franchiseDto">Franchise to add</param>
        /// <returns>Newly added franchise</returns>

        [HttpPost]
        public async Task<ActionResult<FranchiseReadDTO>> PostFranchise([FromBody] FranchiseCreateDTO franchiseDto)
        {
            var franchise = _mapper.Map<Franchise>(franchiseDto);

            try
            {
                _context.Add(franchise);

                await _context.SaveChangesAsync();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var newFranchise = _mapper.Map<FranchiseCreateDTO>(franchise);

            return CreatedAtAction("GetFranchiseById", new { Id = franchise.Id }, newFranchise);
        }

        /// <summary>
        /// Deletes franchise from database
        /// </summary>
        /// <param name="id">Franchise ID</param>
        /// <returns>Deletion result</returns>

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFranchise(int id)
        {
            var franchise = await _context.Franchises.FindAsync(id);

            if (franchise == null)
            {
                return NotFound();
            }

            _context.Remove(franchise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Updates franchise in database
        /// </summary>
        /// <param name="id">Franchise ID</param>
        /// <param name="franchiseDto">New franchise info</param>
        /// <returns>Update result</returns>

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFranchise(int id, [FromBody] FranchiseEditDTO franchiseDto)
        {
            if (id != franchiseDto.Id)
            {
                return BadRequest();
            }

            Franchise domainFranchise = _mapper.Map<Franchise>(franchiseDto);
            _context.Entry(domainFranchise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException)
            {
                if(!FranchiseExist(id))
                {
                    return NotFound();
                } else
                {
                    throw;
                }
            }
            return NoContent();
        }

        /// <summary>
        /// Gets all movies in a franchise
        /// </summary>
        /// <param name="id">Franchise ID</param>
        /// <returns>List of movies in a franchise</returns>

        [HttpGet("moviesByFranchise/{id}")]
        public async Task<ActionResult<List<MovieReadDTO>>> GetMoviesByFranchise(int id)
        {
            var franchise = await _context.Franchises.Include(f => f.Movies).FirstOrDefaultAsync(f => f.Id == id);

            if (franchise == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<MovieReadDTO>>(franchise.Movies.ToList());
        }

        private bool FranchiseExist(int id)
        {
            return _context.Franchises.Any(e => e.Id == id);
        }
    }
}
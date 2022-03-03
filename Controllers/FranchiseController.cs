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
    public class FranchiseController : Controller
    {
        private readonly MovieCharacterDbContext _context;
        private readonly IMapper _mapper;

        public FranchiseController(MovieCharacterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FranchiseReadDTO>>> GetAllFranchises()
        {
            var franchises = await _context.Franchises.ToListAsync();

            var franchisesDto = _mapper.Map<List<FranchiseReadDTO>>(franchises);

            return Ok(franchisesDto);
        }

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
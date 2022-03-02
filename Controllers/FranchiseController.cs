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
        public ActionResult<IEnumerable<FranchiseDTO>> GetAllFranchises()
        {
            var franchises = _context.Franchises.ToList();

            var franchisesDto = _mapper.Map<List<FranchiseDTO>>(franchises);

            return Ok(franchisesDto);
        }

        [HttpGet(template: "{id}")]
        public ActionResult<FranchiseDTO> GetFranchiseById(int id)
        {
            var franchise = _context.Franchises.Find(id);

            if (franchise == null)
            {
                return NotFound();
            }

            var franchiseDto = _mapper.Map<FranchiseDTO>(franchise);

            return Ok(franchiseDto);
        }

        [HttpPost]
        public ActionResult<Franchise> PostFranchise([FromBody] FranchiseDTO franchiseDto)
        {
            var franchise = _mapper.Map<Franchise>(franchiseDto);

            try
            {
                _context.Add(franchise);

                _context.SaveChanges();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var newFranchise = _mapper.Map<FranchiseDTO>(franchise);

            return CreatedAtAction("GetById", new { Id = franchise.Id }, newFranchise);
        }

        [HttpDelete(template: "{id}")]
        public ActionResult DeleteFranchise(int id)
        {
            var franchise = _context.Franchises.Find(id);

            if (franchise == null)
            {
                return NotFound();
            }

            _context.Remove(franchise);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut(template: "{id}")]
        public ActionResult UpdateFranchise(int id, [FromBody] Franchise franchise)
        {
            if (id != franchise.Id)
            {
                return BadRequest();
            }

            _context.Entry(franchise).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }
    }
}

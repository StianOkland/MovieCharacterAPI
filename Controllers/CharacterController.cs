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
    public class CharacterController : Controller
    {
        private readonly MovieCharacterDbContext _context;
        private readonly IMapper _mapper;

        public CharacterController(MovieCharacterDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterReadDTO>>> GetAllCharacters()
        {
            var characters = await _context.Characters.ToListAsync();

            var charactersDto = _mapper.Map<List<CharacterReadDTO>>(characters);

            return Ok(charactersDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterReadDTO>> GetCharacterById(int id)
        {
            var character = await _context.Characters.FindAsync(id);

            if(character == null)
            {
                return NotFound();
            }

            var characterDto = _mapper.Map<CharacterReadDTO>(character);

            return Ok(characterDto);
        }

        [HttpPost]
        public async Task<ActionResult<CharacterReadDTO>> PostCharacter([FromBody] CharacterCreateDTO characterDto)
        {
            var character = _mapper.Map<Character>(characterDto);

            try
            {
                _context.Add(character);

                await _context.SaveChangesAsync();

            }catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var newCharacter = _mapper.Map<CharacterReadDTO>(character);

            return CreatedAtAction("GetCharacterById", new { Id = character.Id }, newCharacter);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            // TODO: remove from linking table on deletion
            var character = await _context.Characters.FindAsync(id);

            if(character == null)
            {
                return NotFound();
            }

            _context.Remove(character);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharacter(int id, [FromBody] CharacterEditDTO characterDto)
        {
            if(id != characterDto.Id)
            {
                return BadRequest();
            }

            Character domainCharacter = _mapper.Map<Character>(characterDto);
            _context.Entry(domainCharacter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException)
            {
                if(!CharacterExist(id))
                {
                    return NotFound();
                } else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool CharacterExist(int id)
        {
            return _context.Characters.Any(e => e.Id == id);
        }

    }
}
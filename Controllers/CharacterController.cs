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
        public ActionResult<IEnumerable<CharacterDTO>> GetAllCharacters()
        {
            var characters = _context.Characters.ToList();

            var charactersDto = _mapper.Map<List<CharacterDTO>>(characters);

            return Ok(charactersDto);
        }

        [HttpGet(template:"{id}")]
        public ActionResult<CharacterDTO> GetCharacterById(int id)
        {
            var character = _context.Characters.Find(id);

            if(character == null)
            {
                return NotFound();
            }

            var characterDto = _mapper.Map<CharacterDTO>(character);

            return Ok(characterDto);
        }

        [HttpPost]
        public ActionResult<Character> PostCharacter([FromBody] CharacterDTO characterDto)
        {
            var character = _mapper.Map<Character>(characterDto);

            try
            {
                _context.Add(character);

                _context.SaveChanges();

            }catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var newCharacter = _mapper.Map<CharacterDTO>(character);

            return CreatedAtAction("GetById", new { Id = character.Id }, newCharacter);
        }

        [HttpDelete(template: "{id}")]
        public ActionResult DeleteCharacter(int id)
        {
            var character = _context.Characters.Find(id);

            if(character == null)
            {
                return NotFound();
            }

            _context.Remove(character);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut(template: "{id}")]
        public ActionResult UpdateCharacter(int id, [FromBody] Character character)
        {
            if(id != character.Id)
            {
                return BadRequest();
            }

            _context.Entry(character).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }
    }
}

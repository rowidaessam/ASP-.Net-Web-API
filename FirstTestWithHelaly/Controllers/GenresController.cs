using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstTestWithHelaly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreServices _genreServices;

        public GenresController( IGenreServices genreServices)
        {
            _genreServices = genreServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var geners = await _genreServices.GetAll();
            return Ok(geners);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateGenreDto dto)
        {
            var genere = new Genre { Name = dto.Name }; 
            await _genreServices.Add(genere);
            return Ok(genere);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] CreateGenreDto dto)
        {
            var genere = await _genreServices.GetById(id);
            if (genere == null)
                return NotFound($"No Genre was found with id {id}");
            genere.Name = dto.Name;

            _genreServices.Update(genere);
            return Ok(genere);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genere = await _genreServices.GetById(id);
            if (genere == null)
                return NotFound($"No Genre was found with id {id}");

            _genreServices.Delete(genere);

            return Ok(genere);


        }
    }
}

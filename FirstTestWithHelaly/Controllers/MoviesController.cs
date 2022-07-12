using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstTestWithHelaly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieServices _movieServices;
        private readonly IGenreServices _genreServices;
        private readonly IMapper _mapper;


        private new List<string> _allowedExtensions =new List<string> { ".jpg",".png"};
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(IMovieServices movieServices, IGenreServices genreServices, IMapper mapper)
        {
            _movieServices = movieServices;
            _genreServices = genreServices;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _movieServices.GetAll();
            // to map dto movies
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movies = await _movieServices.GetAllByID(id);
            if (movies == null)
                return NotFound();

            var dto = _mapper.Map<MovieDetailsDto>(movies);

            return Ok(dto);
        }
        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _movieServices.GetAll(genreId);
            // to map dto movies
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is required");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("only jpg and png are allowed");
            if(dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("max allowed size for poster is 1Mb !!");

            var isValidGenre = await _genreServices.IsValidGenre(dto.GenreId);
            if(!isValidGenre)
                return BadRequest("Invalid Genre ID !!");


            using var datastream = new MemoryStream();
            await dto.Poster.CopyToAsync(datastream);

            var movie =_mapper.Map<Movie>(dto);
            movie.Poster = datastream.ToArray();
           _movieServices.Add(movie);
            return Ok(movie);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _movieServices.GetAllByID(id);
            if (movie == null)
                return NotFound($"No movie was foung in this ID {id}");

            var isValidGenre = await _genreServices.IsValidGenre(dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid Genre ID !!");

            if(dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("only jpg and png are allowed");
                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("max allowed size for poster is 1Mb !!");

                using var datastream = new MemoryStream();
                await dto.Poster.CopyToAsync(datastream);
                movie.Poster = datastream.ToArray();

            }

            movie.Rate = dto.Rate;
            movie.Storeline = dto.Storeline;
            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Year = dto.Year;

          _movieServices.Update(movie);
            return Ok(movie);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _movieServices.GetAllByID(id);
            if (movie == null)
                return NotFound($"No movie was foung in this ID {id}");

            _movieServices.Delete(movie);

            return Ok(movie);
        }
    }
}

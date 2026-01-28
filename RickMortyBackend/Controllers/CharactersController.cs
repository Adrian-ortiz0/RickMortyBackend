using Microsoft.AspNetCore.Mvc;
using RickMortyApplication.Services;
using RickMortyApplication.Interfaces;

namespace RickMortyBackend.Controllers
{
    [ApiController]
    [Route("api/character")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterInterface _characterService;
        private readonly ILogger<CharactersController> _logger;

        public CharactersController(
            ICharacterInterface characterService,
            ILogger<CharactersController> logger)
        {
            _characterService = characterService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCharacters(
            [FromQuery] int page = 1,
            [FromQuery] string? name = null,
            [FromQuery] string? status = null,
            [FromQuery] string? species = null)
        {
            try
            {
                var result = await _characterService.GetCharactersAsync(page, name, status, species);
                return Ok(result);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, new { message = "External API unavailable" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching characters");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacterById(int id)
        {
            try
            {
                var character = await _characterService.GetCharacterByIdAsync(id);

                if (character == null)
                    return NotFound(new { message = $"Character {id} not found" });

                return Ok(character);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, new { message = "External API unavailable" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching character {id}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("episode")]
        public async Task<IActionResult> GetEpisode([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest(new { message = "Episode URL is required" });

            try
            {
                var episode = await _characterService.GetEpisodeAsync(url);
                return Ok(episode);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, new { message = "External API unavailable" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching episode");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}

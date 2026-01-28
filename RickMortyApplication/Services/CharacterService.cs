using Microsoft.Extensions.Logging;
using RickMortyApplication.Dtos;
using RickMortyApplication.Interfaces;
using RickMortyDomain.Entities;
using RickMortyDomain.Repositories;
using RickMortyDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickMortyApplication.Services
{
    public class CharacterService : ICharacterInterface
    {
        private readonly IRickMortyExternalService _externalService;
        private readonly ICharacterRepository _repository;
        private readonly ILogger<CharacterService> _logger;

        public CharacterService(
            IRickMortyExternalService externalService,
            ICharacterRepository repository,
            ILogger<CharacterService> logger)
        {
            _externalService = externalService;
            _repository = repository;
            _logger = logger;
        }

        public async Task<CharacterListResponse> GetCharactersAsync(
            int page,
            string? name,
            string? status,
            string? species)
        {
            try
            {
                var apiResponse = await _externalService.GetCharactersAsync(page, name, status, species);

                _ = Task.Run(async () =>
                {
                    foreach (var character in apiResponse.Results)
                    {
                        await SyncCharacterToDbAsync(character);
                    }
                });

                return new CharacterListResponse
                {
                    Info = new ApiInfoDto
                    {
                        Count = apiResponse.Info.Count,
                        Pages = apiResponse.Info.Pages,
                        Next = apiResponse.Info.Next,
                        Prev = apiResponse.Info.Prev
                    },
                    Results = apiResponse.Results.Select(MapToDto).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching characters");
                throw;
            }
        }

        public async Task<CharacterDetailDto?> GetCharacterByIdAsync(int id)
        {
            try
            {
                var dbCharacter = await _repository.GetByIdAsync(id);

                if (dbCharacter != null &&
                    (DateTime.UtcNow - dbCharacter.LastSync).TotalHours < 24)
                {
                    return MapToDetailDto(dbCharacter);
                }

                var character = await _externalService.GetCharacterByIdAsync(id);

                if (character != null)
                {
                    await SyncCharacterToDbAsync(character);
                }

                return MapToDetailDto(character);
            }
            catch (HttpRequestException)
            {
                _logger.LogWarning($"External API failed for character {id}, using cache");
                var dbCharacter = await _repository.GetByIdAsync(id);
                return dbCharacter != null ? MapToDetailDto(dbCharacter) : null;
            }
        }

        public async Task<EpisodeDto?> GetEpisodeAsync(string url)
        {
            try
            {
                var episode = await _externalService.GetEpisodeAsync(url);

                return new EpisodeDto
                {
                    Name = episode.Name,
                    Episode = episode.Capitule,
                    Air_date = episode.Air_date
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching episode from {url}");
                throw;
            }
        }

        private async Task SyncCharacterToDbAsync(Character character)
        {
            try
            {
                var exists = await _repository.ExistsAsync(character.Id);
                character.LastSync = DateTime.UtcNow;

                if (exists)
                {
                    await _repository.UpdateAsync(character);
                }
                else
                {
                    await _repository.AddAsync(character);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error syncing character {character.Id}");
            }
        }

        private CharacterDto MapToDto(Character character)
        {
            return new CharacterDto
            {
                Id = character.Id,
                Name = character.Name,
                Status = character.Status,
                Species = character.Species,
                Image = character.Image,
                Gender = character.Gender,
                Location = new LocationDto
                {
                    Name = character.LocationName,
                    Url = character.LocationUrl
                }
            };
        }

        private CharacterDetailDto MapToDetailDto(Character character)
        {
            return new CharacterDetailDto
            {
                Id = character.Id,
                Name = character.Name,
                Status = character.Status,
                Species = character.Species,
                Type = character.Type,
                Gender = character.Gender,
                Origin = new LocationDto
                {
                    Name = character.OriginName,
                    Url = character.OriginUrl
                },
                Location = new LocationDto
                {
                    Name = character.LocationName,
                    Url = character.LocationUrl
                },
                Image = character.Image,
                Episode = character.Episodes,
                Url = character.Url,
                Created = character.Created
            };
        }
    }
}

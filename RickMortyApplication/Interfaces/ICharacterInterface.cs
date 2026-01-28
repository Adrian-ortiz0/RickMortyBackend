using RickMortyApplication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickMortyApplication.Interfaces
{
    public interface ICharacterInterface
    {
        Task<CharacterListResponse> GetCharactersAsync(int page, string? name, string? status, string? species);
        Task<CharacterDetailDto?> GetCharacterByIdAsync(int id);
        Task<EpisodeDto?> GetEpisodeAsync(string url);
    }
}

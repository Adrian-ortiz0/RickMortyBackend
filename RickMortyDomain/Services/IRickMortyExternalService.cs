using RickMortyDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickMortyDomain.Services
{
    public interface IRickMortyExternalService
    {
        Task<ApiResponse> GetCharactersAsync(int page, string? name, string? status, string? species);
        Task<Character> GetCharacterByIdAsync(int id);
        Task<Episode> GetEpisodeAsync(string url);
    }

    public class ApiResponse
    {
        public ApiInfo Info { get; set; }
        public List<Character> Results { get; set; }
    }

    public class ApiInfo
    {
        public int Count { get; set; }
        public int Pages { get; set; }
        public string? Next { get; set; }
        public string? Prev { get; set; }
    }

    public class Episode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Capitule { get; set; }
        public string Air_date { get; set; }
    }
}

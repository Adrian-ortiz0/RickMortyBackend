using Microsoft.Extensions.Logging;
using RickMortyDomain.Entities;
using RickMortyDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RickMortyInfraestructure.ExternalServices
{
    public class RickMortyExternalService : IRickMortyExternalService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RickMortyExternalService> _logger;

        public RickMortyExternalService(HttpClient httpClient, ILogger<RickMortyExternalService> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://rickandmortyapi.com/api/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _logger = logger;
        }

        public async Task<ApiResponse> GetCharactersAsync(int page, string? name, string? status, string? species)
        {
            try
            {
                var queryParams = new List<string> { $"page={page}" };
                if (!string.IsNullOrEmpty(name)) queryParams.Add($"name={Uri.EscapeDataString(name)}");
                if (!string.IsNullOrEmpty(status)) queryParams.Add($"status={Uri.EscapeDataString(status)}");
                if (!string.IsNullOrEmpty(species)) queryParams.Add($"species={Uri.EscapeDataString(species)}");

                string url = "character";
                if (queryParams.Count > 0)
                    url += "?" + string.Join("&", queryParams);

                _logger.LogInformation("Calling Rick & Morty API: {url}", _httpClient.BaseAddress + url);

                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Rick & Morty API returned 404 for URL {url}", url);
                    return new ApiResponse { Info = new ApiInfo { Count = 0, Pages = 0 }, Results = new List<Character>() };
                }

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var apiResult = JsonSerializer.Deserialize<RickMortyApiResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return new ApiResponse
                {
                    Info = new ApiInfo
                    {
                        Count = apiResult.Info.Count,
                        Pages = apiResult.Info.Pages,
                        Next = apiResult.Info.Next,
                        Prev = apiResult.Info.Prev
                    },
                    Results = apiResult.Results.Select(MapToCharacter).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching characters from Rick & Morty API");
                return new ApiResponse { Info = new ApiInfo { Count = 0, Pages = 0 }, Results = new List<Character>() };
            }
        }

        public async Task<Character> GetCharacterByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"character/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var apiCharacter = JsonSerializer.Deserialize<RickMortyCharacter>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return MapToCharacter(apiCharacter);
        }

        public async Task<Episode> GetEpisodeAsync(string url)
        {
            try
            {
                string relativeUrl = url.StartsWith("https://rickandmortyapi.com/api") ? url.Replace("https://rickandmortyapi.com/api", "") : url;
                var response = await _httpClient.GetAsync(relativeUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Episode>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching episode");
                return null;
            }
        }

        private Character MapToCharacter(RickMortyCharacter apiChar)
        {
            return new Character
            {
                Id = apiChar.Id,
                Name = apiChar.Name,
                Status = apiChar.Status,
                Species = apiChar.Species,
                Type = apiChar.Type ?? string.Empty,
                Gender = apiChar.Gender,
                OriginName = apiChar.Origin?.Name ?? string.Empty,
                OriginUrl = apiChar.Origin?.Url ?? string.Empty,
                LocationName = apiChar.Location?.Name ?? string.Empty,
                LocationUrl = apiChar.Location?.Url ?? string.Empty,
                Image = apiChar.Image,
                Episodes = apiChar.Episode ?? new List<string>(),
                Url = apiChar.Url,
                Created = apiChar.Created
            };
        }

        private class RickMortyApiResponse
        {
            public RickMortyInfo Info { get; set; }
            public List<RickMortyCharacter> Results { get; set; }
        }

        private class RickMortyInfo
        {
            public int Count { get; set; }
            public int Pages { get; set; }
            public string? Next { get; set; }
            public string? Prev { get; set; }
        }

        private class RickMortyCharacter
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public string Species { get; set; }
            public string? Type { get; set; }
            public string Gender { get; set; }
            public RickMortyLocation? Origin { get; set; }
            public RickMortyLocation? Location { get; set; }
            public string Image { get; set; }
            public List<string>? Episode { get; set; }
            public string Url { get; set; }
            public DateTime Created { get; set; }
        }

        private class RickMortyLocation
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}
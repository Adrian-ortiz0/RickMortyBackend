using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickMortyApplication.Dtos
{
    public class CharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }
        public string Image { get; set; }
        public string Gender { get; set; }
        public LocationDto Location { get; set; }
    }

    public class CharacterDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public LocationDto Origin { get; set; }
        public LocationDto Location { get; set; }
        public string Image { get; set; }
        public List<string> Episode { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; }
    }

    public class LocationDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class CharacterListResponse
    {
        public ApiInfoDto Info { get; set; }
        public List<CharacterDto> Results { get; set; }
    }

    public class ApiInfoDto
    {
        public int Count { get; set; }
        public int Pages { get; set; }
        public string? Next { get; set; }
        public string? Prev { get; set; }
    }

    public class EpisodeDto
    {
        public string Name { get; set; }
        public string Episode { get; set; }
        public string Air_date { get; set; }
    }
}

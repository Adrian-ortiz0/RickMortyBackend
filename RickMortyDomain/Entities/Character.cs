using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickMortyDomain.Entities
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public string OriginName { get; set; }
        public string OriginUrl { get; set; }
        public string LocationName { get; set; }
        public string LocationUrl { get; set; }
        public string Image { get; set; }
        public List<string> Episodes { get; set; } = new();
        public string Url { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastSync { get; set; }
    }
}

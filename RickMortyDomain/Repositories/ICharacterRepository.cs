using RickMortyDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickMortyDomain.Repositories
{
    public interface ICharacterRepository
    {
        Task<Character?> GetByIdAsync(int id);
        Task<List<Character>> GetAllAsync();
        Task<Character?> AddAsync(Character character);
        Task<Character?> UpdateAsync(Character character);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

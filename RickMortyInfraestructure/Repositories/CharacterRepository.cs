using Microsoft.EntityFrameworkCore;
using RickMortyDomain.Entities;
using RickMortyDomain.Repositories;
using RickMortyInfraestructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickMortyInfraestructure.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _context;

        public CharacterRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Character?> GetByIdAsync(int id)
        {
            return await _context.Characters.FindAsync(id);
        }

        public async Task<List<Character>> GetAllAsync()
        {
            return await _context.Characters.ToListAsync();
        }

        public async Task<Character?> AddAsync(Character character)
        {
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            return character;
        }

        public async Task<Character?> UpdateAsync(Character character)
        {
            _context.Characters.Update(character);
            await _context.SaveChangesAsync();
            return character;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null) return false;

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Characters.AnyAsync(c => c.Id == id);
        }
    }

}

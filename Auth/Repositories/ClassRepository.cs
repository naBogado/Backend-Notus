using Microsoft.EntityFrameworkCore;
using Notus.Config;
using Notus.Models.Class;

namespace Notus.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Class>> GetAllAsync()
        {
            return await _context.Clases.ToListAsync();
        }

        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _context.Clases.FindAsync(id);
        }

        public async Task<Class> AddAsync(Class cls)
        {
            _context.Clases.Add(cls);
            await _context.SaveChangesAsync();
            return cls;
        }

        public async Task<Class> UpdateAsync(Class cls)
        {
            _context.Clases.Update(cls);
            await _context.SaveChangesAsync();
            return cls;
        }

        public async Task DeleteAsync(int id)
        {
            var cls = await _context.Clases.FindAsync(id);
            if (cls != null)
            {
                _context.Clases.Remove(cls);
                await _context.SaveChangesAsync();
            }
        }
    }
}

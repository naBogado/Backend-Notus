using Notus.Models.Class;

namespace Notus.Repositories
{
    public interface IClassRepository
    {
        Task<List<Class>> GetAllAsync();
        Task<Class?> GetByIdAsync(int id);
        Task<Class> AddAsync(Class cls);
        Task<Class> UpdateAsync(Class cls);
        Task DeleteAsync(int id);
    }
}

using Notus.Models.Class;
using Notus.Repositories;

namespace Notus.Services
{
    public class ClassServices
    {
        private readonly IClassRepository _repository;

        public ClassServices(IClassRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Class>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Class?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task<Class> AddAsync(Class cls) => await _repository.AddAsync(cls);
        public async Task<Class> UpdateAsync(Class cls) => await _repository.UpdateAsync(cls);
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}

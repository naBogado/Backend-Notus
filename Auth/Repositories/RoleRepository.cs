using Notus.Config;
using Notus.Models.Role;

namespace Notus.Repositories
{
    public interface IRoleRepository : IRepository<Role> { }
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _db;

        public RoleRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }
    }
}

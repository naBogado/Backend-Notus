using Notus.Config;
using Notus.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Notus.Repositories
{
    public interface IUserRepository : IRepository<User> { }
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }

        new async public Task<User> GetOneAsync(Expression<Func<User, bool>>? filter = null)
        {
            IQueryable<User> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter).Include(x => x.Roles);
            }
            return await query.FirstOrDefaultAsync();
        }

        new async public Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>>? filter = null)
        {
            IQueryable<User> query = dbSet.Include(x => x.Roles);
            if (filter != null) {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
    }
}

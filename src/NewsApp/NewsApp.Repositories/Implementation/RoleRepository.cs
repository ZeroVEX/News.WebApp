using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Repositories;
using NewsApp.DomainModel;
using NewsApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.Repositories.Implementation
{
    public class RoleRepository : EntityRepository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext dbContext)
            : base(dbContext)
        {

        }


        public async Task<Role> GetByNameAsync(string normalizedName)
        {
            return await DbContext.Set<Role>().SingleOrDefaultAsync(r => r.NormalizedName == normalizedName);
        }

        public async Task<IReadOnlyCollection<User>> GetUsersInRoleAsync(int roleId)
        {
            var users = await DbContext.Set<UserRole>()
                .Where(r => r.RoleId == roleId)
                .Select(r => r.User)
                .ToListAsync();

            return users;
        }
    }
}
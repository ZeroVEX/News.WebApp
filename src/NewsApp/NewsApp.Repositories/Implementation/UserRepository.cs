using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Repositories;
using NewsApp.DomainModel;
using NewsApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.Repositories.Implementation
{
    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext)
            : base(dbContext)
        {

        }


        public async Task<bool> CheckIfUserIsFirstAsync(User user)
        {
            var userId = await DbContext.Set<User>().OrderBy(u => u.Id).Select(u => u.Id).FirstAsync();

            return userId == user.Id;
        }

        public async Task<int> CountUsersAsync(string nameFilter)
        {
            var users = DbContext.Set<User>();

            var count = nameFilter == null
                ? await users.CountAsync()
                : await users.CountAsync(u => u.DisplayName.Contains(nameFilter));

            return count;
        }

        public async Task<IReadOnlyCollection<User>> GetUsersPageAsync(int skip, int take, string nameFilter)
        {
            IQueryable<User> usersQuery = DbContext.Set<User>()
                .Include(u => u.UserRoles)
                .ThenInclude(r => r.Role);

            if (nameFilter != null)
            {
                usersQuery = usersQuery.Where(u => u.DisplayName.Contains(nameFilter));
            }

            var users = await usersQuery
                .OrderBy(u => u.DisplayName)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return users;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await DbContext.Set<User>().SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IReadOnlyCollection<string>> GetRolesAsync(int userId)
        {
            var roles = await DbContext.Set<UserRole>()
                .Where(r => r.UserId == userId)
                .Select(r => r.Role.Name)
                .ToListAsync();

            return roles;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await DbContext.Set<User>()
                .Include(u => u.UserRoles)
                .ThenInclude(r => r.Role)
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }
    }
}
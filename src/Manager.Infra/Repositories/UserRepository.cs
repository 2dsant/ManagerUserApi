using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Manager.Infra.Interfaces;
using Manager.Domain.Entities;
using System.Threading.Tasks;
using Manager.Infra.Context;
using System.Linq;

namespace Manager.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ManagerContext _context;

        public UserRepository(ManagerContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _context.Users.Where(x => x.Email.ToLower() == email.ToLower())
                .AsNoTracking().ToListAsync();
            
            return user.FirstOrDefault();
        }

        public async Task<List<User>> SearchByEmail(string email)
        {
            var users = await _context.Users.Where(x => x.Email.ToLower().Contains(email.ToLower()))
                .AsNoTracking().ToListAsync();
            
            return users;
        }

        public async Task<List<User>> SearchByName(string name)
        {
            var users = await _context.Users.Where(x => x.Name.ToLower().Contains(name.ToLower()))
                .AsNoTracking().ToListAsync();
            
            return users;
        }
    }
}
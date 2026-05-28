using Data;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly MovieCatalogDbContext _context;

        public UserService(MovieCatalogDbContext context)
        {
            _context = context;
        }

        public void AddUser(string username, string email)
        {
            var user = new User
            {
                Username = username,
                Email = email
            };

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User? GetByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(x => x.Email == email);
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }
    }
}


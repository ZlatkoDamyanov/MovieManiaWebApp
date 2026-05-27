using Data;
using Microsoft.EntityFrameworkCore;
using Services.Implementations;

namespace Tests
{
    public class UserServiceTests : IDisposable
    {
        private readonly UserService _service;
        private readonly MovieCatalogDbContext _context;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovieCatalogDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new MovieCatalogDbContext(options);
            _service = new UserService(_context);
        }

        [Test]
        public void AddUser_ShouldWork()
        {
            _service.AddUser("testuser", "test@mail.com");

            var user = _service.GetByEmail("test@mail.com");

            Assert.AreEqual(1, _context.Users.Count());

            _context.Users.Remove(user);
        }

        [Test]
        public void GetAllUser_ShouldReturnUsers()
        {
            _context.Users.Add(new User { Username = "User1", Email = "user1@mail.com" });
            _context.Users.Add(new User { Username = "User2", Email = "user2@mail.com" });
            _context.Users.Add(new User { Username = "User3", Email = "user3@mail.com" });
            _context.SaveChanges();

            var result = _service.GetAll();

            Assert.AreEqual(3, result.Count);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory; 
using Services.Implementations;

namespace Tests
{
    public class MovieServiceTests : IDisposable
    {
        private readonly MovieService _service;
        private readonly MovieCatalogDbContext _context;

        public MovieServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovieCatalogDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new MovieCatalogDbContext(options);
            _service = new MovieService(_context);
        }

        [Test]
        public void Add_ShouldAddMovie()
        {
            var movie = new Movie
            {
                Title = "Test Movie",
                Director = "Test Director",
                ReleaseYear = 2024,
                Duration = 120,
                Type = MovieType.Филм,
                Actors = new List<Actor>(),
                Reviews = new List<Review>()
            };

            _service.Add(movie);

            Assert.AreEqual(1, _context.Movies.Count());

            _context.Movies.Remove(movie);
        }

        [Test]
        public void Delete_ShouldRemoveMovie()
        {
            var movie = new Movie
            {
                Title = "Test Movie",
                Director = "Test Director",
                ReleaseYear = 2024,
                Duration = 120,
                Type = MovieType.Филм,
                Actors = new List<Actor>(),
                Reviews = new List<Review>()
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();

            int cntBefore = _context.Movies.Count();
            _service.Delete(movie.Id);

            Assert.AreEqual(cntBefore - 1, _context.Movies.Count());
        }

        [Test]
        public void GetById_ShouldReturnCorrectMovie()
        {
            var movie = new Movie
            {
                Title = "Test Movie",
                Director = "Test Director",
                ReleaseYear = 2024,
                Duration = 120,
                Type = MovieType.Филм,
                Actors = new List<Actor>(),
                Reviews = new List<Review>()
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();

            var result = _service.GetById(movie.Id);

            Assert.NotNull(result);
            Assert.AreEqual("Test Movie", result.Title);
        }

        [Test]
        public void GetAll_ShouldReturnAllMovies()
        {
            _context.Movies.Add(new Movie
            {
                Title = "Test Movie",
                Director = "Test Director",
                ReleaseYear = 2024,
                Duration = 120,
                Type = MovieType.Филм,
                Actors = new List<Actor>(),
                Reviews = new List<Review>()
            }
            );
            _context.Movies.Add(new Movie
            {
                Title = "Test Movie",
                Director = "Test Director",
                ReleaseYear = 2024,
                Duration = 120,
                Type = MovieType.Филм,
                Actors = new List<Actor>(),
                Reviews = new List<Review>()
        });

            _context.SaveChanges();

            var result = _service.GetAll();

            Assert.AreEqual(2, result.Count);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
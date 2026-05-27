using Data;
using Microsoft.EntityFrameworkCore;
using Services.Implementations;

namespace Tests
{
    public class ReviewServiceTests : IDisposable
    {
        private readonly ReviewService _service;
        private readonly MovieCatalogDbContext _context;

        public ReviewServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovieCatalogDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new MovieCatalogDbContext(options);
            _service = new ReviewService(_context);
        }

        [Test]
        public void AddReview_ShouldWork()
        {
            var movie = new Movie
            {
                Title = "Test title",
                Director = "Test Director",
                ReleaseYear = 2024,
                Duration = 120,
                Type = MovieType.Филм,
                Actors = new List<Actor>(),
                Reviews = new List<Review>()
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();

            _service.AddReview(movie.Id, null, 5, "Great movie!");

            Assert.AreEqual(1, _context.Reviews.Count());
        }

        [Test]
        public void GetAverageRating_ShouldBeCorrect()
        {
            var movie = new Movie
            {
                Title = "Test title",
                Director = "Test Director",
                ReleaseYear = 2024,
                Duration = 120,
                Type = MovieType.Филм,
                Actors = new List<Actor>(),
                Reviews = new List<Review>()
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();

            _context.Reviews.Add(new Review { MovieId = movie.Id, Rating = 4, UserId = null, Content = "Good" });
            _context.Reviews.Add(new Review { MovieId = movie.Id, Rating = 2, UserId = null, Content = "Ok" });
            _context.SaveChanges();

            var avg = _service.GetAverageRating(movie.Id);

            Assert.AreEqual(3, avg);
        }

        [Test]
        public void InvalidRating_ShouldThrow()
        {
            Assert.Throws<Exception>(() =>
                _service.AddReview(1, null, 10, "Bad"));
        }

        [Test]
        public void GetAverageRating_NoReviews_ShouldReturnZero()
        {
            var movie = new Movie
            {
                Title = "Test title",
                Director = "Test Director",
                ReleaseYear = 2024,
                Duration = 120,
                Type = MovieType.Филм,
                Actors = new List<Actor>(),
                Reviews = new List<Review>()
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();

            var result = _service.GetAverageRating(movie.Id);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetAverageRating_MultipleReviews_ShouldReturnCorrectAverage()
        {
            var movie = new Movie
            {
                Title = "Test title",
                Director = "Test Director",
                ReleaseYear = 2024,
                Duration = 120,
                Type = MovieType.Филм,
                Actors = new List<Actor>(),
                Reviews = new List<Review>()
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();

            _context.Reviews.Add(new Review { MovieId = movie.Id, Rating = 5, UserId = null, Content = "Excellent" });
            _context.Reviews.Add(new Review { MovieId = movie.Id, Rating = 3, UserId = null, Content = "Ok" });
            _context.Reviews.Add(new Review { MovieId = movie.Id, Rating = 1, UserId = null, Content = "Bad" });
            _context.SaveChanges();

            var result = _service.GetAverageRating(movie.Id);

            Assert.AreEqual(3, result);
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}


 
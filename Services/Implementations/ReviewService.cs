using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly MovieCatalogDbContext _context;

        public ReviewService(MovieCatalogDbContext context)
        {
            _context = context;
        }

        public void AddReview(int movieId, int? userId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
            {
                throw new Exception("Rating must be between 1 and 5!");
            }

            var review = new Review
            {
                MovieId = movieId,
                UserId = userId,
                Rating = rating,
                Content = comment
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public List<Review> GetByMovie(int movieId)
        {
            return _context.Reviews
                .Include(r => r.User)
                .Where(r => r.MovieId == movieId)
                .ToList();
        }

        public double GetAverageRating(int movieId)
        {
            var reviews = _context.Reviews
                .Where(r => r.MovieId == movieId);

            if (!reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating);
        }
    }
}
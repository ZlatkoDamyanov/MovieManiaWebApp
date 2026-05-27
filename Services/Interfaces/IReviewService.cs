using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IReviewService
    {
        void AddReview(int movieId, int? userId, int rating, string comment);

        List<Review> GetByMovie(int movieId);

        double GetAverageRating(int movieId);
    }
}


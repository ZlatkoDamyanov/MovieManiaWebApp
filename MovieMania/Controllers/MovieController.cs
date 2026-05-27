using Data;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace MovieMania.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;

        public MoviesController(IMovieService movieService, IReviewService reviewService, IUserService userService)
        {
            _movieService = movieService;
            _reviewService = reviewService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            var movies = _movieService.GetAll();
            return View(movies);
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            var movies = _movieService.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                movies = movies
                    .Where(m => m.Title.Contains(search))
                    .ToList();
            }

            return View(movies);
        }

        public IActionResult Details(int id)
        {
            var movie = _movieService.GetById(id);
            var avgRating = _reviewService.GetAverageRating(id);

            ViewBag.AverageRating = $"{avgRating:f2}";

            return View(movie);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            _movieService.Add(movie);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddReview(int movieId, int rating, string comment, string? username, string? email)
        {
            int userId = -1;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email))
            {
                var user = _userService.GetByEmail(email);

                if (user == null)
                {
                    _userService.AddUser(username, email);
                    user = _userService.GetByEmail(email);
                }

                userId = user.Id;
            }

            _reviewService.AddReview(
                movieId,
                userId == -1 ? null : userId,
                rating,
                comment
            );

            return RedirectToAction("Details", new { id = movieId });
        }
        public IActionResult Edit(int id)
        {
            var movie = _movieService.GetById(id);
            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Movie movie)
        {
            _movieService.Update(movie);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var movie = _movieService.GetById(id);
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _movieService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}

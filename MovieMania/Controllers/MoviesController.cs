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

        [HttpGet]
        public IActionResult Index(string search)
        {
            var movies = _movieService.GetAll();

            if (!string.IsNullOrEmpty(search))
                movies = movies.Where(m => m.Title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            ViewBag.Search = search;
         
            return View(movies);
        }
        
        [HttpGet]
        public IActionResult Details(int id)
        {
            var movie = _movieService.GetById(id);
            var avgRating = _reviewService.GetAverageRating(id);
            ViewBag.AverageRating = $"{avgRating:f2}";

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie, string? actorNames)
        {
            if (!string.IsNullOrEmpty(actorNames))
            {
                movie.Actors = _movieService.ParseActors(actorNames);
                _movieService.Add(movie);
                return RedirectToAction("Index");
            }
            else
            {
                movie.Actors = new List<Actor>();
            }

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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _movieService.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Movie movie, string? actorNames)
        {
            var existing = _movieService.GetById(movie.Id);
            existing.Actors.Clear();

            existing.Title = movie.Title;
            existing.Director = movie.Director;
            existing.Genre = movie.Genre;
            existing.ReleaseYear = movie.ReleaseYear;
            existing.Duration = movie.Duration;
            existing.Type = movie.Type;
            existing.ImageUrl = movie.ImageUrl;
            existing.Description = movie.Description;
            existing.Actors = _movieService.ParseActors(actorNames);

            _movieService.Update(existing);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var movie = _movieService.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _movieService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteReview(int id, int movieId)
        {
            _reviewService.DeleteReview(id);
            return RedirectToAction("Details", new { id = movieId });
        }
    }
}
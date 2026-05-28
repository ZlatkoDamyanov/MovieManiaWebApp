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
    public class MovieService : IMovieService
    {
        private readonly MovieCatalogDbContext _context;

        public MovieService(MovieCatalogDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetAll()
        {
            return _context.Movies
                .Include(m => m.Reviews)
                .Include(m => m.Actors)
                .ToList();
        }

        public Movie? GetById(int id)
        {
            return _context.Movies
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.User)
                .Include(m => m.Actors)
                .FirstOrDefault(m => m.Id == id);
        }

        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public void Update(Movie movie)
        {
            _context.Movies.Update(movie);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
        }

        public List<Actor> ParseActors(string? actorNames)
        {
            if (string.IsNullOrEmpty(actorNames))
                return new List<Actor>();

            return actorNames
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(name => name.Trim())
                .Where(name => !string.IsNullOrEmpty(name))
                .Select(name => new Actor { Name = name })
                .ToList();
        }
    }
}
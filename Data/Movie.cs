using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public enum MovieType
    {
        Movie = 0,
        Series = 1
    }
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public int Duration { get; set; } // Duration in minutes
        public MovieType Type { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public List<Actor> Actors { get; set; } 
        public List<Review> Reviews { get; set; }




    }
}

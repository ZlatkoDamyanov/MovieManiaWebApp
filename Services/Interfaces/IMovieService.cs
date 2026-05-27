using Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMovieService
    {
        List<Movie> GetAll();

        Movie GetById(int id);

        void Add(Movie movie);

        void Update(Movie movie);

        void Delete(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReservation.Server.Application.Common.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetMoviesAsync();
        Task<Movie> GetMovieByIdAsync(Guid id);
        Task AddMovieAsync(Movie movie);
    }
}
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Domain.Entities;
using MovieReservation.Server.Domain.Enums;

namespace MovieReservation.Server.Application.Roles.Queries.GetAllMoviesForARole
{
   public class GetAllMoviesForARoleQuery : IRequest<RoleWithMoviesDto>
    {
        public int RoleId { get; set; }

        public GetAllMoviesForARoleQuery(int roleId)
        {
            RoleId = roleId;
        }
    }
    public class GetAllMoviesForARoleQueryHandler : IRequestHandler<GetAllMoviesForARoleQuery, RoleWithMoviesDto?>
    {
        private readonly IMovieReservationDbContext _context;

        public GetAllMoviesForARoleQueryHandler(IMovieReservationDbContext context)
        {
            _context = context;
        }

        public async Task<RoleWithMoviesDto?> Handle(GetAllMoviesForARoleQuery request, CancellationToken cancellationToken)
        {
            var role = await _context.Roles
                .Include(r => r.MovieRoles)
                .ThenInclude(mr => mr.Movie)
                .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

            if (role == null)
                return null;

            return new RoleWithMoviesDto
            {
                RoleId = role.Id,
                FullName = role.FullName,
                Age = role.Age,
                PictureUrl = role.PictureUrl,
                Movies = role.MovieRoles.Select(mr => new MovieForRoleDto
                {
                    MovieId = mr.Movie.Id,
                    RoleType = mr.RoleType.ToString(),
                    Title = mr.Movie.Title,
                    Summary = mr.Movie.Summary,
                    Year = mr.Movie.Year,
                    Rating = mr.Movie.Rating.ToString(),
                    TrailerUrl = mr.Movie.TrailerUrl,
                    PosterUrl = mr.Movie.PosterUrl,
                    MovieType = mr.Movie.MovieType.ToString()
                }).ToList()
            };
        }
    }
}

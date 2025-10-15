namespace MovieReservation.Server.Application.Roles.Queries.GetAllMoviesForARole
{
    public class MovieForRoleDto
    {
        public int MovieId { get; set; }
        public string RoleType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int Year { get; set; }
        public string? Rating { get; set; }
        public string TrailerUrl { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public string MovieType { get; set; } = string.Empty;
    }
}

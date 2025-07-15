using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace MovieReservation.Server.Infrastructure.Data
{
    public static class InitialiserExtensions
    {
        public static void AddAsyncSeeding(this DbContextOptionsBuilder builder, IServiceProvider serviceProvider)
        {
            builder.UseAsyncSeeding(async (context, _, ct) =>
            {
                var initialiser = serviceProvider.GetRequiredService<MovieReservationDbContextInitialiser>();

                await initialiser.SeedAsync();
            });
        }

        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<MovieReservationDbContextInitialiser>();

            await initialiser.InitialiseAsync();
        }
    }
    public class MovieReservationDbContextInitialiser
    {
        private readonly ILogger<MovieReservationDbContextInitialiser> _logger;
        private readonly MovieReservationDbContext _context;

        public MovieReservationDbContextInitialiser(ILogger<MovieReservationDbContextInitialiser> logger, MovieReservationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task InitialiseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
        public async Task TrySeedAsync()
        {
            // Seed Genres
            if (!_context.Genres.Any())
            {
                _context.Genres.AddRange(
                    new Genre { Id = 1, Name = "Horror" },
                    new Genre { Id = 2, Name = "Action" },
                    new Genre { Id = 3, Name = "Fantasy" },
                    new Genre { Id = 4, Name = "Comedy" },
                    new Genre { Id = 5, Name = "Drama" },
                    new Genre { Id = 6, Name = "Thriller" },
                    new Genre { Id = 7, Name = "Mystery" },
                    new Genre { Id = 8, Name = "Romance" },
                    new Genre { Id = 10, Name = "Sci-Fi" },
                    new Genre { Id = 11, Name = "Crime" }
                );
            }
            // Seed Movies
            if (!_context.Movies.Any())
            {
                _context.Movies.AddRange(
                    new Movie
                    {
                        Id = 4,
                        Title = "GODZILLA VS KONG",
                        Summary = "Legends collide as Godzilla and Kong, the two most powerful forces of nature, clash on the big screen in a spectacular battle for the ages. As a squadron embarks on a perilous mission into fantastic uncharted terrain, unearthing clues to the Titans' very origins and mankind's survival, a conspiracy threatens to wipe the creatures, both good and bad, from the face of the earth forever.",
                        Year = 2021,
                        Rating = 6.5m,
                        TrailerUrl = "https://media.publit.io/file/h_720/godzilla_vs_kong.mp4",
                        PosterUrl = "https://images.squarespace-cdn.com/content/v1/51b3dc8ee4b051b96ceb10de/1615997337403-G2AT6OCZ0LD8VLQ1IHGC/ke17ZwdGBToddI8pDm48kJYq1aWJR-Opw9YCGEJvNoV7gQa3H78H3Y0txjaiv_0fDoOvxcdMmMKkDsyUqMSsMWxHk725yiiHCCLfrh8O1z4YTzHvnKhyp6Da-NYroOW3ZGjoBKy3azqku80C789l0k5fwC0WRNFJBIXiBeNI5fL5tAx0_Wm6zQGcCxuXSnc3-CppMx_loiHYdjEK2HksYg/EwnOBogWEAEiakT.jpeg?format=1500w",
                        MovieType = MovieType.NowShowing
                    },
                    new Movie
                    {
                        Id = 34,
                        Title = "JOKER",
                        Summary = "Arthur Fleck, a party clown, leads an impoverished life with his ailing mother. However, when society shuns him and brands him as a freak, he decides to embrace the life of crime and chaos.",
                        Year = 2019,
                        Rating = 8.4m,
                        TrailerUrl = "https://media.publit.io/file/h_720/joker-x.mp4",
                        PosterUrl = "https://pbs.twimg.com/media/EA4LLfsW4AErVjR.jpg",
                        MovieType = MovieType.NowShowing
                    },
                    new Movie
                    {
                        Id = 35,
                        Title = "SUICIDE SQUAD 2",
                        Summary = "The government sends the most dangerous supervillains in the world -- Bloodsport, Peacemaker, King Shark, Harley Quinn and others -- to the remote, enemy-infused island of Corto Maltese. Armed with high-tech weapons, they trek through the dangerous jungle on a search-and-destroy mission, with only Col. Rick Flag on the ground to make them behave.",
                        Year = 2021,
                        Rating = null,
                        TrailerUrl = "https://media.publit.io/file/h_720/suicide_squad.mp4",
                        PosterUrl = "https://www.inspirationde.com/media/2019/08/cristiano-siqueira-on-behance-1565929796gk84n.png",
                        MovieType = MovieType.ComingSoon
                    },
                    new Movie
                    {
                        Id = 36,
                        Title = "THE BATMAN",
                        Summary = "The Riddler plays a deadly game of cat and mouse with Batman and Commissioner Gordon in Gotham City.",
                        Year = 2022,
                        Rating = null,
                        TrailerUrl = "https://media.publit.io/file/h_720/batman.mp4",
                        PosterUrl = "https://www.inspirationde.com/media/2020/08/the-batman-poster-by-mizuriofficial-on-deviantart-15987584958gkn4.jpg",
                        MovieType = MovieType.ComingSoon
                    }
                );
            }

            // Seed MovieGenres
            if (!_context.MovieGenres.Any())
            {
                _context.MovieGenres.AddRange(
                    new MovieGenre { MovieId = 4, GenreId = 1 },
                    new MovieGenre { MovieId = 4, GenreId = 2 },
                    new MovieGenre { MovieId = 4, GenreId = 4 },
                    new MovieGenre { MovieId = 34, GenreId = 5 },
                    new MovieGenre { MovieId = 34, GenreId = 6 },
                    new MovieGenre { MovieId = 34, GenreId = 11 },
                    new MovieGenre { MovieId = 35, GenreId = 2 },
                    new MovieGenre { MovieId = 35, GenreId = 3 },
                    new MovieGenre { MovieId = 35, GenreId = 4 },
                    new MovieGenre { MovieId = 36, GenreId = 2 },
                    new MovieGenre { MovieId = 36, GenreId = 5 },
                    new MovieGenre { MovieId = 36, GenreId = 11 }
                );
            }

            // Seed Roles
            if (!_context.Roles.Any())
            {
                _context.Roles.AddRange(
                    new Role { Id = 4, FullName = "Robert Pattinson", Age = 35, PictureUrl = "https://i.pinimg.com/564x/8d/e3/89/8de389c84e919d3577f47118e2627d95.jpg" },
                    new Role { Id = 5, FullName = "Zoë Kravitz", Age = 32, PictureUrl = "https://www.simplyceleb.com/wp-content/uploads/2020/06/Zoe-Kravitz-Filmleri.jpg" },
                    new Role { Id = 6, FullName = "Paul Dano", Age = 36, PictureUrl = "https://resizing.flixster.com/1PQhbMray969ia5n7JHLBO3qATA=/506x652/v2/https://flxt.tmsimg.com/v9/AllPhotos/267214/267214_v9_ba.jpg" },
                    new Role { Id = 7, FullName = "Mat Reeves", Age = 55, PictureUrl = "https://i.dailymail.co.uk/1s/2020/04/10/05/27028708-0-image-a-2_1586494056548.jpg" },
                    new Role { Id = 8, FullName = "Margot Robbie", Age = 30, PictureUrl = "https://hips.hearstapps.com/elleuk.cdnds.net/18/10/1520210874-gettyimages-927252130.jpg?crop=0.988xw:0.658xh;0.0119xw,0.0691xh&resize=640:*" },
                    new Role { Id = 9, FullName = "Pete Davidson", Age = 27, PictureUrl = "https://media.nu.nl/m/ansxdkraedri_sqr256.jpg/pete-davidson-vertelt-nieuwe-liefdes-meteen-over-eigenaardigheden.jpg" },
                    new Role { Id = 10, FullName = "John Cena", Age = 44, PictureUrl = "https://ichef.bbci.co.uk/news/976/cpsprodpb/2536/production/_118662590_cena2.jpg" },
                    new Role { Id = 11, FullName = "James Gunn", Age = 50, PictureUrl = "https://ichef.bbci.co.uk/news/976/cpsprodpb/22F5/production/_103794980_gettyimages-678895846.jpg" },
                    new Role { Id = 12, FullName = "Todd Phillips", Age = 50, PictureUrl = "https://i.redd.it/hwk9xgheqzm51.jpg" },
                    new Role { Id = 13, FullName = "Zazie Beetz", Age = 30, PictureUrl = "https://resizing.flixster.com/t3iiAgmwNQpbDsiVNvNc8s3Zuug=/506x652/v2/https://flxt.tmsimg.com/v9/AllPhotos/981946/981946_v9_bb.jpg" },
                    new Role { Id = 14, FullName = "Robert De Niro", Age = 77, PictureUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/58/Robert_De_Niro_Cannes_2016.jpg/220px-Robert_De_Niro_Cannes_2016.jpg" },
                    new Role { Id = 15, FullName = "Joaquin Phoenix", Age = 46, PictureUrl = "https://i.pinimg.com/originals/1d/2e/12/1d2e12756addc022144c4a8ac437f5c0.jpg" },
                    new Role { Id = 16, FullName = "Millie Bobby Brown", Age = 17, PictureUrl = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcQhVfKxsjWZM-30ElFKfETvE1VUZyZ9OV3UcxZ_5O6hQMmawqCV" },
                    new Role { Id = 17, FullName = "Alexander Skarsgård", Age = 44, PictureUrl = "https://encrypted-tbn3.gstatic.com/images?q=wn:GccTXROK8cAOBwEOohepzhbjJdpAUVQBTVOaWQ4Rtp6iR0wMLyx4W" },
                    new Role { Id = 18, FullName = "Rebecca Hall", Age = 39, PictureUrl = "https://i.pinimg.com/originals/30/ac/b4/30acb4e1f6f8f0437a8fb7ceb04085db.jpg" },
                    new Role { Id = 19, FullName = "Adam Wingard", Age = 38, PictureUrl = "https://encrypted-tbn2.gstatic.com/images?q=wn:GcRkWTdn0iu8DiewFAFNvOEOXtFctVTC2-fCX5LWZJN8tc8l035q" },
                    new Role { Id = 20, FullName = "Mary Parent", Age = 53, PictureUrl = "https://encrypted-tbn2.gstatic.com/images?q=wn:GcTai8Ehm5_kioXJMqPfvIoFx8QwIOYsyoqUhFT0Im6FiIL1_mI_" }
                );
            }

            // Seed MovieRoles
            if (!_context.MovieRoles.Any())
            {
                _context.MovieRoles.AddRange(
                    new MovieRole { MovieId = 4, RoleId = 16, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 4, RoleId = 17, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 4, RoleId = 18, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 4, RoleId = 19, RoleType = RoleType.Director },
                    new MovieRole { MovieId = 4, RoleId = 20, RoleType = RoleType.Producer },
                    new MovieRole { MovieId = 34, RoleId = 12, RoleType = RoleType.Director },
                    new MovieRole { MovieId = 34, RoleId = 13, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 34, RoleId = 14, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 34, RoleId = 15, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 35, RoleId = 8, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 35, RoleId = 9, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 35, RoleId = 10, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 35, RoleId = 11, RoleType = RoleType.Director },
                    new MovieRole { MovieId = 36, RoleId = 4, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 36, RoleId = 5, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 36, RoleId = 6, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 36, RoleId = 7, RoleType = RoleType.Director }
                );
            }

            // Seed Theaters
            if (!_context.Theaters.Any())
            {
                _context.Theaters.AddRange(
                    new Theater { Id = 8, Name = "A", NumOfRows = 10, SeatsPerRow = 20, Type = TheaterType.Normal },
                    new Theater { Id = 10, Name = "B", NumOfRows = 5, SeatsPerRow = 5, Type = TheaterType.Royal },
                    new Theater { Id = 15, Name = "C", NumOfRows = 10, SeatsPerRow = 10, Type = TheaterType.Normal },
                    new Theater { Id = 16, Name = "D", NumOfRows = 12, SeatsPerRow = 15, Type = TheaterType.Royal },
                    new Theater { Id = 22, Name = "E", NumOfRows = 8, SeatsPerRow = 13, Type = TheaterType.Normal }
                );
            }

            // Seed TheaterSeats
            if (!_context.TheaterSeats.Any())
            {
                _context.TheaterSeats.AddRange(
                    new TheaterSeat { SeatRow = "A", SeatNumber = 11, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 0, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "A", SeatNumber = 11, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 11, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 11, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 11, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "A", SeatNumber = 3, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 3, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 3, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 3, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 3, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 3, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "G", SeatNumber = 3, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 3, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 3, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "J", SeatNumber = 3, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "A", SeatNumber = 7, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 7, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 7, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 7, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 7, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 7, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "G", SeatNumber = 7, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 7, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 7, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "J", SeatNumber = 7, TheaterId = 15, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "A", SeatNumber = 4, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 4, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 4, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 4, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 0, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 1, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 2, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 3, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 0, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 1, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 2, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 3, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 4, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "J", SeatNumber = 4, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "K", SeatNumber = 4, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "L", SeatNumber = 4, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "A", SeatNumber = 10, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 10, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 10, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 10, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 11, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 12, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 13, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 14, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 10, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "J", SeatNumber = 10, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "K", SeatNumber = 10, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "L", SeatNumber = 10, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 11, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 12, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 13, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 14, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 5, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 6, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 7, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 8, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 9, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 5, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 9, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "G", SeatNumber = 5, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "G", SeatNumber = 9, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 5, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 6, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 7, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 8, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 9, TheaterId = 16, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 0, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 1, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 2, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 3, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 4, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 5, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 6, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 7, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 8, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 9, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 10, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 11, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 12, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 0, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 1, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 2, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 3, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 4, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 5, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 6, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 7, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 8, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 9, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 10, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 11, TheaterId = 22, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 12, TheaterId = 22, Type = SeatType.Missing }
                );
            }

            // Seed Shows
            if (!_context.Shows.Any())
            {
                _context.Shows.AddRange(
                    new Show { Id = 23, StartTime = TimeSpan.Parse("10:00:00"), EndTime = TimeSpan.Parse("12:15:00"), Date = new DateTime(2021, 6, 5), MovieId = 4, TheaterId = 16, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 24, StartTime = TimeSpan.Parse("12:30:00"), EndTime = TimeSpan.Parse("14:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 4, TheaterId = 16, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 25, StartTime = TimeSpan.Parse("18:30:00"), EndTime = TimeSpan.Parse("20:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 4, TheaterId = 15, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { Id = 26, StartTime = TimeSpan.Parse("21:30:00"), EndTime = TimeSpan.Parse("23:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 4, TheaterId = 15, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 27, StartTime = TimeSpan.Parse("21:30:00"), EndTime = TimeSpan.Parse("23:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 4, TheaterId = 15, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 28, StartTime = TimeSpan.Parse("18:30:00"), EndTime = TimeSpan.Parse("20:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 4, TheaterId = 22, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { Id = 29, StartTime = TimeSpan.Parse("19:30:00"), EndTime = TimeSpan.Parse("21:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 4, TheaterId = 16, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { Id = 30, StartTime = TimeSpan.Parse("12:30:00"), EndTime = TimeSpan.Parse("14:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 4, TheaterId = 16, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { Id = 31, StartTime = TimeSpan.Parse("18:30:00"), EndTime = TimeSpan.Parse("20:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 4, TheaterId = 16, Status = ShowStatus.Full, Type = ShowType.ThreeD },
                    new Show { Id = 32, StartTime = TimeSpan.Parse("18:30:00"), EndTime = TimeSpan.Parse("20:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 4, TheaterId = 15, Status = ShowStatus.Full, Type = ShowType.ThreeD },
                    new Show { Id = 33, StartTime = TimeSpan.Parse("19:30:00"), EndTime = TimeSpan.Parse("21:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 4, TheaterId = 22, Status = ShowStatus.Full, Type = ShowType.ThreeD },
                    new Show { Id = 34, StartTime = TimeSpan.Parse("21:30:00"), EndTime = TimeSpan.Parse("23:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 4, TheaterId = 16, Status = ShowStatus.Full, Type = ShowType.ThreeD },
                    new Show { Id = 35, StartTime = TimeSpan.Parse("12:30:00"), EndTime = TimeSpan.Parse("14:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 34, TheaterId = 15, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 36, StartTime = TimeSpan.Parse("13:30:00"), EndTime = TimeSpan.Parse("15:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 34, TheaterId = 22, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { Id = 37, StartTime = TimeSpan.Parse("16:30:00"), EndTime = TimeSpan.Parse("18:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 34, TheaterId = 16, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 38, StartTime = TimeSpan.Parse("16:30:00"), EndTime = TimeSpan.Parse("18:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 34, TheaterId = 16, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 39, StartTime = TimeSpan.Parse("18:30:00"), EndTime = TimeSpan.Parse("20:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 34, TheaterId = 15, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { Id = 40, StartTime = TimeSpan.Parse("21:30:00"), EndTime = TimeSpan.Parse("23:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 34, TheaterId = 22, Status = ShowStatus.Full, Type = ShowType.ThreeD },
                    new Show { Id = 41, StartTime = TimeSpan.Parse("21:30:00"), EndTime = TimeSpan.Parse("23:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 34, TheaterId = 15, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 42, StartTime = TimeSpan.Parse("15:30:00"), EndTime = TimeSpan.Parse("17:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 34, TheaterId = 15, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 43, StartTime = TimeSpan.Parse("10:30:00"), EndTime = TimeSpan.Parse("12:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 34, TheaterId = 15, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 44, StartTime = TimeSpan.Parse("10:30:00"), EndTime = TimeSpan.Parse("12:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 35, TheaterId = 22, Status = ShowStatus.Free, Type = ShowType.TwoD },
                    new Show { Id = 45, StartTime = TimeSpan.Parse("13:30:00"), EndTime = TimeSpan.Parse("15:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 35, TheaterId = 22, Status = ShowStatus.Free, Type = ShowType.TwoD },
                    new Show { Id = 46, StartTime = TimeSpan.Parse("15:30:00"), EndTime = TimeSpan.Parse("17:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 35, TheaterId = 16, Status = ShowStatus.Full, Type = ShowType.TwoD },
                    new Show { Id = 47, StartTime = TimeSpan.Parse("16:30:00"), EndTime = TimeSpan.Parse("18:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 35, TheaterId = 22, Status = ShowStatus.Free, Type = ShowType.TwoD },
                    new Show { Id = 48, StartTime = TimeSpan.Parse("13:30:00"), EndTime = TimeSpan.Parse("15:45:00"), Date = new DateTime(2021, 6, 8), MovieId = 35, TheaterId = 22, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { Id = 49, StartTime = TimeSpan.Parse("16:00:00"), EndTime = TimeSpan.Parse("18:15:00"), Date = new DateTime(2021, 6, 8), MovieId = 35, TheaterId = 16, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { Id = 50, StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("20:15:00"), Date = new DateTime(2021, 6, 8), MovieId = 35, TheaterId = 15, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { Id = 51, StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("20:15:00"), Date = new DateTime(2021, 6, 8), MovieId = 35, TheaterId = 22, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { Id = 52, StartTime = TimeSpan.Parse("21:00:00"), EndTime = TimeSpan.Parse("23:15:00"), Date = new DateTime(2021, 6, 8), MovieId = 35, TheaterId = 15, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { Id = 53, StartTime = TimeSpan.Parse("21:00:00"), EndTime = TimeSpan.Parse("23:15:00"), Date = new DateTime(2021, 6, 8), MovieId = 35, TheaterId = 16, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { Id = 54, StartTime = TimeSpan.Parse("09:00:00"), EndTime = TimeSpan.Parse("11:15:00"), Date = new DateTime(2021, 6, 6), MovieId = 35, TheaterId = 22, Status = ShowStatus.Free, Type = ShowType.TwoD },
                    new Show { Id = 55, StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("19:30:00"), Date = new DateTime(2021, 6, 10), MovieId = 36, TheaterId = 22, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 56, StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("19:30:00"), Date = new DateTime(2021, 6, 10), MovieId = 36, TheaterId = 16, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 57, StartTime = TimeSpan.Parse("19:00:00"), EndTime = TimeSpan.Parse("20:30:00"), Date = new DateTime(2021, 6, 10), MovieId = 36, TheaterId = 15, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 58, StartTime = TimeSpan.Parse("22:00:00"), EndTime = TimeSpan.Parse("23:30:00"), Date = new DateTime(2021, 6, 10), MovieId = 36, TheaterId = 15, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { Id = 59, StartTime = TimeSpan.Parse("21:00:00"), EndTime = TimeSpan.Parse("22:30:00"), Date = new DateTime(2021, 6, 10), MovieId = 36, TheaterId = 16, Status = ShowStatus.Free, Type = ShowType.ThreeD }
                );
            }

            // Seed Users
            if (!_context.Users.Any())
            {
                _context.Users.AddRange(
                    new User { Id = 1, FullName = "Test Admin", Email = "admin@gmail.com", Password = "$2a$08$y4hYovfviz31AZhru3t4FObYrLFrhdBNETXikx6WFD9VYGbUZucYO", Address = "Karachi", Contact = "+923009999999", Role = UserRole.Admin },
                    new User { Id = 2, FullName = "Test User", Email = "user@gmail.com", Password = "$2a$08$V5FWrXIoHeJPGe0WDgQcJ.eJEX.rz6tCtevaNSKBH6deaoVDKMosS", Address = "Karachi", Contact = "+923009999999", Role = UserRole.ApiUser },
                    new User { Id = 10, FullName = "Email Test User", Email = "arafaysaleem@gmail.com", Password = "$2a$08$36p1ozvvj/MEoHBBp5ZxY.f46YUg.wACpZ8W7/wA52qhAKvFUaxfu", Address = "Karachi", Contact = "+923009999999", Role = UserRole.ApiUser },
                    new User { Id = 11, FullName = "Test User", Email = "user2@gmail.com", Password = "$2a$08$dP89ppGNiMczZM0JToQwF.WtiR53NL0j.r55wBNJP03sjDh/VpcrC", Address = "Karachi", Contact = "+923009999999", Role = UserRole.ApiUser },
                    new User { Id = 14, FullName = "Test User 3", Email = "user3@gmail.com", Password = "$2a$08$hyIqip2MuwqbmkKIoiWG9eihI0HkAF0aB.3aWVUBO0YiBPIBawEDK", Address = "Karachi", Contact = "+923009999999", Role = UserRole.ApiUser },
                    new User { Id = 15, FullName = "Delete Me", Email = "deleteme@eztickets.com", Password = "$2a$08$lWgRO3U/XnZ3Etw0HQE4cOgCXsb/UDu3tYcRx.NzoIb6y96Cwa0ea", Address = "Re", Contact = "+923009268622", Role = UserRole.ApiUser }
                );
            }

            // Seed OtpCodes
            if (!_context.OtpCodes.Any())
            {
                _context.OtpCodes.AddRange(
                    new OtpCode { UserId = 10, Email = "arafaysaleem@gmail.com", OTP = "$2a$08$qEG2K154Ki4xZnxpVZ2znu3r.1rKutIRG57P4fBbxpvSBZXYSikM6", ExpirationDateTime = new DateTime(2022, 1, 12, 15, 22, 30) }
                );
            }

            // Seed Bookings
            if (!_context.Bookings.Any())
            {
                _context.Bookings.AddRange(
                    new Booking { Id = 25, UserId = 10, ShowId = 29, SeatRow = "E", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:29") },
                    new Booking { Id = 26, UserId = 10, ShowId = 29, SeatRow = "E", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:31") },
                    new Booking { Id = 27, UserId = 10, ShowId = 29, SeatRow = "E", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:33") },
                    new Booking { Id = 28, UserId = 10, ShowId = 29, SeatRow = "E", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:35") },
                    new Booking { Id = 29, UserId = 10, ShowId = 29, SeatRow = "E", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:37") },
                    new Booking { Id = 30, UserId = 10, ShowId = 29, SeatRow = "F", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:38") },
                    new Booking { Id = 31, UserId = 10, ShowId = 29, SeatRow = "F", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:40") },
                    new Booking { Id = 32, UserId = 10, ShowId = 29, SeatRow = "F", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:42") },
                    new Booking { Id = 33, UserId = 10, ShowId = 29, SeatRow = "G", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:43") },
                    new Booking { Id = 34, UserId = 10, ShowId = 29, SeatRow = "H", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:45") },
                    new Booking { Id = 35, UserId = 10, ShowId = 29, SeatRow = "H", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:47") },
                    new Booking { Id = 36, UserId = 10, ShowId = 29, SeatRow = "H", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:48") },
                    new Booking { Id = 37, UserId = 10, ShowId = 29, SeatRow = "H", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:50") },
                    new Booking { Id = 38, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:52") },
                    new Booking { Id = 39, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:53") },
                    new Booking { Id = 40, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:55") },
                    new Booking { Id = 41, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:57") },
                    new Booking { Id = 42, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:58") },
                    new Booking { Id = 43, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:00") },
                    new Booking { Id = 44, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:02") },
                    new Booking { Id = 45, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:03") },
                    new Booking { Id = 46, UserId = 10, ShowId = 29, SeatRow = "J", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:05") },
                    new Booking { Id = 47, UserId = 10, ShowId = 29, SeatRow = "J", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:06") },
                    new Booking { Id = 48, UserId = 10, ShowId = 29, SeatRow = "B", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:08") },
                    new Booking { Id = 49, UserId = 10, ShowId = 29, SeatRow = "B", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:09") },
                    new Booking { Id = 50, UserId = 10, ShowId = 29, SeatRow = "C", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:11") },
                    new Booking { Id = 51, UserId = 10, ShowId = 29, SeatRow = "C", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:13") },
                    new Booking { Id = 52, UserId = 10, ShowId = 29, SeatRow = "C", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:14") },
                    new Booking { Id = 53, UserId = 10, ShowId = 29, SeatRow = "B", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:16") },
                    new Booking { Id = 54, UserId = 10, ShowId = 29, SeatRow = "I", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:17") },
                    new Booking { Id = 55, UserId = 10, ShowId = 29, SeatRow = "I", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:19") },
                    new Booking { Id = 56, UserId = 10, ShowId = 29, SeatRow = "I", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:20") },
                    new Booking { Id = 57, UserId = 10, ShowId = 29, SeatRow = "J", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:22") },
                    new Booking { Id = 58, UserId = 10, ShowId = 29, SeatRow = "J", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:24") },
                    new Booking { Id = 59, UserId = 10, ShowId = 29, SeatRow = "J", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:25") },
                    new Booking { Id = 60, UserId = 10, ShowId = 29, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:27") },
                    new Booking { Id = 61, UserId = 10, ShowId = 29, SeatRow = "J", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:28") },
                    new Booking { Id = 62, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:30") },
                    new Booking { Id = 63, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:32") },
                    new Booking { Id = 64, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:34") },
                    new Booking { Id = 65, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:35") },
                    new Booking { Id = 66, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:37") },
                    new Booking { Id = 67, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:39") },
                    new Booking { Id = 68, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:40") },
                    new Booking { Id = 69, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:42") },
                    new Booking { Id = 70, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:44") },
                    new Booking { Id = 71, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:45") },
                    new Booking { Id = 72, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:47") },
                    new Booking { Id = 73, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:49") },
                    new Booking { Id = 74, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:50") },
                    new Booking { Id = 75, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:52") },
                    new Booking { Id = 76, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:53") },
                    new Booking { Id = 77, UserId = 10, ShowId = 29, SeatRow = "J", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:55") },
                    new Booking { Id = 78, UserId = 10, ShowId = 29, SeatRow = "J", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:57") },
                    new Booking { Id = 79, UserId = 10, ShowId = 29, SeatRow = "J", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:58") },
                    new Booking { Id = 80, UserId = 10, ShowId = 29, SeatRow = "J", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:00") },
                    new Booking { Id = 81, UserId = 10, ShowId = 29, SeatRow = "L", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:01") },
                    new Booking { Id = 82, UserId = 10, ShowId = 29, SeatRow = "H", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:03") },
                    new Booking { Id = 83, UserId = 10, ShowId = 29, SeatRow = "H", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:05") },
                    new Booking { Id = 84, UserId = 10, ShowId = 29, SeatRow = "H", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:06") },
                    new Booking { Id = 85, UserId = 10, ShowId = 29, SeatRow = "H", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:08") },
                    new Booking { Id = 86, UserId = 10, ShowId = 29, SeatRow = "H", SeatNumber = 10, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:09") },
                    new Booking { Id = 87, UserId = 10, ShowId = 29, SeatRow = "G", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:11") },
                    new Booking { Id = 88, UserId = 10, ShowId = 29, SeatRow = "G", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:13") },
                    new Booking { Id = 89, UserId = 10, ShowId = 29, SeatRow = "G", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:14") },
                    new Booking { Id = 90, UserId = 10, ShowId = 29, SeatRow = "G", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:16") },
                    new Booking { Id = 91, UserId = 10, ShowId = 29, SeatRow = "G", SeatNumber = 10, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:17") },
                    new Booking { Id = 92, UserId = 10, ShowId = 29, SeatRow = "F", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:19") },
                    new Booking { Id = 93, UserId = 10, ShowId = 29, SeatRow = "F", SeatNumber = 10, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:21") },
                    new Booking { Id = 94, UserId = 10, ShowId = 29, SeatRow = "E", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:22") },
                    new Booking { Id = 95, UserId = 10, ShowId = 29, SeatRow = "E", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:24") },
                    new Booking { Id = 96, UserId = 10, ShowId = 29, SeatRow = "B", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:25") },
                    new Booking { Id = 97, UserId = 10, ShowId = 29, SeatRow = "B", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:27") },
                    new Booking { Id = 98, UserId = 10, ShowId = 29, SeatRow = "B", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:29") },
                    new Booking { Id = 99, UserId = 10, ShowId = 29, SeatRow = "C", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:30") },
                    new Booking { Id = 100, UserId = 10, ShowId = 29, SeatRow = "C", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:32") },
                    new Booking { Id = 101, UserId = 10, ShowId = 29, SeatRow = "C", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:33") },
                    new Booking { Id = 102, UserId = 10, ShowId = 29, SeatRow = "D", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:35") },
                    new Booking { Id = 103, UserId = 10, ShowId = 29, SeatRow = "D", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:37") },
                    new Booking { Id = 104, UserId = 10, ShowId = 29, SeatRow = "D", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:38") },
                    new Booking { Id = 105, UserId = 10, ShowId = 29, SeatRow = "D", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:40") },
                    new Booking { Id = 106, UserId = 10, ShowId = 29, SeatRow = "D", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:41") },
                    new Booking { Id = 107, UserId = 10, ShowId = 29, SeatRow = "B", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:43") },
                    new Booking { Id = 108, UserId = 10, ShowId = 29, SeatRow = "G", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:44") },
                    new Booking { Id = 109, UserId = 10, ShowId = 29, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:46") },
                    new Booking { Id = 110, UserId = 10, ShowId = 35, SeatRow = "J", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:08") },
                    new Booking { Id = 111, UserId = 10, ShowId = 35, SeatRow = "J", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:10") },
                    new Booking { Id = 112, UserId = 10, ShowId = 35, SeatRow = "J", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:12") },
                    new Booking { Id = 113, UserId = 10, ShowId = 35, SeatRow = "J", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:14") },
                    new Booking { Id = 114, UserId = 10, ShowId = 35, SeatRow = "J", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:16") },
                    new Booking { Id = 115, UserId = 10, ShowId = 35, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:18") },
                    new Booking { Id = 116, UserId = 10, ShowId = 35, SeatRow = "J", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:19") },
                    new Booking { Id = 117, UserId = 10, ShowId = 35, SeatRow = "J", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:21") },
                    new Booking { Id = 118, UserId = 10, ShowId = 35, SeatRow = "I", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:23") },
                    new Booking { Id = 119, UserId = 10, ShowId = 35, SeatRow = "I", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:24") },
                    new Booking { Id = 120, UserId = 10, ShowId = 35, SeatRow = "I", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:26") },
                    new Booking { Id = 121, UserId = 10, ShowId = 35, SeatRow = "I", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:27") },
                    new Booking { Id = 122, UserId = 10, ShowId = 35, SeatRow = "I", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:29") },
                    new Booking { Id = 123, UserId = 10, ShowId = 35, SeatRow = "I", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:31") },
                    new Booking { Id = 124, UserId = 10, ShowId = 35, SeatRow = "I", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:32") },
                    new Booking { Id = 125, UserId = 10, ShowId = 35, SeatRow = "I", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:34") },
                    new Booking { Id = 126, UserId = 10, ShowId = 35, SeatRow = "F", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:35") },
                    new Booking { Id = 127, UserId = 10, ShowId = 35, SeatRow = "F", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:37") },
                    new Booking { Id = 128, UserId = 10, ShowId = 35, SeatRow = "G", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:39") },
                    new Booking { Id = 129, UserId = 10, ShowId = 35, SeatRow = "G", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:40") },
                    new Booking { Id = 130, UserId = 10, ShowId = 35, SeatRow = "G", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:42") },
                    new Booking { Id = 131, UserId = 10, ShowId = 54, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:41:58") },
                    new Booking { Id = 132, UserId = 10, ShowId = 54, SeatRow = "H", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:00") },
                    new Booking { Id = 133, UserId = 10, ShowId = 54, SeatRow = "G", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:02") },
                    new Booking { Id = 134, UserId = 10, ShowId = 54, SeatRow = "H", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:03") },
                    new Booking { Id = 135, UserId = 10, ShowId = 54, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:05") },
                    new Booking { Id = 136, UserId = 10, ShowId = 54, SeatRow = "H", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:07") },
                    new Booking { Id = 137, UserId = 10, ShowId = 54, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:08") },
                    new Booking { Id = 138, UserId = 10, ShowId = 54, SeatRow = "H", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:10") },
                    new Booking { Id = 139, UserId = 10, ShowId = 54, SeatRow = "D", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:11") },
                    new Booking { Id = 140, UserId = 10, ShowId = 54, SeatRow = "E", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:13") },
                    new Booking { Id = 141, UserId = 10, ShowId = 54, SeatRow = "D", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:14") },
                    new Booking { Id = 142, UserId = 10, ShowId = 54, SeatRow = "E", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:16") },
                    new Booking { Id = 143, UserId = 10, ShowId = 54, SeatRow = "B", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:18") },
                    new Booking { Id = 144, UserId = 10, ShowId = 54, SeatRow = "B", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:19") },
                    new Booking { Id = 145, UserId = 10, ShowId = 54, SeatRow = "B", SeatNumber = 10, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:21") },
                    new Booking { Id = 146, UserId = 10, ShowId = 54, SeatRow = "E", SeatNumber = 10, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:23") },
                    new Booking { Id = 147, UserId = 10, ShowId = 54, SeatRow = "E", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:24") },
                    new Booking { Id = 148, UserId = 10, ShowId = 29, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 21:24:31") },
                    new Booking { Id = 149, UserId = 10, ShowId = 29, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 21:24:35") },
                    new Booking { Id = 150, UserId = 10, ShowId = 29, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 21:24:37") },
                    new Booking { Id = 151, UserId = 10, ShowId = 39, SeatRow = "H", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:19") },
                    new Booking { Id = 152, UserId = 10, ShowId = 39, SeatRow = "H", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:21") },
                    new Booking { Id = 153, UserId = 10, ShowId = 39, SeatRow = "H", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:22") },
                    new Booking { Id = 154, UserId = 10, ShowId = 39, SeatRow = "H", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:24") },
                    new Booking { Id = 155, UserId = 10, ShowId = 39, SeatRow = "H", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:25") },
                    new Booking { Id = 156, UserId = 10, ShowId = 39, SeatRow = "H", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:27") },
                    new Booking { Id = 157, UserId = 10, ShowId = 30, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-06-10 04:10:13") },
                    new Booking { Id = 158, UserId = 10, ShowId = 30, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-06-10 04:10:15") },
                    new Booking { Id = 159, UserId = 10, ShowId = 30, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-06-10 04:10:17") },
                    new Booking { Id = 160, UserId = 10, ShowId = 30, SeatRow = "L", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-06-10 04:10:18") },
                    new Booking { Id = 161, UserId = 10, ShowId = 30, SeatRow = "L", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-06-10 04:10:20") },
                    new Booking { Id = 164, UserId = 2, ShowId = 41, SeatRow = "E", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:43") },
                    new Booking { Id = 165, UserId = 2, ShowId = 41, SeatRow = "F", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:45") },
                    new Booking { Id = 166, UserId = 2, ShowId = 41, SeatRow = "G", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:47") },
                    new Booking { Id = 167, UserId = 2, ShowId = 41, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:49") },
                    new Booking { Id = 168, UserId = 2, ShowId = 41, SeatRow = "J", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:50") },
                    new Booking { Id = 169, UserId = 2, ShowId = 41, SeatRow = "J", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:52") },
                    new Booking { Id = 170, UserId = 10, ShowId = 43, SeatRow = "A", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-28 17:59:59") },
                    new Booking { Id = 171, UserId = 10, ShowId = 43, SeatRow = "E", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-28 18:00:01") },
                    new Booking { Id = 172, UserId = 10, ShowId = 43, SeatRow = "F", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-28 18:00:03") },
                    new Booking { Id = 173, UserId = 10, ShowId = 43, SeatRow = "H", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-28 18:00:04") },
                    new Booking { Id = 174, UserId = 10, ShowId = 30, SeatRow = "G", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:45") },
                    new Booking { Id = 175, UserId = 10, ShowId = 30, SeatRow = "H", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:47") },
                    new Booking { Id = 176, UserId = 10, ShowId = 30, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:49") },
                    new Booking { Id = 177, UserId = 10, ShowId = 30, SeatRow = "G", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:51") },
                    new Booking { Id = 178, UserId = 10, ShowId = 30, SeatRow = "H", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:52") },
                    new Booking { Id = 179, UserId = 10, ShowId = 30, SeatRow = "H", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:53") },
                    new Booking { Id = 180, UserId = 10, ShowId = 30, SeatRow = "I", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-08-08 03:35:41") },
                    new Booking { Id = 181, UserId = 10, ShowId = 23, SeatRow = "J", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-08-17 19:47:19") },
                    new Booking { Id = 184, UserId = 10, ShowId = 23, SeatRow = "I", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-08-18 10:46:44") },
                    new Booking { Id = 185, UserId = 10, ShowId = 23, SeatRow = "I", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-08-18 10:47:03") },
                    new Booking { Id = 186, UserId = 10, ShowId = 27, SeatRow = "E", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-14 00:24:41") },
                    new Booking { Id = 187, UserId = 10, ShowId = 27, SeatRow = "F", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-14 00:24:44") },
                    new Booking { Id = 188, UserId = 10, ShowId = 27, SeatRow = "G", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-14 00:24:45") },
                    new Booking { Id = 189, UserId = 10, ShowId = 26, SeatRow = "I", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:43:46") },
                    new Booking { Id = 190, UserId = 10, ShowId = 26, SeatRow = "I", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:43:49") },
                    new Booking { Id = 191, UserId = 10, ShowId = 26, SeatRow = "I", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:43:52") },
                    new Booking { Id = 192, UserId = 10, ShowId = 26, SeatRow = "I", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:43:53") },
                    new Booking { Id = 193, UserId = 10, ShowId = 26, SeatRow = "I", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:43:56") },
                    new Booking { Id = 194, UserId = 10, ShowId = 26, SeatRow = "I", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:02") },
                    new Booking { Id = 195, UserId = 10, ShowId = 26, SeatRow = "H", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:04") },
                    new Booking { Id = 196, UserId = 10, ShowId = 26, SeatRow = "H", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:05") },
                    new Booking { Id = 197, UserId = 10, ShowId = 26, SeatRow = "H", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:07") },
                    new Booking { Id = 198, UserId = 10, ShowId = 26, SeatRow = "G", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:10") },
                    new Booking { Id = 199, UserId = 10, ShowId = 26, SeatRow = "G", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:11") },
                    new Booking { Id = 200, UserId = 10, ShowId = 26, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:13") },
                    new Booking { Id = 201, UserId = 10, ShowId = 26, SeatRow = "G", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:14") },
                    new Booking { Id = 202, UserId = 10, ShowId = 26, SeatRow = "G", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:16") },
                    new Booking { Id = 203, UserId = 10, ShowId = 26, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:17") },
                    new Booking { Id = 204, UserId = 10, ShowId = 26, SeatRow = "E", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:20") },
                    new Booking { Id = 205, UserId = 10, ShowId = 26, SeatRow = "D", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:22") },
                    new Booking { Id = 206, UserId = 10, ShowId = 26, SeatRow = "D", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:24") },
                    new Booking { Id = 207, UserId = 10, ShowId = 26, SeatRow = "D", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:26") },
                    new Booking { Id = 208, UserId = 10, ShowId = 26, SeatRow = "E", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:27") },
                    new Booking { Id = 209, UserId = 10, ShowId = 26, SeatRow = "E", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:28") },
                    new Booking { Id = 210, UserId = 10, ShowId = 26, SeatRow = "F", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:30") },
                    new Booking { Id = 211, UserId = 10, ShowId = 26, SeatRow = "F", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:32") },
                    new Booking { Id = 212, UserId = 10, ShowId = 26, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:33") },
                    new Booking { Id = 213, UserId = 10, ShowId = 26, SeatRow = "G", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:36") },
                    new Booking { Id = 214, UserId = 10, ShowId = 26, SeatRow = "J", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:38") },
                    new Booking { Id = 215, UserId = 10, ShowId = 26, SeatRow = "J", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:40") },
                    new Booking { Id = 216, UserId = 10, ShowId = 26, SeatRow = "E", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:41") },
                    new Booking { Id = 217, UserId = 10, ShowId = 26, SeatRow = "E", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:43") },
                    new Booking { Id = 218, UserId = 10, ShowId = 26, SeatRow = "C", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:44") },
                    new Booking { Id = 219, UserId = 10, ShowId = 26, SeatRow = "C", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:46") },
                    new Booking { Id = 220, UserId = 10, ShowId = 23, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-23 16:01:30") },
                    new Booking { Id = 221, UserId = 10, ShowId = 23, SeatRow = "K", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-23 16:01:32") },
                    new Booking { Id = 222, UserId = 10, ShowId = 23, SeatRow = "K", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-23 16:01:33") },
                    new Booking { Id = 223, UserId = 10, ShowId = 30, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-10-14 16:08:49") },
                    new Booking { Id = 224, UserId = 10, ShowId = 30, SeatRow = "J", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-10-14 16:08:52") },
                    new Booking { Id = 225, UserId = 10, ShowId = 30, SeatRow = "J", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-10-14 16:08:53") },
                    new Booking { Id = 226, UserId = 10, ShowId = 30, SeatRow = "J", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-10-14 16:08:55") },
                    new Booking { Id = 227, UserId = 10, ShowId = 23, SeatRow = "J", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-11-20 02:31:31") },
                    new Booking { Id = 228, UserId = 10, ShowId = 23, SeatRow = "J", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-11-20 02:31:33") },
                    new Booking { Id = 229, UserId = 10, ShowId = 23, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-11-20 02:31:34") },
                    new Booking { Id = 230, UserId = 10, ShowId = 23, SeatRow = "J", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-11-20 02:31:36") },
                    new Booking { Id = 231, UserId = 10, ShowId = 26, SeatRow = "J", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-12 19:26:39") },
                    new Booking { Id = 232, UserId = 10, ShowId = 26, SeatRow = "J", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-12 19:26:42") },
                    new Booking { Id = 233, UserId = 10, ShowId = 26, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-12 19:26:50") },
                    new Booking { Id = 234, UserId = 10, ShowId = 26, SeatRow = "H", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-12 19:26:53") },
                    new Booking { Id = 235, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-14 19:05:08") },
                    new Booking { Id = 236, UserId = 10, ShowId = 29, SeatRow = "K", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-14 19:05:11") },
                    new Booking { Id = 237, UserId = 10, ShowId = 29, SeatRow = "I", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-14 19:05:16") },
                    new Booking { Id = 238, UserId = 10, ShowId = 29, SeatRow = "I", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-14 19:05:19") },
                    new Booking { Id = 239, UserId = 10, ShowId = 27, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-15 05:49:12") },
                    new Booking { Id = 240, UserId = 10, ShowId = 24, SeatRow = "F", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:49:31") },
                    new Booking { Id = 241, UserId = 10, ShowId = 24, SeatRow = "F", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:49:33") },
                    new Booking { Id = 242, UserId = 10, ShowId = 24, SeatRow = "F", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:49:34") },
                    new Booking { Id = 243, UserId = 10, ShowId = 24, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:49:36") },
                    new Booking { Id = 244, UserId = 10, ShowId = 24, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:49:38") },
                    new Booking { Id = 245, UserId = 10, ShowId = 24, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-01-17 14:49:40") },
                    new Booking { Id = 246, UserId = 10, ShowId = 49, SeatRow = "A", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:51:09") },
                    new Booking { Id = 247, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 0, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:17") },
                    new Booking { Id = 248, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 1, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:18") },
                    new Booking { Id = 249, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:20") },
                    new Booking { Id = 250, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:22") },
                    new Booking { Id = 251, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:23") },
                    new Booking { Id = 252, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:25") },
                    new Booking { Id = 253, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:27") },
                    new Booking { Id = 254, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:29") },
                    new Booking { Id = 255, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:30") },
                    new Booking { Id = 256, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:32") },
                    new Booking { Id = 257, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:34") },
                    new Booking { Id = 258, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:36") },
                    new Booking { Id = 259, UserId = 10, ShowId = 36, SeatRow = "A", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:37") },
                    new Booking { Id = 260, UserId = 10, ShowId = 36, SeatRow = "B", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:39") },
                    new Booking { Id = 261, UserId = 10, ShowId = 36, SeatRow = "B", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:41") },
                    new Booking { Id = 262, UserId = 10, ShowId = 36, SeatRow = "B", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:42") },
                    new Booking { Id = 263, UserId = 10, ShowId = 36, SeatRow = "B", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:44") },
                    new Booking { Id = 264, UserId = 10, ShowId = 36, SeatRow = "B", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:46") },
                    new Booking { Id = 265, UserId = 10, ShowId = 36, SeatRow = "B", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:48") },
                    new Booking { Id = 266, UserId = 10, ShowId = 36, SeatRow = "B", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:49") },
                    new Booking { Id = 267, UserId = 10, ShowId = 36, SeatRow = "B", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:51") },
                    new Booking { Id = 268, UserId = 10, ShowId = 36, SeatRow = "B", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:52") },
                    new Booking { Id = 269, UserId = 10, ShowId = 36, SeatRow = "B", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:54") },
                    new Booking { Id = 270, UserId = 10, ShowId = 36, SeatRow = "B", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:56") },
                    new Booking { Id = 271, UserId = 10, ShowId = 36, SeatRow = "E", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:57") },
                    new Booking { Id = 272, UserId = 10, ShowId = 36, SeatRow = "D", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:59") },
                    new Booking { Id = 273, UserId = 10, ShowId = 36, SeatRow = "D", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:01") },
                    new Booking { Id = 274, UserId = 10, ShowId = 36, SeatRow = "E", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:02") },
                    new Booking { Id = 275, UserId = 10, ShowId = 36, SeatRow = "D", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:04") },
                    new Booking { Id = 276, UserId = 10, ShowId = 36, SeatRow = "E", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:06") },
                    new Booking { Id = 277, UserId = 10, ShowId = 36, SeatRow = "D", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:07") },
                    new Booking { Id = 278, UserId = 10, ShowId = 36, SeatRow = "E", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:09") },
                    new Booking { Id = 279, UserId = 10, ShowId = 36, SeatRow = "D", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:11") },
                    new Booking { Id = 280, UserId = 10, ShowId = 36, SeatRow = "E", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:13") },
                    new Booking { Id = 281, UserId = 10, ShowId = 36, SeatRow = "E", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:14") },
                    new Booking { Id = 282, UserId = 10, ShowId = 36, SeatRow = "D", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:16") },
                    new Booking { Id = 283, UserId = 10, ShowId = 36, SeatRow = "E", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:18") },
                    new Booking { Id = 284, UserId = 10, ShowId = 36, SeatRow = "D", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:20") },
                    new Booking { Id = 285, UserId = 10, ShowId = 36, SeatRow = "D", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:21") },
                    new Booking { Id = 286, UserId = 10, ShowId = 36, SeatRow = "E", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:23") },
                    new Booking { Id = 287, UserId = 10, ShowId = 36, SeatRow = "D", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:25") },
                    new Booking { Id = 288, UserId = 10, ShowId = 36, SeatRow = "E", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:27") },
                    new Booking { Id = 289, UserId = 10, ShowId = 36, SeatRow = "D", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:28") },
                    new Booking { Id = 290, UserId = 10, ShowId = 36, SeatRow = "E", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:30") },
                    new Booking { Id = 291, UserId = 10, ShowId = 36, SeatRow = "D", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:32") },
                    new Booking { Id = 292, UserId = 10, ShowId = 36, SeatRow = "E", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:33") },
                    new Booking { Id = 293, UserId = 10, ShowId = 36, SeatRow = "H", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:35") },
                    new Booking { Id = 294, UserId = 10, ShowId = 36, SeatRow = "G", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:37") },
                    new Booking { Id = 295, UserId = 10, ShowId = 36, SeatRow = "G", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:38") },
                    new Booking { Id = 296, UserId = 10, ShowId = 36, SeatRow = "H", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:40") },
                    new Booking { Id = 297, UserId = 10, ShowId = 36, SeatRow = "G", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:42") },
                    new Booking { Id = 298, UserId = 10, ShowId = 36, SeatRow = "H", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:44") },
                    new Booking { Id = 299, UserId = 10, ShowId = 36, SeatRow = "H", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:45") },
                    new Booking { Id = 300, UserId = 10, ShowId = 36, SeatRow = "G", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:47") },
                    new Booking { Id = 301, UserId = 10, ShowId = 36, SeatRow = "H", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:49") },
                    new Booking { Id = 302, UserId = 10, ShowId = 36, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:50") },
                    new Booking { Id = 303, UserId = 10, ShowId = 36, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:52") },
                    new Booking { Id = 304, UserId = 10, ShowId = 36, SeatRow = "H", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:54") },
                    new Booking { Id = 305, UserId = 10, ShowId = 36, SeatRow = "H", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:55") },
                    new Booking { Id = 306, UserId = 10, ShowId = 36, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:57") },
                    new Booking { Id = 307, UserId = 10, ShowId = 36, SeatRow = "G", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:59") },
                    new Booking { Id = 308, UserId = 10, ShowId = 36, SeatRow = "H", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:00") },
                    new Booking { Id = 309, UserId = 10, ShowId = 36, SeatRow = "G", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:02") },
                    new Booking { Id = 310, UserId = 10, ShowId = 36, SeatRow = "H", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:04") },
                    new Booking { Id = 311, UserId = 10, ShowId = 36, SeatRow = "H", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:06") },
                    new Booking { Id = 312, UserId = 10, ShowId = 36, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:07") },
                    new Booking { Id = 313, UserId = 10, ShowId = 36, SeatRow = "G", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:09") },
                    new Booking { Id = 314, UserId = 10, ShowId = 36, SeatRow = "H", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:11") },
                    new Booking { Id = 315, UserId = 1, ShowId = 55, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-18 15:34:50") },
                    new Booking { Id = 316, UserId = 1, ShowId = 55, SeatRow = "D", SeatNumber = 0, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-18 15:34:52") }
                    );
                _context.SaveChanges();
            }
            // Seed Payments
            if (!_context.Payments.Any())
            {
                _context.Payments.AddRange(
                    new Payment { Id = 27, Amount = 68000, PaymentDateTime = DateTime.Parse("2021-06-06 03:33:47"), Method = PaymentMethod.Card, UserId = 10, ShowId = 29 },
                    new Payment { Id = 28, Amount = 16800, PaymentDateTime = DateTime.Parse("2021-06-06 03:36:43"), Method = PaymentMethod.Card, UserId = 10, ShowId = 35 },
                    new Payment { Id = 29, Amount = 13600, PaymentDateTime = DateTime.Parse("2021-06-06 03:42:26"), Method = PaymentMethod.Card, UserId = 10, ShowId = 54 },
                    new Payment { Id = 30, Amount = 2400, PaymentDateTime = DateTime.Parse("2021-06-06 21:24:38"), Method = PaymentMethod.Card, UserId = 10, ShowId = 29 },
                    new Payment { Id = 31, Amount = 4800, PaymentDateTime = DateTime.Parse("2021-06-06 22:43:28"), Method = PaymentMethod.Card, UserId = 10, ShowId = 39 },
                    new Payment { Id = 32, Amount = 4800, PaymentDateTime = DateTime.Parse("2021-06-10 19:00:53"), Method = PaymentMethod.Card, UserId = 2, ShowId = 41 },
                    new Payment { Id = 33, Amount = 3200, PaymentDateTime = DateTime.Parse("2021-06-28 18:00:06"), Method = PaymentMethod.Card, UserId = 10, ShowId = 43 },
                    new Payment { Id = 34, Amount = 4800, PaymentDateTime = DateTime.Parse("2021-07-16 00:44:55"), Method = PaymentMethod.Card, UserId = 10, ShowId = 30 },
                    new Payment { Id = 35, Amount = 800, PaymentDateTime = DateTime.Parse("2021-08-08 03:35:43"), Method = PaymentMethod.Card, UserId = 10, ShowId = 30 },
                    new Payment { Id = 36, Amount = 800, PaymentDateTime = DateTime.Parse("2021-08-17 19:47:21"), Method = PaymentMethod.Card, UserId = 10, ShowId = 23 },
                    new Payment { Id = 37, Amount = 1600, PaymentDateTime = DateTime.Parse("2021-08-18 10:47:04"), Method = PaymentMethod.Card, UserId = 10, ShowId = 23 },
                    new Payment { Id = 38, Amount = 2400, PaymentDateTime = DateTime.Parse("2021-09-14 00:24:47"), Method = PaymentMethod.Card, UserId = 10, ShowId = 27 },
                    new Payment { Id = 39, Amount = 24800, PaymentDateTime = DateTime.Parse("2021-09-17 00:44:47"), Method = PaymentMethod.Card, UserId = 10, ShowId = 26 },
                    new Payment { Id = 40, Amount = 2400, PaymentDateTime = DateTime.Parse("2021-09-23 16:01:34"), Method = PaymentMethod.Card, UserId = 10, ShowId = 23 },
                    new Payment { Id = 41, Amount = 3200, PaymentDateTime = DateTime.Parse("2021-10-14 16:08:56"), Method = PaymentMethod.Card, UserId = 10, ShowId = 30 },
                    new Payment { Id = 42, Amount = 3200, PaymentDateTime = DateTime.Parse("2021-11-20 02:31:38"), Method = PaymentMethod.Card, UserId = 10, ShowId = 23 },
                    new Payment { Id = 43, Amount = 3200, PaymentDateTime = DateTime.Parse("2022-01-12 19:26:56"), Method = PaymentMethod.Card, UserId = 10, ShowId = 26 },
                    new Payment { Id = 44, Amount = 3200, PaymentDateTime = DateTime.Parse("2022-01-14 19:05:21"), Method = PaymentMethod.Card, UserId = 10, ShowId = 29 }
                );
                _context.SaveChanges();
            }

        }
    }
}

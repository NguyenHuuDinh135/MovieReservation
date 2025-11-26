using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieReservation.Server.Infrastructure.Authorization;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace MovieReservation.Server.Infrastructure
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
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public MovieReservationDbContextInitialiser(ILogger<MovieReservationDbContextInitialiser> logger, MovieReservationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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
            // Định nghĩa GUID cố định cho từng user
            var user1Id = "daeffa56-9a2f-46e6-93fb-b63b50e00302";
            var user2Id = "2b6dfb56-8a4d-4df7-8b9f-b2d0a18bb58e";
            var user3Id = "72c94147-cb3a-4f80-b015-9acb882bc5c0";
            var user4Id = "ce129c6e-1990-4db8-97e2-e4daec6c8d10";
            var user5Id = "0f3a28a2-0d76-47d2-aee7-f607d83ec707";
            var user6Id = "b1220e65-7095-4d5a-b76d-8411f42a3bb7";
            var user7Id = "d1562b8b-92ad-4c92-b6c0-e4974acb942e";
            var user8Id = "2950b631-f5fd-4680-b911-657f8727f7b8";
            var user9Id = "6c4dc5b6-8b89-4c35-a19e-5b08c680a3dc";
            var user10Id = "abf85a44-d167-46f7-8d38-b5b3c52f9611";
            // Seed Identity Roles
            var roles = new[] { "Admin", "ApiUser" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Danh sách user với ID cố định
            var users = new[]
            {
                new { Id = user1Id, Email = "proladinh144@gmail.com", Password = "Admin@123", Address = "Karachi", Contact = "+923009999999", Role = "Admin" },
                new { Id = user2Id, Email = "dinhknd3@gmail.com", Password = "User@123", Address = "Karachi", Contact = "+923009999999", Role = "ApiUser" },
                new { Id = user3Id, Email = "qvinhkl10@gmail.com", Password = "User@123", Address = "Karachi", Contact = "+923009999999", Role = "Admin" },
                new { Id = user4Id, Email = "qvinhkl01@gmail.com", Password = "User@123", Address = "Karachi", Contact = "+923009999999", Role = "ApiUser" },
                new { Id = user5Id, Email = "user3@gmail.com", Password = "User@123", Address = "Karachi", Contact = "+923009999999", Role = "ApiUser" },
                new { Id = user6Id, Email = "deleteme@eztickets.com", Password = "User@123", Address = "Re", Contact = "+923009268622", Role = "ApiUser" },
                new { Id = user7Id, Email = "user4@gmail.com", Password = "User@123", Address = "Lahore", Contact = "+923001234567", Role = "ApiUser" },
                new { Id = user8Id, Email = "user5@gmail.com", Password = "User@123", Address = "Islamabad", Contact = "+923008765432", Role = "ApiUser" },
                new { Id = user9Id, Email = "user6@gmail.com", Password = "User@123", Address = "Peshawar", Contact = "+923007654321", Role = "ApiUser" },
                new { Id = user10Id, Email = "user7@gmail.com", Password = "User@123", Address = "Quetta", Contact = "+923006543210", Role = "ApiUser" }
            };

            // Seed Identity Users
            foreach (var user in users)
            {
                if (!_userManager.Users.Any(u => u.Id == user.Id))
                {
                    var identityUser = new User
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.Email,
                        Address = user.Address,
                        Contact = user.Contact,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(identityUser, user.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(identityUser, user.Role);
                    }
                }
            }

            // --- Seed permissions into AspNetRoleClaims and AspNetUserClaims ---
            // Lấy tất cả permissions từ PermissionConstants
            var allPermissions = PermissionConstants.Permissions.GetAll();

            // Gán TẤT CẢ permissions cho Admin role (full access)
            var adminRole = await _roleManager.FindByNameAsync("Admin");
            if (adminRole != null)
            {
                var adminRoleClaims = await _roleManager.GetClaimsAsync(adminRole);
                foreach (var permission in allPermissions)
                {
                    if (!adminRoleClaims.Any(c => c.Type == PermissionConstants.Permission && c.Value == permission))
                    {
                        await _roleManager.AddClaimAsync(adminRole, new Claim(PermissionConstants.Permission, permission));
                    }
                }
            }

            // Gán một số permissions cơ bản cho ApiUser role (read-only access)
            var apiUserRole = await _roleManager.FindByNameAsync("ApiUser");
            if (apiUserRole != null)
            {
                var apiUserRoleClaims = await _roleManager.GetClaimsAsync(apiUserRole);
                var apiUserPermissions = new[]
                {
                    PermissionConstants.Permissions.MoviesView,
                    PermissionConstants.Permissions.ShowsView,
                    PermissionConstants.Permissions.TheatersView,
                    PermissionConstants.Permissions.BookingsView,
                    PermissionConstants.Permissions.GenresView
                };

                foreach (var permission in apiUserPermissions)
                {
                    if (!apiUserRoleClaims.Any(c => c.Type == PermissionConstants.Permission && c.Value == permission))
                    {
                        await _roleManager.AddClaimAsync(apiUserRole, new Claim(PermissionConstants.Permission, permission));
                    }
                }
            }

            // Gán permission trực tiếp cho một user cụ thể (ví dụ: user2)
            // Đây là ví dụ về cách gán permission trực tiếp cho user (không thông qua role)
            var sampleUser = await _userManager.FindByIdAsync(user2Id);
            if (sampleUser != null)
            {
                var userClaims = await _userManager.GetClaimsAsync(sampleUser);
                var directPermission = PermissionConstants.Permissions.PaymentsView;
                if (!userClaims.Any(c => c.Type == PermissionConstants.Permission && c.Value == directPermission))
                {
                    await _userManager.AddClaimAsync(sampleUser, new Claim(PermissionConstants.Permission, directPermission));
                }
            }
            // --- end permissions seeding ---

            // Seed Genres
            if (!_context.Genres.Any())
            {
                _context.Genres.AddRange(
                    new Genre { Name = "Horror" },
                    new Genre { Name = "Action" },
                    new Genre { Name = "Fantasy" },
                    new Genre { Name = "Comedy" },
                    new Genre { Name = "Drama" },
                    new Genre { Name = "Thriller" },
                    new Genre { Name = "Mystery" },
                    new Genre { Name = "Romance" },
                    new Genre { Name = "Sci-Fi" },
                    new Genre { Name = "Crime" }
                );
                await _context.SaveChangesAsync();
            }
            // Seed Movies
            if (!_context.Movies.Any())
            {
                _context.Movies.AddRange(
                    new Movie
                    {
                        
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
                        
                        Title = "THE BATMAN",
                        Summary = "The Riddler plays a deadly game of cat and mouse with Batman and Commissioner Gordon in Gotham City.",
                        Year = 2022,
                        Rating = null,
                        TrailerUrl = "https://media.publit.io/file/h_720/batman.mp4",
                        PosterUrl = "https://www.inspirationde.com/media/2020/08/the-batman-poster-by-mizuriofficial-on-deviantart-15987584958gkn4.jpg",
                        MovieType = MovieType.ComingSoon
                    }
                );
                await _context.SaveChangesAsync();
            }

            // Seed MovieGenres
            if (!_context.MovieGenres.Any())
            {

                _context.MovieGenres.AddRange(
                    new MovieGenre { MovieId = 1, GenreId = 1 },
                    new MovieGenre { MovieId = 1, GenreId = 2 },
                    new MovieGenre { MovieId = 1, GenreId = 4 },
                    new MovieGenre { MovieId = 2, GenreId = 5 },
                    new MovieGenre { MovieId = 2, GenreId = 6 },
                    new MovieGenre { MovieId = 2, GenreId = 10 },
                    new MovieGenre { MovieId = 3, GenreId = 2 },
                    new MovieGenre { MovieId = 3, GenreId = 3 },
                    new MovieGenre { MovieId = 3, GenreId = 4 },
                    new MovieGenre { MovieId = 4, GenreId = 2 },
                    new MovieGenre { MovieId = 4, GenreId = 5 },
                    new MovieGenre { MovieId = 4, GenreId = 10 }
                );
                await _context.SaveChangesAsync();
            }

            // Seed Roles
            if (!_context.Roles.Any())
            {
                _context.Roles.AddRange(
                    new Role { FullName = "Robert Pattinson", Age = 35, PictureUrl = "https://i.pinimg.com/564x/8d/e3/89/8de389c84e919d3577f47118e2627d95.jpg" },
                    new Role { FullName = "Zoë Kravitz", Age = 32, PictureUrl = "https://www.simplyceleb.com/wp-content/uploads/2020/06/Zoe-Kravitz-Filmleri.jpg" },
                    new Role { FullName = "Paul Dano", Age = 36, PictureUrl = "https://resizing.flixster.com/1PQhbMray969ia5n7JHLBO3qATA=/506x652/v2/https://flxt.tmsimg.com/v9/AllPhotos/267214/267214_v9_ba.jpg" },
                    new Role { FullName = "Mat Reeves", Age = 55, PictureUrl = "https://i.dailymail.co.uk/1s/2020/04/10/05/27028708-0-image-a-2_1586494056548.jpg" },
                    new Role { FullName = "Margot Robbie", Age = 30, PictureUrl = "https://hips.hearstapps.com/elleuk.cdnds.net/18/10/1520210874-gettyimages-927252130.jpg?crop=0.988xw:0.658xh;0.0119xw,0.0691xh&resize=640:*" },
                    new Role { FullName = "Pete Davidson", Age = 27, PictureUrl = "https://media.nu.nl/m/ansxdkraedri_sqr256.jpg/pete-davidson-vertelt-nieuwe-liefdes-meteen-over-eigenaardigheden.jpg" },
                    new Role { FullName = "John Cena", Age = 44, PictureUrl = "https://ichef.bbci.co.uk/news/976/cpsprodpb/2536/production/_118662590_cena2.jpg" },
                    new Role { FullName = "James Gunn", Age = 50, PictureUrl = "https://ichef.bbci.co.uk/news/976/cpsprodpb/22F5/production/_103794980_gettyimages-678895846.jpg" },
                    new Role { FullName = "Todd Phillips", Age = 50, PictureUrl = "https://i.redd.it/hwk9xgheqzm51.jpg" },
                    new Role { FullName = "Zazie Beetz", Age = 30, PictureUrl = "https://resizing.flixster.com/t3iiAgmwNQpbDsiVNvNc8s3Zuug=/506x652/v2/https://flxt.tmsimg.com/v9/AllPhotos/981946/981946_v9_bb.jpg" },
                    new Role { FullName = "Robert De Niro", Age = 77, PictureUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/58/Robert_De_Niro_Cannes_2016.jpg/220px-Robert_De_Niro_Cannes_2016.jpg" },
                    new Role { FullName = "Joaquin Phoenix", Age = 46, PictureUrl = "https://i.pinimg.com/originals/1d/2e/12/1d2e12756addc022144c4a8ac437f5c0.jpg" },
                    new Role { FullName = "Millie Bobby Brown", Age = 17, PictureUrl = "https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcQhVfKxsjWZM-30ElFKfETvE1VUZyZ9OV3UcxZ_5O6hQMmawqCV" },
                    new Role { FullName = "Alexander Skarsgård", Age = 44, PictureUrl = "https://encrypted-tbn3.gstatic.com/images?q=wn:GccTXROK8cAOBwEOohepzhbjJdpAUVQBTVOaWQ4Rtp6iR0wMLyx4W" },
                    new Role { FullName = "Rebecca Hall", Age = 39, PictureUrl = "https://i.pinimg.com/originals/30/ac/b4/30acb4e1f6f8f0437a8fb7ceb04085db.jpg" },
                    new Role { FullName = "Adam Wingard", Age = 38, PictureUrl = "https://encrypted-tbn2.gstatic.com/images?q=wn:GcRkWTdn0iu8DiewFAFNvOEOXtFctVTC2-fCX5LWZJN8tc8l035q" },
                    new Role { FullName = "Mary Parent", Age = 53, PictureUrl = "https://encrypted-tbn2.gstatic.com/images?q=wn:GcTai8Ehm5_kioXJMqPfvIoFx8QwIOYsyoqUhFT0Im6FiIL1_mI_" }
                );
                await _context.SaveChangesAsync();
            }
            
            // Seed MovieRoles
            if (!_context.MovieRoles.Any())
            {
                _context.MovieRoles.AddRange(
                    new MovieRole { MovieId = 1, RoleId = 13, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 1, RoleId = 14, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 1, RoleId = 15, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 1, RoleId = 16, RoleType = RoleType.Director },
                    new MovieRole { MovieId = 1, RoleId = 17, RoleType = RoleType.Producer },
                    new MovieRole { MovieId = 2, RoleId = 9, RoleType = RoleType.Director },
                    new MovieRole { MovieId = 2, RoleId = 10, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 2, RoleId = 11, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 2, RoleId = 14, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 3, RoleId = 5, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 3, RoleId = 6, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 3, RoleId = 7, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 3, RoleId = 8, RoleType = RoleType.Director },
                    new MovieRole { MovieId = 4, RoleId = 1, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 4, RoleId = 2, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 4, RoleId = 3, RoleType = RoleType.Cast },
                    new MovieRole { MovieId = 4, RoleId = 4, RoleType = RoleType.Director }
                );
                await _context.SaveChangesAsync();
            }
            
            // Seed Theaters
            if (!_context.Theaters.Any())
            {
                _context.Theaters.AddRange(
                    new Theater { Name = "A", NumOfRows = 10, SeatsPerRow = 20, Type = TheaterType.Normal },
                    new Theater { Name = "B", NumOfRows = 5, SeatsPerRow = 5, Type = TheaterType.Royal },
                    new Theater { Name = "C", NumOfRows = 10, SeatsPerRow = 10, Type = TheaterType.Normal },
                    new Theater { Name = "D", NumOfRows = 12, SeatsPerRow = 15, Type = TheaterType.Royal },
                    new Theater { Name = "E", NumOfRows = 8, SeatsPerRow = 13, Type = TheaterType.Normal }
                );
                await _context.SaveChangesAsync();
            }

            // Seed TheaterSeats
            if (!_context.TheaterSeats.Any())
            {
                _context.TheaterSeats.AddRange(
                    new TheaterSeat { SeatRow = "A", SeatNumber = 11, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 0, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "A", SeatNumber = 11, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 11, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 11, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 11, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "A", SeatNumber = 3, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 3, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 3, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 3, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 3, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 3, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "G", SeatNumber = 3, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 3, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 3, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "J", SeatNumber = 3, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "A", SeatNumber = 7, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 7, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 7, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 7, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 7, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 7, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "G", SeatNumber = 7, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 7, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 7, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "J", SeatNumber = 7, TheaterId = 3, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "A", SeatNumber = 4, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 4, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 4, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 4, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 0, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 1, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 2, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 3, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 0, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 1, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 2, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 3, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 4, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "J", SeatNumber = 4, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "K", SeatNumber = 4, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "L", SeatNumber = 4, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "A", SeatNumber = 10, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "B", SeatNumber = 10, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 10, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 10, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 12, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 13, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "D", SeatNumber = 14, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 10, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "J", SeatNumber = 10, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "K", SeatNumber = 10, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "L", SeatNumber = 10, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 11, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 12, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 13, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "I", SeatNumber = 14, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 5, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 6, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 7, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 8, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "E", SeatNumber = 9, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 5, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 9, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "G", SeatNumber = 5, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "G", SeatNumber = 9, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 5, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 6, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 7, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 8, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "H", SeatNumber = 9, TheaterId = 4, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 0, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 1, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 2, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 3, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 4, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 5, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 6, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 7, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 8, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 9, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 10, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 11, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "C", SeatNumber = 12, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 0, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 1, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 2, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 3, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 4, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 5, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 6, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 7, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 8, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 9, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 10, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 11, TheaterId = 5, Type = SeatType.Missing },
                    new TheaterSeat { SeatRow = "F", SeatNumber = 12, TheaterId = 5, Type = SeatType.Missing }
                );
                await _context.SaveChangesAsync();
            }

            // Seed Shows
            if (!_context.Shows.Any())
            {
                _context.Shows.AddRange(
                    new Show { StartTime = TimeSpan.Parse("10:00:00"), EndTime = TimeSpan.Parse("12:15:00"), Date = new DateTime(2021, 6, 5), MovieId = 4, TheaterId = 4, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("12:30:00"), EndTime = TimeSpan.Parse("14:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 4, TheaterId = 4, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("18:30:00"), EndTime = TimeSpan.Parse("20:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 4, TheaterId = 3, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("21:30:00"), EndTime = TimeSpan.Parse("23:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 4, TheaterId = 3, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("21:30:00"), EndTime = TimeSpan.Parse("23:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 4, TheaterId = 3, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("18:30:00"), EndTime = TimeSpan.Parse("20:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 4, TheaterId = 5, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("19:30:00"), EndTime = TimeSpan.Parse("21:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 4, TheaterId = 4, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("12:30:00"), EndTime = TimeSpan.Parse("14:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 4, TheaterId = 4, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("18:30:00"), EndTime = TimeSpan.Parse("20:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 4, TheaterId = 4, Status = ShowStatus.Full, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("18:30:00"), EndTime = TimeSpan.Parse("20:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 4, TheaterId = 3, Status = ShowStatus.Full, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("19:30:00"), EndTime = TimeSpan.Parse("21:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 4, TheaterId = 5, Status = ShowStatus.Full, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("21:30:00"), EndTime = TimeSpan.Parse("23:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 4, TheaterId = 4, Status = ShowStatus.Full, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("12:30:00"), EndTime = TimeSpan.Parse("14:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 2, TheaterId = 3, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("13:30:00"), EndTime = TimeSpan.Parse("15:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 2, TheaterId = 5, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("16:30:00"), EndTime = TimeSpan.Parse("18:45:00"), Date = new DateTime(2021, 6, 5), MovieId = 2, TheaterId = 4, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("16:30:00"), EndTime = TimeSpan.Parse("18:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 2, TheaterId = 4, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("18:30:00"), EndTime = TimeSpan.Parse("20:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 2, TheaterId = 3, Status = ShowStatus.AlmostFull, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("21:30:00"), EndTime = TimeSpan.Parse("23:45:00"), Date = new DateTime(2021, 6, 6), MovieId = 2, TheaterId = 5, Status = ShowStatus.Full, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("21:30:00"), EndTime = TimeSpan.Parse("23:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 2, TheaterId = 3, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("15:30:00"), EndTime = TimeSpan.Parse("17:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 2, TheaterId = 3, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("10:30:00"), EndTime = TimeSpan.Parse("12:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 2, TheaterId = 3, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("10:30:00"), EndTime = TimeSpan.Parse("12:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 3, TheaterId = 5, Status = ShowStatus.Free, Type = ShowType.TwoD },
                    new Show { StartTime = TimeSpan.Parse("13:30:00"), EndTime = TimeSpan.Parse("15:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 3, TheaterId = 5, Status = ShowStatus.Free, Type = ShowType.TwoD },
                    new Show { StartTime = TimeSpan.Parse("15:30:00"), EndTime = TimeSpan.Parse("17:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 3, TheaterId = 4, Status = ShowStatus.Full, Type = ShowType.TwoD },
                    new Show { StartTime = TimeSpan.Parse("16:30:00"), EndTime = TimeSpan.Parse("18:45:00"), Date = new DateTime(2021, 6, 7), MovieId = 3, TheaterId = 5, Status = ShowStatus.Free, Type = ShowType.TwoD },
                    new Show { StartTime = TimeSpan.Parse("13:30:00"), EndTime = TimeSpan.Parse("15:45:00"), Date = new DateTime(2021, 6, 8), MovieId = 3, TheaterId = 5, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { StartTime = TimeSpan.Parse("16:00:00"), EndTime = TimeSpan.Parse("18:15:00"), Date = new DateTime(2021, 6, 8), MovieId = 3, TheaterId = 4, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("20:15:00"), Date = new DateTime(2021, 6, 8), MovieId = 3, TheaterId = 3, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("20:15:00"), Date = new DateTime(2021, 6, 8), MovieId = 3, TheaterId = 5, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { StartTime = TimeSpan.Parse("21:00:00"), EndTime = TimeSpan.Parse("23:15:00"), Date = new DateTime(2021, 6, 8), MovieId = 3, TheaterId = 3, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { StartTime = TimeSpan.Parse("21:00:00"), EndTime = TimeSpan.Parse("23:15:00"), Date = new DateTime(2021, 6, 8), MovieId = 3, TheaterId = 4, Status = ShowStatus.AlmostFull, Type = ShowType.TwoD },
                    new Show { StartTime = TimeSpan.Parse("09:00:00"), EndTime = TimeSpan.Parse("11:15:00"), Date = new DateTime(2021, 6, 6), MovieId = 3, TheaterId = 5, Status = ShowStatus.Free, Type = ShowType.TwoD },
                    new Show { StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("19:30:00"), Date = new DateTime(2021, 6, 10), MovieId = 4, TheaterId = 5, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("18:00:00"), EndTime = TimeSpan.Parse("19:30:00"), Date = new DateTime(2021, 6, 10), MovieId = 4, TheaterId = 4, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("19:00:00"), EndTime = TimeSpan.Parse("20:30:00"), Date = new DateTime(2021, 6, 10), MovieId = 4, TheaterId = 3, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("22:00:00"), EndTime = TimeSpan.Parse("23:30:00"), Date = new DateTime(2021, 6, 10), MovieId = 4, TheaterId = 3, Status = ShowStatus.Free, Type = ShowType.ThreeD },
                    new Show { StartTime = TimeSpan.Parse("21:00:00"), EndTime = TimeSpan.Parse("22:30:00"), Date = new DateTime(2021, 6, 10), MovieId = 4, TheaterId = 4, Status = ShowStatus.Free, Type = ShowType.ThreeD }
                );
                await _context.SaveChangesAsync();
            }




            // Seed Bookings
            if (!_context.Bookings.Any())
            {
                _context.Bookings.AddRange(
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "E", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:29") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "E", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:31") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "E", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:33") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "E", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:35") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "E", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:37") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "F", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:38") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "F", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:40") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "F", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:42") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "G", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:43") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "H", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:45") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "H", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:47") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "H", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:48") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "H", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:50") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:52") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:53") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:55") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:57") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:31:58") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:00") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:02") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:03") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "J", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:05") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "J", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:06") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "B", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:08") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "B", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:09") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "C", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:11") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "C", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:13") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "C", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:14") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "B", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:16") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "I", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:17") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "I", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:19") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "I", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:20") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "J", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:22") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "J", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:24") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "J", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:25") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:27") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "J", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:28") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:30") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:32") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:34") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:35") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:37") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:39") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:40") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:42") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:44") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:45") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:47") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:49") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:50") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:52") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:53") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "J", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:55") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "J", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:57") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "J", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:32:58") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "J", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:00") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "L", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:01") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "H", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:03") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "H", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:05") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "H", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:06") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "H", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:08") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "H", SeatNumber = 10, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:09") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "G", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:11") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "G", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:13") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "G", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:14") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "G", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:16") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "G", SeatNumber = 10, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:17") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "F", SeatNumber = 14, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:19") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "F", SeatNumber = 10, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:21") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "E", SeatNumber = 12, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:22") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "E", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:24") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "B", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:25") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "B", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:27") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "B", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:29") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "C", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:30") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "C", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:32") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "C", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:33") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "D", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:35") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "D", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:37") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "D", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:38") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "D", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:40") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "D", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:41") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "B", SeatNumber = 13, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:43") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "G", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:44") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:33:46") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "J", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:08") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "J", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:10") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "J", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:12") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "J", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:14") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "J", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:16") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:18") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "J", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:19") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "J", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:21") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "I", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:23") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "I", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:24") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "I", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:26") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "I", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:27") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "I", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:29") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "I", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:31") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "I", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:32") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "I", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:34") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "F", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:35") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "F", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:37") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "G", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:39") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "G", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:40") },
                    new Booking { UserId = user2Id, ShowId = 13, SeatRow = "G", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:36:42") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:41:58") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "H", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:00") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "G", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:02") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "H", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:03") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:05") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "H", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:07") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:08") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "H", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:10") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "D", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:11") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "E", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:13") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "D", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:14") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "E", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:16") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "B", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:18") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "B", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:19") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "B", SeatNumber = 10, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:21") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "E", SeatNumber = 10, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:23") },
                    new Booking { UserId = user2Id, ShowId = 32, SeatRow = "E", SeatNumber = 11, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 03:42:24") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 21:24:31") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 21:24:35") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 21:24:37") },
                    new Booking { UserId = user2Id, ShowId = 17, SeatRow = "H", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:19") },
                    new Booking { UserId = user2Id, ShowId = 17, SeatRow = "H", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:21") },
                    new Booking { UserId = user2Id, ShowId = 17, SeatRow = "H", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:22") },
                    new Booking { UserId = user2Id, ShowId = 17, SeatRow = "H", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:24") },
                    new Booking { UserId = user2Id, ShowId = 17, SeatRow = "H", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:25") },
                    new Booking { UserId = user2Id, ShowId = 17, SeatRow = "H", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-06 22:43:27") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-06-10 04:10:13") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-06-10 04:10:15") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-06-10 04:10:17") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "L", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-06-10 04:10:18") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "L", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-06-10 04:10:20") },
                    new Booking { UserId = user3Id, ShowId = 19, SeatRow = "E", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:43") },
                    new Booking { UserId = user3Id, ShowId = 19, SeatRow = "F", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:45") },
                    new Booking { UserId = user3Id, ShowId = 19, SeatRow = "G", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:47") },
                    new Booking { UserId = user3Id, ShowId = 19, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:49") },
                    new Booking { UserId = user3Id, ShowId = 19, SeatRow = "J", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:50") },
                    new Booking { UserId = user3Id, ShowId = 19, SeatRow = "J", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-10 19:00:52") },
                    new Booking { UserId = user2Id, ShowId = 21, SeatRow = "A", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-28 17:59:59") },
                    new Booking { UserId = user2Id, ShowId = 21, SeatRow = "E", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-28 18:00:01") },
                    new Booking { UserId = user2Id, ShowId = 21, SeatRow = "F", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-28 18:00:03") },
                    new Booking { UserId = user2Id, ShowId = 21, SeatRow = "H", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-06-28 18:00:04") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "G", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:45") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "H", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:47") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:49") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "G", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:51") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "H", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:52") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "H", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-07-16 00:44:53") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "I", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-08-08 03:35:41") },
                    new Booking { UserId = user2Id, ShowId = 23, SeatRow = "J", SeatNumber = 3, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-08-17 19:47:19") },
                    new Booking { UserId = user2Id, ShowId = 23, SeatRow = "I", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-08-18 10:46:44") },
                    new Booking { UserId = user2Id, ShowId = 23, SeatRow = "I", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-08-18 10:47:03") },
                    new Booking { UserId = user2Id, ShowId = 27, SeatRow = "E", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-14 00:24:41") },
                    new Booking { UserId = user2Id, ShowId = 27, SeatRow = "F", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-14 00:24:44") },
                    new Booking { UserId = user2Id, ShowId = 27, SeatRow = "G", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-14 00:24:45") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "I", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:43:46") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "I", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:43:49") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "I", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:43:52") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "I", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:43:53") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "I", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:43:56") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "I", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:02") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "H", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:04") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "H", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:05") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "H", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:07") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "G", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:10") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "G", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:11") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:13") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "G", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:14") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "G", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:16") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:17") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "E", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:20") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "D", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:22") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "D", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:24") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "D", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:26") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "E", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:27") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "E", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:28") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "F", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:30") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "F", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:32") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:33") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "G", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:36") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "J", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:38") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "J", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:40") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "E", SeatNumber = 1, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:41") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "E", SeatNumber = 0, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:43") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "C", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:44") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "C", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-17 00:44:46") },
                    new Booking { UserId = user2Id, ShowId = 23, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-23 16:01:30") },
                    new Booking { UserId = user2Id, ShowId = 23, SeatRow = "K", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-23 16:01:32") },
                    new Booking { UserId = user2Id, ShowId = 23, SeatRow = "K", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-09-23 16:01:33") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-10-14 16:08:49") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "J", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-10-14 16:08:52") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "J", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-10-14 16:08:53") },
                    new Booking { UserId = user2Id, ShowId = 8, SeatRow = "J", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-10-14 16:08:55") },
                    new Booking { UserId = user2Id, ShowId = 23, SeatRow = "J", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-11-20 02:31:31") },
                    new Booking { UserId = user2Id, ShowId = 23, SeatRow = "J", SeatNumber = 7, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-11-20 02:31:33") },
                    new Booking { UserId = user2Id, ShowId = 23, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-11-20 02:31:34") },
                    new Booking { UserId = user2Id, ShowId = 23, SeatRow = "J", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2021-11-20 02:31:36") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "J", SeatNumber = 4, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-12 19:26:39") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "J", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-12 19:26:42") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "J", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-12 19:26:50") },
                    new Booking { UserId = user2Id, ShowId = 26, SeatRow = "H", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-12 19:26:53") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 5, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-14 19:05:08") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "K", SeatNumber = 6, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-14 19:05:11") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "I", SeatNumber = 8, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-14 19:05:16") },
                    new Booking { UserId = user2Id, ShowId = 7, SeatRow = "I", SeatNumber = 9, Price = 800, Status = BookingStatus.Confirmed, BookingDateTime = DateTime.Parse("2022-01-14 19:05:19") },
                    new Booking { UserId = user2Id, ShowId = 27, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-15 05:49:12") },
                    new Booking { UserId = user2Id, ShowId = 24, SeatRow = "F", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:49:31") },
                    new Booking { UserId = user2Id, ShowId = 24, SeatRow = "F", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:49:33") },
                    new Booking { UserId = user2Id, ShowId = 24, SeatRow = "F", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:49:34") },
                    new Booking { UserId = user2Id, ShowId = 24, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:49:36") },
                    new Booking { UserId = user2Id, ShowId = 24, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:49:38") },
                    new Booking { UserId = user2Id, ShowId = 24, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2021-01-17 14:49:40") },
                    new Booking { UserId = user2Id, ShowId = 27, SeatRow = "A", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 14:51:09") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 0, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:17") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 1, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:18") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:20") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:22") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:23") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:25") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:27") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:29") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:30") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:32") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:34") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:36") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "A", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:37") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "B", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:39") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "B", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:41") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "B", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:42") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "B", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:44") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "B", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:46") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "B", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:48") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "B", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:49") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "B", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:51") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "B", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:52") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "B", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:54") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "B", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:56") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "E", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:57") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "D", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:39:59") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "D", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:01") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "E", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:02") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "D", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:04") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "E", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:06") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "D", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:07") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "E", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:09") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "D", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:11") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "E", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:13") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "E", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:14") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "D", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:16") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "E", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:18") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "D", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:20") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "D", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:21") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "E", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:23") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "D", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:25") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "E", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:27") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "D", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:28") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "E", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:30") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "D", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:32") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "E", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:33") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "H", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:35") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "G", SeatNumber = 12, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:37") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "G", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:38") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "H", SeatNumber = 11, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:40") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "G", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:42") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "H", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:44") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "H", SeatNumber = 10, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:45") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "G", SeatNumber = 9, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:47") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "H", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:49") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "G", SeatNumber = 8, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:50") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "G", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:52") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "H", SeatNumber = 7, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:54") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "H", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:55") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "G", SeatNumber = 6, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:57") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "G", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:40:59") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "H", SeatNumber = 5, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:00") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "G", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:02") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "H", SeatNumber = 4, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:04") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "H", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:06") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:07") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "G", SeatNumber = 3, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:09") },
                    new Booking { UserId = user2Id, ShowId = 14, SeatRow = "H", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-17 15:41:11") },
                    new Booking { UserId = user4Id, ShowId = 33, SeatRow = "G", SeatNumber = 2, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-18 15:34:50") },
                    new Booking { UserId = user4Id, ShowId = 33, SeatRow = "D", SeatNumber = 0, Price = 800, Status = BookingStatus.Reserved, BookingDateTime = DateTime.Parse("2022-01-18 15:34:52") }
                    );
                await _context.SaveChangesAsync();
            }
            // Seed Payments
            if (!_context.Payments.Any())
            {
                _context.Payments.AddRange(
                    new Payment { Amount = 68000, PaymentDateTime = DateTime.Parse("2021-06-06 03:33:47"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 7 },
                    new Payment { Amount = 16800, PaymentDateTime = DateTime.Parse("2021-06-06 03:36:43"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 13 },
                    new Payment { Amount = 13600, PaymentDateTime = DateTime.Parse("2021-06-06 03:42:26"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 32 },
                    new Payment { Amount = 2400, PaymentDateTime = DateTime.Parse("2021-06-06 21:24:38"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 7 },
                    new Payment { Amount = 4800, PaymentDateTime = DateTime.Parse("2021-06-06 22:43:28"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 17 },
                    new Payment { Amount = 4800, PaymentDateTime = DateTime.Parse("2021-06-10 19:00:53"), Method = PaymentMethod.Card, UserId = user3Id, ShowId = 19 },
                    new Payment { Amount = 3200, PaymentDateTime = DateTime.Parse("2021-06-28 18:00:06"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 21 },
                    new Payment { Amount = 4800, PaymentDateTime = DateTime.Parse("2021-07-16 00:44:55"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 8 },
                    new Payment { Amount = 800, PaymentDateTime = DateTime.Parse("2021-08-08 03:35:43"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 8 },
                    new Payment { Amount = 800, PaymentDateTime = DateTime.Parse("2021-08-17 19:47:21"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 23 },
                    new Payment { Amount = 1600, PaymentDateTime = DateTime.Parse("2021-08-18 10:47:04"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 23 },
                    new Payment { Amount = 2400, PaymentDateTime = DateTime.Parse("2021-09-14 00:24:47"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 27 },
                    new Payment { Amount = 24800, PaymentDateTime = DateTime.Parse("2021-09-17 00:44:47"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 26 },
                    new Payment { Amount = 2400, PaymentDateTime = DateTime.Parse("2021-09-23 16:01:34"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 23 },
                    new Payment { Amount = 3200, PaymentDateTime = DateTime.Parse("2021-10-14 16:08:56"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 8 },
                    new Payment { Amount = 3200, PaymentDateTime = DateTime.Parse("2021-11-20 02:31:38"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 23 },
                    new Payment { Amount = 3200, PaymentDateTime = DateTime.Parse("2022-01-12 19:26:56"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 26 },
                    new Payment { Amount = 3200, PaymentDateTime = DateTime.Parse("2022-01-14 19:05:21"), Method = PaymentMethod.Card, UserId = user2Id, ShowId = 7 }
                );
                await _context.SaveChangesAsync();
            }
            
        }
    }
}
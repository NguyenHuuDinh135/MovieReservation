using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation.Server.Infrastructure.Data
{
    public static class InitialiserExtensions
    {
        public static void AddAsyncSeeding(this DbContextOptionsBuilder builder, IServiceProvider serviceProvider)
        {
            builder.UseAsyncSeeding(async (context, _, ct) =>
            {
                var initialiser = serviceProvider.GetRequiredService<MovieReservationDbContextInitialiser>();

                //await initialiser.SeedAsync();
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

        public void SeedAsync()
        {
            try
            {
                //await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
        public async Task TrySeedAsync()
        {
            //// Default roles
            //var administratorRole = new IdentityRole(Roles.Administrator);

            //if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            //{
            //    await _roleManager.CreateAsync(administratorRole);
            //}


            //// Default users
            //var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            //if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            //{
            //    await _userManager.CreateAsync(administrator, "Administrator1!");
            //    if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            //    {
            //        await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            //    }
            //}

            //Default data
            // Seed, if necessary
            //if (!_context.TodoLists.Any())
            //    {
            //        _context.TodoLists.Add(new TodoList
            //        {
            //            Title = "Todo List",
            //            Items =
            //    {
            //        new TodoItem { Title = "Make a todo list 📃" },
            //        new TodoItem { Title = "Check off the first item ✅" },
            //        new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
            //        new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
            //    }
            //        });

            //        await _context.SaveChangesAsync();
            //    }
        }
    }
}

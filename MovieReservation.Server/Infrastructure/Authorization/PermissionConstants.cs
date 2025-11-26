namespace MovieReservation.Server.Infrastructure.Authorization
{
    public static class PermissionConstants
    {
        // Claim type used to represent a permission
        public const string Permission = "permission";

        /// <summary>
        /// Centralized permission definitions for the application.
        /// Follow convention: Permission.{Module}.{Action}
        /// </summary>
        public static class Permissions
        {
            // Permission Management
            public const string ManagePermissions = "Permission.ManagePermissions";

            // Payments
            public const string PaymentsCreate = "Permission.Payments.Create";
            public const string PaymentsEdit = "Permission.Payments.Edit";
            public const string PaymentsDelete = "Permission.Payments.Delete";
            public const string PaymentsView = "Permission.Payments.View";

            // Movies
            public const string MoviesCreate = "Permission.Movies.Create";
            public const string MoviesEdit = "Permission.Movies.Edit";
            public const string MoviesDelete = "Permission.Movies.Delete";
            public const string MoviesView = "Permission.Movies.View";

            // Shows
            public const string ShowsCreate = "Permission.Shows.Create";
            public const string ShowsEdit = "Permission.Shows.Edit";
            public const string ShowsDelete = "Permission.Shows.Delete";
            public const string ShowsView = "Permission.Shows.View";

            // Theaters
            public const string TheatersCreate = "Permission.Theaters.Create";
            public const string TheatersEdit = "Permission.Theaters.Edit";
            public const string TheatersDelete = "Permission.Theaters.Delete";
            public const string TheatersView = "Permission.Theaters.View";

            // Bookings
            public const string BookingsCreate = "Permission.Bookings.Create";
            public const string BookingsEdit = "Permission.Bookings.Edit";
            public const string BookingsDelete = "Permission.Bookings.Delete";
            public const string BookingsView = "Permission.Bookings.View";

            // Genres
            public const string GenresCreate = "Permission.Genres.Create";
            public const string GenresEdit = "Permission.Genres.Edit";
            public const string GenresDelete = "Permission.Genres.Delete";
            public const string GenresView = "Permission.Genres.View";

            // Users
            public const string UsersCreate = "Permission.Users.Create";
            public const string UsersEdit = "Permission.Users.Edit";
            public const string UsersDelete = "Permission.Users.Delete";
            public const string UsersView = "Permission.Users.View";

            /// <summary>
            /// Get all permission values as an array (useful for seeding, validation, etc.)
            /// </summary>
            public static string[] GetAll()
            {
                return new[]
                {
                    ManagePermissions,
                    PaymentsCreate, PaymentsEdit, PaymentsDelete, PaymentsView,
                    MoviesCreate, MoviesEdit, MoviesDelete, MoviesView,
                    ShowsCreate, ShowsEdit, ShowsDelete, ShowsView,
                    TheatersCreate, TheatersEdit, TheatersDelete, TheatersView,
                    BookingsCreate, BookingsEdit, BookingsDelete, BookingsView,
                    GenresCreate, GenresEdit, GenresDelete, GenresView,
                    UsersCreate, UsersEdit, UsersDelete, UsersView,
                };
            }
        }
    }
}
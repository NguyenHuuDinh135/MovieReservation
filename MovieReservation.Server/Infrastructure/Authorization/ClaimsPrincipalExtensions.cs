using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MovieReservation.Server.Infrastructure.Authorization
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasPermission(this ClaimsPrincipal? principal, string permission)
        {
            if (principal == null) return false;
            return principal.Claims.Any(c =>
                string.Equals(c.Type, PermissionConstants.Permission, StringComparison.Ordinal)
                && string.Equals(c.Value, permission, StringComparison.Ordinal));
        }

        public static IEnumerable<string> GetPermissions(this ClaimsPrincipal? principal)
        {
            if (principal == null) return Enumerable.Empty<string>();
            return principal.FindAll(PermissionConstants.Permission).Select(c => c.Value);
        }
    }
}
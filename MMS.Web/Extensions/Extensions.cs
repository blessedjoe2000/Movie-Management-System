using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace MMS.Web
{
    public static class Extensions
    {
        // ClaimsPrincipal - HasOneOfRoles extension method to check
        //if a user has any of the roles in a comma separated string

        public static bool HasOneOfRoles(this ClaimsPrincipal claims, string rolesString)
        {
            // split string into an array of roles
            var roles = rolesString.Split(",");

            // linq query to check that ClaimsPrincipal has one of these roles
            return roles.FirstOrDefault(role => claims.IsInRole(role)) != null;
        }

        // IServiceCollection - AddCookieAuthentication extension 
        //method - to be called in Startup ConfigureServices
        public static void AddCookieAuthentication(this IServiceCollection services, 
                                                    string notAuthorised = "/User/ErrorNotAuthorised",
                                                    string notAuthenticated = "/User/ErrorNotAuthenticated")
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => {
                        options.AccessDeniedPath = notAuthorised;
                        options.LoginPath = notAuthenticated;
                    });
        }
    }
}
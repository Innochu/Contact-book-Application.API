using ContactBook.Core.Services.Implementation;
using ContactBook.Core.Services.Interfaces;
using ContactBook.Data;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.API.Extension
{
    public static class StartUp
    {
        public static void AddDependencies( this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ContactBookContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ContactBookCS"));

            });

            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
        }



    }
}

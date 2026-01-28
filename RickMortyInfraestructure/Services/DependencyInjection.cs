using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RickMortyDomain.Repositories;
using RickMortyDomain.Services;
using RickMortyInfraestructure.ExternalServices;
using RickMortyInfraestructure.Persistence;
using RickMortyInfraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickMortyInfraestructure.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))
                ));

            services.AddScoped<ICharacterRepository, CharacterRepository>();

            services.AddHttpClient<IRickMortyExternalService, RickMortyExternalService>();

            return services;
        }
    }
}

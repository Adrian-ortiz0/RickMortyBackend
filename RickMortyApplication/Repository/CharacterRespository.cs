using Microsoft.Extensions.DependencyInjection;
using RickMortyApplication.Interfaces;
using RickMortyApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickMortyApplication.Repository
{
    public static class CharacterRespository
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICharacterInterface, CharacterService>();
            return services;
        }
    }
}

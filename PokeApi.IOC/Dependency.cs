using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PokeApi.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PokeApi.DAL.Repositorys.Contract;
using PokeApi.DAL.Repositorys;
using PokeApi.Utility;
using PokeApi.BLL.Services;
using PokeApi.BLL.Services.Contract;

namespace PokeApi.IOC
{
    public static class Dependency
    {
        public static void InyectDependencys(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PokedexdbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("stringSQL"));
                options.LogTo(Console.WriteLine);

            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IPokemonRepository, PokemonRepository>();
            services.AddScoped<IPokePublicApiRepository, PokePublicApiRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IPokemonService, PokemonService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IApiPublicPokemonService, ApiPublicPokemonService>();
            services.AddScoped<GeneralPokemonService>();
            services.AddScoped<IAlbumService, AlbumService>(); // Esto es solo un ejemplo, debes reemplazar AlbumService con la implementación real de IAlbumService


        }
    }
}

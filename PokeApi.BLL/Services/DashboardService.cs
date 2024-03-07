using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PokeApi.DAL.Repositorys.Contract;
using PikeApi.DTO;
using PokeApi.Model;
using PokeApi.BLL.Services.Contract;

namespace PokeApi.BLL.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IGenericRepository<Pokemon> _pokemonRepository;
        private readonly IMapper _mapper;

        public DashboardService(IGenericRepository<Pokemon> pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        public async Task<Dashboard_DTO> Resume()
        {
            Dashboard_DTO dashboard = new Dashboard_DTO();
            try
            {
                Console.WriteLine("uwu");
                return dashboard; 
            }
            catch
            {
                throw;
            }
        }
    }
}

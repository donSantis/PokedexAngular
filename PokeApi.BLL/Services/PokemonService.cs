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
using Microsoft.EntityFrameworkCore;
using PokeApi.DAL.Repositorys;

namespace PokeApi.BLL.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IGenericRepository<Pokemon> _pokemonRepository;
        private readonly IPokemonRepository _pokemonRepository2;
        private readonly IMapper _mapper;

        public PokemonService(IGenericRepository<Pokemon> pokemonRepository, IPokemonRepository pokemonRepository2, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _pokemonRepository2 = pokemonRepository2;
            _mapper = mapper;
        }

 

        public async Task<Pokemon_DTO> Create(Pokemon_DTO model)
        {
            try
            {
                var query = await _pokemonRepository.CreateModel(_mapper.Map<Pokemon>(model));
                if(query.IdPokemon == 0)
                    throw new TaskCanceledException("El pokemon no fue creado");
                return _mapper.Map<Pokemon_DTO>(query);
            }
            catch { throw; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var pokemonFind = await _pokemonRepository.GetModel(p => p.IdPokemon == id);
                if(pokemonFind == null)
                    throw new TaskCanceledException("El pokemon no fue encontrado");
                bool response = await _pokemonRepository.DeleteModel(pokemonFind);
                if (!response)
                    throw new TaskCanceledException("El pokemon no fue eliminado");
                return response;

            }
            catch { throw; }
        }

        public async Task<List<Pokemon_DTO>> List()
        {
            try
            {
                var query = await _pokemonRepository.Consulta();
                var pokemonList = query.ToList();
                return _mapper.Map<List<Pokemon_DTO>>(pokemonList.ToList());

           }
            catch {
                throw;
            }
        }

        public async Task<string> ListAllFirstGenerationPkmn()
        {
            try
            {
                var uwu = await _pokemonRepository2.ListAllFirstGenerationPkmn();
                return uwu;

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> update(Pokemon_DTO model)
        {
            try
            {
                var pokemonModel = _mapper.Map<Pokemon>(model);
                var pokemonFind = await _pokemonRepository.GetModel(p => p.IdPokemon  == pokemonModel.IdPokemon);
                if(pokemonFind == null)
                    throw new TaskCanceledException("El pokemon no fue encontrado");
                pokemonFind.Name = pokemonModel.Name;
                pokemonFind.IdPokemonApi = pokemonModel.IdPokemonApi;
                bool response = await _pokemonRepository.EditModel(pokemonFind);
                if (!response)
                    throw new TaskCanceledException("El pokemon no editado");
                return response;
            }
            catch { throw; }
        }
    }
}

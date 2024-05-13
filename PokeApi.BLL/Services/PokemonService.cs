using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PokeApi.DAL.Repositorys.Contract;
using PikeApi.DTO;
using PokeApi.Model.PokeApiClasses;
using PokeApi.BLL.Services.Contract;
using Microsoft.EntityFrameworkCore;
using PokeApi.DAL.Repositorys;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Linq;
using PokeApi.Model.Filter;
using static System.Runtime.InteropServices.JavaScript.JSType;
using PokeApi.Model.Response;


namespace PokeApi.BLL.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IGenericRepository<Pokemon> _pokemonRepository;
        private readonly IPokePublicApiRepository _pokePublicApiRepository;
        private readonly IPokemonRepository _pokemonRepository2;

        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly string _urlApiPublicPokemonById = "https://pokeapi.co/api/v2/pokemon/";
        private readonly ApiPublicPokemonService _apiPublicPokemonService;



        public PokemonService(IGenericRepository<Pokemon> pokemonRepository, IPokemonRepository pokemonRepository2, IPokePublicApiRepository pokePublicApiRepository, IMapper mapper, IApiPublicPokemonService apiPublicPokemonService)
        {
            _pokemonRepository = pokemonRepository;
            _pokemonRepository2 = pokemonRepository2;
            _pokePublicApiRepository = pokePublicApiRepository;
            _mapper = mapper;
            _httpClient = new HttpClient();
            _apiPublicPokemonService = (ApiPublicPokemonService?)apiPublicPokemonService;

        }



        public async Task<Pokemon_DTO> Create(Pokemon_DTO model)
        {
            try
            {
                var query = await _pokemonRepository.CreateModel(_mapper.Map<Pokemon>(model));
                if (query.IdPokemon == 0)
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
                if (pokemonFind == null)
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
            catch
            {
                throw;
            }
        }

        public async Task<string> ListAllFirstGenerationPkmn()
        {
            try
            {
                var data = await _pokePublicApiRepository.getAll();
                ReturnPokemonApiResponseClass pokemonesResponse = await Task.Run(() => _apiPublicPokemonService.GetAllPokemonFromApi(data));

                //Console.WriteLine(uwu);

                return data;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ReturnPokemonApiResponseClass> ListAllPkmnFromApi()
        {
            try
            {
                var data = await _pokePublicApiRepository.getAll();
                //HACER METODO PARA OBTENER NEXT / PREVIOUS
                ReturnPokemonApiResponseClass pokemonesResponse = await _apiPublicPokemonService.GetAllPokemonFromApi(data);

                return pokemonesResponse;
            }
            catch
            {
                throw;
            }
        }

        //public async Task<string> ListAllPkmnFromApiWithFilters(string filter)
        //{
        //    try
        //    {
        //        Filter filterObject = JsonConvert.DeserializeObject<Filter>(filter);

        //        var data = await _pokePublicApiRepository.getPokemonByFilter(filter);
        //        //HACER METODO PARA OBTENER NEXT / PREVIOUS
        //        return data;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        public async Task<string> ListPkmnByURL(string url)
        {
            try
            {
                var data = await _pokemonRepository2.ListPkmnByURL(url);
                //Console.WriteLine(uwu);

                return data;
            }
            catch
            {
                throw;
            }
        }
        public async Task<PokemonApiResponse> GetPkmnByURL(string url)
        {
            try
            {
                var data = await _pokemonRepository2.ListPkmnByURL(url);
                PokemonApiResponse pokemonesResponse = await Task.Run(() => _apiPublicPokemonService.GetPokemonDataFromURL(url));
                //Console.WriteLine(uwu);

                return pokemonesResponse;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ReturnPokemonApiResponseClass> ListAllPkmnFromGenerationByUrl(string url)
        {
            try
            {
                var data = await _pokemonRepository2.ListAllPkmnFromGenerationByUrl(url);

                ResponseApiGenerationPokemon data2 = JsonConvert.DeserializeObject<ResponseApiGenerationPokemon>(data);
                string jsonString = JsonConvert.SerializeObject(data2);
                ReturnPokemonApiResponseClass pokemonesResponse = await Task.Run(() => _apiPublicPokemonService.ListAllPkmnFromGenerationByUrl(jsonString));
                //Console.WriteLine(uwu);

                return pokemonesResponse;
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
                var pokemonFind = await _pokemonRepository.GetModel(p => p.IdPokemon == pokemonModel.IdPokemon);
                if (pokemonFind == null)
                    throw new TaskCanceledException("El pokemon no fue encontrado");
                pokemonFind.name = pokemonModel.name;
                pokemonFind.IdPokemonApi = pokemonModel.IdPokemonApi;
                bool response = await _pokemonRepository.EditModel(pokemonFind);
                if (!response)
                    throw new TaskCanceledException("El pokemon no editado");
                return response;
            }
            catch { throw; }
        }

        //----------------------------------------------------------------------------------------------------------------
        
        public async Task<ReturnPokemonApiResponseClass> ListPokemonFromApiWithFilters(Filter filter)
        {
            try
            {
                var data = await _pokePublicApiRepository.getPokemonByFilter(filter);
                ReturnPokemonApiResponseClass pokemonesResponse = await _apiPublicPokemonService.GetAllPokemonFromApi(data);

                return pokemonesResponse;
            }
            catch
            {
                throw;
            }
        }

 
    }
}

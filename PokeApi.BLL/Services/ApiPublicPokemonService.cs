using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PokeApi.DAL.Repositorys.Contract;
using PikeApi.DTO;
using PokeApi.Model;
using PokeApi.Model.PokeApiClasses;
using PokeApi.BLL.Services.Contract;
using Microsoft.EntityFrameworkCore;
using PokeApi.DAL.Repositorys;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Linq;

namespace PokeApi.BLL.Services
{
    public class ApiPublicPokemonService : IApiPublicPokemonService
    {
        private readonly IGenericRepository<Pokemon> _pokemonRepository;
        private readonly IPokePublicApiRepository _pokePublicApiRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly string _urlApiPublicPokemonById = "https://pokeapi.co/api/v2/pokemon/";
        private readonly GeneralPokemonService _generalPokemonService;


        public ApiPublicPokemonService(IGenericRepository<Pokemon> pokemonRepository, IPokePublicApiRepository pokePublicApiRepository, IMapper mapper, GeneralPokemonService generalPokemonService)
        {
            _pokemonRepository = pokemonRepository;
            _pokePublicApiRepository = pokePublicApiRepository;
            _mapper = mapper;
            _httpClient = new HttpClient();
            _generalPokemonService = generalPokemonService;
        }
        public async Task<ReturnPokemonApiResponseClass> GetAllPokemonFromApi(string data)
        {
            try
            {
                ReturnPokemonApiResponseClass returnPokemonApiResponseClass = new ReturnPokemonApiResponseClass();
                // Deserializar el JSON en un objeto ResponseApiClass
                ResponseApiClass response = JsonConvert.DeserializeObject<ResponseApiClass>(data);
                if (response.results == null || !response.results.Any())
                {
                    Console.WriteLine("La lista de resultados está vacía.");
                }
                // Crear una lista para almacenar los pokemones
                var pokemonesApiResponse = new List<PokemonApiResponse>();
                // Crear una lista para almacenar las tareas de obtención de datos del Pokémon
                var pokemonTasks = new List<Task<PokemonApiResponse>>();
                // Recorrer todos los objetos en la lista de resultados
                foreach (var result in response.results)
                {
                    // Obtener el PokemonApiResponse correspondiente y agregarlo a la lista
                    pokemonTasks.Add(GetPokemonDataFromURL(result.url));
                }
                // Esperar a que todas las tareas de obtención de datos del Pokémon se completen
                await Task.WhenAll(pokemonTasks);
                foreach (var task in pokemonTasks)
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        pokemonesApiResponse.Add(task.Result);
                        Console.WriteLine("Procesando Pokémon: " + task.Result.name);
                    }
                    else if (task.IsFaulted)
                    {
                        // Manejar la excepción de la tarea fallida
                        Console.WriteLine("Error al procesar el Pokémon: " + task.Exception?.Message);
                    }
                }
                returnPokemonApiResponseClass.next = response.next;
                returnPokemonApiResponseClass.previous = response.previous;
                returnPokemonApiResponseClass.count = response.count;
                returnPokemonApiResponseClass.results = pokemonesApiResponse;
                // Devolver la lista de pokemones
                return returnPokemonApiResponseClass;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al procesar el JSON: " + ex.Message);
                // Lanzar una excepción personalizada o devolver null según sea necesario
                throw;
            }
        }

        public async Task<ReturnPokemonApiResponseClass> ListAllPkmnFromGenerationByUrl(string data)
        {
            try
            {
                ReturnPokemonApiResponseClass returnPokemonApiResponseClass = new ReturnPokemonApiResponseClass();
                // Deserializar el JSON en un objeto ResponseApiClass
                ResponseApiGenerationPokemon response = JsonConvert.DeserializeObject<ResponseApiGenerationPokemon>(data);
                if (response == null || !response.pokemonSpecies.Any())
                {
                    Console.WriteLine("La lista de resultados está vacía.");
                }
                // Crear una lista para almacenar los pokemones
                var pokemonesApiResponse = new List<PokemonApiResponse>();
                // Crear una lista para almacenar las tareas de obtención de datos del Pokémon
                var pokemonTasks = new List<Task<PokemonApiResponse>>();
                // Recorrer todos los objetos en la lista de resultados
                foreach (var result in response.pokemonSpecies)
                {
                    Console.WriteLine("Procesando Pokémon: " + result.name);
                    int id = _generalPokemonService.GetPokemonNumberFromURL(result.url);
                    string url = "https://pokeapi.co/api/v2/pokemon/" + id + "/";
                    // Obtener el PokemonApiResponse correspondiente y agregarlo a la lista
                    pokemonTasks.Add(GetPokemonDataFromURL(url));
                }
                // Esperar a que todas las tareas de obtención de datos del Pokémon se completen
                await Task.WhenAll(pokemonTasks);
                foreach (var task in pokemonTasks)
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        pokemonesApiResponse.Add(task.Result);
                        Console.WriteLine("Procesando Pokémon: " + task.Result.name);
                    }
                    else if (task.IsFaulted)
                    {
                        // Manejar la excepción de la tarea fallida
                        Console.WriteLine("Error al procesar el Pokémon: " + task.Exception?.Message);
                    }
                }
                returnPokemonApiResponseClass.results = pokemonesApiResponse;
                // Devolver la lista de pokemones
                return returnPokemonApiResponseClass;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al procesar el JSON: " + ex.Message);
                // Lanzar una excepción personalizada o devolver null según sea necesario
                throw;
            }
        }

        public async Task<PokemonApiResponse> GetPokemonDataFromURL(string url)
        {
            try
            {
                //NUEVO OBJETO DEL POKEMON CONSULTADO 
                PokemonApiResponse basePokemonData = new();
                // ID DE LAS 2 EVOLUCIONES EN EL Caso de que tenga
                int idPokemonFirstEvolution = 0;
                int idPokemonSecondEvolution = 0;
                //Verificadores de evoluciones
                int haveFirstEvolution = 0;
                int haveSecondEvolution = 0;
                if (url != null)
                {
                    //NUEVO OBJETO DEL POKEMON CONSULTADO 
                    PokemonApiResponse pokemonReturned = new();
                    //Seteamos el objetos con los valores encontrados x el metodo
                    basePokemonData = await SetPokemonData(url);
                    //Creamos los 3 objetos en blanco que se mezlcaran para generar al pokemon que se devuelve del metodo 
                    PokemonApiResponse pokemonFirstEvolution = new();
                    PokemonApiResponse pokemonSecondEvolution = new();

                    if (basePokemonData.chain.EvolveTo.Count > 0)
                    {
                        idPokemonFirstEvolution = _generalPokemonService.GetPokemonNumberFromURL(basePokemonData.chain.EvolveTo[0].species.url);
                        haveFirstEvolution = 1;
                    }
                    if(basePokemonData.chain.EvolveTo.Count > 0)
                    {
                        if (basePokemonData.chain.EvolveTo[0].EvolveToPlus.Count > 0)
                        {
                            idPokemonSecondEvolution = _generalPokemonService.GetPokemonNumberFromURL(basePokemonData.chain.EvolveTo[0].EvolveToPlus[0].species.url);
                            haveSecondEvolution = 1;
                        }
                    }

                    

                    if (basePokemonData.EvolveFrom == null)
                    {
                        if (haveFirstEvolution == 1)
                        {
                            pokemonFirstEvolution = await SetPokemonDataShort(_urlApiPublicPokemonById + idPokemonFirstEvolution + '/');
                        }

                        if (haveSecondEvolution == 1)
                        {
                            pokemonSecondEvolution = await SetPokemonDataShort(_urlApiPublicPokemonById + idPokemonSecondEvolution + '/');
                        }
                        var pokemonOrderConditional = "PokemonBase";
                        pokemonReturned = await CreatePokemonByDataCollect(basePokemonData, pokemonFirstEvolution, pokemonSecondEvolution, pokemonOrderConditional);
                    }
                    if (basePokemonData.evolutionChain != null && basePokemonData.id == idPokemonFirstEvolution && idPokemonFirstEvolution > 0)
                    {
                        pokemonFirstEvolution = basePokemonData;
                        int idPokemonBase = _generalPokemonService.GetPokemonNumberFromURL(basePokemonData.EvolveFrom.url);
                        basePokemonData = await SetPokemonDataShort(_urlApiPublicPokemonById + idPokemonBase + '/');
                        
                        if (haveSecondEvolution == 1)
                        {
                            pokemonSecondEvolution = await SetPokemonDataShort(_urlApiPublicPokemonById + idPokemonSecondEvolution + '/');
                        }
                        var pokemonOrderConditional = "PokemonFirstEvolution";
                        pokemonReturned = await CreatePokemonByDataCollect(basePokemonData, pokemonFirstEvolution, pokemonSecondEvolution, pokemonOrderConditional);
                    }
                    if (basePokemonData.evolutionChain != null && basePokemonData.id > idPokemonFirstEvolution && basePokemonData.id == idPokemonFirstEvolution && idPokemonSecondEvolution > 0 || basePokemonData.evolutionChain != null && basePokemonData.id >= idPokemonFirstEvolution && basePokemonData.id == idPokemonSecondEvolution && idPokemonSecondEvolution > 0)
                    {
                        pokemonSecondEvolution = basePokemonData;

                        pokemonFirstEvolution = await SetPokemonDataShort(_urlApiPublicPokemonById + _generalPokemonService.GetPokemonNumberFromURL(pokemonSecondEvolution.EvolveFrom.url) + '/');
                        basePokemonData = await SetPokemonDataShort(_urlApiPublicPokemonById + _generalPokemonService.GetPokemonNumberFromURL(pokemonFirstEvolution.EvolveFrom.url) + '/');

                        var pokemonOrderConditional = "PokemonSecondEvolution";
                        pokemonReturned = await CreatePokemonByDataCollect(basePokemonData, pokemonFirstEvolution, pokemonSecondEvolution, pokemonOrderConditional);
                    }
                    return pokemonReturned;
                }
                else
                {
                    throw new HttpRequestException($"La solicitud a {url} no fue exitosa. Código de estado:");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos del Pokémon: " +  url);
                Console.WriteLine("Error al obtener los datos del Pokémon: " + ex.Message);

                throw;
            }
        }

        public async Task<PokemonApiResponse> GetPokemonDataByUrl(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    PokemonApiResponse pokemon = JsonConvert.DeserializeObject<PokemonApiResponse>(responseData);
                    return pokemon;
                }
                else
                {
                    throw new HttpRequestException($"La solicitud a {url} no fue exitosa. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos del Pokémon: " + ex.Message);
                throw;
            }
        }

        public async Task<PokemonApiResponse> SetPokemonData(string url)
        {
            try
            {
                var firstDataTask = GetPokemonDataByUrl(url);
                var getFirstData = await firstDataTask;

                var secondDataTask = GetPokemonDataByUrl(getFirstData.species.url);
                var getSecondData = await secondDataTask;

                var thirdDataTask = GetPokemonDataByUrl(getSecondData.evolutionChain.url);
                var getThirdData = await thirdDataTask;

                var pokemon = new PokemonApiResponse
                {
                    id = getFirstData.id,
                    name = getFirstData.name,
                    weight = getFirstData.weight,
                    types = getFirstData.types,
                    sprites = getFirstData.sprites,
                    species = getFirstData.species,
                    chain = getThirdData.chain,
                    evolutionChain = getSecondData.evolutionChain,
                    EvolveFrom = getSecondData.EvolveFrom,
                    flavorTextEntries = getSecondData.flavorTextEntries.Where(entry => entry.language?.name == "es").ToList()
                    // ... agregar otros campos según sea necesario
                };
                return pokemon;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos del Pokémon: " + ex.Message);
                throw;
            }
        }


        public async Task<PokemonApiResponse> SetSecondEvolutionPokemonData(PokemonApiResponse url)
        {
            try
            {
                int idPokemonEvolution2 = _generalPokemonService.GetPokemonNumberFromURL(url.chain.EvolveTo[0].species.url);

                PokemonApiResponse getFirstData = await GetPokemonDataByUrl(_urlApiPublicPokemonById + idPokemonEvolution2);
                PokemonApiResponse getSecondData = await GetPokemonDataByUrl(getFirstData.species.url);
                PokemonApiResponse getThirdData = await GetPokemonDataByUrl(getSecondData.evolutionChain.url);
                PokemonApiResponse pokemon = new PokemonApiResponse
                {
                    id = getFirstData.id,
                    name = getFirstData.name,
                    weight = getFirstData.weight,
                    types = getFirstData.types,
                    sprites = getFirstData.sprites,
                    species = getFirstData.species,
                    evolutionChain = getSecondData.evolutionChain,// Aquí se mantiene el officialArtwork del primer objeto
                    chain = getThirdData.chain,
                    Evolution1 = url,
                    EvolveFrom = getSecondData.EvolveFrom,
                };
                if (url.id > getFirstData.id)
                {
                    pokemon.Evolution3 = url;
                    pokemon.Evolution1 = null;

                }
                return pokemon;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos del Pokémon: " + ex.Message);
                throw;
            }
        }

        public async Task<PokemonApiResponse> SetThirdEvolutionPokemonData(PokemonApiResponse url)
        {
            try
            {
                int idPokemonEvolution = _generalPokemonService.GetPokemonNumberFromURL(url.chain.EvolveTo[0].EvolveToPlus[0].species.url);
                PokemonApiResponse getFirstData = await GetPokemonDataByUrl(_urlApiPublicPokemonById + idPokemonEvolution);
                PokemonApiResponse getSecondData = await GetPokemonDataByUrl(url.chain.EvolveTo[0].EvolveToPlus[0].species.url);
                PokemonApiResponse getThirdData = await GetPokemonDataByUrl(getSecondData.evolutionChain.url);
                PokemonApiResponse pokemon = new PokemonApiResponse
                {
                    id = getFirstData.id,
                    name = getFirstData.name,
                    weight = getFirstData.weight,
                    types = getFirstData.types,
                    sprites = getFirstData.sprites,
                    species = getFirstData.species,
                    evolutionChain = getSecondData.evolutionChain,// Aquí se mantiene el officialArtwork del primer objeto
                    chain = getThirdData.chain,
                    EvolveFrom = getSecondData.EvolveFrom,
                };
                return pokemon;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos del Pokémon: " + ex.Message);
                throw;
            }
        }


        public async Task<PokemonApiResponse> CreatePokemonByDataCollect(PokemonApiResponse pokemonBase, PokemonApiResponse pokemonFirstEvolution, PokemonApiResponse pokemonSecondEvolution, string pokemonOrderConditional)
        {
            try
            {
                PokemonApiResponse pokemon = new();
                if (pokemonOrderConditional == "PokemonBase")
                {
                    pokemon = new PokemonApiResponse
                    {
                        id = pokemonBase.id,
                        name = pokemonBase.name,
                        weight = pokemonBase.weight,
                        types = pokemonBase.types,
                        sprites = pokemonBase.sprites,
                        species = pokemonBase.species,
                        evolutionChain = pokemonBase.evolutionChain,// Aquí se mantiene el officialArtwork del primer objeto
                        chain = pokemonBase.chain,
                        Evolution2 = pokemonFirstEvolution,
                        Evolution3 = pokemonSecondEvolution,
                        flavorTextEntries = pokemonBase.flavorTextEntries
                    };
                }
                if (pokemonOrderConditional == "PokemonFirstEvolution")
                {
                    pokemon = new PokemonApiResponse
                    {
                        id = pokemonFirstEvolution.id,
                        name = pokemonFirstEvolution.name,
                        weight = pokemonFirstEvolution.weight,
                        types = pokemonFirstEvolution.types,
                        sprites = pokemonFirstEvolution.sprites,
                        species = pokemonFirstEvolution.species,
                        evolutionChain = pokemonFirstEvolution.evolutionChain,// Aquí se mantiene el officialArtwork del primer objeto
                        EvolveFrom = pokemonFirstEvolution.EvolveFrom,
                        chain = pokemonFirstEvolution.chain,
                        Evolution1 = pokemonBase,
                        Evolution3 = pokemonSecondEvolution,
                    };
                }
                if (pokemonOrderConditional == "PokemonSecondEvolution")
                {
                    pokemon = new PokemonApiResponse
                    {
                        id = pokemonSecondEvolution.id,
                        name = pokemonSecondEvolution.name,
                        weight = pokemonSecondEvolution.weight,
                        types = pokemonSecondEvolution.types,
                        sprites = pokemonSecondEvolution.sprites,
                        species = pokemonSecondEvolution.species,
                        evolutionChain = pokemonSecondEvolution.evolutionChain,// Aquí se mantiene el officialArtwork del primer objeto
                        chain = pokemonSecondEvolution.chain,
                        Evolution1 = pokemonBase,
                        Evolution2 = pokemonFirstEvolution,
                        EvolveFrom = pokemonSecondEvolution.EvolveFrom,

                    };
                }
                return pokemon;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos del Pokémon: " + ex.Message);
                throw;
            }
        }



        public async Task<PokemonApiResponse> SetPokemonDataShort(string url)
        {
            try
            {
                int id = _generalPokemonService.GetPokemonNumberFromURL(url);
                PokemonApiResponse getFirstData = await GetPokemonDataByUrl(url);
                PokemonApiResponse getSecondData = await GetPokemonDataByUrl(getFirstData.species.url);
                PokemonApiResponse pokemon = new PokemonApiResponse
                {
                    id = getFirstData.id,
                    name = getFirstData.name,
                    sprites = getFirstData.sprites,
                    EvolveFrom = getSecondData.EvolveFrom
                    // ... agregar otros campos según sea necesario
                };
                return pokemon;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos del Pokémon: " + ex.Message);
                throw;
            }
        }


    }
}


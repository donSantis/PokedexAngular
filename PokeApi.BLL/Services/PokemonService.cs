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

namespace PokeApi.BLL.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IGenericRepository<Pokemon> _pokemonRepository;
        private readonly IPokePublicApiRepository _pokePublicApiRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly string _urlApiPublicPokemonById = "https://pokeapi.co/api/v2/pokemon/";


        public PokemonService(IGenericRepository<Pokemon> pokemonRepository, IPokePublicApiRepository pokePublicApiRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _pokePublicApiRepository = pokePublicApiRepository;
            _mapper = mapper;
            _httpClient = new HttpClient();
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
                List<PokemonApiResponse> pokemonesResponse = await Task.Run(() => GetPokemon(data));

                //Console.WriteLine(uwu);

                return data;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> ListPkmnByURL(string url)
        {
            try
            {
                var data = await _pokePublicApiRepository.ListPkmnByURL(url);
                PokemonApiResponse pokemonesResponse = await Task.Run(() => GetPokemonDataFromURL(url));

                //Console.WriteLine(uwu);

                return data;
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


        public async Task<string> FormatearJson(string data)
        {
            try
            {

                JObject jsonObject = JObject.Parse(data);

                return data;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<PokemonApiResponse>> GetPokemon(string data)
        {
            try
            {
                // Deserializar el JSON en un objeto ResponseApiClass
                ResponseApiClass response = JsonConvert.DeserializeObject<ResponseApiClass>(data);

                // Verificar si la lista de resultados no es nula
                if (response.results != null)
                {
                    // Crear una lista para almacenar los pokemones
                    List<PokemonApiResponse> pokemonesApiResponse = new List<PokemonApiResponse>();
                    List<PokemonApiResponse> pokemonSpeciesApiResponse = new List<PokemonApiResponse>();
                    List<Pokemon> pokemones = new List<Pokemon>();
                    // Crear una lista para almacenar las tareas de obtención de datos del Pokémon
                    List<Task<PokemonApiResponse>> pokemonTasks = new List<Task<PokemonApiResponse>>();

                    // Recorrer todos los objetos en la lista de resultados
                    foreach (var result in response.results)
                    {
                        // Obtener el PokemonApiResponse correspondiente y agregarlo a la lista
                        if (result.url == "https://pokeapi.co/api/v2/pokemon/19/")
                        {
                            var uwu = "uwu";
                        }
                        PokemonApiResponse pokemon = await GetPokemonDataFromURL(result.url);
                        pokemonesApiResponse.Add(pokemon);
                        Console.WriteLine("procesando.. pokemonnro:" + pokemon.name);
                    }
                    // Devolver la lista de pokemones
                    return pokemonesApiResponse;
                }
                else
                {
                    Console.WriteLine("La lista de resultados está vacía.");
                    return new List<PokemonApiResponse>(); // Devuelve una lista vacía si no hay resultados
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al procesar el JSON: " + ex.Message);
                return null; // Devuelve null si hay algún error
            }
        }



        public int GetPokemonNumberFromURL(string url)
        {
            try
            {
                // Encontrar la última ocurrencia de '/'
                int lastIndex = url.LastIndexOf('/');

                // Verificar si se encontró la barra
                if (lastIndex >= 0)
                {
                    // Encontrar el índice de la penúltima barra
                    int penultimateIndex = url.LastIndexOf('/', lastIndex - 1);

                    // Verificar si se encontró la penúltima barra
                    if (penultimateIndex >= 0)
                    {
                        // Obtener la parte del string que contiene el número
                        string numberString = url.Substring(penultimateIndex + 1, lastIndex - penultimateIndex - 1);

                        // Convertir la parte del string a un número entero
                        int number = int.Parse(numberString);

                        return number;
                    }
                    else
                    {
                        throw new InvalidOperationException("La URL no sigue el formato esperado.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("La URL no sigue el formato esperado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener el número de pokemon: " + ex.Message);
                throw;
            }
        }


        public async Task<PokemonApiResponse> GetPokemonDataFromURL(string url)
        {
            try
            {
                PokemonApiResponse BasePokemonData = new();
                PokemonApiResponse SecondEvolutionPokemonData = new();
                PokemonApiResponse ThirdEvolutionPokemonData = new();
                int uwu2 =0;
                int uwu3 = 0;
                int haveFirstEvolution = 0; 
                int haveSecondEvolution = 0;

                if (url != null)
                {
                    #region oldCode
                    //PokemonApiResponse getFirstData = await GetPokemonDataByUrl(url);
                    //PokemonApiResponse getSecondData = await GetPokemonDataByUrl(getFirstData.species.url);
                    //PokemonApiResponse getThirdData = await GetPokemonDataByUrl(getSecondData.evolutionChain.url);
                    //PokemonApiResponse FirstPokemonData = new PokemonApiResponse
                    //{
                    //    id = getFirstData.id,
                    //    name = getFirstData.name,
                    //    weight = getFirstData.weight,
                    //    types = getFirstData.types,
                    //    sprites = getFirstData.sprites,
                    //    species = getFirstData.species,
                    //    //chain = pokemonData3.chain
                    //    evolutionChain = getSecondData.evolutionChain,// Aquí se mantiene el officialArtwork del primer objeto
                    //    chain = getThirdData.chain,
                    //    Evolution2 = ThirdEvolutionPokemonData,
                    //    Evolution3 = SecondEvolutionPokemonData,
                    //    //firstEvolution = pokemonData3.chain.EvolveTo.species.name
                    //    // ... agregar otros campos según sea necesario
                    //};

                    //if (getThirdData.chain.EvolveTo[0].species.url != null)
                    //{
                    //    int idPokemonEvolution = GetPokemonNumberFromURL(getThirdData.chain.EvolveTo[0].species.url);
                    //    PokemonApiResponse getFirstData2 = await GetPokemonDataByUrl(_urlApiPublicPokemonById + idPokemonEvolution);
                    //    PokemonApiResponse getSecondData2 = await GetPokemonDataByUrl(getThirdData.chain.EvolveTo[0].species.url);
                    //    PokemonApiResponse getThirdData2 = await GetPokemonDataByUrl(getSecondData2.evolutionChain.url);

                    //    SecondEvolutionPokemonData = new PokemonApiResponse
                    //    {
                    //        id = getFirstData2.id,
                    //        name = getFirstData2.name,
                    //        weight = getFirstData2.weight,
                    //        types = getFirstData2.types,
                    //        sprites = getFirstData2.sprites,
                    //        species = getFirstData2.species,
                    //        //chain = pokemonData3.chain
                    //        evolutionChain = getSecondData2.evolutionChain,// Aquí se mantiene el officialArtwork del primer objeto
                    //        chain = getThirdData2.chain,
                    //        Evolution1 = ThirdEvolutionPokemonData,

                    //        //firstEvolution = pokemonData3.chain.EvolveTo.species.name
                    //        // ... agregar otros campos según sea necesario
                    //    };

                    //    FirstPokemonData.Evolution2 = SecondEvolutionPokemonData;

                    //    if (getThirdData.chain.EvolveTo[0].EvolveToPlus[0].species.url != null)
                    //    {
                    //        int idPokemonEvolution2 = GetPokemonNumberFromURL(getThirdData.chain.EvolveTo[0].EvolveToPlus[0].species.url);
                    //        PokemonApiResponse getFirstData3 = await GetPokemonDataByUrl(_urlApiPublicPokemonById + idPokemonEvolution2);
                    //        PokemonApiResponse getSecondData3 = await GetPokemonDataByUrl(getThirdData.chain.EvolveTo[0].EvolveToPlus[0].species.url);
                    //        PokemonApiResponse getThirdData3 = await GetPokemonDataByUrl(getSecondData3.evolutionChain.url);

                    //        ThirdEvolutionPokemonData = new PokemonApiResponse
                    //        {
                    //            id = getFirstData3.id,
                    //            name = getFirstData3.name,
                    //            weight = getFirstData3.weight,
                    //            types = getFirstData3.types,
                    //            sprites = getFirstData3.sprites,
                    //            species = getFirstData3.species,
                    //            //chain = pokemonData3.chain
                    //            evolutionChain = getSecondData3.evolutionChain,// Aquí se mantiene el officialArtwork del primer objeto
                    //            chain = getThirdData3.chain,
                    //            Evolution1 = ThirdEvolutionPokemonData,
                    //            Evolution2 = ThirdEvolutionPokemonData,

                    //            //firstEvolution = pokemonData3.chain.EvolveTo.species.name
                    //            // ... agregar otros campos según sea necesario
                    //        };
                    //        FirstPokemonData.Evolution3 = ThirdEvolutionPokemonData;

                    //    }

                    //}
                    #endregion 
                    


                    PokemonApiResponse uwu = await SetPokemonData(url);
                    PokemonApiResponse uwu4 = new();
                    PokemonApiResponse uwu5 = new();
                    PokemonApiResponse pokemonBase = new();
                    PokemonApiResponse pokemonFirstEvolution = new();
                    PokemonApiResponse pokemonSecondEvolution = new();



                    if (uwu.chain.EvolveTo.Count > 0)
                    {
                        uwu2 = GetPokemonNumberFromURL(uwu.chain.EvolveTo[0].species.url);
                        haveFirstEvolution = 1;
                        
                    }

                    if (uwu.chain.EvolveTo[0].EvolveToPlus.Count > 0)
                        {
                            uwu3 = GetPokemonNumberFromURL(uwu.chain.EvolveTo[0].EvolveToPlus[0].species.url);
                            haveSecondEvolution = 1;
                        }


               


                    PokemonApiResponse pokemonReturned = new();

                    if (uwu.EvolveFrom == null)
                    {
                        pokemonBase = uwu;

                        if(haveFirstEvolution == 1)
                        {
                            int idPokemonFirstEvolution = GetPokemonNumberFromURL(pokemonBase.chain.EvolveTo[0].species.url);
                            pokemonFirstEvolution = await SetPokemonDataShort(_urlApiPublicPokemonById + idPokemonFirstEvolution + '/');
                        }

                        if(haveSecondEvolution == 1)
                        {
                            int idPokemonSecondEvolution = GetPokemonNumberFromURL(pokemonBase.chain.EvolveTo[0].EvolveToPlus[0].species.url);
                            pokemonSecondEvolution = await SetPokemonDataShort(_urlApiPublicPokemonById + idPokemonSecondEvolution + '/');
                        }

                        var pokemonOrderConditional = "PokemonBase";
                        pokemonReturned = await CreatePokemonByDataCollect(pokemonBase, pokemonFirstEvolution, pokemonSecondEvolution, pokemonOrderConditional);

                    }
                    if (uwu.evolutionChain != null && uwu.id ==  uwu2 && uwu2 > 0)
                    {
                        int idPokemonBase = GetPokemonNumberFromURL(uwu.EvolveFrom.url);
                        pokemonBase = await SetPokemonDataShort(_urlApiPublicPokemonById + idPokemonBase + '/');

                        pokemonFirstEvolution = uwu;

                        if (haveSecondEvolution == 1)
                        {
                            int idPokemonSecondEvolution = GetPokemonNumberFromURL(uwu.chain.EvolveTo[0].EvolveToPlus[0].species.url);
                            pokemonSecondEvolution = await SetPokemonDataShort(_urlApiPublicPokemonById + idPokemonSecondEvolution + '/');
                        }
                        

                        var pokemonOrderConditional = "PokemonFirstEvolution";
                        pokemonReturned = await CreatePokemonByDataCollect(pokemonBase, pokemonFirstEvolution, pokemonSecondEvolution, pokemonOrderConditional);


                    }
                    if (uwu.evolutionChain != null && uwu.id > uwu2 && uwu.id == uwu3 && uwu3 > 0 || uwu.evolutionChain != null && uwu.id >= uwu2 && uwu.id == uwu3 && uwu3 > 0)
                    {
                        int idPokemonSecondEvolution = GetPokemonNumberFromURL(uwu.chain.EvolveTo[0].species.url);
                        pokemonFirstEvolution = await SetPokemonDataShort(_urlApiPublicPokemonById + idPokemonSecondEvolution + '/');

                        int idPokemonBase = GetPokemonNumberFromURL(pokemonFirstEvolution.EvolveFrom.url);
                        pokemonBase = await SetPokemonDataShort(_urlApiPublicPokemonById + idPokemonBase + '/');

                        pokemonSecondEvolution = uwu;

                        var pokemonOrderConditional = "PokemonSecondEvolution";
                        pokemonReturned = await CreatePokemonByDataCollect(pokemonBase, pokemonFirstEvolution, pokemonSecondEvolution, pokemonOrderConditional);

                    }

                    return pokemonReturned;
                }
                else
                {
                    throw new HttpRequestException($"La solicitud a {url} no fue exitosa. Código de estado: ");
                }
            }
            catch (Exception ex)
            {
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
                //int id = GetPokemonNumberFromURL(url);
                PokemonApiResponse getFirstData = await GetPokemonDataByUrl(url);
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
                    chain = getThirdData.chain,
                    evolutionChain = getSecondData.evolutionChain,// Aquí se mantiene el officialArtwork del primer objeto
                    EvolveFrom = getSecondData.EvolveFrom,
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
                int idPokemonEvolution2 = GetPokemonNumberFromURL(url.chain.EvolveTo[0].species.url);

                PokemonApiResponse getFirstData = await GetPokemonDataByUrl(_urlApiPublicPokemonById+ idPokemonEvolution2);
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
                int idPokemonEvolution = GetPokemonNumberFromURL(url.chain.EvolveTo[0].EvolveToPlus[0].species.url);

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


        public async Task<PokemonApiResponse> CreatePokemonByDataCollect(PokemonApiResponse pokemonBase, PokemonApiResponse pokemonFirstEvolution, PokemonApiResponse pokemonSecondEvolution,string pokemonOrderConditional)
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
                int id = GetPokemonNumberFromURL(url);
                PokemonApiResponse getFirstData = await GetPokemonDataByUrl(url);
                PokemonApiResponse getSecondData = await GetPokemonDataByUrl(getFirstData.species.url);
                PokemonApiResponse getThirdData = await GetPokemonDataByUrl(getSecondData.evolutionChain.url);
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

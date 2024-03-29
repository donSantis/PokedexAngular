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

namespace PokeApi.BLL.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IGenericRepository<Pokemon> _pokemonRepository;
        private readonly IPokePublicApiRepository _pokePublicApiRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;


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
                var uwu = FormatearJson(data);
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
                //ResponseApiClass responseJson = this.getPokemon(data);
                List<PokemonApiResponse> pokemonesResponse = await Task.Run(() => GetPokemon(data));

                Console.WriteLine(pokemonesResponse.Count);



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
                    List<Pokemon> pokemones = new List<Pokemon>();


                    // Crear una lista para almacenar las tareas de obtención de datos del Pokémon
                    List<Task<PokemonApiResponse>> pokemonTasks = new List<Task<PokemonApiResponse>>();

                    // Recorrer todos los objetos en la lista de resultados
                    foreach (var result in response.results)
                    {
                        // Crear un nuevo objeto Pokemon y asignar los valores
                        int idFromUrl = this.GetPokemonNumberFromURL(result.url);
                        pokemonTasks.Add(GetPokemonDataFromURL(result.url));
                    }

                    // Esperar a que todas las tareas de obtención de datos del Pokémon se completen
                    await Task.WhenAll(pokemonTasks);

                    // Agregar los pokemones obtenidos a la lista de pokemones
                    foreach (var pokemonTask in pokemonTasks)
                    {
                        pokemonesApiResponse.Add(await pokemonTask);
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
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    responseData = responseData.Replace("official-artwork:", "officialArtwork:");
                    PokemonApiResponse pokemonData = JsonConvert.DeserializeObject<PokemonApiResponse>(responseData);
                    return pokemonData;
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




    }
}

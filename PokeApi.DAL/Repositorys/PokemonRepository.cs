using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PokeApi.DAL.DBContext;
using PokeApi.DAL.Repositorys.Contract;
using PokeApi.Model;

namespace PokeApi.DAL.Repositorys
{
    public class PokemonRepository : GenericRepository<Pokemon>, IPokemonRepository
    {
        private readonly PokedexdbContext _dbContext;
        private readonly HttpClient _httpClient;

        public PokemonRepository(PokedexdbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
        }

        public async Task<string> ListAllFirstGenerationPkmn()
        {
            try
            {
                // Construye la URL completa utilizando el BaseAddress y la ruta relativa
                var url = $"{_httpClient.BaseAddress}pokemon";

                // Realiza la solicitud GET utilizando el HttpClient
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Lee el contenido de la respuesta como una cadena
                string responseBody = await response.Content.ReadAsStringAsync();

                // Retornar los datos obtenidos
                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al hacer la solicitud HTTP: {ex.Message}");
                throw;
            }
        }

        public async Task<Pokemon> Register(Pokemon model)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Pokemon pokemonFind = _dbContext.Pokemons.FirstOrDefault(p => p.IdPokemon == model.IdPokemon);
                    if (pokemonFind == null)
                    {
                        // No existe un Pokémon con el mismo IdPokemon, así que puedes agregar el nuevo Pokémon.
                        _dbContext.Pokemons.Add(model);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        // Puedes manejar el caso en el que ya existe un Pokémon con el mismo IdPokemon si es necesario.
                        // Por ejemplo, lanzar una excepción, realizar alguna acción especial, etc.
                        transaction.Rollback();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                    // Manejar la excepción, ya sea lanzándola nuevamente o realizando alguna acción específica.
                }
            }
            return model;
        }

        public async Task<string> ListAllPkmnFromGenerationByUrl(string uwu)
        {
            try
            {
                // Construye la URL completa utilizando el BaseAddress y la ruta relativa
                var url = uwu;

                // Realiza la solicitud GET utilizando el HttpClient
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Lee el contenido de la respuesta como una cadena
                string responseBody = await response.Content.ReadAsStringAsync();

                // Retornar los datos obtenidos
                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al hacer la solicitud HTTP: {ex.Message}");
                throw;
            }
        }

        public async Task<string> ListPkmnByURL(string uwu)
        {
            try
            {
                // Construye la URL completa utilizando el BaseAddress y la ruta relativa
                var url = uwu;

                // Realiza la solicitud GET utilizando el HttpClient
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Lee el contenido de la respuesta como una cadena
                string responseBody = await response.Content.ReadAsStringAsync();

                // Retornar los datos obtenidos
                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al hacer la solicitud HTTP: {ex.Message}");
                throw;
            }
        }

    }
}

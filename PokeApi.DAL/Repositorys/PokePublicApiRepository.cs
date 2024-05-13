using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PokeApi.DAL.DBContext;
using PokeApi.DAL.Repositorys.Contract;
using PokeApi.Model.Filter;
using PokeApi.Model.PokeApiClasses;

namespace PokeApi.DAL.Repositorys
{
    public class PokePublicApiRepository : GenericRepository<Pokemon>, IPokePublicApiRepository
    {
        private readonly HttpClient _httpClient;

        public PokePublicApiRepository(PokedexdbContext dbContext) : base(dbContext)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
        }

        public async Task<string> getAll()
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

        public Task<string> ListAllFirstGenerationPkmn()
        {
            throw new NotImplementedException();
        }

        public async Task<string> getPokemonByFilter(string filter)
        {
            try
            {
                // Construye la URL completa utilizando el BaseAddress y la ruta relativa
                Filter filterObject = JsonConvert.DeserializeObject<Filter>(filter);
                // Construye el paginador que utiliza la api de pokeapi


                HttpResponseMessage response = await _httpClient.GetAsync(filterObject.filterPages.actualPageUrl);
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


        public async Task<string> getPokemonByFilter(Filter filter)
        {
            try
            {
                // Construye la URL completa utilizando el BaseAddress y la ruta relativa
                // Construye el paginador que utiliza la api de pokeapi


                HttpResponseMessage response = await _httpClient.GetAsync(filter.filterPages.actualPageUrl);
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

        public async Task<string> ListPkmnByURL(string url)
        {
            try
            {
                // Construye la URL completa utilizando el BaseAddress y la ruta relativa

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

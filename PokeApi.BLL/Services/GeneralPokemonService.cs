using AutoMapper;
using Newtonsoft.Json;
using PokeApi.Model.Filter;
using PokeApi.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PokeApi.BLL.Services
{
    public class GeneralPokemonService
    {
        private readonly IMapper _mapper;

        public GeneralPokemonService(IMapper mapper)
        {
            _mapper = mapper;
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

        public ResponseApiClass GetPokemonResponseData(string data)
        {
            try
            {
                ResponseApiClass response = JsonConvert.DeserializeObject<ResponseApiClass>(data);


                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener el número de pokemon: " + ex.Message);
                throw;
            }
        }

        public string GetUrlFromfilter(string filter)
        {
            try
            {
                var url = "";
                Filter filterObject = JsonConvert.DeserializeObject<Filter>(filter);

                if (filterObject.filterPages.nextPageUrl == null && filterObject.filterPages.offSet == null)
                {
                    filterObject.filterPages.actualPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + filterObject.albumBase.pokemonStart + "&limit=20";
                    filterObject.filterPages.nextPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + (filterObject.albumBase.pokemonStart + 20) + "&limit=20";
                    filterObject.filterPages.actualPage = 1;
                    filterObject.filterPages.nextPage = 2;
                    filterObject.filterPages.prevPage = 1;
                    filterObject.filterPages.offSet = 20;

                }
                else
                {
                    filterObject.filterPages.prevPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + (filterObject.filterPages.actualOffSet - 20) + "&limit=20";

                    filterObject.filterPages.actualPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + filterObject.filterPages.actualOffSet + "&limit=20";
                    filterObject.filterPages.nextPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + (filterObject.filterPages.actualOffSet + 20) + "&limit=20";
                    filterObject.filterPages.actualPage += 1;
                    filterObject.filterPages.nextPage += 1;
                    filterObject.filterPages.prevPage -= 1;

                }
                string jsonString = JsonConvert.SerializeObject(filterObject);

                return jsonString;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener la url de los pokemones a consultar: " + ex.Message);
                throw;
            }
        }

        public string GetPagesForAlbumFromfilter(string filter)
        {
            try
            {
                var url = "";
                filter = GetUrlFromfilter(filter);

                Filter filterObject = JsonConvert.DeserializeObject<Filter>(filter);
                filterObject.filterPages.actualPageUrl = GetUrlFromfilter(filter);


                if (filterObject.filterPages.nextPageUrl == null && filterObject.filterPages.offSet == null)
                {
                    url = "https://pokeapi.co/api/v2/pokemon?offset=" + filterObject.albumBase.pokemonStart + "&limit=20";
                }
                else
                {
                    var offSet = 20 + filterObject.filterPages.offSet;
                    var limitPokemonPage = 20;
                    url = "https://pokeapi.co/api/v2/pokemon?offset=" + filterObject.albumBase.pokemonStart + "&limit=" + limitPokemonPage;

                }

                return url;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener la url de los pokemones a consultar: " + ex.Message);
                throw;
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public Filter GetUrlFromfilter(Filter filter)
        {
            try
            {
                var url = "";

                if (filter.filterPages.nextPageUrl == null)
                {
                    filter.filterPages.actualPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + filter.filterPages.offSet + "&limit=" + filter.filterPages.limitPokemonPage;
                    filter.filterPages.prevPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + filter.filterPages.offSet + "&limit=" + filter.filterPages.limitPokemonPage;
                    filter.filterPages.nextPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + (filter.filterPages.offSet + filter.filterPages.limitPokemonPage) + "&limit=" + filter.filterPages.limitPokemonPage;
                    filter.filterPages.actualPage = 1;
                    filter.filterPages.nextPage = 2;
                    filter.filterPages.prevPage = 1;
                    filter.filterPages.offSet = filter.filterPages.offSet;
                }
                else
                {
                    filter.filterPages.nextPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + (filter.filterPages.offSet + filter.filterPages.limitPokemonPage) + "&limit=" + filter.filterPages.limitPokemonPage;
                    filter.filterPages.actualPage += 1;
                    filter.filterPages.nextPage += 1;
                    filter.filterPages.offSet = filter.filterPages.offSet + filter.filterPages.limitPokemonPage;

                }
                return filter;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener la url de los pokemones a consultar: " + ex.Message);
                throw;
            }
        }

        public int GetRandomNumber(int id)
        {
            // Obtener la fecha y hora actual
            DateTime fechaHoraActual = DateTime.Now;

            // Combinar la fecha y hora actual con el ID de usuario
            string combinedString = $"{fechaHoraActual.ToString("yyyyMMddHHmmss")}{id}";

            // Calcular un hash para la cadena combinada
            int hashCode = combinedString.GetHashCode();

            // Asegurarse de que el hash sea positivo
            int numeroRandom = Math.Abs(hashCode);

            return numeroRandom;
        }

    }
}

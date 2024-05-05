using AutoMapper;
using Newtonsoft.Json;
using PokeApi.Model;
using PokeApi.Model.Filter;
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

                if (filterObject.Pages.nextPageUrl == null && filterObject.Pages.offSet == null)
                {
                    filterObject.Pages.actualPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + filterObject.AlbumBase.pokemonStart + "&limit=20";
                    filterObject.Pages.nextPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + (filterObject.AlbumBase.pokemonStart + 20) + "&limit=20";
                    filterObject.Pages.actualPage = 1;
                    filterObject.Pages.nextPage = 2;
                    filterObject.Pages.prevPage = 1;
                    filterObject.Pages.offSet = 20;

                }
                else
                {
                    filterObject.Pages.prevPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + (filterObject.Pages.actualOffSet - 20) + "&limit=20";

                    filterObject.Pages.actualPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + filterObject.Pages.actualOffSet + "&limit=20";
                    filterObject.Pages.nextPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + (filterObject.Pages.actualOffSet + 20) + "&limit=20";
                    filterObject.Pages.actualPage += 1;
                    filterObject.Pages.nextPage +=  1;
                    filterObject.Pages.prevPage -= 1;

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
                filterObject.Pages.actualPageUrl = GetUrlFromfilter(filter);


                if (filterObject.Pages.nextPageUrl == null && filterObject.Pages.offSet == null)
                {
                    url = "https://pokeapi.co/api/v2/pokemon?offset=" + filterObject.AlbumBase.pokemonStart + "&limit=20";
                }
                else
                {
                    var offSet = 20 + filterObject.Pages.offSet;
                    var limitPokemonPage = 20;
                    url = "https://pokeapi.co/api/v2/pokemon?offset=" + filterObject.AlbumBase.pokemonStart + "&limit=" + limitPokemonPage;

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

                if (filter.Pages.nextPageUrl == null && filter.Pages.offSet == null)
                {
                    filter.Pages.actualPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + filter.AlbumBase.pokemonStart + "&limit=20";
                    filter.Pages.nextPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + (filter.AlbumBase.pokemonStart + 20) + "&limit=20";
                    filter.Pages.actualPage = 1;
                    filter.Pages.nextPage = 2;
                    filter.Pages.prevPage = 1;
                    filter.Pages.offSet = 20;
                }
                //else
                //{
                //    filter.Pages.prevPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + 4 + "&limit=20";

                //    filter.Pages.actualPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + filter.Pages.actualOffSet + "&limit=20";
                //    filter.Pages.nextPageUrl = "https://pokeapi.co/api/v2/pokemon?offset=" + (filter.Pages.actualOffSet + 20) + "&limit=20";
                //    filter.Pages.actualPage += 1;
                //    filter.Pages.nextPage += 1;
                //    filter.Pages.prevPage -= 1;

                //}
                return filter;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener la url de los pokemones a consultar: " + ex.Message);
                throw;
            }
        }
    }
}

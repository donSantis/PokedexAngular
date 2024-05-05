using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PikeApi.DTO;
using PokeApi.Model;
using PokeApi.Model.Filter;
using PokeApi.Model.PokeApiClasses;


namespace PokeApi.BLL.Services.Contract
{
    public interface IPokemonService
    {
        Task<List<Pokemon_DTO>> List();
        Task<Pokemon_DTO> Create(Pokemon_DTO model);
        Task<bool> update(Pokemon_DTO model);
        Task<bool> Delete(int id);
        Task<string> ListAllFirstGenerationPkmn();
        Task<ReturnPokemonApiResponseClass> ListAllPkmnFromApi();
        Task<ReturnPokemonApiResponseClass> ListPokemonFromApiWithFilters(Filter filter);
        Task<string> ListPkmnByURL(string url);
        Task<PokemonApiResponse> GetPkmnByURL(string url);

    }
}

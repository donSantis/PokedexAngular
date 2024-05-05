using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApi.Model;

namespace PokeApi.DAL.Repositorys.Contract
{
    public interface IPokemonRepository : IGenericRepository<Pokemon>
    {
        Task<Pokemon> Register(Pokemon model);
        Task<string> ListAllFirstGenerationPkmn(); // Agrega esta definición al método
        Task<string> ListAllPkmnFromGenerationByUrl(string url);
        Task<string> ListPkmnByURL(string url);

    }
}

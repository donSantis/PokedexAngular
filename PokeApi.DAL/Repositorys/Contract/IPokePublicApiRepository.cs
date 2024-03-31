using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApi.Model;

namespace PokeApi.DAL.Repositorys.Contract
{
    public interface IPokePublicApiRepository : IGenericRepository<Pokemon>
    {
        Task<string> getAll();
        Task<string> ListAllFirstGenerationPkmn();
        Task<string> ListPkmnByURL(string url);

    }
}


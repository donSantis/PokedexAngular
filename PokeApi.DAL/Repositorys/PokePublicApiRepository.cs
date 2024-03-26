using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApi.DAL.DBContext;
using PokeApi.DAL.Repositorys.Contract;
using PokeApi.Model;

namespace PokeApi.DAL.Repositorys
{
    public class PokePublicApiRepository : GenericRepository<Pokemon>, IPokePublicApiRepository
    {
        public PokePublicApiRepository(PokedexdbContext dbContext) : base(dbContext)
        {
        }

        public Task<Pokemon> getAll()
        {
            throw new NotImplementedException();
        }

        public Task<string> ListAllFirstGenerationPkmn()
        {
            throw new NotImplementedException();
        }
    }
}

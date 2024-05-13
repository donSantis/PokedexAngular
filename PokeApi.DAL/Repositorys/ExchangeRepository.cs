using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PokeApi.DAL.DBContext;
using PokeApi.DAL.Repositorys.Contract;
using PokeApi.Model;
using PokeApi.Model.Filter;
using PokeApi.Model.Sticker;
using PikeApi.DTO;
using PokeApi.Model.Exchange;

namespace PokeApi.DAL.Repositorys
{
    public class ExchangeRepository : GenericRepository<Exchanges>, IExchangeRepository
    {
        private readonly PokedexdbContext _dbContext;
        private readonly HttpClient _httpClient;

        public ExchangeRepository(PokedexdbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
        }

        public Task<string> getExchanges(Filter filter)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    List<Exchanges> data = _dbContext.Exchange.ToList();
                    if (data != null)
                    {
                    }
                    return null;


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                    // Manejar la excepción, ya sea lanzándola nuevamente o realizando alguna acción específica.
                }
            }
        }
    }
}

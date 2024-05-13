using PokeApi.Model.Exchange;
using PokeApi.Model.Filter;
using PokeApi.Model.Sticker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.DAL.Repositorys.Contract
{
    public interface IExchangeRepository : IGenericRepository<Exchanges>
    {
        Task<string> getExchanges(Filter filter);
    
    }
}

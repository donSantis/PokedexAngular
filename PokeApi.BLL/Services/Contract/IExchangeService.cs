using PikeApi.DTO;
using PokeApi.Model.Exchange;
using PokeApi.Model.Filter;
using PokeApi.Model.Sticker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.BLL.Services.Contract
{
    public interface IExchangeService
    {
        Task<List<Exchanges>> List(Filter filter);
        Task<List<Exchanges>> CreateExchange(Filter filter);
        Task<List<Exchanges>> UpdateExchange(Filter filter);


    }
}

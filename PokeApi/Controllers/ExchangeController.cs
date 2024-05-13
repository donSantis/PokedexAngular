using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokeApi.BLL.Services;
using PokeApi.BLL.Services.Contract;
using PikeApi.DTO;
using PokeApi.API.Utility;
using PokeApi.Model;
using PokeApi.Model.Album;
using PokeApi.Model.Sticker;
using PokeApi.Model.Filter;
using PokeApi.Model.Exchange;

namespace PokeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        [HttpPost]
        [Route("List")]
        public async Task<IActionResult> List(Filter filter)
        {
            var rsp = new Response<List<Exchanges>>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _exchangeService.List(filter);
            }
            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;

            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("CreateExchange")]
        public async Task<IActionResult> CreateExchange(Filter filter)
        {
            var rsp = new Response<List<Exchanges>>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _exchangeService.CreateExchange(filter);
            }
            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;

            }
            return Ok(rsp);
        }
        [HttpPut]
        [Route("UpdateExchange")]
        public async Task<IActionResult> UpdateExchange(Filter filter)
        {
            var rsp = new Response<List<Exchanges>>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _exchangeService.UpdateExchange(filter);
            }
            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;

            }
            return Ok(rsp);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokeApi.BLL.Services;
using PokeApi.BLL.Services.Contract;
using PikeApi.DTO;
using PokeApi.API.Utility;
using PokeApi.Model;
using Newtonsoft.Json;
using PokeApi.Model.PokeApiClasses;


namespace PokeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokeService;
        private readonly ApiPublicPokemonService _apiPublicPokemonService;
        private readonly GeneralPokemonService _GeneralPokemonService;

        public PokemonController(IPokemonService pokeService)
        {
            _pokeService = pokeService;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var rsp = new Response<List<Pokemon_DTO>>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _pokeService.List();

            }
            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;

            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ListAllPkmnFromApi")]
        public async Task<IActionResult> ListAllPkmnFromApi()
        {
            //var rsp = new ResponseString();
            var rsp = new PokemonResponse();

            try
            {
                var uwu = await _pokeService.ListAllPkmnFromApi();
                rsp.previous = uwu.previous;
                rsp.next = uwu.next;
                rsp.results = uwu.results;
                rsp.count = uwu.count;
                //rsp.rsp = await _pokeService.ListAllPkmnFromApi();

            }
            catch (Exception ex)
            {


            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("List3")]
        public async Task<IActionResult> List3(string url)
        {
            var rsp = new ResponseString();
            try
            {

                rsp.rsp = await _pokeService.ListPkmnByURL(url);
            }
            catch (Exception ex)
            {


            }
            return Ok(rsp);
        }


        [HttpPost]
        [Route("CreatePokemon")]
        public async Task<IActionResult> CreatePokemon([FromBody]Pokemon_DTO pokemon)
        {
            var rsp = new Response<Pokemon_DTO>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _pokeService.Create(pokemon);
            }
            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;

            }
            return Ok(rsp);
        }

        [HttpPut]
        [Route("UpdatePokemon")]
        public async Task<IActionResult> UpdateUser([FromBody] Pokemon_DTO pokemon)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _pokeService.update(pokemon);
            }
            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;

            }
            return Ok(rsp);
        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> DeletePokemon(int id)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _pokeService.Delete(id);
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

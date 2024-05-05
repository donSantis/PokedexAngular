using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokeApi.BLL.Services;
using PokeApi.BLL.Services.Contract;
using PikeApi.DTO;
using PokeApi.API.Utility;
using PokeApi.Model;
using Newtonsoft.Json;
using PokeApi.Model.PokeApiClasses;
using System.Linq;
using PokeApi.Model.Album;
using static System.Runtime.InteropServices.JavaScript.JSType;
using PokeApi.Model.Filter;


namespace PokeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokeService;
        private readonly ApiPublicPokemonService _apiPublicPokemonService;
        private readonly GeneralPokemonService _generalPokemonService;

        public PokemonController(IPokemonService pokeService, GeneralPokemonService generalPokemonService)
        {
            _pokeService = pokeService;
            _generalPokemonService = generalPokemonService;
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

        //[HttpGet]
        //[Route("ListAllPkmnFromApiWithFilters")]
        //public async Task<IActionResult> ListAllPkmnFromApiWithFilters(string filter)
        //{
        //    var rsp = new PokemonResponse();
        //    try
        //    {
        //        var filtro = _generalPokemonService.GetUrlFromfilter(filter);
        //        var dataPokemon = await _pokeService.ListAllPkmnFromApiWithFilters(filter);
        //        ReturnPokemonApiResponseClass pokemonesResponse = await Task.Run(() => _apiPublicPokemonService.ListAllPkmnFromGenerationByUrl(dataPokemon));


        //        Filter filterObject = JsonConvert.DeserializeObject<Filter>(filtro);
        //        ReturnPokemonApiResponseClass filterObject2 = JsonConvert.DeserializeObject<ReturnPokemonApiResponseClass>(dataPokemon);

        //        rsp.results = filterObject2.results;
        //        string filterJson = JsonConvert.SerializeObject(filterObject);
        //        //rsp.rsp = await _pokeService.ListAllPkmnFromApi();

        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //    return Ok(rsp);
        //}


        //[HttpGet]
        //[Route("ListAllPkmnFromGenerationByUrl")]
        //public async Task<IActionResult> ListAllPkmnFromGenerationByUrl(string url)
        //{
        //    var rsp = new PokemonResponse();
        //    try
        //    {
        //        var uwu = await _pokeService.ListAllPkmnFromGenerationByUrl(url);
        //        rsp.results = uwu.results;
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //    return Ok(rsp);
        //}

        [HttpGet]
        [Route("ListPokemonFromApiToAlbumByAlbum")]
        public async Task<IActionResult> ListPokemonFromApiToAlbumByAlbum(string album)
        {
            var rsp = new PokemonResponse();
            try
            {
                AlbumBase data = JsonConvert.DeserializeObject<AlbumBase>(album);


                string jsonString = JsonConvert.SerializeObject(data);
                //var uwu = await _pokeService.ListAllPkmnFromGenerationByUrl(url);
                //  rsp.results = uwu.results;
            }
            catch (Exception ex)
            {


            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("GetPkmnByUrl")]
        public async Task<IActionResult> GetPkmnByUrl(string url)
        {
            var rsp = new PokemonApiResponse();
            try
            {
                rsp = await _pokeService.GetPkmnByURL(url);
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


        //---------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        [Route("ListPokemonFromApiWithFilters")]
        public async Task<IActionResult> ListPokemonFromApiWithFilters([FromBody] Filter filter)
        {
            var rsp = new PokemonResponse();
            try
            {
                ReturnPokemonApiResponseClass dataPokemon = await _pokeService.ListPokemonFromApiWithFilters(filter);
                Filter filtro = _generalPokemonService.GetUrlFromfilter(filter);
                rsp.next = filtro.Pages.nextPageUrl;
                rsp.previous = filtro.Pages.prevPageUrl;
                rsp.results = dataPokemon.results;
            }
            catch (Exception ex)
            {


            }
            return Ok(rsp);
        }

    }
}

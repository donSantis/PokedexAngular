using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokeApi.BLL.Services;
using PokeApi.BLL.Services.Contract;
using PikeApi.DTO;
using PokeApi.API.Utility;
using PokeApi.Model;
using PokeApi.Model.Album;

namespace PokeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var rsp = new Response<List<AlbumBase>>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _albumService.List();
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

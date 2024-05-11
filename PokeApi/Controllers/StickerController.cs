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


namespace PokeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StickerController : ControllerBase
    {
        private readonly IStickerService _stickerService;


        public StickerController(IStickerService stickerService)
        {
            _stickerService = stickerService;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List(int id)
        {
            var rsp = new Response<List<Sticker_DTO>>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _stickerService.List(id);
            }
            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;

            }
            return Ok(rsp);
        }


        [HttpPost]
        [Route("CreateStickerBox")]
        public async Task<IActionResult> CreateStickerBox(Filter filter)
        {
            var rsp = new Response<StickerBox>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _stickerService.CreateStickerBox(filter);
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

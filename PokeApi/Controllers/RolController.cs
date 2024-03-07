using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokeApi.BLL.Services.Contract;
using PikeApi.DTO;
using PokeApi.API.Utility;

namespace PokeApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var rsp = new Response<List<Rol_DTO>>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _rolService.GetAll();
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

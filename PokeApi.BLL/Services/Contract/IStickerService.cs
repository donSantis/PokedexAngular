using PikeApi.DTO;
using PokeApi.Model.Filter;
using PokeApi.Model.Sticker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.BLL.Services.Contract
{
    public interface IStickerService
    {
        Task<List<Sticker_DTO>> List(int id);
        Task<StickerBox> CreateStickerBox(Filter filter);

    }
}

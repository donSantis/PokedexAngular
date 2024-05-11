using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApi.Model;
using PokeApi.Model.Filter;
using PokeApi.Model.Sticker;

namespace PokeApi.DAL.Repositorys.Contract
{
    public interface IStickerRepository : IGenericRepository<Stickers>
    {
        Task<string> getUserSticker(Filter filter);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApi.Model.Album;
 

namespace PokeApi.BLL.Services.Contract
{
    public interface IAlbumService
    {
        Task<List<AlbumBase>> List();
    }
}

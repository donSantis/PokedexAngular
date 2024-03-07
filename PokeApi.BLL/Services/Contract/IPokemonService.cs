using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PikeApi.DTO;


namespace PokeApi.BLL.Services.Contract
{
    public interface IPokemonService
    {
        Task<List<Pokemon_DTO>> List();
        Task<Pokemon_DTO> Create(Pokemon_DTO model);
        Task<bool> update(Pokemon_DTO model);
        Task<bool> Delete(int id);

    }
}

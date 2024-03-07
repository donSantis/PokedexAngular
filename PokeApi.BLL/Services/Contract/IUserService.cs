using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PikeApi.DTO;


namespace PokeApi.BLL.Services.Contract
{
    public interface IUserService
    {
        Task<List<User_DTO>> List();
        Task<Sesion_DTO> ValidateCredencial(string email,string password);
        Task<User_DTO> Create(User_DTO model);
        Task<bool> update(User_DTO model);
        Task<bool> Delete(int id);

    }
}

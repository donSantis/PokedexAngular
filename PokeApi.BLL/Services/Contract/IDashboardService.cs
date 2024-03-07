using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PikeApi.DTO;

namespace PokeApi.BLL.Services.Contract
{
    public interface IDashboardService
    {
        Task<Dashboard_DTO> Resume();
    }
}

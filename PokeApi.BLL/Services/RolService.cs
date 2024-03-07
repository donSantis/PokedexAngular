using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using PokeApi.BLL.Services.Contract;
using PokeApi.DAL.Repositorys.Contract;
using PikeApi.DTO;
using PokeApi.Model;

namespace PokeApi.BLL.Services
{
    public class RolService : IRolService
    {
        private readonly IGenericRepository<Rol> _rolRepository;
        private readonly IMapper _mapper;

        public RolService(IGenericRepository<Rol> rolRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
        }

        public async Task<List<Rol_DTO>> GetAll()
        {
            try
            {
                var roleList = await _rolRepository.Consulta();
                return _mapper.Map<List<Rol_DTO>>(roleList.ToList());
            }
            catch {
                throw;
            }
        }
    }
}

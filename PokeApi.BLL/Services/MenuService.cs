using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PokeApi.DAL.Repositorys.Contract;
using PikeApi.DTO;
using PokeApi.Model;
using PokeApi.BLL.Services.Contract;
using PokeApi.Model.Menu;

namespace PokeApi.BLL.Services
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<MenuRol> _menuRolRepository;
        private readonly IGenericRepository<Menu> _menuRepository;
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<User> userRepository, IGenericRepository<MenuRol> menuRolRepository, IGenericRepository<Menu> menuRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _menuRolRepository = menuRolRepository;
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<Menu_DTO>> List(int idUser)
        {
            try
            {
                // Obtener el rol del usuario incluyendo la propiedad de navegación Role
                var user = await _userRepository.Consulta(u => u.id == idUser);

                if (user == null || !user.Any() || user.First().idRol == null)
                {
                    throw new Exception("No se encontró el usuario o su rol asociado.");
                }

                var roleId = user.First().idRol;

                // Obtener los menús asociados al rol del usuario
                var menuRoles = await _menuRolRepository.Consulta(mr => mr.idRol == roleId);

                // Obtener los IDs de los menús asociados al rol del usuario
                var menuIds = menuRoles.Select(mr => mr.idMenu).ToList();

                // Obtener los detalles de los menús
                var menus = await _menuRepository.Consulta(m => menuIds.Contains(m.id));

                // Mapear los menús a DTO
                var menuDTOs = _mapper.Map<List<Menu_DTO>>(menus);

                return menuDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los menús del usuario.", ex);
            }
        }



    }
}

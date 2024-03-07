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
            IQueryable<User> tbUser = await _userRepository.Consulta(u => u.IdUser == idUser);
            IQueryable<MenuRol> tbMenuRol = await _menuRolRepository.Consulta();
            IQueryable<Menu> tbMenu = await _menuRepository.Consulta();
            try
            {
                IQueryable<Menu> tbResult = (from u in tbUser
                                             join mr in tbMenuRol on u.IdRol equals mr.IdRol
                                             join m in tbMenu on mr.IdMenu equals m.IdMenu
                                             select m).AsQueryable();
                var menuList = tbResult.ToList();
                return _mapper.Map<List<Menu_DTO>>(menuList);   
            }
            catch
            {
                throw;
            }
        }
    }
}

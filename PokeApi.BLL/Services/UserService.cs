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
using Microsoft.EntityFrameworkCore;

namespace PokeApi.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<User_DTO> Create(User_DTO model)
        {
            try
            {
                var user = await _userRepository.CreateModel(_mapper.Map<User>(model));
                if(user.IdUser == 0)
                    throw new TaskCanceledException("El usuario no fue creado");
                var query = await _userRepository.Consulta(u => u.IdUser == user.IdUser);
                user = query.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<User_DTO>(user);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var userFind = await _userRepository.GetModel(u => u.IdUser == id);
                if (userFind == null)
                    throw new TaskCanceledException("El usuario no existe");

                bool response = await _userRepository.DeleteModel(userFind);
                if (!response)
                    throw new TaskCanceledException("No se ha eliminado al usuario");
                return response;

            }
            catch {
                throw;
            }
        }

        public async Task<List<User_DTO>> List()
        {
            try
            {
                var query = await _userRepository.Consulta();
                var userList = query.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<User_DTO>>(userList);

            }
            catch {
                throw;
            }
        }

        public async Task<bool> update(User_DTO model)
        {
            try
            {
                var userModel = _mapper.Map<User>(model);
                var userFind = await _userRepository.GetModel(u => u.IdUser == userModel.IdUser);
                if(userFind == null)
                    throw new TaskCanceledException("El usuario no existe");
                userFind.Name = userModel.Name;
                userFind.SecondName = userModel.SecondName;
                userFind.Email = userModel.Email;
                userFind.IdRol = userModel.IdRol;
                userFind.Password = userModel.Password;
                userFind.Status = userModel.Status;
                bool response = await _userRepository.EditModel(userFind);
                if (!response)
                    throw new TaskCanceledException("No se han realizado cambios al usuario");
                return response;
            }
            catch {
                throw;
            }
        }

        public async Task<Sesion_DTO> ValidateCredencial(string email, string password)
        {
            try
            {
                var query = await _userRepository.Consulta( u => 
                u.Email == email &&
                u.Password == password
                );

                if (query.FirstOrDefault() == null)
                    throw new TaskCanceledException("El usuario no existe");

                User returnUser = query.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<Sesion_DTO>(returnUser);

            }
            catch {
                throw;
            }
        }
    }
}

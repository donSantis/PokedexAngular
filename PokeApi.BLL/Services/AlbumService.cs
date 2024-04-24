using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PokeApi.DAL.Repositorys.Contract;
using PokeApi.BLL.Services.Contract;
using PokeApi.Model.Album;
using PikeApi.DTO;
using PokeApi.DAL.Repositorys;
using PokeApi.Model;

namespace PokeApi.BLL.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IGenericRepository<AlbumBase> _albumBaseRepository;

        public AlbumService(IGenericRepository<AlbumBase> albumBaseRepository)
        {
            _albumBaseRepository = albumBaseRepository;
        }

        public async Task<List<AlbumBase>> List()
        {
            
            try
            {
                IQueryable<AlbumBase> tbAlbumBase = await _albumBaseRepository.Consulta();
                var albumBaseList = tbAlbumBase.ToList();
                return albumBaseList;
            }
            catch
            {
                throw;
            }
        }

    }
}

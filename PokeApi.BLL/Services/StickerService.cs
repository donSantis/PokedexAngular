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
using PokeApi.Model.Sticker;
using PokeApi.Model.Filter;

namespace PokeApi.BLL.Services
{
    public class StickerService : IStickerService
    {
        private readonly IGenericRepository<Stickers> _stickerRepository;
        private readonly IMapper _mapper;
        public StickerService(IGenericRepository<Stickers> stickerRepository, IMapper mapper)
        {
            _stickerRepository = stickerRepository;
            _mapper = mapper;
        }
        public async Task<List<Sticker_DTO>> List(int id)
        {
            try
            {
                IQueryable<Stickers> query = await _stickerRepository.Consulta(u => u.idUser == id && u.state == 1);
                var stickerList = query.ToList();
                return _mapper.Map<List<Sticker_DTO>>(stickerList);

            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Sticker_DTO>> ListWithFilter(Filter filter)
        {
            try
            {
                var query = await _stickerRepository.Consulta();
                var stickerList = query.ToList();
                return _mapper.Map<List<Sticker_DTO>>(stickerList);

            }
            catch
            {
                throw;
            }
        }

        public async Task<StickerBox> CreateStickerBox(Filter filter)
        {
            try
            {
                StickerBox stickerBox = new StickerBox();

                if (filter != null && filter.AlbumBase.pokemonStart != null && filter.AlbumBase.pokemonEnd != null)
                {
                    int pokemonStart = (int)filter.AlbumBase.pokemonStart;
                    int pokemonEnd = (int)filter.AlbumBase.pokemonEnd;
                    // Crear una lista para almacenar los stickers
                    List<Stickers> stickers = new List<Stickers>();

                    // Generar 5 stickers con valores aleatorios dentro del rango especificado
                    Random random = new Random();
                    for (int i = 0; i < 5; i++)
                    {
                        int randomValue = random.Next(pokemonStart, pokemonEnd + 1);
                        Stickers sticker = new Stickers
                        {
                            idPokemon = randomValue,
                            idUser = filter.user.IdUser,
                            state = 1,
                            version = filter.AlbumBase.version,
                            shiny = await IsShiny(randomValue),
                            registerDate = DateTime.Now

                        };
                        await _stickerRepository.CreateModel(sticker);

                        stickers.Add(sticker);
                    }
                    Console.WriteLine("uwu");

                    stickerBox.stickers = stickers;
                }
                return stickerBox;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Sticker_DTO> CreateSticker(Stickers model)
        {
            try
            {
                var data = await _stickerRepository.CreateModel(_mapper.Map<Stickers>(model));
                if (data.Id == 0)
                    throw new TaskCanceledException("El sticker no fue creado");
                var query = await _stickerRepository.Consulta(u => u.Id == data.Id);
                data = query.First();
                return _mapper.Map<Sticker_DTO>(data);

            }
            catch
            {
                throw;
            }
        }


        public async Task<int> IsShiny(int pokemonId)
        {
            try
            {
                // Aquí puedes diseñar una lógica personalizada para determinar si un Pokémon es shiny
                // Puedes considerar diferentes factores, como el ID del Pokémon y la probabilidad asignada a ciertos rangos de IDs

                // Ejemplo: Mayor probabilidad de ser shiny si el ID del Pokémon está en un rango específico
                if (pokemonId >= 100 && pokemonId <= 200)
                {
                    // Por ejemplo, si el ID está entre 100 y 200, hay un 20% de probabilidad de ser shiny
                    return (new Random().Next(1, 101) <= 20) ? 1 : 0; // 20% de probabilidad de ser shiny
                }
                else
                {
                    // Para otros IDs, probabilidad estándar de 1/4096 de ser shiny
                    return (new Random().Next(1, 4097) == 1) ? 1 : 0; // 1/4096 de probabilidad de ser shiny
                }
            }
            catch
            {
                throw;
            }
        }


    }
}

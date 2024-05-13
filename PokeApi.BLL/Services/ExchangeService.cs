using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using PokeApi.DAL.Repositorys.Contract;
using PikeApi.DTO;
using PokeApi.Model;
using PokeApi.BLL.Services.Contract;
using Microsoft.EntityFrameworkCore;
using PokeApi.Model.Sticker;
using PokeApi.Model.Filter;
using PokeApi.Model.Exchange;
using PokeApi.DAL.Repositorys;

namespace PokeApi.BLL.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly GeneralPokemonService _generalPokemonService;
        private readonly IGenericRepository<Exchanges> _exchangeRepository;
        private readonly IMapper _mapper;
        public ExchangeService(IGenericRepository<Exchanges> exchangeRepository, IMapper mapper, GeneralPokemonService generalPokemonService)
        {
            _exchangeRepository = exchangeRepository;
            _mapper = mapper;
            _generalPokemonService = generalPokemonService;

        }

        public async Task<List<Exchanges>> List(Filter filter)
        {
            try
            {
                // Obtener los IDs de usuario de los filtros
                List<int?> ids = filter.filterUser.idsUsers;

                // Lista para almacenar todos los intercambios encontrados
                List<Exchanges> exchangeList = new List<Exchanges>();

                // Iterar sobre cada ID de usuario
                foreach (int id in ids)
                {
                    // Consultar los intercambios que involucran al ID de usuario actual
                    IQueryable<Exchanges> query = await _exchangeRepository.Consulta(u =>
                        (u.idReceivingUser == id) ||
                        (u.idSenderUser == id));

                    // Agregar los intercambios encontrados a la lista principal
                    exchangeList.AddRange(await query.ToListAsync());
                }

                return exchangeList;
            }
            catch
            {
                throw;
            }
        }


        public async Task<List<Exchanges>> CreateExchange(Filter filter)
        {
            try
            {
                var exchanges = new List<Exchanges>();
                var id = _generalPokemonService.GetRandomNumber(filter.user.id);

                // Iterar sobre cada objeto Exchange en la lista del filtro
                foreach (var exchangeFilter in filter.filterExchange.exchanges)
                {
                    // Mapear el modelo de filtro a un objeto Exchange
                    var exchange = _mapper.Map<Exchanges>(exchangeFilter);
                    // Generar un número de intercambio aleatorio
                    exchange.idExchange = id;
                    // Establecer los campos específicos del intercambio actual
                    exchange.registerDate = DateTime.Now;
                    // Crear el intercambio en el repositorio
                    var createdExchange = await _exchangeRepository.CreateModel(exchange);
                    // Verificar si el intercambio fue creado exitosamente
                    if (createdExchange.idExchange == 0)
                    {
                        throw new Exception("El intercambio no fue creado");
                    }
                    // Agregar el intercambio creado a la lista de intercambios
                    exchanges.Add(createdExchange);
                }

                // Retornar la lista de intercambios creados
                return exchanges;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el intercambio", ex);
            }
        }

        public async Task<List<Exchanges>> UpdateExchange(Filter filter)
        {
            try
            {
                var exchanges = new List<Exchanges>();
                foreach (var exchangeFilter in filter.filterExchange.exchanges)
                {
                    var exchange = _mapper.Map<Exchanges>(exchangeFilter);
                    var exchangeFind = await _exchangeRepository.GetModel(u => u.idExchange == exchange.idExchange && u.id == exchange.id);
                    if (exchangeFind == null)
                        throw new Exception("El intercambio no existe");

                    // Modificar el estado de la entidad existente
                    exchangeFind.lastModification = DateTime.Now;
                    exchangeFind.status = exchange.status;

                    // Actualizar la entidad en la base de datos
                    bool response = await _exchangeRepository.EditModel(exchangeFind);

                    if (!response)
                        throw new Exception("No se han realizado cambios al estado del intercambio");

                    exchanges.Add(exchange);
                }
                return exchanges;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el intercambio", ex);
            }
        }




    }
}

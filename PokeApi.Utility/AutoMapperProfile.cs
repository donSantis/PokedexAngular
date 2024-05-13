using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using PokeApi.Model;
using PikeApi.DTO;
using PokeApi.Model.Sticker;
using PokeApi.Model.Menu;
using PokeApi.Model.PokeApiClasses;
using PokeApi.Model.Exchange;
using PokeApi.Model.Filter;



namespace PokeApi.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, Rol_DTO>().ReverseMap();
            #endregion Rol
            #region Menu
            CreateMap<Menu, Menu_DTO>().ReverseMap();
            #endregion Menu
            #region User
            CreateMap<User, User_DTO>()
                .ForMember(d => 
                    d.rol,
                    opt => opt.MapFrom(o => o.idRolNavigation.name))
                .ForMember(d => 
                    d.status,
                    opt => opt.MapFrom(o => o.status == true ? 1 : 0)
                    );

            CreateMap<User, Sesion_DTO>()
                .ForMember(d =>
                    d.rol,
                    opt => opt.MapFrom(o => o.idRolNavigation.name)
                );
            CreateMap<User_DTO, User>()
                .ForMember(d =>
                    d.idRolNavigation,
                    opt => opt.Ignore())
                .ForMember( d =>
                    d.status,
                    opt => opt.MapFrom(o => o.status == 1? true:false)
                );
            #endregion User
            #region Pokemon
 
            CreateMap<Pokemon, Pokemon_DTO>().ReverseMap();

            #endregion Pokemon
            #region Sticker

            CreateMap<Stickers, Sticker_DTO>().ReverseMap();

            #endregion Sticker
            #region Exchanges

            CreateMap<FilterExchange, Exchanges>();

            #endregion Exchanges
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using PokeApi.Model;
using PikeApi.DTO;
using PokeApi.Model.Sticker;



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
                    d.Rol,
                    opt => opt.MapFrom(o => o.IdRolNavigation.Name))
                .ForMember(d => 
                    d.Status,
                    opt => opt.MapFrom(o => o.Status == true ? 1 : 0)
                    );

            CreateMap<User, Sesion_DTO>()
                .ForMember(d =>
                    d.Rol,
                    opt => opt.MapFrom(o => o.IdRolNavigation.Name)
                );
            CreateMap<User_DTO, User>()
                .ForMember(d =>
                    d.IdRolNavigation,
                    opt => opt.Ignore())
                .ForMember( d =>
                    d.Status,
                    opt => opt.MapFrom(o => o.Status == 1? true:false)
                );
            #endregion User
            #region Pokemon
 
            CreateMap<Pokemon, Pokemon_DTO>().ReverseMap();

            #endregion Pokemon
            #region Sticker

            CreateMap<Stickers, Sticker_DTO>().ReverseMap();

            #endregion Sticker
        }

    }
}

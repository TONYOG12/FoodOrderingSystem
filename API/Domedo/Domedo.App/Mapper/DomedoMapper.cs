using AutoMapper;
using Domedo.Domain.Entities;
using Domedo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Mapper
{
    public class DomedoMapper : Profile
    {
        public DomedoMapper()
        {
            CreateMap<Meal, MealDto>().ReverseMap();

            CreateMap<Menu, MenuDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().ReverseMap();

            CreateMap<MealMenuDto, Menu>().ReverseMap();

            CreateMap<UserDto, User>().ReverseMap();









        }
    }
}

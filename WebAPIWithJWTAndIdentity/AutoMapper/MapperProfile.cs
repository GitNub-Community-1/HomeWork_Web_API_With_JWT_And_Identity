using AutoMapper;
using WebAPIWithJWTAndIdentity.Models.Dtos;
using WebAPIWithJWTAndIdentity.Models.Entity;

namespace WebAPIWithJWTAndIdentity.AutoMapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<TodoItemAddDto,TodoItem>().ReverseMap();
        CreateMap<TodoItemDto,TodoItem>().ReverseMap();
    }
    //Making true structure by  Abubakr
}
using AutoMapper;
using InterviewBackEnd.Model.DAO;
using InterviewBackEnd.Model.POCOS;
using InterviewBackEnd.Model.Request;

namespace InterviewBackEnd.AutoMapperProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderRequest, Order>()
                .ForMember(x => x.OrderedItems, a => a.MapFrom(z => z.Items))
                .ForMember(x => x.OrderId, a => a.MapFrom(z => z.OrderId))
                .ForMember(x => x.CreatedAt, a => a.MapFrom(z => BitConverter.GetBytes(new DateTimeOffset(z.CreatedAt).ToUnixTimeSeconds())))
                .ForMember(x => x.CustomerName, a => a.MapFrom(z => z.CustomerName));

            CreateMap<Items, OrderedItem>()
                .ForMember(x => x.Id, a => a.MapFrom(z => z.ProductId))
                .ForMember(x => x.Quantity, a => a.MapFrom(z => z.Quantity));

        }
    }
}

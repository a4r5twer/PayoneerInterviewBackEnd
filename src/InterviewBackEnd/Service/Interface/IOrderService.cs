using InterviewBackEnd.Model;
using InterviewBackEnd.Model.Request;
using InterviewBackEnd.Model.Response;

namespace InterviewBackEnd.Service.Interface
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request);
    }
}

using WebApi_CloudNautica.Dto;

namespace WebApi_CloudNautica.Services.Interface
{

    public interface IOrderService
    {
        Task<OrderResponse> GetOrderDetailsAsync(OrderRequest request);
    }

}

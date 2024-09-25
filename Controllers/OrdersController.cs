
// Controllers/OrdersController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi_CloudNautica.Dto;
using WebApi_CloudNautica.Services.Interface;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> GetOrderDetails([FromBody] OrderRequest request)
    {
        try
        {
            var response = await _orderService.GetOrderDetailsAsync(request);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}

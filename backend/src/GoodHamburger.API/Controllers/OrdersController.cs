using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ICreateOrderUseCase _createOrderUseCase;
    private readonly IGetOrderByIdUseCase _getOrderByIdUseCase;
    private readonly IGetAllOrdersUseCase _getAllOrdersUseCase;
    private readonly IUpdateOrderUseCase _updateOrderUseCase;
    private readonly IDeleteOrderUseCase _deleteOrderUseCase;

    public OrdersController(
        ICreateOrderUseCase createOrderUseCase,
        IGetOrderByIdUseCase getOrderByIdUseCase,
        IGetAllOrdersUseCase getAllOrdersUseCase,
        IUpdateOrderUseCase updateOrderUseCase,
        IDeleteOrderUseCase deleteOrderUseCase)
    {
        _createOrderUseCase = createOrderUseCase;
        _getOrderByIdUseCase = getOrderByIdUseCase;
        _getAllOrdersUseCase = getAllOrdersUseCase;
        _updateOrderUseCase = updateOrderUseCase;
        _deleteOrderUseCase = deleteOrderUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var result = await _createOrderUseCase.ExecuteAsync(request);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getAllOrdersUseCase.ExecuteAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getOrderByIdUseCase.ExecuteAsync(id);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderRequest request)
    {
        var result = await _updateOrderUseCase.ExecuteAsync(id, request);

        if (!result.IsSuccess)
        {
            if (result.Error!.Contains("not found"))
                return NotFound(new { error = result.Error });
            
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _deleteOrderUseCase.ExecuteAsync(id);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Error });

        return Ok();
    }
}

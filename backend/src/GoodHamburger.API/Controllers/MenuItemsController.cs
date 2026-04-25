using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly IGetMenuUseCase _getMenuUseCase;
    private readonly IGetMenuItemByIdUseCase _getMenuItemByIdUseCase;
    private readonly ICreateMenuItemUseCase _createMenuItemUseCase;
    private readonly IUpdateMenuItemUseCase _updateMenuItemUseCase;
    private readonly IDeleteMenuItemUseCase _deleteMenuItemUseCase;

    public MenuItemsController(
        IGetMenuUseCase getMenuUseCase,
        IGetMenuItemByIdUseCase getMenuItemByIdUseCase,
        ICreateMenuItemUseCase createMenuItemUseCase,
        IUpdateMenuItemUseCase updateMenuItemUseCase,
        IDeleteMenuItemUseCase deleteMenuItemUseCase)
    {
        _getMenuUseCase = getMenuUseCase;
        _getMenuItemByIdUseCase = getMenuItemByIdUseCase;
        _createMenuItemUseCase = createMenuItemUseCase;
        _updateMenuItemUseCase = updateMenuItemUseCase;
        _deleteMenuItemUseCase = deleteMenuItemUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getMenuUseCase.ExecuteAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _getMenuItemByIdUseCase.ExecuteAsync(id);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateMenuItemRequest request)
    {
        var result = await _createMenuItemUseCase.ExecuteAsync(request);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMenuItemRequest request)
    {
        var result = await _updateMenuItemUseCase.ExecuteAsync(id, request);

        if (!result.IsSuccess)
        {
            if (result.Error!.Contains("not found"))
                return NotFound(new { error = result.Error });
            
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _deleteMenuItemUseCase.ExecuteAsync(id);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Error });

        return NoContent();
    }
}

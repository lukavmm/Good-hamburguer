using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IGetMenuUseCase _getMenuUseCase;

    public MenuController(IGetMenuUseCase getMenuUseCase)
    {
        _getMenuUseCase = getMenuUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        var result = await _getMenuUseCase.ExecuteAsync();
        return Ok(result.Data);
    }
}

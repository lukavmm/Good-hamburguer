using GoodHamburger.Application.Common;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.UseCases;

public class DeleteOrderUseCase : IDeleteOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result> ExecuteAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order == null)
            return Result.Failure($"Pedido com id {id} não encontrado.");

        await _orderRepository.DeleteAsync(id);
        return Result.Success();
    }
}

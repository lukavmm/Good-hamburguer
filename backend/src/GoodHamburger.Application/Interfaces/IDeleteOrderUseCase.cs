using GoodHamburger.Application.Common;

namespace GoodHamburger.Application.Interfaces;

public interface IDeleteOrderUseCase
{
    Task<Result> ExecuteAsync(Guid id);
}

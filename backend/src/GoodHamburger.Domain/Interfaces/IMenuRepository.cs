using GoodHamburger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GoodHamburger.Domain.Interfaces;

public interface IMenuRepository
{
    Task<IEnumerable<MenuItem>> GetAllAsync();
    Task<MenuItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<MenuItem>> GetByIdsAsync(List<Guid> ids);
    Task AddAsync(MenuItem menuItem);
    Task UpdateAsync(MenuItem menuItem);
    Task DeleteAsync(Guid id);
}

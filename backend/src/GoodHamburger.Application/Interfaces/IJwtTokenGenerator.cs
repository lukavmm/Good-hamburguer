using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}

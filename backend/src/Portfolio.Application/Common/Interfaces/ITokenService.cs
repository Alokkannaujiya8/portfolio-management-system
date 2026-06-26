using Portfolio.Domain.Entities;

namespace Portfolio.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(Admin admin);
    }
}

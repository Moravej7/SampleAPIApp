using Entities.User;
using Services.Models;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}
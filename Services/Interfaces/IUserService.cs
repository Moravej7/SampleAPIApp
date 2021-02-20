
using Entities.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);
        Task AddAsync(User user, string password, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
        Task<User> GetByIdAsync(int userId);
        Task<List<User>> Get();
        Task<ActionResult> Token(string username, string password, CancellationToken cancellationToken);
        Task Update(int id, User updateUser, string Roles, CancellationToken cancellationToken);
        Task ResetPasswordByAdmin(int userId, string newPassword);
    }
}

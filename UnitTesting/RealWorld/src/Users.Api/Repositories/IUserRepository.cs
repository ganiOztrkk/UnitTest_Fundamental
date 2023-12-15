using Microsoft.EntityFrameworkCore.ChangeTracking;
using Users.Api.DTOs;
using Users.Api.Models;

namespace Users.Api.Repositories;

public interface IUserRepository
{
    Task<List<User>?> GetAllAsync( CancellationToken cancellationToken = default);
    
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<bool> CreateAsync(User user, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(User user, CancellationToken cancellationToken = default);
}
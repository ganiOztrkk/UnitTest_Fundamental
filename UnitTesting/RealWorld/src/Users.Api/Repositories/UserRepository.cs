using Microsoft.EntityFrameworkCore;
using Users.Api.Context;
using Users.Api.Models;

namespace Users.Api.Repositories;

public sealed class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<User>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>().ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Set<User>().FindAsync(id, cancellationToken);
        return user;
    }

    public async Task<bool> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(user, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Remove(user);
        var result = await _context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }
}
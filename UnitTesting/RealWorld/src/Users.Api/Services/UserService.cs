using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Users.Api.DTOs;
using Users.Api.Logging;
using Users.Api.Models;
using Users.Api.Repositories;
using ValidationException = FluentValidation.ValidationException;

namespace Users.Api.Services;

public sealed class UserService(IUserRepository userRepository, ILoggerAdapter<UserService> _logger) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<List<User>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Displaying all users");
        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await _userRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong while listing users.");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation("Users listed in {0}ms", stopWatch.ElapsedMilliseconds);
        }
        
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Displaying id:{0} user", id);
        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await _userRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong while getting user.");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation("User is displayed in {0}ms", stopWatch.ElapsedMilliseconds);
        }
    }

    public async Task<bool> CreateAsync(CreateUserDto request, CancellationToken cancellationToken = default)
    {
        CreateUserValidator validator = new CreateUserValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        User user = CreateUserDtoToUserObject(request);
        
        _logger.LogInformation("Creating user with id: {0} and name: {1}", user.Id, user.FullName);
        var stopWatch = Stopwatch.StartNew();
        try
        {
            var result = await _userRepository.CreateAsync(user, cancellationToken);
            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong when creating user.");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation("User with id: {0} created in {1}ms", user.Id, stopWatch.ElapsedMilliseconds);
        }
    }

    public User CreateUserDtoToUserObject(CreateUserDto request)
    {
        User user = new()
        {
            FullName = request.FullName
        };
        return user;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
            throw new ArgumentException("User not found");
        _logger.LogInformation("Deleting user with id: {0} and name: {1}", user.Id, user.FullName);
        var stopWatch = Stopwatch.StartNew();
        try
        {
            return await _userRepository.DeleteAsync(user!, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Something went wrong while deleting user.");
            throw;
        }
        finally
        {
            stopWatch.Stop();
            _logger.LogInformation("User with id: {0} deleted in {1}ms", user.Id, stopWatch.ElapsedMilliseconds);
        }
    }
}
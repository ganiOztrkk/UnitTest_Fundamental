using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Users.Api.DTOs;
using Users.Api.Logging;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;

namespace Users.Api.Tests.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly ILoggerAdapter<UserService> _logger = Substitute.For<ILoggerAdapter<UserService>>();
    //direk ctorda iuserrepo geçmedik çünkü gerçek veritabanıyla çalışılsın istemiyoruz. bunu simüle edecek olan mock yapısını substitute ile oluşturduk.

    public UserServiceTests()
    {
        _sut = new(_userRepository, _logger);
    }

    #region GetAllTests
    //getall start
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        //arrange
        _userRepository.GetAllAsync()
            .Returns(Enumerable.Empty<User>()
                .ToList());
        //act
        var result = await _sut.GetAllAsync();
        //assert
        result
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnsUsers_WhenSomeUsersExist()
    {
        //arrange
        User user = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Gani"
        };
        _userRepository.GetAllAsync()
            .Returns(new List<User>() { user });
        //act
        var testResult = await _sut.GetAllAsync();
        //assert
        testResult
            .Should()
            .BeEquivalentTo(new List<User>() { user });
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessages_WhenInvoked()
    {
        //arrange
        _userRepository.GetAllAsync()
            .Returns(Enumerable.Empty<User>()
                .ToList());
        //act
        await _sut.GetAllAsync();

        //assert
        _logger
            .Received(1).LogInformation(Arg.Is("Displaying all users"));
        _logger
            .Received(1).LogInformation(Arg.Is("Users listed in {0}ms"), Arg.Any<long>());
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
    {
        //arrange
        var exception = new ArgumentException("Something went wrong while listing users.");
        _userRepository.GetAllAsync()
            .Throws(exception);
        //act
        var requestAction = async () => await _sut.GetAllAsync();
        //assert
        await requestAction
            .Should()
            .ThrowAsync<ArgumentException>();
        _logger
            .Received(1)
            .LogError(Arg.Is(exception), Arg.Is("Something went wrong while listing users."));
    }
    //getall end
    

    #endregion


    #region GeyByIdTests
//getbyid start
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNoUsersExists()
    {
        //arrange
        _userRepository.GetByIdAsync(Arg.Any<Guid>())
            .ReturnsNull();
        //act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());
        //assert
        result
            .Should()
            .BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenSomeUserExists()
    {
        //arrange
        User expectedUser = new User()
        {
            Id = Guid.NewGuid(),
            FullName = "Gani"
        };
        _userRepository.GetByIdAsync(Arg.Any<Guid>())
            .Returns(expectedUser);
        //act
        var testResult = await _sut.GetByIdAsync(Guid.NewGuid());
        //assert
        testResult
            .Should()
            .BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldLogMessages_WhenInvoked()
    {
        //arrange
        var userId = Guid.NewGuid();
        _userRepository.GetByIdAsync(userId)
            .ReturnsNull();
        //act
        await _sut.GetByIdAsync(userId);

        //assert
        _logger
            .Received(1).LogInformation(Arg.Is("Displaying id:{0} user"), userId);
        _logger
            .Received(1).LogInformation(Arg.Is("User is displayed in {0}ms"), Arg.Any<long>());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldLogMessageAndException_WhenExceptionIsThrown()
    {
        //arrange
        var userId = Guid.NewGuid();
        var exception = new ArgumentException("Something went wrong while getting user.");
        _userRepository.GetByIdAsync(userId)
            .Throws(exception);
        //act
        var requestAction = async () => await _sut.GetByIdAsync(userId);
        //assert
        await requestAction
            .Should()
            .ThrowAsync<ArgumentException>();
        _logger
            .Received(1)
            .LogError(
                Arg.Is(exception),
                Arg.Is("Something went wrong while getting user."));
    }
    //getbyid end
    #endregion

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_ShouldThrownAnError_WhenUserCreateDetailsAreNotValid()
    {
        //arrange
        CreateUserDto request = new("");
        //act
        var action = async () => await _sut.CreateAsync(request);
        //assert
        await action
            .Should()
            .ThrowAsync<ValidationException>();
    }

    
    #endregion
}
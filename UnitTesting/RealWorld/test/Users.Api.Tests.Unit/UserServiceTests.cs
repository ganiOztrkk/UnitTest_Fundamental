using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;

namespace Users.Api.Tests.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly ILogger<User> _logger = Substitute.For<ILogger<User>>();
    //direk ctorda iuserrepo geçmedik çünkü gerçek veritabanıyla çalışılsın istemiyoruz. bunu simüle edecek olan mock yapısını substitute ile oluşturduk.
    
    public UserServiceTests()
    {
        _sut = new(_userRepository, _logger);
    }


    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        //arrange
        _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>().ToList());
        //act
        var result = await _sut.GetAllAsync();
        //assert
        result.Should().BeEmpty();
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
        _userRepository.GetAllAsync().Returns(new List<User>(){user});
        //act
        var testResult = await _sut.GetAllAsync();
        //assert
        testResult.Should().BeEquivalentTo(new List<User>(){user});
    }
    
}
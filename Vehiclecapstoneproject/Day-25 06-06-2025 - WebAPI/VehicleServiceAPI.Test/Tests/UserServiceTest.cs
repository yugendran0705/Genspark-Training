using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleServiceAPI.Interfaces;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Models.DTOs;
using VehicleServiceAPI.Repositories;
using VehicleServiceAPI.Services;
using VehicleServiceAPI.Utils;

namespace VehicleServiceAPI.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<UserRepository> _userRepositoryMock;
        private Mock<RoleRepository> _roleRepositoryMock;
        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<UserRepository>();
            _roleRepositoryMock = new Mock<RoleRepository>();
            _userService = new UserService(_userRepositoryMock.Object, _roleRepositoryMock.Object);
        }

        [Test]
        public async Task GetUserByIdAsync_UserExists_ReturnsUserDTO()
        {
            var user = new User { Id = 1, Name = "John", Email = "john@example.com", Phone = "1234567890", PasswordHash = "dummyhash", Role = new Role { Id = 1, RoleName = "Admin" } };
            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(1);

            Assert.Equals(result,user);
            Assert.Equals(user.Id, result.Id);
            Assert.Equals(user.Name, result.Name);
            Assert.Equals(user.Email, result.Email);
        }

        [Test]
        public async Task GetUserByEmailAsync_UserExists_ReturnsUserDTO()
        {
            var user = new User { Id = 2, Name = "Jane", Email = "jane@example.com", Phone = "9876543210", PasswordHash = "dummyhash", Role = new Role { Id = 2, RoleName = "User" } };
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync("jane@example.com")).ReturnsAsync(user);

            var result = await _userService.GetUserByEmailAsync("jane@example.com");

            Assert.Equals(result, user);
            Assert.Equals(user.Id, result.Id);
            Assert.Equals(user.Name, result.Name);
            Assert.Equals(user.Email, result.Email);
            Assert.Equals(user.Role.RoleName, result.RoleName);
        }

        [Test]
        public void GetUserByEmailAsync_UserNotFound_ThrowsException()
        {
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync("notfound@example.com")).ReturnsAsync((User)null);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _userService.GetUserByEmailAsync("notfound@example.com"));
            Assert.Equals("User not found.", ex.Message);
        }

        [Test]
        public async Task CreateUserAsync_ValidUser_ReturnsUserDTO()
        {
            var userRequest = new UserCreationRequestDTO { Name = "New User", Email = "newuser@example.com", Phone = "5556667777", Password = "securepassword", RoleId = 2 };
            var role = new Role { Id = 2, RoleName = "User" };
            var user = new User { Id = 3, Name = "New User", Email = "newuser@example.com", Phone = "5556667777", Role = role, PasswordHash = SecurityUtils.ComputeSha256Hash(userRequest.Password) };

            _roleRepositoryMock.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(role);
            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>())).ReturnsAsync(user);

            var result = await _userService.CreateUserAsync(userRequest);

            Assert.Equals(result,user);
            Assert.Equals(user.Id, result.Id);
            Assert.Equals(user.Email, result.Email);
        }

        [Test]
        public async Task UpdateUserAsync_ValidUser_ReturnsUpdatedUserDTO()
        {
            var existingUser = new User { Id = 4, Name = "Old Name", Email = "old@example.com", Phone = "1112223333", Role = new Role { Id = 1, RoleName = "Admin" }, PasswordHash = "dummyhash" };
            var updateRequest = new UserUpdateRequestDTO { Name = "Updated Name", Email = "updated@example.com", Phone = "4445556666", RoleId = 1 };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(4)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<User>())).ReturnsAsync(existingUser);

            var result = await _userService.UpdateUserAsync(4, updateRequest);

            Assert.That(result, Is.True);
            Assert.Equals(updateRequest.Name, result.Name);
            Assert.Equals(updateRequest.Email, result.Email);
            Assert.Equals(updateRequest.Phone, result.Phone);
        }

        [Test]
        public async Task DeleteUserAsync_UserExists_ReturnsTrue()
        {
            _userRepositoryMock.Setup(repo => repo.DeleteAsync(5)).ReturnsAsync(true);

            var result = await _userService.DeleteUserAsync(5);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, Name = "Alice", Email = "alice@example.com", Phone = "1234567890", Role = new Role { Id = 2, RoleName = "User" }, PasswordHash = "dummyhash1" },
                new User { Id = 2, Name = "Bob", Email = "bob@example.com", Phone = "9876543210", Role = new Role { Id = 1, RoleName = "Admin" }, PasswordHash = "dummyhash2" }
            };
            
            _userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            var result = await _userService.GetAllUsersAsync();

            Assert.That(result, Is.Not.Null);
            Assert.Equals(2, result.Count());
            Assert.Equals("Alice", result.First().Name);
            Assert.Equals("Bob", result.Last().Name);
        }
    }
}

using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleServiceAPI.Models;
using VehicleServiceAPI.Models.DTOs;
using VehicleServiceAPI.Repositories;
using VehicleServiceAPI.Services;

namespace VehicleServiceAPI.Tests.Services
{
    [TestFixture]
    public class VehicleServiceTests
    {
        private Mock<VehicleRepository> _vehicleRepoMock;
        private Mock<UserRepository> _userRepoMock;
        private VehicleService _vehicleService;

        [SetUp]
        public void SetUp()
        {
            _vehicleRepoMock = new Mock<VehicleRepository>();
            _userRepoMock = new Mock<UserRepository>();
            _vehicleService = new VehicleService(_vehicleRepoMock.Object, _userRepoMock.Object);
        }

        [Test]
        public async Task GetVehicleByIdAsync_ReturnsMappedDTO()
        {
            var owner = new User { Id = 99, Name = "Yugendran", Email = "yu@example.com", Phone = "9999999999", PasswordHash = "dummyhash", Role = new Role { Id = 1, RoleName = "User" } };
            var vehicle = new Vehicle { Id = 1, Make = "Honda", Model = "Civic", Year = 2020, OwnerId = 99, RegistrationNumber = "TN01AA1234", Owner = owner };

            _vehicleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(vehicle);
            _userRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync(owner);

            var result = await _vehicleService.GetVehicleByIdAsync(1);

            Assert.Equals(vehicle.Make, result.Make);
            Assert.Equals(owner.Email, result.OwnerEmail);
        }

        [Test]
        public async Task GetVehicleByUserAsync_ReturnsAllVehiclesMapped()
        {
            var owner = new User { Id = 99, Name = "Yugendran", Email = "yu@example.com", Phone = "9999999999", PasswordHash = "dummyhash", Role = new Role { Id = 1, RoleName = "User" } };
            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, Make = "Suzuki", Model = "Swift", Year = 2018, OwnerId = 50, RegistrationNumber = "TN22BB4567", Owner=owner },
                new Vehicle { Id = 2, Make = "Hyundai", Model = "i20", Year = 2021, OwnerId = 50, RegistrationNumber = "TN22BB7890", Owner=owner }
            };
            
            _vehicleRepoMock.Setup(r => r.GetAllByUserAsync(50)).ReturnsAsync(vehicles);
            _userRepoMock.Setup(r => r.GetByIdAsync(50)).ReturnsAsync(owner);

            var result = await _vehicleService.GetVehicleByUserAsync(50);

            Assert.Equals(2, result.Count());
        }

        [Test]
        public async Task CreateVehicleAsync_ReturnsCreatedDTO()
        {
            var request = new CreateVehicleDTO
            {
                Make = "Tata", Model = "Nexon", Year = 2023, RegistrationNumber = "TN09ZZ1234"
            };
            var user = new User { Id = 99, Name = "Yugendran", Email = "yu@example.com", Phone = "9999999999", PasswordHash = "dummyhash", Role = new Role { Id = 1, RoleName = "User" } };
            var createdVehicle = new Vehicle { Id = 2, Make = "Hyundai", Model = "i20", Year = 2021, OwnerId = 50, RegistrationNumber = "TN22BB7890", Owner = user };

            _userRepoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(user);
            _vehicleRepoMock.Setup(r => r.AddAsync(It.IsAny<Vehicle>())).ReturnsAsync(createdVehicle);

            var result = await _vehicleService.CreateVehicleAsync(10, request);

            Assert.Equals("Nexon", result.Model);
            Assert.Equals("Driver", result.OwnerName);
        }

        [Test]
        public async Task UpdateVehicleAsync_ValidUser_UpdatesAndReturnsDTO()
        {
            var owner = new User { Id = 99, Name = "Yugendran", Email = "yu@example.com", Phone = "9999999999", PasswordHash = "dummyhash", Role = new Role { Id = 1, RoleName = "User" } };
            var existing = new Vehicle { Id = 2, Make = "Hyundai", Model = "i20", Year = 2021, OwnerId = 50, RegistrationNumber = "TN22BB7890", Owner = owner };
            var updated = new Vehicle { Id = 2, Make = "Ford", Model = "EcoSport", Year = 2022, OwnerId = 50, RegistrationNumber = "TN05RR2222", Owner = owner };
            var dto = new UpdateVehicleDTO { Make = "Ford", Model = "EcoSport", Year = 2022, RegistrationNumber = "TN05RR2222" };

            _vehicleRepoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);
            _vehicleRepoMock.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(updated);
            _userRepoMock.Setup(r => r.GetByIdAsync(21)).ReturnsAsync(owner);

            var result = await _vehicleService.UpdateVehicleAsync(7, 21, dto);

            Assert.Equals("EcoSport", result.Model);
            Assert.Equals("TN05RR2222", result.RegistrationNumber);
        }

        [Test]
        public void UpdateVehicleAsync_WrongUser_ThrowsUnauthorizedAccessException()
        {
            var owner = new User { Id = 99, Name = "Yugendran", Email = "yu@example.com", Phone = "9999999999", PasswordHash = "dummyhash", Role = new Role { Id = 1, RoleName = "User" } };
            var vehicle = new Vehicle { Id = 8, Make = "Ford", Model = "EcoSport", Year = 2022, OwnerId = 50, RegistrationNumber = "TN05RR2222", Owner = owner };

            _vehicleRepoMock.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(vehicle);

            var dto = new UpdateVehicleDTO { Make = "Kia", Model = "Seltos", Year = 2021, RegistrationNumber = "TN10X1111" };

            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => 
                await _vehicleService.UpdateVehicleAsync(8, 201, dto));
        }

        [Test]
        public async Task DeleteVehicleAsync_ValidUser_DeletesVehicle()
        {
            var owner = new User { Id = 42, Name = "Test Owner", Email = "owner@example.com", Phone = "1234567890", PasswordHash = "hash", Role = new Role { Id = 1, RoleName = "User" } };
            var vehicle = new Vehicle
            {
                Id = 4,
                OwnerId = 42,
                Owner = owner,
                Make = "TestMake",
                Model = "TestModel",
                RegistrationNumber = "TEST1234",
                Year = 2020
            };

            _vehicleRepoMock.Setup(r => r.GetByIdAsync(4)).ReturnsAsync(vehicle);
            _vehicleRepoMock.Setup(r => r.DeleteAsync(4)).ReturnsAsync(true);

            var result = await _vehicleService.DeleteVehicleAsync(4, 42);

            Assert.That(result, Is.True);
        }

        [Test]
        public void DeleteVehicleAsync_WrongUser_ThrowsUnauthorizedAccessException()
        {
            var vehicle = new Vehicle
            {
                Id = 9,
                OwnerId = 88,
                Owner = new User { Id = 88, Name = "Test User", Email = "test@example.com", Phone = "0000000000", PasswordHash = "hash", Role = new Role { Id = 1, RoleName = "User" } },
                Make = "TestMake",
                Model = "TestModel",
                RegistrationNumber = "TEST9999",
                Year = 2020
            };

            _vehicleRepoMock.Setup(r => r.GetByIdAsync(9)).ReturnsAsync(vehicle);

            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => 
                await _vehicleService.DeleteVehicleAsync(9, 99));
        }
    }
}

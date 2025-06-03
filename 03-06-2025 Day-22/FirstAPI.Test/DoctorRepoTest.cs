using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Repositories;
using FirstAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Test;

public class Tests
{
    private ClinicContext _context;

    [SetUp]
    public void Setup()
    {
        // Use a unique database name for every test to ensure isolation and avoid duplicate key errors.
        var options = new DbContextOptionsBuilder<ClinicContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;
        _context = new ClinicContext(options);
    }

    [Test]
    public async Task AddDoctorTest()
    {
        // Arrange
        var email = " test@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();

        var doctor = new Doctor
        {
            Name = "test",
            YearsOfExperience = 2,
            Email = email
        };

        IRepository<int, Doctor> doctorRepository = new DoctorRepository(_context);

        // Action
        var result = await doctorRepository.Add(doctor);

        // Assert
        Assert.That(result, Is.Not.Null, "Doctor is not added");
        Assert.That(result.Id, Is.EqualTo(1));
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task GetDoctorPassTest(int id)
    {
        // Arrange
        var email = " test@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();

        var doctor = new Doctor
        {
            Name = "test",
            YearsOfExperience = 2,
            Email = email
        };

        IRepository<int, Doctor> doctorRepository = new DoctorRepository(_context);

        // Action
        await doctorRepository.Add(doctor);

        // Retrieve the doctor by id
        var result = doctorRepository.Get(id);

        // Assert
        Assert.That(result.Id, Is.EqualTo(id));
    }

    [TestCase(3)]
    public async Task GetDoctorExceptionTest(int id)
    {
        // Arrange
        var email = " test@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();

        var doctor = new Doctor
        {
            Name = "test",
            YearsOfExperience = 2,
            Email = email
        };

        IRepository<int, Doctor> doctorRepository = new DoctorRepository(_context);

        // Action
        await doctorRepository.Add(doctor);

        // Expect an exception since there's no doctor with the given id.
        var ex = Assert.ThrowsAsync<Exception>(async () => await doctorRepository.Get(id));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("No doctor with teh given ID"));
    }

    [TearDown]
    public void TearDown() 
    {
        _context.Dispose();
    }
}

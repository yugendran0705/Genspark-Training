namespace FirstApi.Misc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FirstApi.Contexts;
using FirstApi.Models;

public class ExperiencedDoctorHandler : AuthorizationHandler<ExperiencedDoctorRequirement>
{
    private readonly ClinicContext _context;
    public ExperiencedDoctorHandler(ClinicContext context)
    {
        _context = context;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExperiencedDoctorRequirement requirement)
    {
        var email = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(email))
            return Task.CompletedTask;

        var doctor = _context.doctors.FirstOrDefault(d => d.Email == email);
        if (doctor != null && doctor.YearsOfExperience >= requirement.MinimumYears)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
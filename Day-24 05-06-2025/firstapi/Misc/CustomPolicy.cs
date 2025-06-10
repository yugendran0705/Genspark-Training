namespace FirstApi.Misc;
using Microsoft.AspNetCore.Authorization;

public class ExperiencedDoctorRequirement : IAuthorizationRequirement
{
    public int MinimumYears { get; }
    public ExperiencedDoctorRequirement(int minimumYears)
    {
        MinimumYears = minimumYears;
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FirstApi.Models.DTOs.DoctorSpecialities;

namespace FirstApi.Misc;

public class CustomExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.Result = new BadRequestObjectResult(new ErrorObjectDTO
        {
            ErrorNumber = 500,
            ErrorMessage = context.Exception.Message + "and this is from custom exception filter"
        });
    }
}



using Microsoft.AspNetCore.Mvc;

namespace Okala.Api.Controllers;

public class ApiController : Controller
{
    protected ObjectResult Problem(ErrorOr.Error error)
    {
        int statusCode = error.Type == ErrorOr.ErrorType.Validation ? 400 : 500;
        return base.Problem(error.Description, error.Code.ToString(), statusCode, error.Code.ToString(), error.NumericType.ToString());
    }
}
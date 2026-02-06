using Microsoft.AspNetCore.Mvc;
using Shared.Error_Models;

namespace Library.Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult ValidtionErrorResponse(ActionContext Context)
        {

            var errors = Context.ModelState.Where(M => M.Value!.Errors.Any())
            .Select(M => new ValidationErrorDetails()
            {
                Field = M.Key,
                Errors = M.Value!.Errors.Select(E => E.ErrorMessage)
            });

            var response = new ValidationErrorToReturn()
            {
                validtionErrors = errors
            };
            return new BadRequestObjectResult(response);
        }
    }
}

using Domain_Layer.Exceptions;
using Shared.Error_Models;

namespace Library.Web.Exception_midelWare
{
    public class CustomeExceptionHandlerMidelWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomeExceptionHandlerMidelWare> _logger;

        public CustomeExceptionHandlerMidelWare(RequestDelegate next, ILogger<CustomeExceptionHandlerMidelWare> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

                await HandleNotFoundEndPoint(context);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Occurs Error"); // Internal server Error (500,....)=> Back End 

                await HandleExceptionAsync(context, ex);

            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //context.Response.ContentType = "Application/Json";
            var response = new ErrorToReturn()
            {
                ErrorMessage = ex.Message,
                //StatusCode = context.Response.StatusCode //Number In Body
            };

            response.StatusCode = ex switch
            {
                NotFoundExceptions => StatusCodes.Status404NotFound,
                BadRequestException badRequestException => GetErrors(badRequestException, response),
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            //var ResponseToReturn = JsonSerializer.Serialize(response);
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }

        private static int GetErrors(BadRequestException badRequestException, ErrorToReturn response)
        {
            response.Errors = badRequestException.Errors;
            return StatusCodes.Status400BadRequest;
        }

        private static async Task HandleNotFoundEndPoint(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End Point : {context.Request.Path} Is Not Found"
                };
                await context.Response.WriteAsJsonAsync(Response);
            }
        }
    }
}

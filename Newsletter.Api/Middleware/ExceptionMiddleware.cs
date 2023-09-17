using Newsletter.Api.Shared;
using Newsletter.Api.Shared.Exceptions;
using System.Net;

namespace Newsletter.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ValidationException => (int)HttpStatusCode.UnprocessableEntity,
                NotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new ErrorResponse
            {
                Message = exception.Message,
                ErrorList = exception.Data["errors"] as IEnumerable<string>
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

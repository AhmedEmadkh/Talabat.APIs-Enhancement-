using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    // Convension based Middleware
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                //  Take an Action for Request

                await _next.Invoke(httpContext); // Go To Next Middleware

                // Take an Action for Response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message); // Development Env


                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";


                var response = _env.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString())
                                                    :
                                                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                var Options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var JsonResponse = JsonSerializer.Serialize(response, Options);
                await httpContext.Response.WriteAsync(JsonResponse);
            }
        }
    }
}

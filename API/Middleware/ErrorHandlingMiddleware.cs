using MobileAppCottage.Domain.Exceptions;

namespace MobileAppCottage.API.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain; charset=utf-8";

                if (_env.IsDevelopment())
                {
                    // Zamiast "Something went wrong", wypisujemy co naprawdę boli serwer
                    var errorMessage = $"BŁĄD: {e.Message}";
                    if (e.InnerException != null)
                    {
                        errorMessage += $"\nSZCZEGÓŁY: {e.InnerException.Message}";
                    }
                    await context.Response.WriteAsync(errorMessage);
                }
                else
                {
                    await context.Response.WriteAsync("Wystąpił nieoczekiwany błąd serwera.");
                }
            }
        }
    }
}
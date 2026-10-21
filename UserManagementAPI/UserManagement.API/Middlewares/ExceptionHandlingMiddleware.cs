using UserManagement.Application.Exceptions;

namespace UserManagement.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var (statusCode, message) = ex switch
                {
                    KeyNotFoundException =>
                        (StatusCodes.Status404NotFound, ex.Message),

                    ConflictException =>
                        (StatusCodes.Status409Conflict, ex.Message),

                    ArgumentException =>
                        (StatusCodes.Status400BadRequest, ex.Message),

                    _ =>
                        (StatusCodes.Status500InternalServerError,
                         "Ocorreu um erro interno no servidor.")
                };

                if (statusCode >= 500)
                    _logger.LogError(ex, "Erro interno na API");
                else
                    _logger.LogWarning(
                        "Requisição rejeitada: {Message}", message);

                if (context.Response.HasStarted)
                    throw;

                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsJsonAsync(new
                {
                    status = statusCode,
                    message
                });
            }
        }
    }
}

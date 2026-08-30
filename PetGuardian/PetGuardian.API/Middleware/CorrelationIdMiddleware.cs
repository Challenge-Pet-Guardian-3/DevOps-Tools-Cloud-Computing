using Serilog.Context;

namespace PetGuardian.API.Middleware;

/// <summary>
/// Injeta o TraceIdentifier da requisição no contexto de log do Serilog,
/// permitindo correlacionar todas as linhas de log geradas durante uma mesma requisição HTTP.
/// </summary>
public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        using (LogContext.PushProperty("CorrelationId", httpContext.TraceIdentifier))
        {
            await next(httpContext);
        }
    }
}
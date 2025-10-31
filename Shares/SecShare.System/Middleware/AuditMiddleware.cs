using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SecShare.Core;
using SecShare.Infrastructure.Data;
using System.Security.Claims;

namespace SecShare.SystemConfig.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;

    public AuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        // Cho request chạy tiếp
        await _next(context);

        // Chỉ log khi request là api/doc/view/{id}
        if (context.Request.Path.StartsWithSegments("/api/file/view"))
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SecShareDbContext>();

                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var ip = context.Connection.RemoteIpAddress?.ToString();

                var audit = new AuditLog
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DocumentId = ExtractDocumentId(context.Request.Path),
                    Action = "VIEW",
                    IpAddress = ip,
                    Timestamp = DateTime.UtcNow,
                    DeviceInfo = "Unknown"
                };

                db.AuditLogs.Add(audit);
                await db.SaveChangesAsync();
            }
        }
    }

    private Guid? ExtractDocumentId(PathString path)
    {
        var parts = path.Value.Split('/');
        if (Guid.TryParse(parts.LastOrDefault(), out var id))
            return id;
        return null;
    }

}

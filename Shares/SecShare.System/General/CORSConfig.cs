using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecShare.SystemConfig.General;
public static class CORSConfig
{
    public static WebApplicationBuilder AddAppCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()   // Cho phép tất cả origins
                      .AllowAnyMethod()   // Cho phép tất cả HTTP methods
                      .AllowAnyHeader();  // Cho phép tất cả headers
            });
        });

        return builder;
    }
}

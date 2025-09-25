using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecShare.Base.Auth;
using SecShare.Servicer.Auth;


namespace SecShare.SystemConfig.Dependencies;
public static partial class DependencyInjection
{

    public static void AddServiceSingleton(this IHostApplicationBuilder builder)
    {
       
    }

    public static void AddServiceScoped(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthService, AuthService>();

    }

    public static void AddServiceTransient(this IHostApplicationBuilder builder)
    {
    }

    public static void AddServiceHttpClient(this IHostApplicationBuilder builder)
    {
        //builder.Services.AddHttpClient("AuthAPI", u => u.BaseAddress = new Uri("https://localhost:7001/"));

    }
}

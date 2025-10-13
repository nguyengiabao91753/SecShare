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
using SecShare.Base.Document;
using SecShare.Base.File;
using SecShare.Helper.EmailHelper;
using SecShare.Servicer.Auth;
using SecShare.Servicer.Document;
using SecShare.Servicer.File;


namespace SecShare.SystemConfig.Dependencies;
public static partial class DependencyInjection
{

    public static void AddServiceSingleton(this IHostApplicationBuilder builder)
    {
       
    }
    public static void AddAuthServiceScoped(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthAPIService, AuthAPIService>();

        builder.Services.AddScoped<IUserAPIService, UserAPIService>();
        builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        builder.Services.AddScoped<IUserAPIService, UserAPIService>();

        builder.Services.AddScoped<IotpService, OtpAPIService>();
        builder.Services.AddScoped<IEmailHelper, EmailHelper>();
        builder.Services.AddScoped<IEmailAPIService, EmailAPIService>();
    }
    public static void AddServiceScoped(this IHostApplicationBuilder builder)
    {
       
        builder.Services.AddScoped<IClouDinaryService, ClouDinaryService>();
        builder.Services.AddScoped<IDocumentAPIService, DocumentAPIService>();
    }

    public static void AddServiceTransient(this IHostApplicationBuilder builder)
    {
    }

    public static void AddServiceHttpClient(this IHostApplicationBuilder builder)
    {
        //builder.Services.AddHttpClient("AuthAPI", u => u.BaseAddress = new Uri("https://localhost:7001/"));

    }
}

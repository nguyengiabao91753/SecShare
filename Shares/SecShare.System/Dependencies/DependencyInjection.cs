using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SecShare.System.Dependencies;
public static partial class DependencyInjection
{

    public static void AddServiceSingleton(this IHostApplicationBuilder builder)
    {
       
    }

    public static void AddServiceScoped(this IHostApplicationBuilder builder)
    {
    }

    public static void AddServiceTransient(this IHostApplicationBuilder builder)
    {
    }
}

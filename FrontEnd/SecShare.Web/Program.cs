using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SecShare.Base.Auth;
using SecShare.Helper.Utils;
using SecShare.Servicer.Auth;
using SecShare.SystemConfig.Dependencies;
using SecShare.Web.Components;
using SecShare.Web.Services;
using SecShare.Web.Services.IServices;

var builder = WebApplication.CreateBuilder(args);





// Cấu hình Map Assembly
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


// Add HttpClient
builder.Services.AddHttpClient<IAuthService, AuthService>();

//---------------------------------------------------------
//Add Services

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
//----------------------------------------------------------
//Add API URL 

SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
//------------------------------------------------



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

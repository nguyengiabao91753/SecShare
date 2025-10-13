using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SecShare.Base.Auth;
using SecShare.Helper.Utils;
using SecShare.Servicer.Auth;
using SecShare.Services;
using SecShare.SystemConfig.Dependencies;
using SecShare.Web.Components;
using SecShare.Web.Services;
using SecShare.Web.Services.IServices;
using SecShare.Web.Services.IServices.IUserServices;

var builder = WebApplication.CreateBuilder(args);





// Cấu hình Map Assembly
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<CircuitOptions>(options => options.DetailedErrors = true);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


// Add HttpClient
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IBaseService, BaseService>();
builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddHttpClient<IDocumentService, DocumentService>();
builder.Services.AddHttpClient<IFileService, FileService>();
//---------------------------------------------------------
//Add Services
builder.Services.AddSingleton<NotificationService>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailConfirmedService, EmailConfirmedService>();

builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IFileService, FileService>();
//----------------------------------------------------------
//Add API URL 

SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
SD.DocumentAPIBase = builder.Configuration["ServiceUrls:DocumentAPI"];
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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

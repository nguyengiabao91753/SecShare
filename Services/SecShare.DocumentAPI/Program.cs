using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecShare.Core.Auth;
using SecShare.DocumentAPI.Services;
using SecShare.DocumentAPI.Services.IService;
using SecShare.Helper.Utils;
using SecShare.Infrastructure.Data;
using SecShare.SystemConfig.Authentication;
using SecShare.SystemConfig.Dependencies;
using SecShare.SystemConfig.Extensions;
using SecShare.SystemConfig.General;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppCors();

//Config sql connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection").ToString();
builder.Services.AddDbContext<SecShareDbContext>(options =>
   options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("SecShare.Infrastructure")
    ));


builder.Services.AddDbContext<IdentityApplicationDbContext>(options =>
   options.UseSqlServer(
        builder.Configuration.GetConnectionString("AuthConnection"),
        b => b.MigrationsAssembly("SecShare.Infrastructure")
    ));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<IdentityApplicationDbContext>().AddDefaultTokenProviders();


SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
// Add services to the container.
builder.Services.AddScoped<IUServiceConnect, UServiceConnect>();
builder.AddServiceSingleton();
builder.AddServiceScoped();
builder.AddServiceTransient();

//Config Verify Token
builder.AddAppAuthentication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.AddSwaggerWithJWT();
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<SecShareDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
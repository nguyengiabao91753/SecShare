using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SecShare.Core.Auth;
using SecShare.Infrastructure.Data;
using SecShare.SystemConfig.Authentication;
using SecShare.SystemConfig.Dependencies;
using SecShare.SystemConfig.Extensions;
using System;

var builder = WebApplication.CreateBuilder(args);


//Config DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection").ToString();

builder.Services.AddDbContext<IdentityApplicationDbContext>(options =>
   options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("SecShare.Infrastructure")
    ));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<IdentityApplicationDbContext>().AddDefaultTokenProviders();




// Add services to the container.
builder.AddServiceSingleton();
builder.AddAuthServiceScoped();
builder.AddServiceTransient();

//Config Verify Token
builder.AddAppAuthentication();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.AddSwaggerWithJWT();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigration();
app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<IdentityApplicationDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
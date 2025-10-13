using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecShare.Core.Auth;
using SecShare.Infrastructure.Data;
using SecShare.SystemConfig.Authentication;
using SecShare.SystemConfig.Dependencies;
using SecShare.SystemConfig.Extensions;

var builder = WebApplication.CreateBuilder(args);

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


// Add services to the container.
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
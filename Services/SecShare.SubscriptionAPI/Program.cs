using Microsoft.EntityFrameworkCore;
using SecShare.Infrastructure.Data;
using SecShare.SystemConfig.Dependencies;

var builder = WebApplication.CreateBuilder(args);



//Config sql connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection").ToString();
builder.Services.AddDbContext<SecShareDbContext>(options =>
   options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("SecShare.Infrastructure")
    ));


// Add services to the container.

builder.Services.AddControllers();
builder.AddServiceSingleton();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();

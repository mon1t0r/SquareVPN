using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Relays;
using WebAPI.Relays.ControlServer;
using WebAPI.Relays.ControlServer.Packet;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.secrets.json");

builder.Services.AddControllers();
builder.Services.AddDbContext<WebContext>(opt =>
    opt.UseMySql(
        builder.Configuration["ConnectionStrings:DefaultConnection"],
        new MySqlServerVersion(new Version(8, 0, 33))
    ));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

RelayManager.Initialize();
PacketManager.RegisterPackets();
ControlServerManager.InitializeServer(int.Parse(builder.Configuration["ControlServerPort"]));

app.Run();
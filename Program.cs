using BackCriptoDisk2;
using BackCriptoDisk2.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using Fleck;

var builder = WebApplication.CreateBuilder(args);
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var server = new WebSocketServer("ws://0.0.0.0:8181");
var wsConnection = new List<IWebSocketConnection>();

server.Start(ws =>
{
    ws.OnOpen = () =>
    {
        wsConnection.Add(ws);
    };
    ws.OnMessage = message =>
    {
        foreach (var webSocketConnect in wsConnection)
        {
            webSocketConnect.Send(message);
        }
    };
});
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseAzureSql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

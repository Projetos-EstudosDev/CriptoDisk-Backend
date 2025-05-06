using System.Net;
using System.Net.WebSockets;
using System.Text;
using BackCriptoDisk2;
using BackCriptoDisk2.Models;
using BackCriptoDisk2.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

var builder = WebApplication.CreateBuilder(args);
var connections = new List<WebSocket>();
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseAzureSql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<Usuarios>();
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
app.UseWebSockets();

app.MapGet("/ws",async ( context) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var clientName = context.Request.Query["username"];
        var websocket = await context.WebSockets.AcceptWebSocketAsync();
        connections.Add(websocket);

        await chat($"{clientName} Entrou na sala");
        
        await ReceberMensagem(websocket, async(result,buffer) =>
        {
            if (result.MessageType == WebSocketMessageType.Text)
            { 
                string message = Encoding.UTF8.GetString(buffer,0,result.Count);
                await chat($"{clientName}: {message}",websocket);
            }
            else if(result.MessageType == WebSocketMessageType.Close || websocket.State == WebSocketState.Aborted)
            {
                connections.Remove(websocket);
                await chat($"{clientName} deixou a sala");
                await chat($"{connections.Count} sairam da sala");
                await websocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
        });
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

async Task ReceberMensagem(WebSocket socket, Action<WebSocketReceiveResult, Byte[]> handleMessage)
{
    var buffer = new byte[1024 * 4];
    while (socket.State == WebSocketState.Open)
    {
        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer),CancellationToken.None);
        handleMessage(result, buffer);
    }
}
async Task chat(string message, WebSocket? sender = null)
{
    var bytes = Encoding.UTF8.GetBytes(message);
    foreach (var socket in connections)
    {
        if (socket.State == WebSocketState.Open && socket != sender)
        {
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();
app.Run();

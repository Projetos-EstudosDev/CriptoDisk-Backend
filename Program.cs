using BackCriptoDisk2.Data;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseAzureSql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
var specificOrgins = "AppOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: specificOrgins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000");
        });
});
// Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using dotnet_signalr_starter.Providers;
using dotnet_signalr_starter.Data;
using dotnet_signalr_starter.SignalRHubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// START: My Code
builder.Services.AddSignalR();
builder.Services.AddSingleton<NewsStore>();

var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NewsContext>(options =>
    options.UseSqlite(defaultConnection),
    ServiceLifetime.Singleton
);

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowMyOrigins",
//         builder =>
//         {
//             builder
//                 .AllowCredentials()
//                 .AllowAnyHeader()
//                 .SetIsOriginAllowedToAllowWildcardSubdomains()
//                 .AllowAnyMethod()
//                 .WithOrigins(
//                     "https://localhost:44311",
//                     "https://localhost:44390",
//                     "https://localhost:44395",
//                     "https://localhost:5001");
//         });
// });

// END: My Code

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// START: My Code
app.MapHub<NewsHub>("/looney");
// app.UseCors("AllowMyOrigins");
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
// END: My Code

app.Run();

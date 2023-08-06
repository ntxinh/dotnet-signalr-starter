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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigins",
        builder =>
        {
            builder
                .AllowCredentials()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .WithOrigins(
                    "http://localhost:4200"
                    // "http://localhost:5000"
                );
        });
});

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
app.MapHub<NewsHub>(HubRoutes.News);
app.UseCors("AllowMyOrigins");
// END: My Code

app.Run();

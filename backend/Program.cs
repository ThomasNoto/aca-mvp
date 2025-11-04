using backend.Data;
using backend.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

try {
    Env.Load(); 
} catch {
    Console.WriteLine("No .env file found."); 
}

// build connection string from env components
string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
string db   = Environment.GetEnvironmentVariable("DB_NAME") ?? "aca_mvp";
string user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
string pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";

var connString = $"Host={host};Port={port};Database={db};Username={user};Password={pass}";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseNpgsql(connString));

// TODO: dependency injection learning
builder.Services.AddScoped<AirportService>();
builder.Services.AddScoped<FlightService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// swagger is always on because it's awesome
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

using backend.Data;
using backend.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls(builder.Configuration["applicationUrl"]!);

try {
    Env.Load("../.env");
    Console.WriteLine(".env file found!"); 
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

// not supporting https because this is a locally hosted mvp
// however, I would in production to protect user data
// app.UseHttpsRedirection();

// IMPORTANT
// allows CORS (Cross-Origin Resource Sharing) so that frontend on port 4200 can
// talk to and exchange resources with backend on port 8080
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.MapControllers();

app.Run();

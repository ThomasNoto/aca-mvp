using backend.Data;
using backend.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load("../.env");

//add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// loads connection string from .env
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

builder.Services.AddDbContext<ApiDbContext>(opt =>
    opt.UseNpgsql(connectionString));    
    
builder.Services.AddScoped<FlightService>();
builder.Services.AddScoped<AirportService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddControllers();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

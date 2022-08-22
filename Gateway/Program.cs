using Gateway;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("configuration.production.json");

builder.Services.AddOcelot();

var app = builder.Build();
app.UseMiddleware<CorrelationIdMiddleware>();
app.MapGet("/", () => "Hello World!");
app.UseOcelot().Wait();
app.Run();
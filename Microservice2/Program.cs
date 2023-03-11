using Microservice2;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenTelemetry().WithTracing(providerBuilder =>
{
    providerBuilder
        .AddConsoleExporter()
        .AddZipkinExporter(options => options.Endpoint = new Uri("http://localhost:9411/api/v2/spans"))
        .AddSource("Microservice 1")
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
var _ = new AppActivity();
app.Run();
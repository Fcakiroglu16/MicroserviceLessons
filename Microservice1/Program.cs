using Microservice1;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestHeaders;

});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("httpClientWithMicroservice2", context =>
{
    context.BaseAddress = new Uri("http://localhost:5082");

});


builder.Services.AddOpenTelemetry().WithTracing(builder =>
{
    builder
        .AddConsoleExporter()
        .AddSource("Microservice 1")
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation();


});
var app = builder.Build();





app.Use(async (context, next) =>
{

    // Log/Print all Headers
    var header = context.Request.Headers;

    foreach (var head in header.ToList())
    {
        Console.WriteLine($"{head.Key} : {head.Value.First()}");
    }



    await next();
});




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
new AppActivity();
app.Run();
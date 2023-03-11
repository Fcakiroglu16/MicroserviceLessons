using Microservice1;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;

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
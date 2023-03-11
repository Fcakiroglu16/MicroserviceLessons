using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>("DatabaseHealth",tags:new[]{"ready"});




builder.Services.Configure<HealthCheckPublisherOptions>(options =>
{
  options.Delay = TimeSpan.FromSeconds(2);
  options.Period = TimeSpan.FromSeconds(3);
  //apply filter
  //options.Predicate = healthCheck => healthCheck.Tags.Contains("sample");
});

builder.Services.AddSingleton<IHealthCheckPublisher, ExampleHealthCheckPublisher>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapHealthChecks("/healthz/ready",new HealthCheckOptions()
{
  
  Predicate = healthCheck=> healthCheck.Tags.Contains("ready")
  
}).AllowAnonymous();
app.MapHealthChecks("/healthz/live",new HealthCheckOptions()
{
  Predicate = healthCheck=> false // sadece ana olan tag'siz olan çalışır, null dersek default değerdir hepsi çalışır.
  
}).AllowAnonymous();



// Configure the HTTP request pipeline.
  app.UseSwagger();
  app.UseSwaggerUI();


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
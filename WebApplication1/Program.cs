using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);
 builder.Services.AddHealthChecks();


//Custom HealthCheck
// builder.Services.AddHealthChecks()
//   .AddCheck<ExampleHealthCheck>("Exaple");



//SQL Server HealthCheck
// builder.Services.AddHealthChecks()
//   .AddSqlServer(
//     builder.Configuration.GetConnectionString("DefaultConnection"));




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
app.MapHealthChecks("/healthz");



// Configure the HTTP request pipeline.
  app.UseSwagger();
  app.UseSwaggerUI();


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
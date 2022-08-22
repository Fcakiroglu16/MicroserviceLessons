using Logging;
using Serilog;
using ServiceA;
using ServiceA.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Logging.Logging.ConfigureLogger);
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AddCorrelationIdRequestHandler>();
builder.Services.AddHttpClient<BService>(x =>
{
    //x.BaseAddress = new Uri("http://localhost:5201/api/");
     x.BaseAddress = new Uri("http://ServiceB.api/api/");
}).AddHttpMessageHandler<AddCorrelationIdRequestHandler>();
var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
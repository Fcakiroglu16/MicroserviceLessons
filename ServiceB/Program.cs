using Logging;
using Serilog;
using ServiceB;
using ServiceB.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Host.UseSerilog(Logging.Logging.ConfigureLogger);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AddCorrelationIdRequestHandler>();
builder.Services.AddHttpClient<CService>(x =>
{
    //x.BaseAddress = new Uri("http://localhost:5004/api/");
     x.BaseAddress = new Uri("http://ServiceC.api/api/");
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
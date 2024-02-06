using MassTransit;
using Microservice1.API;
using Microservice1.API.Services;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{

    

    x.AddConsumer<EventConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        
        config.UseTimeout(x=>x.Timeout = TimeSpan.FromSeconds(3));

        

        // config.UseMessageRetry(r => r.Immediate(5));

        // config.UseMessageRetry(r => r.Incremental(5,TimeSpan.FromSeconds(3),TimeSpan.FromSeconds(3)));
        //
        // // ignore / Handle exceptions
        // config.UseMessageRetry(r => 
        // {
        //     r.Handle<ArgumentNullException>();
        //     r.Ignore(typeof(InvalidOperationException), typeof(InvalidCastException));
        //     r.Ignore<ArgumentException>(t => t.ParamName == "orderTotal");
        // });

        // 5 times retry
        // 5 minute * 5 + 15 minute* 5  30 minute * 5 = 15 times 
        // total 20 times

        // config.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));

        // config.UseInMemoryOutbox();

        config.ReceiveEndpoint("queue-event-consumer",
            e => { e.ConfigureConsumer<EventConsumer>(context); });



        //config.Host(new Uri("amqps://jsqjpfwe:mG9pvqLZB_-NRUz9CO40xPzDC4vFI7P8@woodpecker.rmq.cloudamqp.com/jsqjpfwe"));


        config.Host(new Uri($"rabbitmq://{builder.Configuration.GetConnectionString("rabbitmqConnection")}"), h =>
        {
            h.PublisherConfirmation = true;
            h.Username("guest");
            h.Password("guest");
        });




    });
});


builder.Services.AddHttpClient<WeatherForcastService>(x => x.BaseAddress = new Uri("https://localhost:7111"))
    // .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(5,
        (context, timespan, task) =>
        {
            Console.WriteLine("request has been cancelled by policy");
            return Task.CompletedTask;
        }));

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer("your connection string",
        optionBuilder => { optionBuilder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), null); });
});
builder.Services.AddControllers();

builder.Services.AddHealthChecks();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/healthz");
app.Run();
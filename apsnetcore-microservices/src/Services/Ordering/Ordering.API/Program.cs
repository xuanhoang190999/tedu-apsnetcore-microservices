
using Common.Logging;
using Contracts.Common.Interfaces;
using Contracts.Messages;
using Infrastructure.Common;
using Infrastructure.Messages;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information("Start Ordering API up");

try
{
    // Add services to the container.
    builder.Host.AddAppConfigurations();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Services.AddScoped<IMessageProducer, RabbitMQProducer>();
    builder.Services.AddScoped<ISerializeService, SerializeService>();

    // Configura Mass Transit
    builder.Services.ConfiguraMassTransit();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => 
            c.SwaggerEndpoint("/swagger/v1/swagger.json",
                "Swagger Order API v1"));
    }

    // Initialiise and seed database
    using(var scope = app.Services.CreateScope())
    {
        var orderContextSeed = scope.ServiceProvider.GetRequiredService<OrderContextSeed>();
        await orderContextSeed.InitialiseAsync();
        await orderContextSeed.SeedAsync();
    }

    app.UseHttpsRedirection(); //production only

    app.UseAuthorization();

    app.MapControllers();

    app.MapDefaultControllerRoute();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) // Chặn chuyện phát sinh lỗi khi mà chạy migration
    {
        throw;
    }

    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Ordering API complete");
    Log.CloseAndFlush();
}



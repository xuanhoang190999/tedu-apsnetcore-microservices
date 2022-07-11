
using Common.Logging;
using Contracts.Common.Interfaces;
using Customer.API.Controllers;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information("Start Customer Minimal API up");

try
{
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services.AddDbContext<CustomerContext>(
        options => options.UseNpgsql(connectionString));

    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>()
        .AddScoped(typeof(IRepositoryQueryBase<,,>), typeof(RepositoryQueryBase<,,>))
        .AddScoped<ICustomerService, CustomerService>();

    var app = builder.Build();

    app.MapGet("/", () => "Welcome to Customer Minimal API!");

    app.MapCustomersAPI();

    //app.MapGet("/api/customers/{username}",
    //    async (string username, ICustomerService customerService) =>
    //        await customerService.GetCustomerByUsernameAsync(username));

    //app.MapGet("/api/customers/{username}",
    //    async (string username, ICustomerService customerService) =>
    //    {
    //        var customer = await customerService.GetCustomerByUsernameAsync(username);
    //        return customer != null ? Results.Ok(customer) : Results.NotFound();
    //    });

    //app.MapPost("/api/customers",
    //    async (Customer.API.Entities.Customer customer, ICustomerRepository customerRepository) =>
    //    {
    //        await customerRepository.CreateAsync(customer);
    //        await customerRepository.SaveChangesAsync();
    //    });

    //app.MapDelete("/api/customers/{id}",
    //    async (int id, ICustomerRepository customerRepository) =>
    //    {
    //        var customer = await customerRepository.FindByCondition(x => x.Id.Equals(id))
    //            .SingleOrDefaultAsync();

    //        if(customer == null) return Results.NotFound();

    //        await customerRepository.DeleteAsync(customer);
    //        await customerRepository.SaveChangesAsync();

    //        return Results.NoContent();
    //    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection(); production only

    app.UseAuthorization();

    app.MapControllers();

    app.SeedCustomerData()
        .Run();
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
    Log.Information("Shut down Customer Minimal API complete");
    Log.CloseAndFlush();
}



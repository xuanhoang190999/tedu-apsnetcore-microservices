
using Common.Logging;
using Product.API.Extenstions;
using Product.API.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Start Product API up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Host.AddAppConfigurations();
    // Add services to the container.
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    app.UseInfrastructure();

    // Migration tự động cho project Product - Do không sử dụng IServiceProvider, nhưng Action<TContext, IServiceProvider> lại có nên sử dụng dấu _
    app.MigrateDatabase<ProductContext>((context, _) => 
        {
            ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait(); // Tự động thêm dữ liệu vào db nếu chưa có
        });

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if(type.Equals("StopTheHostException", StringComparison.Ordinal)) // Chặn chuyện phát sinh lỗi khi mà chạy migration
    {
        throw;
    }

    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Product API complete");
    Log.CloseAndFlush();
}



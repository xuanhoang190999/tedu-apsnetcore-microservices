using Customer.API.Services.Interfaces;

namespace Customer.API.Controllers
{
    public static class CustomersController
    {
        public static void MapCustomersAPI(this WebApplication app)
        {
            app.MapGet("/api/customers",
                async (ICustomerService customerService) => await customerService.GetCustomersAsync());

            app.MapGet("/api/customers/{username}",
                async (string username, ICustomerService customerService) =>
                {
                    var customer = await customerService.GetCustomerByUsernameAsync(username);
                    return customer != null ? Results.Ok(customer) : Results.NotFound();
                });
        }
    }
}

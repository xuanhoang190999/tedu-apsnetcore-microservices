using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        public readonly ICustomerRepository _repository;
        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IResult> GetCustomerByUsernameAsync(string username) =>
            Results.Ok(await _repository.GetCustormerByUserNameAsync(username));

        public async Task<IResult> GetCustomersAsync() =>
            Results.Ok(await _repository.GetCustomerAsync());
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        private readonly ILogger _logger;
        private readonly OrderContext _context;

        public OrderContextSeed(ILogger logger, OrderContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if(_context.Database.IsSqlServer()) // Kiểm tra OrderContext có đúng là sql Server không thì mới cho Migration.
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            if(!_context.Orders.Any())
            {
                await _context.Orders.AddRangeAsync(
                    new Domain.Entities.Order
                    {
                        UserName = "customer1",
                        FirstName = "customer1",
                        LastName = "customer",
                        EmailAddress = "customer1@gmail.com",
                        ShippingAddress = "Wollongong",
                        InvoiceAddress = "Australia",
                        TotalPrice = 250
                    });
            }
        }
    }
}

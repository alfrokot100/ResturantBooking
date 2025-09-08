using Microsoft.EntityFrameworkCore;
using ResturantBooking.Data;
using ResturantBooking.Models;
using ResturantBooking.Repositories.IRepositories;

namespace ResturantBooking.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDBContext _context;

        public CustomerRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            return customers;
        }

        public async Task<Customer> GetCustomerByIDAsync(int customerID)
        {
            var customers = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerID);
            return customers;
        }

        public async Task<bool> UpdateCustomerAsync(Customer Customer)
        {
            _context.Customers.Update(Customer);
            var result = await _context.SaveChangesAsync();
            if(result != 0) { return true; }
            return false;
        }

        public async Task<int> CreateCustomerAsync(Customer Customer)
        {
            _context.Customers.Add(Customer);
            await _context.SaveChangesAsync();
            return Customer.Id;
        }
        public async Task<bool> DeleteCustomerAsync(int customerID)
        {
            var rowsAffected = await _context.Customers.Where(c => c.Id == customerID).ExecuteDeleteAsync();
            if(rowsAffected > 0) { return true; }
            return false;
        }


    }
}

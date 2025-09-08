using ResturantBooking.DTOs.CustomerDTOs;
using ResturantBooking.Models;
using ResturantBooking.Repositories.IRepositories;
using ResturantBooking.Services.IServices;

namespace ResturantBooking.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomerService(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<List<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers = await _customerRepo.GetAllCustomersAsync();
            var customerDTO = customers.Select(c => new CustomerDTO
            {
                Id = c.Id,
                Name = c.Name,
                PhoneNbr = c.PhoneNbr
            }).ToList();
            return customerDTO;
        }

        public async Task<CustomerDTO> GetCustomerByID(int CustomerID)
        {
            var customer = await _customerRepo.GetCustomerByIDAsync(CustomerID);
            if(customer == null) { return null; }

            var customerDTO = new CustomerDTO
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNbr = customer.PhoneNbr
            };
            return customerDTO;
        }

        public async Task<int> CreateCustomerAsync(CustomerDTO customerDTO)
        {
            var customer = new Customer
            {
                Id = customerDTO.Id,
                Name = customerDTO.Name,
                PhoneNbr = customerDTO.PhoneNbr
            };
            var newCustomerId = await _customerRepo.CreateCustomerAsync(customer);
            return newCustomerId;
        }

        public async Task<bool> UpdateCustomerAsync(CustomerDTO customerDTO)
        {
            var customer = new Customer
            {
                Id = customerDTO.Id,
                Name = customerDTO.Name,
                PhoneNbr = customerDTO.PhoneNbr
            };
            return await _customerRepo.UpdateCustomerAsync(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int CustomerID)
        {
            return await _customerRepo.DeleteCustomerAsync(CustomerID);
        }
    }
}

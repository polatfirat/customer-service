using Customer.Domain.Dto;
using Customer.Domain.Entities;
using Customer.Domain.Exceptions;
using Customer.Repository.Abstract;
using Customer.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Customer.Service.Concrete
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customers> _repository;
        public CustomerService(IRepository<Customers> repository)
        {
            _repository = repository;
        }

        public async Task DeleteCustomerAsync(int entityId)
        {
            if (entityId <= 0)
            {
                throw new CustomerOperationException("Customer id must be greather than 0");
            }
            var entity = await GetCustomerByIdAsync(entityId);
            if (entity == null)
            {
                throw new CustomerOperationException($"Customer number {entityId} is not exists");
            }
            await _repository.DeleteAsync(entityId);
        }

        public async Task<IEnumerable<Customers>> GetAllCustomersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Customers> GetCustomerByIdAsync(int entityId)
        {
            if (entityId <= 0)
            {
                throw new CustomerOperationException("Customer id must be greater than 0");
            }
            return await _repository.GetByIdAsync(entityId);
        }

        public async Task<Customers> InsertCustomerAsync(Customers entity)
        {
            return await _repository.InsertAsync(entity);
        }

        public async Task<IEnumerable<Customers>> SearchCustomerAsync(CustomerFilter customerFilter)
        {
            var entity = _repository.GetAllQueryable();
            if (!String.IsNullOrEmpty(customerFilter.FirstName))
            {
                entity = entity.Where(r => r.FirstName.Equals(customerFilter.FirstName.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            if (!String.IsNullOrEmpty(customerFilter.LastName))
            {
                entity = entity.Where(r => r.LastName.Equals(customerFilter.LastName.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            if (customerFilter.Age.HasValue)
            {
                entity = entity.Where(r => r.Age == customerFilter.Age);
            }
            if (!String.IsNullOrEmpty(customerFilter.Address))
            {
                entity = entity.Where(r => r.Address.Contains(customerFilter.Address.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            return await entity.ToListAsync();
        }

        public async Task<Customers> UpdateCustomerAsync(Customers entity)
        {
            if (entity.Id <= 0)
            {
                throw new CustomerOperationException("Customer id should be greater than 0");
            }

            var existingEntity = await GetCustomerByIdAsync(entity.Id);
            if (existingEntity == null)
            {
                throw new CustomerOperationException($"Customer {entity.Id} is not exists");
            }

            return await _repository.UpdateAsync(entity);
        }
    }
}
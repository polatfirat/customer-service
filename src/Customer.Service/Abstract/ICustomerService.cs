using Customer.Domain.Dto;
using Customer.Domain.Entities;

namespace Customer.Service.Abstract
{
    public interface ICustomerService
    {
        Task<Customers> InsertCustomerAsync(Customers entity);
        Task<Customers> UpdateCustomerAsync(Customers entity);
        Task DeleteCustomerAsync(int entityId);
        Task<Customers> GetCustomerByIdAsync(int id);
        Task<IEnumerable<Customers>> GetAllCustomersAsync();
        Task<IEnumerable<Customers>> SearchCustomerAsync(CustomerFilter customerFilter);
    }
}

using Customer.Domain.Dto;
using Customer.Domain.Entities;
using Customer.Domain.Exceptions;
using Customer.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Customer.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            var entities = await _customerService.GetAllCustomersAsync();
            return entities.Any() ? Ok(entities) : NoContent();
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerByIdAsync(int customerId)
        {
            var entity = await _customerService.GetCustomerByIdAsync(customerId);
            return entity != null ? Ok(entity) : NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> InsertCustomerAsync(Customers customer)
        {
            ValidateModel(ModelState);
            var entities = await _customerService.InsertCustomerAsync(customer);
            return Ok(entities);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomerAsync(Customers customer)
        {
            ValidateModel(ModelState);
            var entities = await _customerService.UpdateCustomerAsync(customer);
            return Ok(entities);
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomerAsync(int customerId)
        {
            await _customerService.DeleteCustomerAsync(customerId);
            return Ok();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomerAsync([FromQuery] CustomerFilter customerFilter)
        {
            var entities = await _customerService.SearchCustomerAsync(customerFilter);
            return entities.Any() ? Ok(entities) : NoContent();
        }

        private void ValidateModel(ModelStateDictionary modelStateDictionary)
        {
            if (!modelStateDictionary.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                throw new CustomerValidationException(message);
            }
        }
    }
}

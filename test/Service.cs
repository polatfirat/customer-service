using Customer.Domain.Dto;
using Customer.Domain.Entities;
using Customer.Domain.Exceptions;
using Customer.Repository.Abstract;
using Customer.Service.Abstract;
using Customer.Service.Concrete;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace Customer.Service.Test
{
    public class CustomerServiceValidation
    {
        private readonly Mock<IRepository<Customers>> customerRepositoryMock = new Mock<IRepository<Customers>>();

        [Fact]
        public void Customer_Service_GetCustomerById_Should_Return_Error_When_EntityId_IsZero()
        {
            var customerService = new CustomerService(customerRepositoryMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<CustomerOperationException>(() => customerService.GetCustomerByIdAsync(0));
        }

        [Fact]
        public void Customer_Service_DeleteCustomer_Should_Return_Error_When_EntityId_IsZero()
        {
            var customerService = new CustomerService(customerRepositoryMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<CustomerOperationException>(() => customerService.DeleteCustomerAsync(0));
        }

        [Fact]
        public void Customer_Service_UpdateCustomer_Should_Return_Error_When_EntityId_IsZero()
        {
            int id = 0;
            var entity = new Customers { Id = id };
            var customerService = new CustomerService(customerRepositoryMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<CustomerOperationException>(() => customerService.UpdateCustomerAsync(entity));
        }

        [Fact]
        public void Customer_Service_UpdateCustomer_Should_Return_Error_When_Customer_Not_Exists()
        {
            int id = 11111;
            var entity = new Customers { Id = id };

            customerRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Customers)null);
            var customerService = new CustomerService(customerRepositoryMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<CustomerOperationException>(() => customerService.UpdateCustomerAsync(entity));
        }

        [Fact]
        public async Task Customer_Service_UpdateCustomer_Should_Return_Updated_Customer()
        {
            int id = 123;
            var entity = new Customers { Id = id };
            var existingEntity = new Customers { Id = id, FirstName = "FirstName", LastName = "LastName", Age = 10, Address = "TestAddress" };
            var updatedEntity = new Customers { Id = id, FirstName = "Updated_FirstName", LastName = "Updated_LastName", Age = 11, Address = "Updated_TestAddress" };
            customerRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingEntity);
            customerRepositoryMock.Setup(r => r.UpdateAsync(entity)).ReturnsAsync(updatedEntity);
            var customerService = new CustomerService(customerRepositoryMock.Object);

            // Act
            var result = await customerService.UpdateCustomerAsync(entity);

            // Assert
            Assert.Equal(updatedEntity, result);
        }

        [Fact]
        public async Task InsertCustomerAsync_ShouldInsertCustomer()
        {
            // Arrange
            var customer = new Customers { Id = 1, FirstName = "Jack", LastName = "Pitt", Age = 30, Address = "123 Baker Str" };

            var repositoryMock = new Mock<IRepository<Customers>>();
            repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Customers>())).ReturnsAsync(customer);

            var customerService = new CustomerService(repositoryMock.Object);

            // Act
            var result = await customerService.InsertCustomerAsync(customer);

            // Assert
            Assert.Equal(customer, result);
            repositoryMock.Verify(r => r.InsertAsync(customer), Times.Once);
        }


        [Fact]
        public async Task Customer_Service_SearchCustomerAsync_ShouldReturnCustomersMatchingFirstName()
        {
            var customerFilter = new CustomerFilter { FirstName = "jack" };
            var customers = new List<Customers>
            {
                new Customers { FirstName = "jack", LastName = "Doe" },
                new Customers { FirstName = "JACK", LastName = "Smith" },
                new Customers { FirstName = "Jack", LastName = "Doe" }
            };

            var mock = customers.BuildMock();
            customerRepositoryMock.Setup(s => s.GetAllQueryable()).Returns(mock);

            var customerService = new CustomerService(customerRepositoryMock.Object);
            var result = await customerService.SearchCustomerAsync(customerFilter);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task Customer_Service_SearchCustomerAsync_ShouldReturnCustomersMatchingLastName()
        {
            var customerFilter = new CustomerFilter { LastName = "Doe" };
            var customers = new List<Customers>
            {
                new Customers { FirstName = "jack", LastName = "Doe" },
                new Customers { FirstName = "JACK", LastName = "Smith" },
                new Customers { FirstName = "Jack", LastName = "Doe" }
            };

            var mock = customers.BuildMock();
            customerRepositoryMock.Setup(s => s.GetAllQueryable()).Returns(mock);

            var customerService = new CustomerService(customerRepositoryMock.Object);
            var result = await customerService.SearchCustomerAsync(customerFilter);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Customer_Service_SearchCustomerAsync_ShouldReturnCustomersMatchingAge()
        {
            var customerFilter = new CustomerFilter { Age = 30 };
            var customers = new List<Customers>
            {
                new Customers { FirstName = "jack", LastName = "Doe", Age=30 },
                new Customers { FirstName = "JACK", LastName = "Smith", Age=30 },
                new Customers { FirstName = "Jack", LastName = "Doe", Age=30 }
            };

            var mock = customers.BuildMock();
            customerRepositoryMock.Setup(s => s.GetAllQueryable()).Returns(mock);

            var customerService = new CustomerService(customerRepositoryMock.Object);
            var result = await customerService.SearchCustomerAsync(customerFilter);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task Customer_Service_SearchCustomerAsync_ShouldReturnCustomersMatchingAll()
        {
            var customerFilter = new CustomerFilter { };
            var customers = new List<Customers>
            {
                new Customers { FirstName = "jack", LastName = "Doe", Age=30 },
                new Customers { FirstName = "JACK", LastName = "Smith", Age=30 },
                new Customers { FirstName = "Jack", LastName = "Doe", Age=30 }
            };

            var mock = customers.BuildMock();
            customerRepositoryMock.Setup(s => s.GetAllQueryable()).Returns(mock);

            var customerService = new CustomerService(customerRepositoryMock.Object);
            var result = await customerService.SearchCustomerAsync(customerFilter);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task Customer_Service_SearchCustomerAsync_ShouldReturnCustomersMatchingNothing()
        {
            var customerFilter = new CustomerFilter { FirstName = "Test" };
            var customers = new List<Customers>
            {
                new Customers { FirstName = "jack", LastName = "Doe", Age=30 },
                new Customers { FirstName = "JACK", LastName = "Smith", Age=30 },
                new Customers { FirstName = "Jack", LastName = "Doe", Age=30 }
            };

            var mock = customers.BuildMock();
            customerRepositoryMock.Setup(s => s.GetAllQueryable()).Returns(mock);

            var customerService = new CustomerService(customerRepositoryMock.Object);
            var result = await customerService.SearchCustomerAsync(customerFilter);

            // Assert
            Assert.Empty(result);
        }
    }
}
using Customer.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Customer.Service.Test
{
    public class Validation
    {
        [Fact]
        public void Customer_Validation_Should_Return_Error_When_FirstName_IsNullOrEmpty()
        {
            var model = new Customers
            {
                Id = 1,
                LastName = "Test LastName",
                Age = 10,
                Address = "Test Address"
            };

            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);

            var errorMessage = validationResults.Select(r => r.ErrorMessage).First();

            Assert.Equal("First name is required", errorMessage);
        }

        [Fact]
        public void Customer_Validation_Should_Return_Error_When_LastName_IsNullOrEmpty()
        {
            var model = new Customers
            {
                Id = 1,
                FirstName = "Test FirstName",
                Age = 10,
                Address = "Test Address"
            };

            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);

            var errorMessage = validationResults.Select(r => r.ErrorMessage).First();

            Assert.Equal("Last name is required", errorMessage);
        }

        [Fact]
        public void Customer_Validation_Should_Return_Error_When_Address_IsNullOrEmpty()
        {
            var model = new Customers
            {
                Id = 1,
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                Age = 10,
            };

            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);

            var errorMessage = validationResults.Select(r => r.ErrorMessage).First();

            Assert.Equal("Address is required", errorMessage);
        }

        [Fact]
        public void Customer_Validation_Should_Return_Error_When_Age_IsOutOfRange()
        {
            var model = new Customers
            {
                Id = 1,
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                Address = "Test Address",
                Age = 120,
            };

            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);

            var errorMessage = validationResults.Select(r => r.ErrorMessage).First();

            Assert.Equal("Age must be between 0 and 100", errorMessage);
        }
    }
}
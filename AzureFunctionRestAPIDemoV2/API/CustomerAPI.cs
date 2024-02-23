using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using DAL2.Models;
using BL2.Manager;
using Microsoft.AspNetCore.Http;
using System;

namespace AzureFunctionRestAPIDemoV2
{
    public class CustomerAPI
    {
        private readonly ILogger _logger;

        public static readonly List<Customer> Items = new List<Customer>();

        public CustomerAPI(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CustomerAPI>();
        }

        [Function("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "customer")] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            CustomerManager cm = new CustomerManager();
            
            var customer = cm.CreateCustomer(requestBody);

            return new OkObjectResult(customer);
        }

        [Function("GetAllCustomersByAge")]
        public async Task<IActionResult> GetAllCustomersByAge([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customers/age")] HttpRequest req, int age)
        {
            /*
             * Test calls from postman: http://localhost:7195/api/customers/age?age=45
             */

            CustomerManager cm = new CustomerManager();

            var customer = cm.GetCustomerByAge(age);

            return new OkObjectResult(customer);
        }

        [Function("GetCustomerByID")]
        public async Task<IActionResult> GetCustomerByID([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customers/id")] HttpRequest req, Guid guid)
        {

            CustomerManager cm = new CustomerManager();

            var customer = cm.GetCustomerByID(guid);

            return new OkObjectResult(customer);
        }


    }
}

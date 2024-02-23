using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using DAL.Models;
using BusinessLogic.Manager;
using Microsoft.AspNetCore.Http;

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
            var test = req;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            CustomerManager cm = new CustomerManager();
            
            var customer = cm.CreateCustomer(requestBody);

            return new OkObjectResult(customer);
        }

        [Function("GetAllCustomersByAge")]
        public async Task<IActionResult> GetAllCustomersByAge([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customer/{age}")] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            CustomerManager cm = new CustomerManager();

            var customer = cm.CreateCustomer(requestBody);

            return new OkObjectResult(customer);
        }

        [Function("GetCustomerByID")]
        public async Task<IActionResult> GetCustomersByID([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customer/{guid}" )] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            CustomerManager cm = new CustomerManager();

            var customer = cm.GetCustomerByID(requestBody);

            return new OkObjectResult(customer);
        }


    }
}

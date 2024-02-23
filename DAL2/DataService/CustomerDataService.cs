using DAL2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL2.DataService
{
    public class CustomerDataService
    {
        public Customer CreateCustomer(Customer model)
        {
            Customer result = new Customer();
            return result;
        }

        public Customer GetCustomerByID(Guid id)
        {
            Customer result = new Customer();
            return result;
        }

        public List<Customer> GetCustomerByAge(int age)
        {
            List<Customer> result = new List<Customer>();
            return result;
        }

        public List<Customer> GetCustomerByFullName(string fullName)
        {
            List<Customer> result = new List<Customer>();
            return result;
        }
    }
}

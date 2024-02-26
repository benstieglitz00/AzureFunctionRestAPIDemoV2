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
        public Customer CreateCustomer(Customer customer)
        {
            SQLClient customerClient = new SQLClient();
            customerClient.CreateCustomer(ref customer);
            return customer;
        }

        public Customer GetCustomerByID(Guid id)
        {
            SQLClient customerClient = new SQLClient();
            return customerClient.GetCustomerByID(id);
        }

        public List<Customer> GetCustomerByAge(int age)
        {
            SQLClient customerClient = new SQLClient();
            return customerClient.GetCustomerByAge(age);
        }

        public Customer GetCustomerByFullName(string fullName)
        {
            SQLClient customerClient = new SQLClient();
            return customerClient.GetCustomerByFullName(fullName);
        }
    }
}

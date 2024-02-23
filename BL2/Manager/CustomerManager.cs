using DAL2.Models;
using DAL2;
using System.Reflection;
using System.Xml.Linq;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using DAL2.DataService;
using System.Net.Http.Json;

namespace BL2.Manager
{
    public class CustomerManager
    {
        private Customer CreateCustomer(Customer model)
        {
            Customer result = new Customer();
            CustomerDataService ds = new CustomerDataService();
            ds.CreateCustomer(model);
            return result;
        }

        public Customer CreateCustomer(string requestBody)
        {
            Customer result = new Customer();

            var customerInput = JsonConvert.DeserializeObject<Customer>(requestBody);

            //Validate customer
            ValidateCustomerRequest(customerInput);

            //Create Customer
            result = CreateCustomer(customerInput);

            return result;
        }

        public Customer GetCustomerByID(string requestBody)
        {
            Customer result = new Customer();

            var customerInput = JsonConvert.DeserializeObject<Customer>(requestBody);

            CustomerDataService ds = new CustomerDataService();

            ds.GetCustomerByID((Guid)customerInput.CustomerID);

            return result;
        }

        public List<Customer> GetCustomerByAge(string requestBody)
        {
            List<Customer> result = new List<Customer>();

            var customerInput = JsonConvert.DeserializeObject<Customer>(requestBody);

            CustomerDataService ds = new CustomerDataService();

            ds.GetCustomerByID((Guid)customerInput.CustomerID);

            return result;
        }

        public Customer GetCustomerByFullName(string requestBody)
        {
            Customer result = new Customer();

            var customerInput = JsonConvert.DeserializeObject<Customer>(requestBody);

            CustomerDataService ds = new CustomerDataService();

            ds.GetCustomerByID((Guid)customerInput.CustomerID);

            return result;
        }

        public bool ValidateCustomerRequest(Customer customerInput)
        {
            bool result = false;

            //Is the customer a valid name?
            result = IsValidFullName(customerInput.FullName);

            //Is the date of birth a valid date?    
            if (customerInput.DateOfBirth == null)
            {
                result = false;
            }
            else
            {
                IsValidAge((DateTime)customerInput.DateOfBirth);
                result = false;
            }

            //Does customer name alread exist?
            result = IsDuplicateFullName(customerInput.FullName);

            //Other validations can go inline here. 

            //TODO:
            //return detailed error message if customer is invalid.

            return result;
        }

        #region Private Validation Methods
        private bool IsValidFullName(string fullName)
        {
            bool result = false;

            result = String.IsNullOrWhiteSpace(fullName);

            // Regex pattern for full name validation
            string pattern = @"^[A-Za-z]+(?:\s+[A-Za-z]+)*$";

            // Check if the full name matches the pattern
            return Regex.IsMatch(fullName, pattern);
        }

        private bool IsValidAge(DateTime dob)
        {
            int age = DateTime.Now.Year - dob.Year;

            //determine if your birthday has happened yet.
            if (dob.Date > DateTime.Now.AddYears(-age))
            {
                age--;
            }

            //valid from 1 - 120. 
            return age >= 1 && age <= 120;
        }

        private bool IsDuplicateFullName(string fullName)
        {
            CustomerManager cm = new CustomerManager();
            if (cm.GetCustomerByFullName(fullName) != null)
                return true;
            else
                return false;

        }

        private int CalculateAge(DateTime birthdate, DateTime currentDate)
        {
            int age = currentDate.Year - birthdate.Year;

            // Check if the birthday has occurred this year
            if (birthdate.Date > currentDate.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        #endregion
    }
}

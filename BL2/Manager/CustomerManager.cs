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
        private Customer CreateCustomer(Customer customer)
        {
            CustomerDataService ds = new CustomerDataService();
            ds.CreateCustomer(customer);
            return customer;
        }

        public Customer CreateCustomer(string requestBody)
        {
            Customer result = new Customer();

            try
            {

                var customerInput = JsonConvert.DeserializeObject<Customer>(requestBody);

                //Validate customer
                ValidateCustomerRequest(customerInput);

                //Create Customer
                result = CreateCustomer(customerInput);

            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public Customer GetCustomerByID(Guid customerID)
        {
            Customer result = new Customer();

            CustomerDataService ds = new CustomerDataService();

            result = ds.GetCustomerByID(customerID);

            return result;
        }

        public List<Customer> GetCustomerByAge(int age)
        {
            List<Customer> result = new List<Customer>();

            CustomerDataService ds = new CustomerDataService();

            result = ds.GetCustomerByAge(age);

            return result;
        }

        public Customer GetCustomerByFullName(string requestBody)
        {
            Customer result = new Customer();

            var customerInput = JsonConvert.DeserializeObject<Customer>(requestBody);

            CustomerDataService ds = new CustomerDataService();

            if (customerInput != null && customerInput.CustomerID != null)
            {
                ds.GetCustomerByID((Guid)customerInput.CustomerID);
            }

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
                IsValidAge((DateOnly)customerInput.DateOfBirth);
                result = false;
            }

            //Does customer name alread exist?
            //TODO: needs new overload that is not an http request
            //result = IsDuplicateFullName(customerInput.FullName);

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

        private bool IsValidAge(DateOnly dob)
        {
            int age = DateTime.Now.Year - dob.Year;

            //determine if your birthday has happened yet.
            if (DateTime.Parse(dob.ToString()) > DateTime.Now.AddYears(-age))
            {
                age--;
            }

            //valid from 1 - 120. 
            return age >= 1 && age <= 120;
        }

        private bool IsDuplicateFullName(string fullName)
        {
            //TODO: Overload this call.
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

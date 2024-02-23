using DAL2.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DAL2
{
    public class SQLClient
    {
        public static SQLiteConnection _conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True; ");

        public SQLClient()
        {
            //Initialize Table Objects
            InitializeTables();
        }


        #region Initialize Tables

        private void InitializeTables()
        {
            //Create and initialize all tables here:
            CreateCustomerTable();
            //....add more;
        }

        private void CreateCustomerTable()
        {
            try
            {
                SQLiteCommand cmd;
                //TODO: Correct for DateOnly variable type, not DateTime
                string CreateSQL = "CREATE TABLE Customer (CustomerID GUID, FullName NVARCHAR(100), DateOfBirth DATETIME)";
                cmd = _conn.CreateCommand();
                cmd.CommandText = CreateSQL;
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
            catch(Exception ex)
            {
                //Log error and close connection
                _conn.Close();
            }
        }
        #endregion

        #region Test Data
        public void CreateTestData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable(Col1, Col2) VALUES('Test Text ', 1); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable(Col1, Col2) VALUES('Test1 Text1 ', 2); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable(Col1, Col2) VALUES('Test2 Text2 ', 3); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable1(Col1, Col2) VALUES('Test3 Text3 ', 3); ";
            sqlite_cmd.ExecuteNonQuery();
        }
        #endregion

        #region CRUD Operations

        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="customer"></param>
        public bool CreateCustomer(ref Customer customer)
        {
            try
            {
                SQLiteCommand sqlite_cmd;
                customer.CustomerID = Guid.NewGuid();
                sqlite_cmd = _conn.CreateCommand();
                sqlite_cmd.CommandText = "INSERT INTO Customer(CustomerID, FullName, DateOfBirth) VALUES('" + customer.CustomerID + "', '" + customer.FullName + "','" + customer.DateOfBirth + "')";
                _conn.Open();
                sqlite_cmd.ExecuteNonQuery();
                _conn.Close();
            }
            catch(Exception ex)
            {
                //Log error and close connection
                _conn.Close();
            }

            return true;
        }

        /// <summary>
        /// Get customer by FullName
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public Customer GetCustomerByFullName(String fullname)
        {
            Customer c = null;

            try
            {
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = _conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT TOP 1 CustomerID, FullName, DateOfBirth FROM Customer WHERE FullName = " + fullname;
                _conn.Open();
                sqlite_datareader = sqlite_cmd.ExecuteReader();

                while (sqlite_datareader.Read())
                {
                    string myreader = sqlite_datareader.GetString(0);
                    Console.WriteLine(myreader);
                }

                while (sqlite_datareader.Read())
                {
                    c = new Customer
                    {
                        CustomerID = Guid.Parse(Convert.ToString(sqlite_datareader["CustomerID"])),
                        FullName = Convert.ToString(sqlite_datareader["FullName"]),
                        DateOfBirth = Convert.ToDateTime(sqlite_datareader["DateOfBirth"])
                    };

                }

                _conn.Close();
            }
            catch(Exception ex)
            {
                _conn.Close();
            }

            return c;
        }

        /// <summary>
        /// Get customer by GUID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Customer GetCustomerByID(Guid customerID)
        {
            Customer c = null;

            try
            {
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = _conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT CustomerID, FullName, DateOfBirth FROM Customer WHERE CustomerID = '" + customerID + "'";
                _conn.Open();
                sqlite_datareader = sqlite_cmd.ExecuteReader();

                while (sqlite_datareader.Read())
                {
                    c = new Customer
                    {
                        CustomerID = Guid.Parse(Convert.ToString(sqlite_datareader["CustomerID"])),
                        FullName = Convert.ToString(sqlite_datareader["FullName"]),
                        DateOfBirth = Convert.ToDateTime(sqlite_datareader["DateOfBirth"])
                    };

                }

                _conn.Close();
            }
            catch(Exception ex )
            {
                _conn.Close();
            }

            return c;
        }

        /// <summary>
        /// Gets all customers that have a specific age
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        public List<Customer> GetCustomerByAge(int age)
        {
            List<Customer> resultList = null;

            DateTime currentDate = DateTime.Today;

            // Calculate the latest possible birth date
            DateTime latestBirthDate = currentDate.AddYears(-age);

            // Calculate the earliest possible birth date
            DateTime earliestBirthDate = currentDate.AddYears(-age - 1);

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = _conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT CustomerID, FullName, DateOfBirth FROM Customer WHERE DateOfBirth BETWEEN = " + earliestBirthDate + " AND " + latestBirthDate;
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                Console.WriteLine(myreader);
            }

            while (sqlite_datareader.Read())
            {
                Customer c = new Customer
                {
                    CustomerID = Guid.Parse(Convert.ToString(sqlite_datareader["CustomerID"])),
                    FullName = Convert.ToString(sqlite_datareader["FullName"]),
                    DateOfBirth = Convert.ToDateTime(sqlite_datareader["DateOfBirth"])
                };
                resultList.Add(c);
            }

            _conn.Close();

            return resultList;
        }
        #endregion 

    }
}

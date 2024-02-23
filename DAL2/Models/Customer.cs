using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL2.Models
{
    public class Customer
    {
        public Guid? CustomerID { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}

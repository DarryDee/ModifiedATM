using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModifiedATM.BO
{
    public class Customer
    {
        public string? Username { get; set; }
        public string? Pin { get; set; }
        public int AccountNumber { get; set; }
        public string? Balance { get; set; }
    }
}

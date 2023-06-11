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
        public int Pin { get; set; }
        public int? AccountNumber { get; set; }
        public int? Balance { get; set; }
        public string? Typ { get; set; }
        public string? Status { get; set; }
    }
}

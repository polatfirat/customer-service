using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Domain.Dto
{
    public class CustomerFilter
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}

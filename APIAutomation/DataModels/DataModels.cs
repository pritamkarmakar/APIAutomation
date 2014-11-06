using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAutomation.DataModels
{

    public class Company
    {
        public List<Employee> Content { get; set; }
    }
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}

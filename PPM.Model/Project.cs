using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace PPM.Model
{
    [Serializable]
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Default Name";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Define a property to store assigned employees.
        public List<Employee> AssignedEmployees { get; set; }

        public Project()
        {
            
            AssignedEmployees = new List<Employee>();
        }
    }
}
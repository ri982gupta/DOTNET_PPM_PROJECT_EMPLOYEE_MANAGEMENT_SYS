using System;
using System.Collections.Generic;
using System.Linq;

namespace PPM.Model
{
    [Serializable]
    public class Employee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public int RoleId { get; set; }
    }
}

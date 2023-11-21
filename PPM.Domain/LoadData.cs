using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;
using PPM.Model;

namespace PPM.Domain
{
    public class LoadData
    {
        public void Load()
        {
            try
            {
                // Deserialize and load application state from XML files
                XmlSerializer projectSerializer = new XmlSerializer(typeof(List<Project>));
                XmlSerializer employeeSerializer = new XmlSerializer(typeof(List<Employee>));
                XmlSerializer roleSerializer = new XmlSerializer(typeof(List<Role>));

                using (var projectReader = new StreamReader("Projects.xml"))
                {
                    ProjectRepo.projects =
                        (List<Project>)projectSerializer.Deserialize(projectReader);
                }

                using (var employeeReader = new StreamReader("Employees.xml"))
                {
                    EmployeeRepo.employees =
                        (List<Employee>)employeeSerializer.Deserialize(employeeReader);
                }

                using (var roleReader = new StreamReader("Roles.xml"))
                {
                    RoleRepo.roles = (List<Role>)roleSerializer.Deserialize(roleReader);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred while loading data: {ex.Message}");
            }
        }
    }
}

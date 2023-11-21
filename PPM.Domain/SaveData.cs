using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;
using PPM.Model;

namespace PPM.Domain
{
    public class SaveData
    {
        public void Save()
        {
            try
            {
                // Serialize and save application state to an XML file
                XmlSerializer projectSerializer = new XmlSerializer(typeof(List<Project>));
                XmlSerializer employeeSerializer = new XmlSerializer(typeof(List<Employee>));
                XmlSerializer roleSerializer = new XmlSerializer(typeof(List<Role>));

                using (var projectWriter = new StreamWriter("Projects.xml"))
                {
                    projectSerializer.Serialize(projectWriter, ProjectRepo.projects);
                }

                using (var employeeWriter = new StreamWriter("Employees.xml"))
                {
                    employeeSerializer.Serialize(employeeWriter, EmployeeRepo.employees);
                }

                using (var roleWriter = new StreamWriter("Roles.xml"))
                {
                    roleSerializer.Serialize(roleWriter, RoleRepo.roles);
                }

                System.Console.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred while saving data: {ex.Message}");
            }
        }
    }
}

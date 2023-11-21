using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using PPM.Domain;
using PPM.Model;

namespace PPM.Console.UI
{
    public class Menu
    {
        ProjectRepo projectRepoObj = new ProjectRepo();
        EmployeeRepo employeeRepoObj = new EmployeeRepo();
        RoleRepo roleRepoObj = new RoleRepo();
        SaveData saveDataObj = new SaveData();
        LoadData loadDataObj = new LoadData();

        public int ModuleChoices()
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGreen;

            int moduleChoice;

            System.Console.WriteLine("-----------------------------------------------------");
            System.Console.WriteLine("\n                   MAIN MENU                       ");
            System.Console.WriteLine("-----------------------------------------------------");

            System.Console.WriteLine("\n1. Project Module");
            System.Console.WriteLine("\n2. Employee Module");
            System.Console.WriteLine("\n3. Role Module");
            System.Console.WriteLine("\n4. Save");
            System.Console.WriteLine("\n5. Load");
            System.Console.WriteLine("\n6. Quit");
            System.Console.WriteLine();
            System.Console.WriteLine();

            System.Console.WriteLine("Enter your module choice :");
            moduleChoice = int.Parse(System.Console.ReadLine()!);
            System.Console.WriteLine();

            return moduleChoice;
        }

        public int ProjectModuleOperations()
        {
            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;

            int choice;

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine("Module Options  :");
            System.Console.WriteLine("\n1. Add ");
            System.Console.WriteLine("\n2. List all");
            System.Console.WriteLine("\n3. List by ID");
            System.Console.WriteLine("\n4. Delete");
            System.Console.WriteLine("\n5. View");
            System.Console.WriteLine("\n6. Update");
            System.Console.WriteLine("\n7. AssignEmployeeToProject");
            System.Console.WriteLine("\n8. RemoveEmployeeFromProject");
            System.Console.WriteLine("\n9. ViewProjectDetails");
            System.Console.WriteLine("\n10. Return to Main Menu");
            System.Console.WriteLine();
            System.Console.WriteLine();

            System.Console.ResetColor();

            System.Console.WriteLine("Enter your choice for this module :");
            choice = int.Parse(System.Console.ReadLine()!);
            System.Console.WriteLine();

            return choice;
        }

        public int EmployeeModuleOperations()
        {
            System.Console.ForegroundColor = ConsoleColor.DarkCyan;

            int choice;

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine("Module Options  :");
            System.Console.WriteLine("\n1. Add ");
            System.Console.WriteLine("\n2. List all");
            System.Console.WriteLine("\n3. List by ID");
            System.Console.WriteLine("\n4. Delete");
            System.Console.WriteLine("\n5. View");
            System.Console.WriteLine("\n6. Update");
            System.Console.WriteLine("\n7. Return to Main Menu");
            System.Console.WriteLine();
            System.Console.WriteLine();

            System.Console.ResetColor();

            System.Console.WriteLine("Enter your choice for this module :");
            choice = int.Parse(System.Console.ReadLine()!);
            System.Console.WriteLine();

            return choice;
        }

        public int RoleModuleOperations()
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGray;

            int choice;

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine("Module Options  :");
            System.Console.WriteLine("\n1. Add ");
            System.Console.WriteLine("\n2. List all");
            System.Console.WriteLine("\n3. List by ID");
            System.Console.WriteLine("\n4. Delete");
            System.Console.WriteLine("\n5. View");
            System.Console.WriteLine("\n6. Update");
            System.Console.WriteLine("\n7. Return to Main Menu");
            System.Console.WriteLine();
            System.Console.WriteLine();

            System.Console.ResetColor();

            System.Console.WriteLine("Enter your choice for this module :");
            choice = int.Parse(System.Console.ReadLine()!);
            System.Console.WriteLine();

            return choice;
        }

        public void AddProject()
        {
            try
            {
                Project objProject = new();
                System.Console.WriteLine("\nProject details:");

                System.Console.Write("Project ID: ");
                int projectId = int.Parse(System.Console.ReadLine()!);

                // Check if a project with the same ID already exists.
                if (projectRepoObj.GetItemById(projectId) != null)
                {
                    System.Console.WriteLine(
                        $"A project with ID {projectId} already exists. project not added."
                    );
                    return;
                }
                else
                {
                    objProject.Id = projectId;
                }

                System.Console.WriteLine();

                // Validate the project id
                if (projectId <= 0)
                {
                    System.Console.WriteLine(
                        "Project ID must be a positive integer. Project not added."
                    );
                    System.Console.WriteLine();
                }

                System.Console.Write("Project Name: ");
                string projectName = System.Console.ReadLine()!;
                objProject.Name = projectName;

                // Validate the project name
                if (string.IsNullOrWhiteSpace(projectName))
                {
                    System.Console.WriteLine("Project name cannot be empty. Project not added.");
                    System.Console.WriteLine();
                }

                System.Console.Write("Start Date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.Parse(System.Console.ReadLine()!);

                System.Console.Write("End Date (yyyy-MM-dd): ");
                DateTime endDate = DateTime.Parse(System.Console.ReadLine()!);

                // Validate the project dates
                if (startDate >= endDate)
                {
                    System.Console.Write(
                        "Start date must be before the end date. Project not added"
                    );
                    System.Console.WriteLine();
                    return;
                }
                else
                {
                    objProject.StartDate = startDate;
                    objProject.EndDate = endDate;
                }

                Project project = new Project
                {
                    Id = projectId,
                    Name = projectName,
                    StartDate = startDate,
                    EndDate = endDate,
                    AssignedEmployees = new List<Employee>() // Initialize AssignedEmployees list
                };

                projectRepoObj.Add(objProject);
                System.Console.WriteLine("\nProject added successfully.");

                // Now, allow the user to add employees to the project.
                System.Console.WriteLine(
                    "Do you want to add employees to this project? (yes/no): "
                );
                string addEmployeesChoice = System.Console.ReadLine()!.ToLower();
                if (addEmployeesChoice == "yes")
                {
                    System.Console.WriteLine("Adding Employees to the Project: ");

                    AddEmployeesToProjectWithRole(projectId);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void AddEmployeesToProjectWithRole(int projectId)
        {
            try
            {
                while (true)
                {
                    // Display available employees and let the user select an employee.
                    System.Console.WriteLine("\nAvailable Employees:");
                    List<Employee> employees = employeeRepoObj.GetItems();
                    foreach (var employee in employees)
                    {
                        System.Console.WriteLine(
                            $"Employee ID: {employee.Id}, Name: {employee.FirstName} {employee.LastName}, role Id: {employee.RoleId} role: {GetRoleName(employee.RoleId)}"
                        );
                    }

                    System.Console.WriteLine(
                        "Enter Employee Id to add to the project ( or 'done' to finish adding ): "
                    );

                    string? input = System.Console.ReadLine();
                    if (input!.ToLower() == "done")
                    {
                        break;
                    }

                    if (int.TryParse(input, out int employeeId))
                    {
                        Employee employee = employeeRepoObj.GetItemById(employeeId);
                        if (employee != null)
                        {
                            projectRepoObj.AssignEmployeeToProject(
                                employeeId,
                                projectId,
                                employees
                            );
                            System.Console.WriteLine(
                                $"{employee.FirstName} {employee.LastName} with role ID {employee.RoleId} and role Name {GetRoleName(employee.RoleId)} added to the project. "
                            );
                        }
                        else
                        {
                            System.Console.WriteLine(
                                "Employee not found. Please enter a valid Employee ID."
                            );
                        }
                    }
                    else
                    {
                        System.Console.WriteLine(
                            "Invalid input, Please enter a valid Employee ID or 'done' to finish adding employees."
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void ListAllProjects()
        {
            try
            {
                List<Project> projects = projectRepoObj.GetItems();
                if (projects.Count > 0)
                {
                    System.Console.WriteLine("List of All Projects:");
                    foreach (var project in projects)
                    {
                        System.Console.WriteLine($"ID: {project.Id}");
                        System.Console.WriteLine($"Name: {project.Name}");
                        System.Console.WriteLine($"Start Date: {project.StartDate}");
                        System.Console.WriteLine($"End Date: {project.EndDate}");
                        System.Console.WriteLine();
                    }
                }
                else
                {
                    System.Console.WriteLine("No projects found.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void ListProjectById()
        {
            try
            {
                System.Console.WriteLine("Enter Project ID to view details: ");
                if (int.TryParse(System.Console.ReadLine(), out int projectId))
                {
                    Project project = projectRepoObj.GetItemById(projectId);
                    if (project != null)
                    {
                        System.Console.WriteLine($"ID: {project.Id}");
                        System.Console.WriteLine($"Name: {project.Name}");
                        System.Console.WriteLine($"Start Date: {project.StartDate}");
                        System.Console.WriteLine($"End Date: {project.EndDate}");
                    }
                    else
                    {
                        System.Console.WriteLine("Project not found");
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid Project ID.");
                }
            }
            catch (FormatException ex)
            {
                System.Console.WriteLine($"Invalid input format: {ex.Message}");
                // Handle format-related exceptions (e.g., invalid number format)
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void DeleteProject()
        {
            try
            {
                System.Console.Write("\nEnter Employee ID to remove: ");
                int employeeId = int.Parse(System.Console.ReadLine()!);

                System.Console.Write("Enter Project ID to delete: ");
                if (int.TryParse(System.Console.ReadLine(), out int projectId))
                {
                    Project projectToDelete = projectRepoObj.GetItemById(projectId);

                    if (projectToDelete != null)
                    {
                        System.Console.WriteLine("Project to Delete:");
                        System.Console.WriteLine($"ID: {projectToDelete.Id}");
                        System.Console.WriteLine($"Name: {projectToDelete.Name}");
                        System.Console.WriteLine($"Start Date: {projectToDelete.StartDate}");
                        System.Console.WriteLine($"End Date: {projectToDelete.EndDate}");
                        // Display other project properties as needed.

                        System.Console.Write(
                            "Are you sure you want to delete this project? (yes/no): "
                        );
                        string confirmation = System.Console.ReadLine()!.ToLower();

                        if (confirmation == "yes")
                        {
                            // Before deleting the project, remove employees assigned to it.
                            projectRepoObj.RemoveEmployeeFromProject(projectId, employeeId);

                            projectRepoObj.Delete(projectId);
                            System.Console.WriteLine("Project deleted successfully.");
                        }
                        else
                        {
                            System.Console.WriteLine("Deletion canceled.");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Project not found.");
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid Project ID.");
                }
            }
            catch (FormatException ex)
            {
                System.Console.WriteLine($"Invalid input format: {ex.Message}");
                // Handle format-related exceptions (e.g., invalid number format)
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void ViewProjects()
        {
            try
            {
                List<Project> projects = projectRepoObj.GetItems();

                if (projects.Count == 0)
                {
                    System.Console.WriteLine("No projects available.");
                }
                else
                {
                    System.Console.WriteLine("\nList of Projects:");
                    foreach (var project in projects)
                    {
                        System.Console.WriteLine($"Project ID: {project.Id}");
                        System.Console.WriteLine($"Project Name: {project.Name}");
                        System.Console.WriteLine(
                            $"Start Date: {project.StartDate.ToShortDateString()}"
                        );
                        System.Console.WriteLine(
                            $"End Date: {project.EndDate.ToShortDateString()}"
                        );
                        System.Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void UpdateProjects()
        {
            try
            {
                Project objProject = new();

                System.Console.WriteLine("\nEnter the project id which you want to update:");
                int Id = int.Parse(System.Console.ReadLine()!);

                // Validate the project id
                if (Id <= 0)
                {
                    System.Console.WriteLine(
                        "Project ID must be a positive integer. Project not added."
                    );
                    System.Console.WriteLine();
                    return;
                }

                System.Console.WriteLine("\nEnter the project name:");
                string? Name = System.Console.ReadLine();

                // Validate the project name
                if (string.IsNullOrWhiteSpace(Name))
                {
                    System.Console.WriteLine("Project name cannot be empty. Project not added.");
                    System.Console.WriteLine();
                    return;
                }

                System.Console.WriteLine("\nEnter the Start Date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.Parse(System.Console.ReadLine()!);

                System.Console.WriteLine("\nEnd Date (yyyy-MM-dd): ");
                DateTime endDate = DateTime.Parse(System.Console.ReadLine()!);

                // Validate the project startdate and enddate
                if (startDate >= endDate)
                {
                    System.Console.Write(
                        "Start date must be before the end date. Project not added"
                    );
                    System.Console.WriteLine();
                    return;
                }

                objProject.Id = Id;
                objProject.Name = Name!;
                objProject.StartDate = startDate;
                objProject.EndDate = endDate;

                projectRepoObj.UpdateItem(objProject);
                System.Console.WriteLine("\nProject updated successfully.");
            }
            catch (FormatException ex)
            {
                System.Console.WriteLine($"Invalid input format: {ex.Message}");
                // Handle format-related exceptions (e.g., invalid date format)
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void AddEmployee()
        {
            try
            {
                Employee objEmployee = new();

                System.Console.WriteLine("\nEnter employee details:");

                System.Console.Write("Employee ID: ");
                int employeeId = int.Parse(System.Console.ReadLine()!);

                // Validate the employeeid
                if (employeeId <= 0)
                {
                    System.Console.WriteLine(
                        "Employee ID must be a positive integer. Employee not added."
                    );
                    return;
                }

                System.Console.WriteLine();

                // Check if an employee with the same ID already exists.
                if (employeeRepoObj.GetItemById(employeeId) != null)
                {
                    System.Console.WriteLine(
                        $"An employee with ID {employeeId} already exists. Employee not added."
                    );
                    return;
                }
                else
                {
                    objEmployee.Id = employeeId;
                }

                System.Console.Write("First Name: ");
                string firstName = System.Console.ReadLine()!;

                // Validate the firstname
                if (string.IsNullOrWhiteSpace(firstName))
                {
                    System.Console.WriteLine("First name cannot be empty. Employee not added.");
                    return;
                }
                else
                {
                    objEmployee.FirstName = firstName;
                }

                System.Console.Write("Last Name: ");
                string lastName = System.Console.ReadLine()!;

                // Validate the lastname
                if (string.IsNullOrWhiteSpace(lastName))
                {
                    System.Console.WriteLine("Last name cannot be empty. Employee not added.");
                    return;
                }
                else
                {
                    objEmployee.LastName = lastName;
                }

                System.Console.Write("Email: ");
                string email = System.Console.ReadLine()!;

                // Validate the email
                if (!(Regex.Match(email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$").Success))
                {
                    System.Console.WriteLine("Invalid email, Enter the correct email");
                    System.Console.WriteLine();
                    return;
                }
                else
                {
                    objEmployee.Email = email;
                }

                System.Console.Write("Mobile: ");
                string mobile = System.Console.ReadLine()!;

                // Validate the mobile
                if (Regex.Match(mobile, @"^(\+[0-9])$").Success)
                {
                    System.Console.WriteLine("Invalid mobile, Enter the correct mobile");
                    System.Console.WriteLine();
                    return;
                }
                else
                {
                    objEmployee.Mobile = mobile;
                }

                System.Console.Write("Address: ");
                string address = System.Console.ReadLine()!;

                // Validate the address
                if (string.IsNullOrWhiteSpace(address))
                {
                    System.Console.WriteLine("Address cannot be empty. Employee not added.");
                    return;
                }
                else
                {
                    objEmployee.Address = address;
                }

                // Display available roles and let the user select a role.
                System.Console.WriteLine("\nAvailable Roles:");
                List<Role> roles = roleRepoObj.GetItems();
                foreach (var role in roles)
                {
                    System.Console.WriteLine($"Role ID: {role.Id}, Name: {role.Name}");
                }

                System.Console.Write("Enter Role ID: ");
                int roleId = int.Parse(System.Console.ReadLine()!);

                objEmployee.RoleId = roleId;

                // Check if the selected role exists.
                Role selectedRole = roleRepoObj.GetItemById(roleId);
                if (selectedRole == null)
                {
                    System.Console.WriteLine("Invalid Role ID. Employee not added.");
                    return;
                }

                // Add the employee.
                employeeRepoObj.Add(objEmployee);
                System.Console.WriteLine("\nEmployee added successfully.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void ListAllEmployees()
        {
            try
            {
                // Get the list of all employees from the employee repo.
                List<Employee> employees = employeeRepoObj.GetItems();

                if (employees.Count > 0)
                {
                    System.Console.WriteLine("List of All Employees:");

                    foreach (var employee in employees)
                    {
                        System.Console.WriteLine($"ID: {employee.Id}");
                        System.Console.WriteLine($"Name: {employee.FirstName} {employee.LastName}");
                        System.Console.WriteLine($"Email: {employee.Email}");
                        System.Console.WriteLine($"Mobile: {employee.Mobile}");
                        System.Console.WriteLine($"Address: {employee.Address}");
                        System.Console.WriteLine($"Role: {GetRoleName(employee.RoleId)}");
                        // Display other employee properties as needed.
                        System.Console.WriteLine();
                    }
                }
                else
                {
                    System.Console.WriteLine("No employees found.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void ListEmployeeById()
        {
            try
            {
                System.Console.Write("Enter Employee ID to view details: ");
                if (int.TryParse(System.Console.ReadLine(), out int employeeId))
                {
                    // Get the employee by ID from the employee repo
                    Employee employee = employeeRepoObj.GetItemById(employeeId);

                    if (employee != null)
                    {
                        System.Console.WriteLine("Employee Details:");
                        System.Console.WriteLine($"ID: {employee.Id}");
                        System.Console.WriteLine($"Name: {employee.FirstName} {employee.LastName}");
                        System.Console.WriteLine($"Email: {employee.Email}");
                        System.Console.WriteLine($"Mobile: {employee.Mobile}");
                        System.Console.WriteLine($"Address: {employee.Address}");
                        System.Console.WriteLine($"Role: {GetRoleName(employee.RoleId)}");
                        // Display other employee properties as needed.
                    }
                    else
                    {
                        System.Console.WriteLine("Employee not found.");
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid Employee ID.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Helper method to get the role name based on role ID
        public string GetRoleName(int roleId)
        {
            try
            {
                Role role = roleRepoObj.GetItemById(roleId);
                if (role != null)
                {
                    return role.Name!;
                }
                return "Role not found";
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(
                    $"An error occurred while getting role name: {ex.Message}"
                );

                return "Role not found";
            }
        }

        public void DeleteEmployee()
        {
            try
            {
                System.Console.Write("Enter Employee ID to delete: ");
                if (int.TryParse(System.Console.ReadLine(), out int employeeId))
                {
                    // Check if the employee exists
                    Employee employeeToDelete = employeeRepoObj.GetItemById(employeeId);

                    if (employeeToDelete != null)
                    {
                        // Display employee details before deletion
                        System.Console.WriteLine("Employee to Delete:");
                        System.Console.WriteLine($"ID: {employeeToDelete.Id}");
                        System.Console.WriteLine(
                            $"Name: {employeeToDelete.FirstName} {employeeToDelete.LastName}"
                        );
                        System.Console.WriteLine($"Email: {employeeToDelete.Email}");
                        System.Console.WriteLine($"Mobile: {employeeToDelete.Mobile}");
                        System.Console.WriteLine($"Address: {employeeToDelete.Address}");
                        System.Console.WriteLine($"Role: {GetRoleName(employeeToDelete.RoleId)}");
                    }

                    List<Project> projects = projectRepoObj.GetItems();
                    // Check if the employee is assigned to any project
                    foreach (var project in projects)
                    {
                        var employeeInProject = project.AssignedEmployees.FirstOrDefault(
                            e => e.Id == employeeId
                        );
                        if (employeeInProject != null)
                        {
                            System.Console.Write(
                                "Are you sure you want to remove the employee from the project:"
                            );
                            string confirm = System.Console.ReadLine()!.ToLower();

                            if (confirm == "yes")
                            {
                                // Remove the employee from the project
                                project.AssignedEmployees.Remove(employeeInProject);
                                System.Console.WriteLine(
                                    $"Employee {employeeInProject.FirstName} {employeeInProject.LastName} removed from Project {project.Id}"
                                );
                            }
                            else
                            {
                                System.Console.WriteLine("Deletion canceled.");
                            }
                            System.Console.WriteLine($"Employee removed from the project");
                        }
                    }

                    // After removing the employee from projects, delete the employee
                    // Your code for deleting the employee from the system goes here


                    System.Console.Write(
                        "Are you sure you want to delete this employee? (yes/no): "
                    );
                    string confirmation = System.Console.ReadLine()!.ToLower();

                    if (confirmation == "yes")
                    {
                        // Delete the employee from the project manager
                        employeeRepoObj.Delete(employeeId);
                        System.Console.WriteLine("Employee deleted successfully.");
                    }
                    else
                    {
                        System.Console.WriteLine("Deletion canceled.");
                    }

                    System.Console.WriteLine(
                        $"Employee with ID {employeeId} deleted successfully."
                    );
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void UpdateEmployees()
        {
            try
            {
                Employee objEmployee = new();

                System.Console.WriteLine("\nEnter the employee id which you want to update:");
                if (!int.TryParse(System.Console.ReadLine(), out int Id) || Id <= 0)
                {
                    throw new ArgumentException(
                        "Invalid input. Employee ID must be a positive integer."
                    );
                }

                System.Console.WriteLine("\nEnter the employee firstname:");
                string? FirstName = System.Console.ReadLine();

                // Validate the firstname
                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    throw new ArgumentException("First name cannot be empty. Employee not added.");
                }

                System.Console.WriteLine("\nEnter the employee lastname:");
                string? LastName = System.Console.ReadLine();

                // Validate the Lastname
                if (string.IsNullOrWhiteSpace(LastName))
                {
                    throw new ArgumentException("Last name cannot be empty. Employee not added.");
                }

                System.Console.WriteLine("\nEnter the email:");
                string? Email = System.Console.ReadLine();

                // Validate the email
                if (!(Regex.Match(Email!, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$").Success))
                {
                    throw new ArgumentException("Invalid email. Enter the correct email.");
                }

                System.Console.WriteLine("\nEnter the mobile:");
                string? mobile = System.Console.ReadLine();

                // Validate the mobile
                if (Regex.Match(mobile!, @"^(\+[0-9])$").Success)
                {
                    throw new ArgumentException("Invalid mobile. Enter the correct mobile.");
                }

                System.Console.WriteLine("\nEnter the address:");
                string? address = System.Console.ReadLine();

                // Validate the address
                if (string.IsNullOrWhiteSpace(address))
                {
                    throw new ArgumentException("Address cannot be empty. Employee not added.");
                }

                // Display available roles and let the user select a role.
                System.Console.WriteLine("\nAvailable Roles:");
                List<Role> roles = roleRepoObj.GetItems();
                foreach (var role in roles)
                {
                    System.Console.WriteLine($"Role ID: {role.Id}, Name: {role.Name}");
                }

                System.Console.Write("Enter Role ID: ");
                if (!int.TryParse(System.Console.ReadLine(), out int roleId))
                {
                    throw new ArgumentException(
                        "Invalid input. Role ID must be a positive integer."
                    );
                }

                objEmployee.Id = Id;
                objEmployee.FirstName = FirstName;
                objEmployee.LastName = LastName;
                objEmployee.Email = Email;
                objEmployee.Mobile = mobile;
                objEmployee.Address = address;
                objEmployee.RoleId = roleId;

                employeeRepoObj.UpdateItem(objEmployee);
                System.Console.WriteLine("\nEmployee updated successfully");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void ViewEmployees()
        {
            try
            {
                System.Console.WriteLine("\nList of Employees:");
                List<Employee> employees = employeeRepoObj.GetItems();

                if (employees.Count == 0)
                {
                    System.Console.WriteLine("No employees available.");
                }
                else
                {
                    foreach (var employee in employees)
                    {
                        System.Console.WriteLine($"Employee ID: {employee.Id}");
                        System.Console.WriteLine($"Name: {employee.FirstName} {employee.LastName}");
                        System.Console.WriteLine($"Email: {employee.Email}");
                        System.Console.WriteLine($"Mobile: {employee.Mobile}");
                        System.Console.WriteLine($"Address: {employee.Address}");

                        Role role = roleRepoObj.GetItemById(employee.RoleId);
                        if (role != null)
                        {
                            System.Console.WriteLine($"Role: {role.Name}");
                        }

                        System.Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void AddRole()
        {
            try
            {
                Role objRole = new();
                System.Console.WriteLine("\nEnter role details:");

                System.Console.Write("Role ID: ");
                if (!int.TryParse(System.Console.ReadLine(), out int roleId))
                {
                    throw new ArgumentException(
                        "Invalid input. Role ID must be a positive integer."
                    );
                }

                // Validate the role ID
                if (roleId <= 0)
                {
                    throw new ArgumentException(
                        "Role ID must be a positive integer. Role not added."
                    );
                }
                System.Console.WriteLine();

                // Check if the role id already exists
                if (RoleRepo.roles.Exists(role => role.Id == roleId))
                {
                    throw new InvalidOperationException("Role ID already exists.");
                }
                else
                {
                    objRole.Id = roleId;
                }

                System.Console.Write("Role Name: ");
                string roleName = System.Console.ReadLine()!;
                objRole.Name = roleName;

                // Validate the role Name
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    throw new ArgumentException("Role name cannot be empty. Role not added.");
                }

                // Check if the role with the same ID already exists.
                Role existingRole = roleRepoObj.GetItemById(roleId);
                if (existingRole != null)
                {
                    throw new InvalidOperationException(
                        $"Role with ID {roleId} already exists. Role not added."
                    );
                }

                // Add the role.
                roleRepoObj.Add(objRole);
                System.Console.WriteLine("\nRole added successfully.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void ListAllRoles()
        {
            try
            {
                List<Role> roles = roleRepoObj.GetItems();

                if (roles.Count > 0)
                {
                    System.Console.WriteLine("List of All Roles:");

                    foreach (var role in roles)
                    {
                        System.Console.WriteLine($"ID: {role.Id}");
                        System.Console.WriteLine($"Name: {role.Name}");
                        // Display other role properties as needed.
                        System.Console.WriteLine();
                    }
                }
                else
                {
                    System.Console.WriteLine("No roles found.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void ListRoleById()
        {
            try
            {
                System.Console.Write("Enter Role ID to view details: ");
                if (int.TryParse(System.Console.ReadLine(), out int roleId))
                {
                    Role role = roleRepoObj.GetItemById(roleId);

                    if (role != null)
                    {
                        System.Console.WriteLine("Role Details:");
                        System.Console.WriteLine($"ID: {role.Id}");
                        System.Console.WriteLine($"Name: {role.Name}");
                        // Display other role properties as needed.
                    }
                    else
                    {
                        System.Console.WriteLine("Role not found.");
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid Role ID.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void DeleteRole()
        {
            try
            {
                System.Console.Write("Enter Role ID to delete: ");
                if (int.TryParse(System.Console.ReadLine(), out int roleId))
                {
                    // Check if the role exists
                    Role roleToDelete = roleRepoObj.GetItemById(roleId);

                    if (roleToDelete != null)
                    {
                        // Display role details before deletion
                        System.Console.WriteLine("Role to Delete:");
                        System.Console.WriteLine($"ID: {roleToDelete.Id}");
                        System.Console.WriteLine($"Name: {roleToDelete.Name}");
                    }

                    List<Project> projects = projectRepoObj.GetItems();

                    // Check if the role is assigned to any project
                    foreach (var project in projects)
                    {
                        var roleInProject = project.AssignedEmployees.FirstOrDefault(
                            e => e.RoleId == roleId
                        );
                        if (roleInProject != null)
                        {
                            System.Console.Write(
                                "Are you sure you want to remove the role from the project:"
                            );
                            string confirm = System.Console.ReadLine()!.ToLower();

                            if (confirm == "yes")
                            {
                                // Remove the role with details from the project
                                project.AssignedEmployees.Remove(roleInProject);
                                System.Console.WriteLine(
                                    $"Role {roleInProject.Id} {GetRoleName(roleInProject.Id)} removed from Project {project.Id}"
                                );
                            }
                            else
                            {
                                System.Console.WriteLine("Deletion canceled.");
                            }
                            System.Console.WriteLine($"Role removed from the project");
                        }
                    }

                    // After removing the role from projects, delete the role
                    // Your code for deleting the role from the system goes here

                    System.Console.Write("Are you sure you want to delete this role? (yes/no): ");
                    string confirmation = System.Console.ReadLine()!.ToLower();

                    if (confirmation == "yes")
                    {
                        // Delete the role from the project manager
                        roleRepoObj.Delete(roleId);
                        System.Console.WriteLine("Role deleted successfully.");
                    }
                    else
                    {
                        System.Console.WriteLine("Deletion canceled.");
                    }

                    System.Console.WriteLine($"Role with ID {roleId} deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void ViewRoles()
        {
            try
            {
                List<Role> roles = roleRepoObj.GetItems();

                if (roles.Count == 0)
                {
                    System.Console.WriteLine("No roles available.");
                }
                else
                {
                    System.Console.WriteLine("\nList of Roles:");
                    foreach (var role in roles)
                    {
                        System.Console.WriteLine($"Role ID: {role.Id}");
                        System.Console.WriteLine($"Role Name: {role.Name}");
                        System.Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void UpdateRoles()
        {
            try
            {
                Role objRole = new();

                System.Console.WriteLine("\nEnter the role id which you want to update:");
                if (!int.TryParse(System.Console.ReadLine(), out int roleId))
                {
                    throw new ArgumentException(
                        "Invalid input. Role ID must be a positive integer."
                    );
                }

                // Validate the role ID
                if (roleId <= 0)
                {
                    throw new ArgumentException(
                        "Role ID must be a positive integer. Role not added."
                    );
                }

                System.Console.WriteLine("\nEnter the updated role name:");
                string? roleName = System.Console.ReadLine();

                // Validate the role Name
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    throw new ArgumentException("Role name cannot be empty. Role not added.");
                }

                objRole.Id = roleId;
                objRole.Name = roleName;

                roleRepoObj.UpdateItem(objRole);
                System.Console.WriteLine("\nRole updated successfully");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void AssignEmployeeToProject()
        {
            try
            {
                System.Console.WriteLine("\nAssign an Employee to a Project:");

                // Display available projects and let the user select a project.
                System.Console.WriteLine("\nAvailable Projects:");
                List<Project> projects = projectRepoObj.GetItems();
                foreach (var project in projects)
                {
                    System.Console.WriteLine($"Project ID: {project.Id}, Name: {project.Name}");
                }

                System.Console.Write("Enter Project ID: ");
                if (!int.TryParse(System.Console.ReadLine(), out int projectId))
                {
                    throw new ArgumentException("Invalid input. Please enter a valid Project ID.");
                }

                // Check if the selected project exists.
                Project selectedProject = projectRepoObj.GetItemById(projectId);
                if (selectedProject == null)
                {
                    throw new ArgumentException("Invalid Project ID. Assignment failed.");
                }

                // Display available employees and let the user select an employee.
                System.Console.WriteLine("\nAvailable Employees:");
                List<Employee> employees = employeeRepoObj.GetItems();
                foreach (var employee in employees)
                {
                    System.Console.WriteLine(
                        $"Employee ID: {employee.Id}, Name: {employee.FirstName} {employee.LastName}"
                    );
                }

                System.Console.Write("Enter Employee ID: ");
                if (!int.TryParse(System.Console.ReadLine(), out int employeeId))
                {
                    throw new ArgumentException("Invalid input. Please enter a valid Employee ID.");
                }

                // Check if the selected employee exists.
                Employee selectedEmployee = employees.FirstOrDefault(emp => emp.Id == employeeId)!;
                if (selectedEmployee == null)
                {
                    throw new ArgumentException("Invalid Employee ID. Assignment failed.");
                }

                // Assign the employee to the project.
                List<Employee> assignedEmployees = projectRepoObj.AssignEmployeeToProject(
                    projectId,
                    employeeId,
                    employees
                );

                System.Console.WriteLine(
                    $"\nEmployee with ID {employeeId} assigned to project with ID '{projectId}'."
                );
                foreach (Employee assignedEmployee in assignedEmployees)
                {
                    System.Console.WriteLine($"Employee ID: {assignedEmployee.Id}");
                    System.Console.WriteLine(
                        $"Employee Name: {assignedEmployee.FirstName} {assignedEmployee.LastName}"
                    );
                    System.Console.WriteLine($"Employee Email: {assignedEmployee.Email}");
                    System.Console.WriteLine($"Employee Mobile: {assignedEmployee.Mobile}");
                    System.Console.WriteLine($"Employee Address: {assignedEmployee.Address}");
                    System.Console.WriteLine($"Employee RoleId : {assignedEmployee.RoleId}");
                }

                System.Console.WriteLine("\nEmployee assigned to the project successfully.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void RemoveEmployeeFromProject()
        {
            try
            {
                System.Console.WriteLine("\nRemove an Employee from a Project:");

                // Display available projects and let the user select a project.
                System.Console.WriteLine("\nAvailable Projects:");
                List<Project> projects = projectRepoObj.GetItems();
                foreach (var project in projects)
                {
                    System.Console.WriteLine($"Project ID: {project.Id}, Name: {project.Name}");
                }

                System.Console.Write("Enter Project ID: ");
                if (!int.TryParse(System.Console.ReadLine(), out int projectId))
                {
                    throw new ArgumentException("Invalid input. Please enter a valid Project ID.");
                }

                // Check if the selected project exists.
                Project selectedProject = projectRepoObj.GetItemById(projectId);
                if (selectedProject == null)
                {
                    throw new ArgumentException("Invalid Project ID. Removal failed.");
                }

                // Display assigned employees in the selected project.
                List<Employee> assignedEmployees = projectRepoObj.GetAssignedEmployees(projectId);

                if (assignedEmployees.Count == 0)
                {
                    System.Console.WriteLine("No employees assigned to this project.");
                    return;
                }

                System.Console.WriteLine("\nAssigned Employees in the Project:");
                foreach (var employee in assignedEmployees)
                {
                    System.Console.WriteLine(
                        $"Employee ID: {employee.Id}, Name: {employee.FirstName} {employee.LastName}"
                    );
                }

                System.Console.Write("\nEnter Employee ID to remove: ");
                if (!int.TryParse(System.Console.ReadLine(), out int employeeIdToRemove))
                {
                    throw new ArgumentException("Invalid input. Please enter a valid Employee ID.");
                }

                // Check if the selected employee exists in the project.
                Employee selectedEmployeeToRemove = assignedEmployees.FirstOrDefault(
                    emp => emp.Id == employeeIdToRemove
                );
                if (selectedEmployeeToRemove == null)
                {
                    throw new ArgumentException(
                        "Employee not found in this project. Removal failed."
                    );
                }

                // Remove the employee from the project.
                projectRepoObj.RemoveEmployeeFromProject(projectId, employeeIdToRemove);
                System.Console.WriteLine("\nEmployee removed from the project successfully.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void ViewProjectDetails()
        {
            try
            {
                System.Console.WriteLine("\nView Project Details:");

                // Display available projects and let the user select a project.
                System.Console.WriteLine("\nAvailable Projects:");
                List<Project> projects = projectRepoObj.GetItems();
                foreach (var project in projects)
                {
                    System.Console.WriteLine($"Project ID: {project.Id}, Name: {project.Name}");
                }

                System.Console.Write("\nEnter Project ID: ");

                if (!int.TryParse(System.Console.ReadLine(), out int projectId))
                {
                    throw new ArgumentException("Invalid input. Please enter a valid Project ID.");
                }

                // Check if the selected project exists.
                Project selectedProject = projectRepoObj.GetItemById(projectId);
                if (selectedProject == null)
                {
                    throw new ArgumentException(
                        $"Invalid Project ID '{projectId}'. Viewing project details failed."
                    );
                }

                System.Console.WriteLine("\nProject Details:");
                System.Console.WriteLine($"Project Name: {selectedProject.Name}");
                System.Console.WriteLine($"Start Date: {selectedProject.StartDate:yyyy-MM-dd}");
                System.Console.WriteLine($"End Date: {selectedProject.EndDate:yyyy-MM-dd}");

                // Display assigned employees in the selected project.
                List<Employee> assignedEmployees = projectRepoObj.GetAssignedEmployees(projectId);

                if (assignedEmployees.Count == 0)
                {
                    System.Console.WriteLine("\nNo employees assigned to this project.");
                }
                else
                {
                    System.Console.WriteLine("\nAssigned Employees in the Project:");
                    foreach (var employee in assignedEmployees)
                    {
                        System.Console.WriteLine(
                            $"Employee ID: {employee.Id}, Name: {employee.FirstName} {employee.LastName}"
                        );
                    }
                }

                // Display a list of employees in the project grouped by role.
                System.Console.WriteLine("\nEmployees in the Project by Role:");
                foreach (var role in assignedEmployees.GroupBy(emp => emp.RoleId))
                {
                    Role employeeRole = roleRepoObj.GetItemById(role.Key);
                    System.Console.WriteLine($"Role: {employeeRole?.Name ?? "Unknown Role"}");

                    foreach (var employee in role)
                    {
                        System.Console.WriteLine(
                            $"Employee ID: {employee.Id}, Name: {employee.FirstName} {employee.LastName}"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void MenuSave()
        {
            saveDataObj.Save();
            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
            System.Console.WriteLine("App state saved sucessfully.");
        }

        public void LoadAppState()
        {
            loadDataObj.Load();
            System.Console.WriteLine("Data loaded successfully.");
        }

        public void MenuQuit()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Thank you for using Module Options");
        }

        public void Quit()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Thank you for using Prolifics Project Manager (PPM)");
            System.Console.WriteLine("Goodbye!");
            System.Console.WriteLine("-----------------------------------------------------");
            System.Console.WriteLine("*****************************************************");
        }
    }
}

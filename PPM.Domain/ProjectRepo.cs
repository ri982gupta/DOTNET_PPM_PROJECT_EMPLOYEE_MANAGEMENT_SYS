using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.SqlClient;
using PPM.Model;
using PPM.DAL;
using System.Data;

namespace PPM.Domain
{
    public class ProjectRepo : ICommonModule<Project>
    {
        public static List<Project> projects = new List<Project>();

        public static List<Employee> employees = new List<Employee>();

        public void Add(Project project)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("InsertProject", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProjectId", project.Id);
                    command.Parameters.AddWithValue("@Name", project.Name);
                    command.Parameters.AddWithValue("@StartDate", project.StartDate);
                    command.Parameters.AddWithValue("@EndDate", project.EndDate);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Project> GetItems()
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            List<Project> projects = new List<Project>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SelectAllProjects", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project project = new Project
                            {
                                Id = Convert.ToInt32(reader["ProjectId"]),
                                Name = reader["Name"].ToString()!,
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = Convert.ToDateTime(reader["EndDate"]),
                            };

                            projects.Add(project);
                        }
                    }
                }
            }

            return projects;
        }

        public Project GetItemById(int id)
        {
            Project? project = null;
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SelectProjectById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            project = new Project
                            {
                                Id = Convert.ToInt32(reader["ProjectId"]),
                                Name = reader["Name"].ToString()!,
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = Convert.ToDateTime(reader["EndDate"]),
                            };
                        }
                    }
                }
            }

            return project!;
        }

        public void Delete(int id)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DeleteProject", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateItem(Project updatedItem)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UpdateProjects", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProjectId", updatedItem.Id);
                    command.Parameters.AddWithValue("@Name", updatedItem.Name);
                    command.Parameters.AddWithValue("@StartDate", updatedItem.StartDate);
                    command.Parameters.AddWithValue("@EndDate", updatedItem.EndDate);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void ViewItems()
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SelectAllProjects", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("\nList of Projects:");
                            while (reader.Read())
                            {
                                Console.WriteLine(
                                    $"Project ID: {reader["ProjectId"]}, Name: {reader["Name"]}, Start Date: {reader["StartDate"]}, End Date: {reader["EndDate"]}"
                                );
                            }
                        }
                        else
                        {
                            Console.WriteLine("No projects available.");
                        }
                    }
                }
            }
        }

        public List<Employee> AssignEmployeeToProject(
            int projectId,
            int employeeId,
            List<Employee> employees
        )
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                Employee employee = employees.Find(emp => emp.Id == employeeId)!;

                // Check if the project and employee exist
                using (
                    SqlCommand checkCommand = new SqlCommand(
                        "SELECT COUNT(*) FROM Projects WHERE ProjectId = @ProjectId",
                        connection
                    )
                )
                {
                    checkCommand.Parameters.AddWithValue("@ProjectId", projectId);
                    int projectCount = (int)checkCommand.ExecuteScalar();

                    using (
                        SqlCommand checkEmployeeCommand = new SqlCommand(
                            "SELECT COUNT(*) FROM Employees WHERE EmployeeId = @EmployeeId",
                            connection
                        )
                    )
                    {
                        checkEmployeeCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                        int employeeCount = (int)checkEmployeeCommand.ExecuteScalar();

                        if (projectCount > 0 && employeeCount > 0 && employee != null)
                        {
                            // Perform the assignment using a stored procedure
                            using (
                                SqlCommand command = new SqlCommand(
                                    "AssignEmployeeToProject",
                                    connection
                                )
                            )
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@ProjectId", projectId);
                                command.Parameters.AddWithValue("@EmployeeId", employeeId);
                                command.ExecuteNonQuery();
                            }

                            // Retrieve the updated list of assigned employees
                            return GetAssignedEmployees(projectId);
                        }
                        else
                        {
                            Console.WriteLine("Project or employee not found. Assignment failed.");
                        }
                    }
                }
            }

            return new List<Employee>();
        }

        public List<Employee> GetAssignedEmployees(int projectId)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            List<Employee> assignedEmployees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (
                    SqlCommand command = new SqlCommand(
                        "SELECT E.* FROM Employees E JOIN ProjectEmployees PE ON E.EmployeeId = PE.EmployeeId WHERE PE.ProjectId = @ProjectId",
                        connection
                    )
                )
                {
                    command.Parameters.AddWithValue("@ProjectId", projectId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee assignedEmployee = new Employee
                            {
                                Id = Convert.ToInt32(reader["EmployeeId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Mobile = reader["Mobile"].ToString(),
                                Address = reader["Address"].ToString(),
                                RoleId = Convert.ToInt32(reader["RoleId"])
                            };

                            assignedEmployees.Add(assignedEmployee);
                        }
                    }
                }
            }

            return assignedEmployees;
        }

        public void RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (
                    SqlCommand checkCommand = new SqlCommand(
                        "SELECT COUNT(*) FROM Projects WHERE ProjectId = @ProjectId",
                        connection
                    )
                )
                {
                    checkCommand.Parameters.AddWithValue("@ProjectId", projectId);
                    int projectCount = (int)checkCommand.ExecuteScalar();

                    if (projectCount > 0)
                    {
                        // Check if the employee is assigned to the project
                        using (
                            SqlCommand checkEmployeeCommand = new SqlCommand(
                                "SELECT COUNT(*) FROM ProjectEmployees WHERE ProjectId = @ProjectId AND EmployeeId = @EmployeeId",
                                connection
                            )
                        )
                        {
                            checkEmployeeCommand.Parameters.AddWithValue("@ProjectId", projectId);
                            checkEmployeeCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                            int assignmentCount = (int)checkEmployeeCommand.ExecuteScalar();

                            if (assignmentCount > 0)
                            {
                                // Remove the employee from the project using a stored procedure
                                using (
                                    SqlCommand command = new SqlCommand(
                                        "RemoveEmployeeFromProject",
                                        connection
                                    )
                                )
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@ProjectId", projectId);
                                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                                    command.ExecuteNonQuery();
                                }

                                Console.WriteLine("Employee removed from the project.");
                            }
                            else
                            {
                                Console.WriteLine(
                                    "Employee not found in the project. Removal failed."
                                );
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Project not found. Removal failed.");
                    }
                }
            }
        }

        public void ViewProjectDetails(int projectId)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Declare variables to store project details
                string? projectName = null;
                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;

                // Select project Details
                using (SqlCommand command = new SqlCommand("SelectProjectById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", projectId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            projectName = reader["Name"].ToString();
                            startDate = Convert.ToDateTime(reader["StartDate"]);
                            endDate = Convert.ToDateTime(reader["EndDate"]);
                        }
                    }
                }

                // Check if the project exists
                if (!string.IsNullOrEmpty(projectName))
                {
                    // Display project details
                    Console.WriteLine("Project Details:");
                    Console.WriteLine($"Project Name: {projectName}");
                    Console.WriteLine($"Start Date: {startDate}");
                    Console.WriteLine($"End Date: {endDate}");

                    // Display assigned employees
                    Console.WriteLine("Assigned Employees in the Project:");

                    using (
                        SqlCommand employeeCommand = new SqlCommand(
                            "SelectEmployeesByProjectId",
                            connection
                        )
                    )
                    {
                        employeeCommand.CommandType = CommandType.StoredProcedure;
                        employeeCommand.Parameters.AddWithValue("@ProjectId", projectId);

                        using (SqlDataReader employeeReader = employeeCommand.ExecuteReader())
                        {
                            while (employeeReader.Read())
                            {
                                Console.WriteLine(
                                    $"Employee ID: {employeeReader["EmployeeId"]}, "
                                        + $"Name: {employeeReader["FirstName"]} {employeeReader["LastName"]}"
                                );
                            }
                        }
                    }
                }
                else
                {
                    // Project not found
                    Console.WriteLine("Project not found.");
                }
            }
        }

        public Employee GetEmployeeById(int id)
        {
            return employees.Find(employee => employee.Id == id)!;
        }
    }
}

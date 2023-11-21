using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PPM.Model;

namespace PPM.DAL
{
    public class ProjectDal
    {
        public void Add(Project project)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery =
                    "INSERT INTO Projects (ProjectId, Name, StartDate, EndDate)"
                    + " VALUES (@ProjectId, @Name, @StartDate, @EndDate);";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", project.Id);
                    command.Parameters.AddWithValue("@Name", project.Name);
                    command.Parameters.AddWithValue("@StartDate", project.StartDate);
                    command.Parameters.AddWithValue("@EndDate", project.EndDate);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Projects;";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projects.Add(
                                new Project
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Name = reader["Name"].ToString()!,
                                    StartDate = Convert.ToDateTime(reader["StartDate"]),
                                    EndDate = Convert.ToDateTime(reader["EndDate"])
                                }
                            );
                        }
                    }
                }
            }

            return projects;
        }

        public Project? GetProjectById(int id)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Projects WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Project
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString()!,
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = Convert.ToDateTime(reader["EndDate"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void DeleteProject(int id)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Projects WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateProject(Project updatedProject)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery =
                    "UPDATE Projects SET Name = @Name, StartDate = @StartDate, EndDate = @EndDate"
                    + " WHERE ProjectId = @ProjectId;";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", updatedProject.Id);
                    command.Parameters.AddWithValue("@Name", updatedProject.Name);
                    command.Parameters.AddWithValue("@StartDate", updatedProject.StartDate);
                    command.Parameters.AddWithValue("@EndDate", updatedProject.EndDate);

                    command.ExecuteNonQuery();
                }
            }
        }



        public void AssignEmployeesToProject(int projectId, int employeeId, List<int> employeeIds)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create DataTable for the list of employees
                DataTable employeeTable = new DataTable();
                employeeTable.Columns.Add("EmployeeId", typeof(int));

                foreach (int empId in employeeIds)
                {
                    employeeTable.Rows.Add(empId);
                }

                using (SqlCommand command = new SqlCommand("AssignEmployeeToProject", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.AddWithValue("@ProjectId", projectId);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    SqlParameter parameter = command.Parameters.AddWithValue(
                        "@Employees",
                        employeeTable
                    );
                    parameter.SqlDbType = SqlDbType.Structured;
                    parameter.TypeName = "dbo.EmployeesType";

                    command.ExecuteNonQuery();
                }
            }
        }

        
        public void RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the record exists
                string checkExistenceQuery =
                    "SELECT COUNT(*) FROM ProjectEmployees WHERE ProjectId = @ProjectId AND EmployeeId = @EmployeeId;";

                using (
                    SqlCommand checkExistenceCommand = new SqlCommand(
                        checkExistenceQuery,
                        connection
                    )
                )
                {
                    checkExistenceCommand.Parameters.AddWithValue("@ProjectId", projectId);
                    checkExistenceCommand.Parameters.AddWithValue("@EmployeeId", employeeId);

                    int recordCount = (int)checkExistenceCommand.ExecuteScalar();

                    if (recordCount > 0)
                    {
                        // Record exists, proceed with deletion
                        string deleteQuery =
                            "DELETE FROM ProjectEmployees WHERE ProjectId = @ProjectId AND EmployeeId = @EmployeeId;";

                        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ProjectId", projectId);
                            command.Parameters.AddWithValue("@EmployeeId", employeeId);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                Console.WriteLine(
                                    "Employee removed from the project successfully."
                                );
                            }
                            else
                            {
                                Console.WriteLine(
                                    "No matching record found. No deletion performed."
                                );
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No matching record found. No deletion performed.");
                    }
                }
            }
        }

        public Project ViewProjectDetails(int projectId)
        {
            Project project = new Project();
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Projects WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", projectId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            project.Id = Convert.ToInt32(reader["Id"]);
                            project.Name = reader["Name"].ToString()!;
                            project.StartDate = Convert.ToDateTime(reader["StartDate"]);
                            project.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        }
                    }
                }

                // Fetch assigned employees
                project.AssignedEmployees = GetAssignedEmployees(projectId);
            }

            return project;
        }

        private List<Employee> GetAssignedEmployees(int projectId)
        {
            List<Employee> assignedEmployees = new List<Employee>();
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery =
                    "SELECT E.* FROM Employees E "
                    + "JOIN ProjectEmployees PE ON E.Id = PE.EmployeeId "
                    + "WHERE PE.ProjectId = @ProjectId;";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", projectId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            assignedEmployees.Add(
                                new Employee
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Mobile = reader["Mobile"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    RoleId = Convert.ToInt32(reader["RoleId"])
                                }
                            );
                        }
                    }
                }
            }

            return assignedEmployees;
        }

        public List<Project> ViewProjects()
        {
            List<Project> projects = new List<Project>();
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Projects;";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project project = new Project
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString()!,
                                StartDate = Convert.ToDateTime(reader["StartDate"]),
                                EndDate = Convert.ToDateTime(reader["EndDate"])
                            };

                            // Fetch assigned employees
                            project.AssignedEmployees = GetAssignedEmployees(project.Id);

                            projects.Add(project);
                        }
                    }
                }
            }

            return projects;
        }
    }
}

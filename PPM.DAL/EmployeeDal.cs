using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PPM.Model;

namespace PPM.DAL
{
    public class EmployeeDal
    {
        public void Add(Employee employee)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery =
                    "INSERT INTO Employees (EmployeeId, FirstName, LastName, Email, Mobile, Address, RoleId) "
                    + "VALUES (@EmployeeId, @FirstName, @LastName, @Email, @Mobile, @Address, @RoleId);";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employee.Id);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@Email", employee.Email);
                    command.Parameters.AddWithValue("@Mobile", employee.Mobile);
                    command.Parameters.AddWithValue("@Address", employee.Address);
                    command.Parameters.AddWithValue("@RoleId", employee.RoleId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Employees;";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(
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

            return employees;
        }

        public Employee? GetEmployeeById(int id)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Employees WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Employee
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Mobile = reader["Mobile"].ToString(),
                                Address = reader["Address"].ToString(),
                                RoleId = Convert.ToInt32(reader["RoleId"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void UpdateEmployee(Employee updatedEmployee)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery =
                    "UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, "
                    + "Email = @Email, Mobile = @Mobile, Address = @Address, RoleId = @RoleId "
                    + "WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", updatedEmployee.Id);
                    command.Parameters.AddWithValue("@FirstName", updatedEmployee.FirstName);
                    command.Parameters.AddWithValue("@LastName", updatedEmployee.LastName);
                    command.Parameters.AddWithValue("@Email", updatedEmployee.Email);
                    command.Parameters.AddWithValue("@Mobile", updatedEmployee.Mobile);
                    command.Parameters.AddWithValue("@Address", updatedEmployee.Address);
                    command.Parameters.AddWithValue("@RoleId", updatedEmployee.RoleId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteEmployee(int id)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Employees WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Employee> ViewEmployees()
        {
            List<Employee> employees = new List<Employee>();
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Employees;";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Mobile = reader["Mobile"].ToString(),
                                Address = reader["Address"].ToString(),
                                RoleId = Convert.ToInt32(reader["RoleId"])
                            };

                            employees.Add(employee);
                        }
                    }
                }
            }

            return employees;
        }
    }
}

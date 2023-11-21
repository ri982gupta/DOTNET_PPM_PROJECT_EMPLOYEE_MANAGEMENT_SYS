using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.SqlClient;
using PPM.Model;
using System.Data;

namespace PPM.Domain
{
    public class EmployeeRepo : ICommonModule<Employee>
    {
        public static List<Employee> employees = new List<Employee>();

        public void Add(Employee employee)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("InsertEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
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

        public List<Employee> GetItems()
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SelectAllEmployees", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee
                            {
                                Id = Convert.ToInt32(reader["EmployeeId"]),
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

        public Employee GetItemById(int id)
        {
            Employee? employee = null;
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SelectEmployeeById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                Id = Convert.ToInt32(reader["EmployeeId"]),
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

            return employee!;
        }

        public void Delete(int id)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DeleteEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateItem(Employee updatedItem)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UpdateEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", updatedItem.Id);
                    command.Parameters.AddWithValue("@FirstName", updatedItem.FirstName);
                    command.Parameters.AddWithValue("@LastName", updatedItem.LastName);
                    command.Parameters.AddWithValue("@Email", updatedItem.Email);
                    command.Parameters.AddWithValue("@Mobile", updatedItem.Mobile);
                    command.Parameters.AddWithValue("@Address", updatedItem.Address);
                    command.Parameters.AddWithValue("@RoleId", updatedItem.RoleId);

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

                using (SqlCommand command = new SqlCommand("SelectAllEmployees", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No employees available.");
                        }
                        else
                        {
                            Console.WriteLine("List of Employees:");
                            while (reader.Read())
                            {
                                Console.WriteLine(
                                    $"Employee ID: {reader["EmployeeId"]}, Name: {reader["FirstName"]} {reader["LastName"]}"
                                );
                                Console.WriteLine(
                                    $"Email: {reader["Email"]}, Mobile: {reader["Mobile"]}"
                                );
                                Console.WriteLine(
                                    $"Address: {reader["Address"]}, Role: {reader["RoleId"]}"
                                );
                            }
                        }
                    }
                }
            }
        }
    }
}

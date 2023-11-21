using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using PPM.Model;

namespace PPM.Domain
{
    public class RoleRepo : ICommonModule<Role>
    {
        public static List<Role> roles = new List<Role>();

        public void Add(Role role)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("InsertRoles", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoleId", role.Id);
                    command.Parameters.AddWithValue("@Name", role.Name);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Role> GetItems()
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";

            List<Role> roles = new List<Role>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SelectAllRoles", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Role role = new Role
                            {
                                Id = Convert.ToInt32(reader["RoleId"]),
                                Name = reader["Name"].ToString()
                            };

                            roles.Add(role);
                        }
                    }
                }
            }

            return roles;
        }

        public Role GetItemById(int id)
        {
            Role? role = null;
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SelectRoleById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            role = new Role
                            {
                                Id = Convert.ToInt32(reader["RoleId"]),
                                Name = reader["Name"].ToString()
                            };
                        }
                    }
                }
            }

            return role!;
        }

        public void UpdateItem(Role role)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UpdateRoles", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoleId", role.Id);
                    command.Parameters.AddWithValue("@Name", role.Name);

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

                using (SqlCommand command = new SqlCommand("SelectAllRoles", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No roles available.");
                        }
                        else
                        {
                            Console.WriteLine("List of Roles:");
                            while (reader.Read())
                            {
                                Console.WriteLine(
                                    $"Role ID: {reader["RoleId"]}, Name: {reader["Name"]}"
                                );
                            }
                        }
                    }
                }
            }
        }

        public void Delete(int id)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DeleteRole", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

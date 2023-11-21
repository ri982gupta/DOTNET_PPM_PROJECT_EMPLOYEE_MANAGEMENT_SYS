using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PPM.Model;

namespace PPM.DAL
{
    public class RoleDal
    {
        public void Add(Role role)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO Roles (Name) VALUES (@Name);";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", role.Name);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Roles;";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(
                                new Role
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Name = reader["Name"].ToString()
                                }
                            );
                        }
                    }
                }
            }

            return roles;
        }

        public Role? GetRoleById(int id)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Roles WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Role
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void UpdateRole(Role updatedRole)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE Roles SET Name = @Name WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", updatedRole.Id);
                    command.Parameters.AddWithValue("@Name", updatedRole.Name);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRole(int id)
        {
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Roles WHERE Id = @Id;";

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Role> ViewRoles()
        {
            List<Role> roles = new List<Role>();
            string connectionString =
                "Server=RHJ-9F-D209\\SQLEXPRESS;Database=PPM_DB;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Roles;";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Role role = new Role
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString()
                            };

                            roles.Add(role);
                        }
                    }
                }
            }

            return roles;
        }
    }
}

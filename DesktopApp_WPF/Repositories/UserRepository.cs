using DesktopApp_WPF.CustomException;
using DesktopApp_WPF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace DesktopApp_WPF.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel userModel, NetworkCredential password)
        {
            var userExist = CheckUserExist(userModel.Email, password.Password);
            if (userExist)
            {
                throw new DbDuplicationException();
            }

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO [APP_USER] (login,firstname,lastname,password,mail, ID_SERVICE) VALUES (@login,@firstname,@lastname,@password,@email, @Id_Service)";
                command.Parameters.Add("@login", SqlDbType.NVarChar).Value = userModel.Login;
                command.Parameters.Add("@firstname", SqlDbType.NVarChar).Value = userModel.FirstName;
                command.Parameters.Add("@lastname", SqlDbType.NVarChar).Value = userModel.LastName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password.Password;
                command.Parameters.Add("@email", SqlDbType.NVarChar).Value = userModel.Email;
                command.Parameters.Add("@Id_Service", SqlDbType.NVarChar).Value = userModel.Service;

                command.ExecuteNonQuery();

            }

        }

        public bool CheckUserExist(string email, string password)
        {
            bool userExist;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [APP_User] where login=@login and [password]=@password";

                command.Parameters.Add("@login", SqlDbType.NVarChar).Value = email;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
                userExist = command.ExecuteScalar() == null ? false : true;

            }
            return userExist;

        }



        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [APP_User] where LOGIN=@username and [password]=@password";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;

            }
            return validUser;
        }


        public void UpdateRestaurant(RestaurantModel restaurant)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;


                command.CommandText = "UPDATE [APP_USER] FIRSTNAME =@firstname, LOGIN =@login, PASSWORD=@password, LASTNAME=@lastname, MAIL=@email, ID_SERVICE=@service WHERE id=@id";
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = restaurant.Id;
                command.Parameters.Add("@firstname", SqlDbType.NVarChar).Value = restaurant.Password;

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<UserModel> GetByAll()
        {
            List<UserModel> users = new List<UserModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [APP_USER]";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new UserModel()
                        {
                            Id = (int)reader[0],
                            Login = reader[1].ToString(),
                            Password = reader[2].ToString(),
                            FirstName = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Email = reader[5].ToString(),
                            Service = reader[6].ToString(),
                        };
                        users.Add(user);
                    }
                }
            }
            return users;

        }

        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }
        public UserModel GetByUsername(string username)
        {
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select *from [APP_USER] where LOGIN=@username";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = (int)reader[0],
                            Login = reader[1].ToString(),
                            Password = reader[2].ToString(),
                            FirstName = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Email = reader[5].ToString(),
                            Service = reader[6].ToString(),
                        };
                    }
                }
            }
            return user;
        }


        public void DeleteUser(string username)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [APP_USER] WHERE LOGIN=@username";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                command.ExecuteNonQuery();
            }
        }



        public void Edit(UserModel userModel, NetworkCredential credential)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [APP_USER] SET MAIL =@email, LOGIN =@login, PASSWORD=@password WHERE id=@id";
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = userModel.Id;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
                command.Parameters.Add("@email", SqlDbType.NVarChar).Value = userModel.Email;
                command.Parameters.Add("@login", SqlDbType.NVarChar).Value = userModel.Login;
                command.ExecuteNonQuery();
            }
        }

        public void Remove(int id)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "DELETE FROM [APP_USER] WHERE id=@id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                command.ExecuteNonQuery();
            }
        }

    }
}


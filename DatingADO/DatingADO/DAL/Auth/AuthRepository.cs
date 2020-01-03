using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DatingADO.Model;
using Microsoft.Extensions.Configuration;

namespace DatingADO.DAL.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private string url;

        public AuthRepository(IConfiguration config)
        {
            url = config["ConnectionStrings:DatingSQL"];
        }
        public async Task<User> Login(string username, string password)
        {
            //get user from database
            User user = await getUser(username);

            if (user == null)
                return null;

            if (!vertifyPasswordHash(user.PasswordSalt,user.PasswordHash, password))
                return null;

            return user;
        }

        private bool vertifyPasswordHash(byte[] passwordSalt,byte[] passwordHash, string password)
        {
            //hash the password
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                //compare the passwordSalt
                for(int i = 0; i < computed.Length; i++)
                {
                    if (computed[i] != passwordHash[i])
                        return false;
                }
            }

            return true;
        }

        private async Task<User> getUser(string username)
        {
            User user = null;
            //query
            string query = "select * from [User] where username = @p1;";

            //connect to database
            using(SqlConnection cnn = new SqlConnection(url))
            {
                //pass query to database
                using(SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    //open connection
                    await cnn.OpenAsync();

                    //assign value to parameters
                    cmd.Parameters.AddWithValue("@p1", username);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user = new User()
                        {
                            Id = (int)reader["Id"],
                            Username = reader["Username"].ToString(),
                            PasswordHash = (byte[])reader["PasswordHash"],
                            PasswordSalt = (byte[])reader["PasswordSalt"]
                        };
                    }

                }
            }

            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            //passwordHash is encryted password
            //passwordSalt is the key for encryption and dcryption
            byte[] passwordHash, passwordSalt;
            //assign value to passwordHash and passwordSalt
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            //save to database
            //query
            string query = "insert into [User] " +
                "(Username , PasswordHash, PasswordSalt) " +
                "output inserted.id " +
                "values (@p1 , @p2 ,@p3);";

            //connect to database
            using(SqlConnection cnn = new SqlConnection(url))
            {
                //send qeury to database
                using(SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    //open connection
                    await cnn.OpenAsync();

                    //assign value to parameter
                    cmd.Parameters.AddWithValue("@p1", user.Username);
                    cmd.Parameters.AddWithValue("@p2", passwordHash);
                    cmd.Parameters.AddWithValue("@p3", passwordSalt);

                    //execute query
                    user.Id = (int)cmd.ExecuteScalar();
                }
            }

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            return await getUser(username) != null;
        }
    }
}

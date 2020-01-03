using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.DAL.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DatingDbContext _context;
        public AuthRepository(DatingDbContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            //check if user exist
            User user = await _context.User.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            //check if password is match
            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;

        }

        //hash the password and compare with the one in database
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac =new  System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                //compare each character to check if they are same
                for(int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }

            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            //passwordHash is encrypted password
            //passwordSlat is the key for encrypting and decrypting the password
            byte[] passwordHash, passwordSalt;
            //hash the password
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            //assign to user object
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            //save to database
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();


            return user;

        }

        //asign value to passwordHash and passwordSalt
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
            return await _context.User.AnyAsync(x => x.Username == username);
        }
    }
}

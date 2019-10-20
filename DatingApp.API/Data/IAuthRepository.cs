using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using System.Security.Cryptography;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);

        Task<User> Login(string username, string password);

        Task<bool> UserExists(string username);
    }

    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;
        
        public AuthRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<User> Register(User user, string password)
        {
            // validation here???
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();

             return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACMD5())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if(user == null) return null;

            if(!VerifyPassword(user, password)) return null;
            
            return user;
        }

        private bool VerifyPassword(User user, string password)
        {
            using(var hmac = new HMACMD5(user.PasswordSalt))
            {
                var computePassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i =0; i < computePassword.Length; i++)
                {
                    if(user.PasswordHash[i] != computePassword[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<bool> UserExists(string username)
        {
            var userExists = await _dataContext.Users.AnyAsync(x => x.Username == username);
            return userExists;
        }
    }
}
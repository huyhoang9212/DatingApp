using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext dataContext)
        {
            _context = dataContext;
        }

        public void SeedUsers()
        {
            var u = _context.Users.FirstOrDefault();
            if(u != null)
            {
                Console.WriteLine(u.Username);
            }
            if (!_context.Users.Any())
            {
                Console.WriteLine("Seeding data");
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();

                    _context.Users.Add(user);
                }

                _context.SaveChanges();
            }

        }

        // private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        // {
        //     using (var hmac = new System.Security.Cryptography.HMACSHA512())
        //     {
        //         passwordSalt = hmac.Key;
        //         passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //     }
        // }

         private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACMD5())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
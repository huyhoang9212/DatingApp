using System.Threading.Tasks;
using System.Collections.Generic;
using DatingApp.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int userId);
    }


    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _dataContext;
        public DatingRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add<T>(T entity) where T : class
        {
            _dataContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _dataContext.Remove(entity);
        }

        public async Task<User> GetUser(int userId)
        {
            var user = await _dataContext.Users
                        .Include(x => x.Photos)
                        .FirstOrDefaultAsync(x => x.Id == userId);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _dataContext.Users
                                .Include(x=>x.Photos)
                                .ToListAsync();

              return users;                  
        }

        public async Task<bool> SaveAll()
        {
           return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
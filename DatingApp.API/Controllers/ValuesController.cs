using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DatingApp.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public ValuesController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            var result = await _dbContext.Values.ToListAsync();
            //var test = new List<Value>();
            //test.Add(new Value(){Id = 3,Name = "abc"});
            return Ok(result);
        }

        // api/Values/2
        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetById(int id)
        {
            var value = await _dbContext.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }
    }
}
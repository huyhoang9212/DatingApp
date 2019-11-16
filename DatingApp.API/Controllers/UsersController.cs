using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DatingApp.API.Data;
using System.Threading.Tasks;
using System.Linq;
using DatingApp.API.Dtos;
using AutoMapper;
using System.Collections.Generic;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IDatingRepository _datingRepository;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository datingRepository, IMapper mapper)
        {
                _datingRepository = datingRepository;
                _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _datingRepository.GetUsers();

            var usersDto = _mapper.Map<IEnumerable<UserForListDto>>(users);
            // var usersDto = users.Select(x=> new UserForListDto{
            //     Id = x.Id,
            //     Age = x.Id,
            //     City=x.City,
            //     Country=x.Country,
            //     Created=x.Created,
            //     Gender=x.Gender,
            //     KnownAs=x.KnownAs,
            //     PhotoUrl=x.Photos.FirstOrDefault(y=>y.IsMain)?.Url,
            //     LastActive=x.LastActive,
            //     Username=x.Username
            // });
            return Ok(usersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var x = await _datingRepository.GetUser(id);
            var userDto = _mapper.Map<UserForDetailedDto>(x);

            // var userDetailDto = new UserForDetailedDto{
            //     Id = x.Id,
            //     Age = x.Id,
            //     City=x.City,
            //     Country=x.Country,
            //     Created=x.Created,
            //     Gender=x.Gender,
            //     KnownAs=x.KnownAs,
            //     PhotoUrl=x.Photos.FirstOrDefault(y=>y.IsMain)?.Url,
            //     LastActive=x.LastActive,
            //     Username=x.Username
            // };

            return Ok(userDto);
        }
    }
}
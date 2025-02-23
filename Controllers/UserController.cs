using KanbanApp.Requests.UserRequests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("/register")]
        public IResult RegisterUser([FromBody] UserRegisterRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _userService.RegisterUser(request);
            return Results.Ok(result);
        }

        [HttpPost]
        [Route("/login")]
        public IResult LoginUser([FromBody] UserLoginRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _userService.LoginUser(request);
            return Results.Ok(result);
        }
    }
}

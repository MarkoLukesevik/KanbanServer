using KanbanApp.Requests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("/register")]
        public IResult RegisterUser([FromBody] UserRegisterRequest registerRequest)
        {
            if (registerRequest == null)
            {
                throw new ArgumentException("request body cannot be empty");
            }

            var result = _userService.RegisterUser(registerRequest);
            return Results.Ok(result);
        }

        [HttpPost]
        [Route("/login")]
        public IResult LoginUser([FromBody] UserLoginRequest loginRequest)
        {
            var result = _userService.LoginUser(loginRequest);
            return Results.Ok(result);
        }
    }
}

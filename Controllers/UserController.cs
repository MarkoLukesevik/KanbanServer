using KanbanApp.Requests.UserRequests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(UserService userService) : ControllerBase
    {
        [HttpPost]
        [Route("/register")]
        public async Task<IResult> RegisterUser([FromBody] UserRegisterRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = await userService.RegisterUser(request);
            return Results.Ok(result);
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IResult> LoginUser([FromBody] UserLoginRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = await userService.LoginUser(request);
            return Results.Ok(result);
        }
    }
}

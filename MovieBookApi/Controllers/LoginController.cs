using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBookApi.Models.ResultClasses;
using MovieBookApi.Services;
using System.Data;

namespace MovieBookApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]


    public class LoginController : ControllerBase
    {
        private readonly IJWTService service;
        private readonly ILogger<LoginController> _logger;
        public LoginController(IJWTService service, ILogger<LoginController> _logger)
        {
            this.service = service;
            this._logger = _logger;
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(iUser user)
        {
            if (service.AleadyUser(user.Email))
            {
                return Conflict(new DataTransferObject() { IsSuccess=false, Message="EMAIL_EXISTS"});
            }
            string newUserID = service.RegisterUser(user);

            if (string.IsNullOrEmpty(newUserID))
            {
                return BadRequest(new DataTransferObject() { IsSuccess = false, Message = "REG_FAILED" });

            }
            string tkn= service.GenerateToken("User",newUserID); ;
            return Ok(new LoginResult() { Token=tkn,Role="User",Name=user.FullName,IsSuccess=true,Message= "LOGIN_SUCCESS" });
            
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token if authentication is successful.
        /// </summary>
        /// <param name="user">The user credentials containing the username and password.</param>
        /// <returns>An <see cref="IActionResult"/> containing the authentication result and token if successful.</returns>
        [HttpPost]
        [Route("login")]
        public IActionResult Authenticate(UserCredentials user)
        {
            string role = service.AuthenticateUser(user.Useremail, user.Password)!;
            var resobj = new DataTransferObject();
            if (role == "Block")
            {
                resobj.Message = "BLOCKED_USER";
                resobj.IsSuccess = false;
                return StatusCode(StatusCodes.Status403Forbidden, resobj);
            }
            if (role == null)
            {
                resobj.Message = "INVALID_CREDENTIALS";
                resobj.IsSuccess = false;
                return Unauthorized(resobj);
            }
            
                string uid = service.GetUserId(user.Useremail, user.Password);
                string? securityToken = service.GenerateToken(role,uid);
                string FullName=service.GetUserName(user.Useremail,user.Password);

            
            return Ok(new LoginResult() { Token = securityToken, Role = role, Name = FullName, IsSuccess = true, Message = "LOGIN_SUCCESS" });
        }
        [HttpPost]
        [Route("forgotpassword")]
        public IActionResult ForgotPassword(UserCredentials user)
        {
            string role = service.AuthenticateForgotPassword(user.Useremail, user.SercurityQuestion,user.Password)!;
            var resobj = new DataTransferObject();
            if (role == "Block")
            {
                resobj.Message = "BLOCKED_USER";
                resobj.IsSuccess = false;
                return StatusCode(StatusCodes.Status403Forbidden, resobj);

            }
            if (role == null)
            {
                resobj.Message = "INVALID_CREDENTIALS";
                resobj.IsSuccess = false;
                return Unauthorized(resobj);
            }
           
                string uid = service.GetUserId(user.Useremail,user.SercurityQuestion ,user.Password);
                string? securityToken = service.GenerateToken(role, uid);
            string FullName = service.GetUserName(user.Useremail, user.SercurityQuestion, user.Password);


            return Ok(new LoginResult() { Token = securityToken, Role = role, Name = FullName, IsSuccess = true, Message = "LOGIN_SUCCESS" });
        }
        ///// <summary>
        ///// Verifies if a user is logged in by validating their JWT token.
        ///// </summary>
        ///// <param name="user">The user credentials containing the username, password, and token.</param>
        ///// <returns>An <see cref="IActionResult"/> indicating whether the token is valid and the user is logged in.</returns>
        //[HttpPost]
        //[Route("Islogin")]
        //public IActionResult VerifyLogin(UserCredentials user)
        //{
           
        //    string role = service.AuthenticateUser(user.Useremail, user.Password)!;
        //    //_logger.LogInformation(role);
        //    var resobj = new DataTransferObject();
        //    if (role == null)
        //    {
        //        resobj.Message = "INVALID_CREDENTIALS";
        //        resobj.IsSuccess = false;
        //        return Unauthorized(resobj);
        //    }
        //    if(role == "Block")
        //    {
        //        resobj.Message = "BLOCKED_USER";
        //        resobj.IsSuccess = false;
        //        return StatusCode(StatusCodes.Status403Forbidden, resobj);
        //    }
        //    else
        //    {
        //        if (service.VerifyToken(user.Token!))
        //        {
        //            resobj.Message = "LOGIN_SUCCESS";
        //            resobj.IsSuccess = true;

        //        }
        //        else
        //        {
        //            resobj.Message = "LOGIN_EXPIRED";
        //            resobj.IsSuccess = false;
        //        }

        //        // _logger.LogInformation(user.Token);
        //    }
        //    return Ok(resobj);
        //}
    }
}

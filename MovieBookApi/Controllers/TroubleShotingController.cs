using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBookApi.Models.ResultClasses;

namespace WebApi2.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class TroubleShootingController : ControllerBase
    {

        /// <summary>
        /// Retrieves the value of the "err" cookie.
        /// </summary>
        /// <returns>The value of the "err" cookie, or "NOTHING" if the cookie does not exist.</returns>
        [HttpGet]
        [Route("error")]
        public IActionResult ErroAction()
        {
            string? Message = Request.Cookies["err"];
            return StatusCode(StatusCodes.Status500InternalServerError,new DataTransferObject() { IsSuccess=false,Message=Message??"Nothing"} );
        }
    }
}

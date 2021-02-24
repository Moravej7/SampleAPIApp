using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using WebFramework.Filters;

namespace WebFramework
{
    [ApiController]
    [ApiResultFilter]
    [EnableCors]
    [Route("api/[controller]")]
    public class BaseAPIController : ControllerBase
    {
        public bool UserIsAutheticated => HttpContext.User.Identity.IsAuthenticated;
    }
}

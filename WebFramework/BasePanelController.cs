using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Filters;

namespace WebFramework
{
    [Authorize]
    [ControllerResultFilter]
    public class BasePanelController : Controller
    {
    }
}

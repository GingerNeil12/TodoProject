using Microsoft.AspNetCore.Mvc;

namespace TodoProject.Controllers
{
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("TODO HOME");
        }
    }
}

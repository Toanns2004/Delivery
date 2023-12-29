using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("/status")]
    [Authorize]
    public class StatusController : Controller
    {
        
    }
}


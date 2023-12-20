using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("/bill")]
    [Authorize]
    public class BillController : Controller
    {
    
    }
}


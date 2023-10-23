using Microsoft.AspNetCore.Mvc;
using MultifactorAPI.DTOModels;
using MultifactorAPI.Services;

namespace MultifactorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccessController : ControllerBase
    {
        private readonly RequestManager _requestManager;

        public AccessController(RequestManager requestManager)
        {
            _requestManager = requestManager;
        }

        [HttpPost]
        public IActionResult Create(AccessModel request)
        {
            _requestManager.SetCredential(request);
            return Ok();
        }
    }
}

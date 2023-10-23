using Microsoft.AspNetCore.Mvc;
using MultifactorAPI.DTOModels;
using MultifactorAPI.Services;

namespace MultifactorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestsController : ControllerBase
    {
        private readonly RequestManager _requestManager;

        public RequestsController(RequestManager requestManager)
        {
            _requestManager = requestManager;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel>> Create(RequestModel request)
        {
            var credential = await _requestManager.GetCredentialAsync(request);
            return Ok(new ResponseModel(credential));
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController] // Api endpoint olduğu belirtilir.
    [Route("api/[controller]")] // controller burada users demektir. UsersController, Controller çıkarılarak placeholder oluşurur gibi. api/users default endpoint oldu.
    public class BaseApiController : ControllerBase
    {
        
    }
}
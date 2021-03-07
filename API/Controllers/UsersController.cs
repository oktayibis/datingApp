using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController] // Api endpoint olduğu belirtilir.
    [Route("api/[controller]")] // controller burada users demektir. UsersController, Controller çıkarılarak placeholder oluşurur gibi. api/users default endpoint oldu.
    public class UsersController : ControllerBase
    {
        // constructor oluşurulur, buradaki context
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() // ActionResult return type demek, Task ise işlemin async olmasında dolayı server işlemcisi üzerinde threadlere göre bu işlemi böl demek.
            // IEnumerable, liste üzerinde iteration şansı tandır. liste dönülecekse kullanılır. Bu liste AppUser tipinde dedik.
        
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")] // api/users/id -> api/user/2
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
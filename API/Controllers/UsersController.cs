using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
   
    public class UsersController : BaseApiController
    {
        // constructor oluşurulur, buradaki context
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() // ActionResult return type demek, Task ise işlemin async olmasında dolayı server işlemcisi üzerinde threadlere göre bu işlemi böl demek.
            // IEnumerable, liste üzerinde iteration şansı tandır. liste dönülecekse kullanılır. Bu liste AppUser tipinde dedik.
        
        {
            return await _context.Users.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")] // api/users/id -> api/user/2
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
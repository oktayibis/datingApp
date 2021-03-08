using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        
        // Create a user
        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // gelen username bilgisinin uniqe olma durumu kontrol ediyor, eğer varsa, bad request dönüyoruz.
            if (await UserExisits(registerDto.Username)) return BadRequest("Username is taken");
            
            using var hmac = new HMACSHA512(); // using: HMACSHA.. classıyla işin bittiğinde dispose et.

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // ComputeHas metdou byte desteklidir. Bu nedenle gelen string değerli Encoding.GetBytes ile byte çeviriyoruz. Bu bize byte tipinde array döndürecek.
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // databse username göre (ya da eşssiz bir veriye) ilgili user çağrıyoruz.
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == loginDto.Username);
            // Eğer database username bulunamzsa böyle bir user yok demektir ve hata dönüyoruz.
            if (user == null) return Unauthorized("Invalid Username");
            
            // salt tipine göre hash fonksiyonunu çalıştıryporuz, salt random olduğundan her saltta üretilen hash algoritması değişir.
            // bu nedenle hmac objesi üretilirlen, muhakkak kayıtlı salt kullanılarak üretilmelidir.
            using var hmac = new HMACSHA512(user.PasswordSalt);
            
            // kullancının girdiği şifereyi hash'e çeviriyoruz.
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            
            // hasların aynı olması lazım. Hashler byte tipinde dizide kayıtlıydı. İki diziyide karşılaştırıyoruz. Eğer herhangi bir yerde uyuşmazlık varsa, tüm karşılaştırma bitmeden hata çeviriyoruz.
            for (int i = 0; i < computedHash.Length; i++)
            {    
                // hashlar tutmadı, şifre doğru değil!
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Passsword");
            }

            // username var, şifre doğru. username bilgisini ve token bilgisini çeviriyoruz.
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }
    
        // Aynı username den var mı? uniqe
        private async Task<bool> UserExisits(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
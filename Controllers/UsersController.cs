using freeTime.Models;
using freeTime.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace freeTime.Controllers
{
    public class UsersController : Controller
    {
        private readonly FreeTimeContext _context;
        private readonly HashPassword Hash;

        public UsersController(FreeTimeContext context)
        {
            _context = context;
            Hash = new HashPassword();
           
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        } 

        public IActionResult Register(string username, string password)
        {
            //var passwordHash = Hash.Hashing(password);
            //var passwordHash = Hash.Encrypt(password);
            var salt = Hash.createSalt(2);
            var passwordHash = Hash.Salitng(password, salt );

                User user = new User
                {
                    UserName = username,
                    Password = passwordHash,
                    Salt = salt
                };

                _context.Add(user);
               var newUser = _context.SaveChanges();

            if (newUser > 0)
            {
                return View("Index");
            }
                return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Login(string username, string password)
        {

            var user = _context.User.FirstOrDefault(u => u.UserName == username);

            if (user != null)
            {
                //var passwordHash = Hash.Hashing(password);
                //var passwordDecrypt = Hash.Decrypt(user.Password);
                var verifyPassword = Hash.VerifiySalting(password, user.Password, user.Salt);

                if (verifyPassword)
                   return View("Profile");
                    
            }

            return View();

        }

    }
}

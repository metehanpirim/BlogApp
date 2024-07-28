using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository _userRepository;
        public UserController(IUserRepository userRepository){
            _userRepository = userRepository;
        }

        public IActionResult Login(){
            if(User.Identity!.IsAuthenticated){
                return RedirectToAction("index", "post");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model){
            if(ModelState.IsValid){
                var user = _userRepository.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if(user != null){
                    var userClaims = new List<Claim>();

                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, user.UserName ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, user.FullName ?? ""));

                    var ClaimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties{
                        IsPersistent = true
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(ClaimsIdentity),
                        authProperties
                    );
                    return RedirectToAction("Index", "Post");
                }         
            }
            ModelState.AddModelError("", "Please check the fields!");
            return View(model);
        }

        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HideMessage_web.Data;
using HideMessage_web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HideMessage_web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "登录";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User userFromFore)
        {
            using (var context = new DataContext())
            {
                var userFromDB = context.Users.FirstOrDefault(u => u.user_account == userFromFore.user_account
                                                            && u.user_password == userFromFore.user_password);

                if (userFromDB != null)
                {
					var claims = new List<Claim>()
    				{
                        new Claim(ClaimTypes.Name, userFromDB.user_account),
                        new Claim(ClaimTypes.GivenName,userFromDB.user_name),
                        new Claim(ClaimTypes.NameIdentifier, userFromDB.user_id.ToString())
                    };

					//init the identity instances 
					var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "user_cookie"));
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
					return RedirectToAction("Index", "Home");
                } 
                else
    			{
    				ViewBag.ErrMsg = "帐号和密码不正确";
    				return View();
    			}
             }
        }
    }
}

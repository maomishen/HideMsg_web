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
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string userName = User.FindFirst(ClaimTypes.GivenName).Value;
            ViewBag.Title = userName;
            return View();
        }
    }
}

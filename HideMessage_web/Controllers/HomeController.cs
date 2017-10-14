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
using HideMessage_web.Logic;

namespace HideMessage_web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public JsonResult Reload()
        {
            int user_id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            using (var context = new DataContext())
            {
                var result = context.Messages
                    .Where(m => m.message_send_user == user_id)
                    .OrderByDescending(m => m.message_create_time)
                    .ToList();
                return Json(result);
            }
        }

        public IActionResult Index()
        {
            string userName = User.FindFirst(ClaimTypes.GivenName).Value;
            ViewBag.Title = userName;
            ViewBag.NeedAlertMessage = false;
            int user_id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            using (var context = new DataContext())
            {
                ViewBag.Messages = context.Messages
                    .Where(m => m.message_send_user == user_id)
                    .OrderByDescending(m => m.message_create_time)
                    .ToList();
            }
            return View();
        }

		[HttpPost]
        public IActionResult Index(Messages messageFromFore)
		{
            if (messageFromFore.message_phone_number == null || messageFromFore.message_phone_number == "") {
				ViewBag.AlertMessage = "请输入电话号码";
				return View();
            }
            int user_id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			using (var context = new DataContext())
			{
                messageFromFore.message_create_time = DateTime.Now;
                messageFromFore.message_send_user = user_id;
                messageFromFore.phone_state = "分配设备中";
                messageFromFore.send_phone_id = -1;
				context.Messages.Add(messageFromFore);
				context.SaveChanges();
                //ViewBag.NeedAlertMessage = true;
                //ViewBag.AlertMessage = "请求已接收";
				//ViewBag.Messages = context.Messages
					//.Where(m => m.message_send_user == user_id)
					//.OrderByDescending(m => m.message_create_time)
					//.ToList();

                Task.Run(() => new SendRequest(messageFromFore));
                return RedirectToAction("Index", "Home");
			}
		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpOverrides;

namespace HideMessage_web.Controllers
{
	public class DevicesSginController : Controller
	{
        [HttpGet]
		public String Sgin(String SginCode, String password, String cliendId)
		{
            //string ipAddress2 = Request.HttpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            string ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();

            //string ipAddress = request.HttpContext.Connection.RemoteIpAddress;
			//string ipAddress = this.Request.HttpContext.Connection.RemoteIpAddress.IsIPv4MappedToIPv6
					//? Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()
					//: Request.HttpContext.Connection.RemoteIpAddress.ToString();
            //string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return ipAddress + "(SginCode)" + SginCode + "(password)" + password;
		}
	}
}

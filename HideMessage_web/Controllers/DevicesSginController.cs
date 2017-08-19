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
		public String Sgin(String SginCode, String password)
		{
            string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
			return ip;
		}
	}
}

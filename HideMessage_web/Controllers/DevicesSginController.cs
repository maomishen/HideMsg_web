using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HideMessage_web.Data;
using HideMessage_web.Models;

namespace HideMessage_web.Controllers
{
	public class DevicesSginController : Controller
	{
        [HttpGet]
		public String Sgin(String SginCode, String password, String clientId)
		{
            string ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if(SginCode == "luna" || password == "12345ssdlh") {
                if (clientId == null || clientId == "") {
                    return "clientId can not be found";
                }
				using (var context = new DataContext())
				{
                    Devices device = new Devices();
                    device.device_code = SginCode;
                    device.device_client_id = clientId;
                    device.device_sgin_password = password;
                    device.device_ip_v4 = ipAddress;
                    device.device_name = "Meizu M9";
                    context.Devices.Add(device);
                    context.SaveChanges();
			    }
                return "success";
            } else {
                return "SginCode or password not match";
            }
		}
	}
}

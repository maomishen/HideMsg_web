using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HideMessage_web.Data;
using HideMessage_web.Models;

namespace HideMessage_web.Controllers
{
	public class SmsGetController : Controller
	{
		[HttpGet]
		public String getState(String code, String password, String msgId, int state)
		{
			string ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();

			if (code == "56ftugybnj" || password == "rtyuknbk")
			{
				if (msgId == null || msgId == "")
				{
					return "Message Id can not be found";
				}
				using (var context = new DataContext())
				{
                    var message = context.Messages.FirstOrDefault(m => m.message_id == int.Parse(msgId));
					if (state == 3)
					{
						message.phone_state = "开机";
						message.message_get_state_time = DateTime.Now;
					} else if (state == 2) {
                        message.phone_state = "状态未知";
                        message.message_get_state_time = DateTime.Now;
                    } else if (state == 1) {
                        message.phone_state = "获取状态中";
                        message.message_send_time = DateTime.Now;
                    } else if (state == 0) {
                        message.phone_state = "等待获取状态";
                        message.device_ack_time = DateTime.Now;
                    }
                    context.Messages.Update(message);
					context.SaveChanges();
				}
				return "success";
			}
			else
			{
				return "code or password not match";
			}
		}
	}
}

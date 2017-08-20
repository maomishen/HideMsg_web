using System;
using System.ComponentModel.DataAnnotations;

namespace HideMessage_web.Models
{
	public class Devices
	{
		[Key]
		public int device_id { get; set; }
		public string device_name { get; set; }
		public string device_ip_v4 { get; set; }
		public string device_code { get; set; }
		public string device_sgin_password { get; set; }
		public string device_client_id { get; set; }
	}
}

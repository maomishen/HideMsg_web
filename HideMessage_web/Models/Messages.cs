using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HideMessage_web.Models
{
    public class Messages
    {
		[Key]
		public int message_id { get; set; }
        [ForeignKey("User")]
		public int message_send_user { get; set; }
        public string message_phone_number { get; set; }
		public DateTime? message_create_time { get; set; }
		public DateTime? message_request_time { get; set; }
		public DateTime? message_send_time { get; set; }
        public DateTime? message_get_state_time { get; set; }
        public DateTime? device_ack_time { get; set; }
        public string phone_state { get; set; }
        public int send_phone_id { get; set; }
        public string error_message { get; set; }
        public string task_id { get; set; }
    }
}

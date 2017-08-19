﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HideMessage_web.Models
{
    public class User
    {
        [Key]
		public int user_id { get; set; }
		public string user_name { get; set; }
        public string user_account { get; set; }
        public string user_password { get; set; }
        public int user_role { get; set; }
        public int create_from { get; set; }
        public DateTime user_create_time { get; set; }
        public DateTime user_last_time { get; set; }
    }
}

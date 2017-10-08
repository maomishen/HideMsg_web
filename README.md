# HideMsg_web

使用时需要创建一个文件：
HideMessage_web/Data/DataContext.cs
文件内容：
---
`
using Microsoft.EntityFrameworkCore;
using HideMessage_web.Models;

namespace HideMessage_web.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { set; get; }
        public DbSet<Devices> Devices { set; get; }
        public DbSet<Messages> Messages { set; get; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder
				.UseMySql(@"Server=127.0.0.1;database=HideMsg;uid=root;pwd=password");
    }
}
`

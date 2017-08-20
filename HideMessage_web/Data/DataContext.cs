using Microsoft.EntityFrameworkCore;
using HideMessage_web.Models;

namespace HideMessage_web.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { set; get; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder
				.UseMySql(@"Server=120.27.50.157;database=HideMsg;uid=root;pwd=ETTk6xqXEhBKeLXfgeW7e8Js9ZGidj4nm4KsEJbbZ7LYEprPXgpLmeQDUhU8G4A6");
    }
}

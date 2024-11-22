using Microsoft.EntityFrameworkCore;
using privaxnet_api.Models;


namespace privaxnet_api.Data;

public class DataContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseInMemoryDatabase("MyDatabase1");
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Session> Sessions { get; set; }


} 

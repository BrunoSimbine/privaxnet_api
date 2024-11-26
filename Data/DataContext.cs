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
	public DbSet<Product> Products { get; set; }
	public DbSet<Voucher> Vouchers { get; set; }
} 

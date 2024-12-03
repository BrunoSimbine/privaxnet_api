using Microsoft.EntityFrameworkCore;
using privaxnet_api.Models;


namespace privaxnet_api.Data;

public class DataContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		//var connection = "User ID=bruno;Password=bruno1234;Server=5.75.139.11;Port=32768;Database=testes;";
		var connection = "Server=5.75.139.111;Database=bruno;User=bruno;Password=bruno1234;";
		optionsBuilder.UseMySql(connection, new MySqlServerVersion(new Version(10, 6)));
		//optionsBuilder.UseInMemoryDatabase("Database");
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Voucher> Vouchers { get; set; }
} 

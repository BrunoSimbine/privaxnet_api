using Microsoft.EntityFrameworkCore;
using privaxnet_api.Models;


namespace privaxnet_api.Data;

public class DataContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{;
		var connection = "Server=5.75.139.111;Database=bruno;User=bruno;Password=bruno1234;";
		//var connection = "Server=127.0.0.1;Database=bruno;User=root;Password=;";
		optionsBuilder.UseMySql(connection, new MySqlServerVersion(new Version(10, 6)));
		//optionsBuilder.UseInMemoryDatabase("Database");
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Voucher> Vouchers { get; set; }
	public DbSet<Currency> Currencies { get; set; }
	public DbSet<Wallet> Wallets { get; set; }
	public DbSet<Payment> Payments { get; set; }
	public DbSet<PayAgent> PayAgents { get; set; }
} 

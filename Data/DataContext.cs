using Microsoft.EntityFrameworkCore;
using privaxnet_api.Models;


namespace privaxnet_api.Data;

public class DataContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		//var connection = "User ID=bruno;Password=bruno1234;Server=167.235.138.208;Port=32771;Database=testes;";
		var connection = "User ID=bruno;Password=bruno1234;Server=167.235.138.208;Port=32771;Database=bruno;";
		optionsBuilder.UseNpgsql(connection);
		//optionsBuilder.UseNpgsql(connection);
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Voucher> Vouchers { get; set; }
} 

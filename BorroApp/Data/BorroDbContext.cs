using BorroApp.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace BorroApp.Data;

public class BorroDbContext : DbContext {
	public BorroDbContext(DbContextOptions<BorroDbContext> options) : base(options) { }

	public DbSet<Category>    Category    { get; set; }
	public DbSet<Post>        Post        { get; set; }
	public DbSet<Reservation> Reservation { get; set; }
	public DbSet<User>        User        { get; set; }
	public DbSet<UserInfo>   UserInfo    { get; set; }
	public DbSet<Rating>   Ratings    { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		/*modelBuilder.Entity<Reservation>()
					.HasOne(p => p.User)
					.WithMany(b => b.Reservations)
					.OnDelete(DeleteBehavior.Restrict);*/
		
		foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
		{
			relationship.DeleteBehavior = DeleteBehavior.NoAction;
		}
	}
}
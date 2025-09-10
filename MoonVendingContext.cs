using Microsoft.EntityFrameworkCore;

public class VendingContext : DbContext
{
    public VendingContext(DbContextOptions<VendingContext> options) : base(options) { }

    public DbSet<PinHistory> PinHistory { get; set; }
    public DbSet<Receipt> Receipt { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PinHistory>().HasKey(e => e.RecordId);
        modelBuilder.Entity<Receipt>().HasKey(e => e.SaleId);
    }

}
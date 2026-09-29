using Microsoft.EntityFrameworkCore;
using server.Entities;

namespace server.Data;

public class InvoiceDbContext : DbContext
{
    public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options)
        : base(options) { }

    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Merchant> Merchants => Set<Merchant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<MerchantUser> MerchantUsers => Set<MerchantUser>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<MerchantCustomer> MerchantCustomers => Set<MerchantCustomer>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    //key constraint and relationships
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //constraint defination of Admin
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(a => a.AdminId);

            entity.Property(a => a.Name).HasMaxLength(150).IsRequired();

            entity.Property(a => a.Phone).HasMaxLength(30);

            entity.Property(a => a.Email).HasMaxLength(255).IsRequired();

            entity.Property(a => a.PasswordHash).IsRequired();

            entity.HasIndex(a => a.Email).IsUnique();
            
            entity.Property(a => a.AdminId)
                .HasDefaultValueSql("gen_random_uuid()");
        });

        //constraint of Merchant
        modelBuilder.Entity<Merchant>(entity =>
        {
            entity.HasKey(m => m.MerchantId);

            entity.Property(m => m.LegalName).HasMaxLength(200).IsRequired();

            entity.Property(m => m.DisplayName).HasMaxLength(200).IsRequired();

            entity.Property(m => m.Email).HasMaxLength(255).IsRequired();

            entity.Property(m => m.Phone).HasMaxLength(30);

            entity.Property(m => m.Gstin).HasMaxLength(15);

            entity.Property(m => m.Address).HasMaxLength(500);

            entity.Property(m => m.District).HasMaxLength(100);

            entity.Property(m => m.State).HasMaxLength(100);

            entity.Property(m => m.PostalCode).HasMaxLength(20);

            entity.Property(m => m.Country).HasMaxLength(100);

            entity.Property(m => m.IsActive).HasDefaultValue(true);

            entity.Property(m => m.MerchantId)
                .HasDefaultValueSql("gen_random_uuid()");

            entity
                .HasOne(m => m.Admin)
                .WithMany(a => a.Merchants)
                .HasForeignKey(m => m.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(m => m.Gstin).IsUnique();
        });

        // constraint of User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);

            entity.Property(u => u.UserId)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(u => u.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.HasIndex(u => u.Email)
                .IsUnique();
        });

        // constraint of MerchantUser
        modelBuilder.Entity<MerchantUser>(entity =>
        {
            entity.HasKey(mu => mu.MerchantUserId);

            entity.Property(mu => mu.MerchantUserId)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(mu => mu.IsActive)
                .HasDefaultValue(true);

            entity
                .HasOne(mu => mu.User)
                .WithMany(u => u.MerchantUsers)
                .HasForeignKey(mu => mu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(mu => mu.Merchant)
                .WithMany(m => m.MerchantUsers)
                .HasForeignKey(mu => mu.MerchantId)
                .OnDelete(DeleteBehavior.Restrict);

            // A user can have only one membership record for the same merchant.
            entity
                .HasIndex(mu => new { mu.UserId, mu.MerchantId })
                .IsUnique();

            // A user can have only one active merchant membership at a time.
            entity
                .HasIndex(mu => mu.UserId)
                .IsUnique()
                .HasFilter("\"IsActive\" = TRUE");
        });

        //CONSTRAINT OF CUSTOMER
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.CustomerId);

            entity.Property(c => c.CustomerId)
                .HasDefaultValueSql("gen_random_uuid()");
            entity.Property(c => c.Name).HasMaxLength(200).IsRequired();

            entity.Property(c => c.Email).HasMaxLength(255);

            entity.Property(c => c.Phone).HasMaxLength(30);

            entity.Property(c => c.Gstin).HasMaxLength(15);

            entity.Property(c => c.BillingAddress).HasMaxLength(500);

            entity.Property(c => c.ShippingAddress).HasMaxLength(500);

            entity.Property(c => c.District).HasMaxLength(100);

            entity.Property(c => c.State).HasMaxLength(100);

            entity.Property(c => c.PostalCode).HasMaxLength(20);

            entity.Property(c => c.Country).HasMaxLength(100);

            entity.Property(c => c.IsActive).HasDefaultValue(true);

            entity
                .HasOne(c => c.User)
                .WithMany(u => u.Customers)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasIndex(c => c.UserId)
                .IsUnique();
        });

        //constraint of MerchantCustomer
        modelBuilder.Entity<MerchantCustomer>(entity =>
        {
            entity.HasKey(mc => new { mc.MerchantId, mc.CustomerId });

            entity.Property(mc => mc.IsActive).HasDefaultValue(true);

            entity
                .HasOne(mc => mc.Merchant)
                .WithMany(m => m.MerchantCustomers)
                .HasForeignKey(mc => mc.MerchantId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(mc => mc.Customer)
                .WithMany(c => c.MerchantCustomers)
                .HasForeignKey(mc => mc.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //constraint for service
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(s => s.ServiceId);

            entity.Property(s => s.ServiceId)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(s => s.Name).HasMaxLength(200).IsRequired();

            entity.Property(s => s.Description).HasMaxLength(1000);

            entity.Property(s => s.HsnSacCode).HasMaxLength(20);

            entity.Property(s => s.DefaultUnit).HasMaxLength(30);

            entity.Property(s => s.DefaultUnitPrice).HasPrecision(18, 2);

            entity.Property(s => s.DefaultTaxRate).HasPrecision(5, 2);
            
            entity.Property(s => s.IsActive).HasDefaultValue(true);

            entity
                .HasOne(s => s.Merchant)
                .WithMany(m => m.Services)
                .HasForeignKey(s => s.MerchantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // constraint for invoice
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(i => i.InvoiceId);
            entity.Property(i => i.InvoiceId)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(i => i.InvoiceNumber).HasMaxLength(50).IsRequired();

            entity.Property(i => i.PlaceOfSupply).HasMaxLength(100);

            entity.Property(i => i.Currency).HasMaxLength(3).IsRequired();

            entity.Property(i => i.Subtotal).HasPrecision(18, 2);

            entity.Property(i => i.DiscountAmount).HasPrecision(18, 2);

            entity.Property(i => i.TaxableAmount).HasPrecision(18, 2);

            entity.Property(i => i.TotalAmount).HasPrecision(18, 2);

            entity.Property(i => i.Status).HasConversion<string>().HasMaxLength(30).IsRequired();

            entity.Property(i => i.Notes).HasMaxLength(2000);

            entity.Property(i => i.TermsAndCondition).HasMaxLength(5000);

            entity.HasIndex(i => new { i.MerchantId, i.InvoiceNumber }).IsUnique();

            entity
                .HasOne(i => i.Merchant)
                .WithMany()
                .HasForeignKey(i => i.MerchantId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(i => i.Customer)
                .WithMany()
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(i => i.CreatedByMerchantUser)
                .WithMany()
                .HasForeignKey(i => i.CreatedByMerchantUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(i => i.UpdatedByMerchantUser)
                .WithMany()
                .HasForeignKey(i => i.UpdatedByMerchantUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //constraint for invoiceItem
        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(ii => ii.InvoiceItemId);
            entity.Property(ii => ii.InvoiceItemId)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(ii => ii.Description).HasMaxLength(1000).IsRequired();

            entity.Property(ii => ii.HsnSacCode).HasMaxLength(20);

            entity.Property(ii => ii.Quantity).HasPrecision(12, 3);

            entity.Property(ii => ii.Unit).HasMaxLength(30);

            entity.Property(ii => ii.UnitPrice).HasPrecision(18, 2);

            entity.Property(ii => ii.TaxRate).HasPrecision(5, 2);

            entity.Property(ii => ii.DiscountAmount).HasPrecision(18, 2);

            entity.Property(ii => ii.TaxableAmount).HasPrecision(18, 2);

            entity.Property(ii => ii.TaxAmount).HasPrecision(18, 2);

            entity.Property(ii => ii.LineTotal).HasPrecision(18, 2);

            entity
                .HasOne(ii => ii.Invoice)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(ii => ii.Service)
                .WithMany()
                .HasForeignKey(ii => ii.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //constraint for Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.PaymentId);

            entity.Property(p => p.PaymentId)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(p => p.Amount).HasPrecision(18, 2);

            entity.Property(p => p.TransactionId).HasMaxLength(200);

            entity.Property(p => p.PayerName).HasMaxLength(200);

            entity.Property(p => p.BankName).HasMaxLength(200);

            entity.Property(p => p.PaymentMethod).HasMaxLength(50).IsRequired();

            entity.Property(p => p.Status).HasConversion<string>().HasMaxLength(30).IsRequired();

            entity
                .HasOne(p => p.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(p => p.RecordedByMerchantUser)
                .WithMany()
                .HasForeignKey(p => p.RecordedByMerchantUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

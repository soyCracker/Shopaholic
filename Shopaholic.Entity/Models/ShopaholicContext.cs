using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Shopaholic.Entity.Models
{
    public partial class ShopaholicContext : DbContext
    {
        public ShopaholicContext()
        {
        }

        public ShopaholicContext(DbContextOptions<ShopaholicContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderHeader> OrderHeaders { get; set; }
        public virtual DbSet<OrderLog> OrderLogs { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<WebFlow> WebFlows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Shopaholic;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<CustomerAccount>(entity =>
            {
                entity.ToTable("CustomerAccount");

                entity.Property(e => e.AccountId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhotoUrl).HasColumnName("PhotoURL");

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasPrincipalKey(p => p.OrderId)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_OrderHeader");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Product");
            });

            modelBuilder.Entity<OrderHeader>(entity =>
            {
                entity.ToTable("OrderHeader");

                entity.HasIndex(e => e.OrderId, "UQ__OrderHea__C3905BCE7A7339B4")
                    .IsUnique();

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FailCode).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsArrived).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsCancel).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsFinish).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsPaid).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsReturn).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSent).HasDefaultValueSql("((0))");

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OrderLog>(entity =>
            {
                entity.ToTable("OrderLog");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FailCode).HasDefaultValueSql("((0))");

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLogs)
                    .HasPrincipalKey(p => p.OrderId)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderLog_OrderHeader");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnName("Description ");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.ToTable("Shipment");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ReceiveMan)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShipNumber).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Shipments)
                    .HasPrincipalKey(p => p.OrderId)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shipment_OrderHeader");
            });

            modelBuilder.Entity<WebFlow>(entity =>
            {
                entity.ToTable("WebFlow");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Enter)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ip)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IP");

                entity.Property(e => e.UpdateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Entities;

public partial class NorthwindContext : DbContext
{

    private readonly string dbPath = @"/Users/erikvanappeldoorn/Projects/Northwind.SqlLite/Northwind.Entities/northwind.db";
    public NorthwindContext()
    {
    }

    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Category> Categories { get; set; }
    
    public virtual DbSet<Customer> Customers { get; set; }
    
    public virtual DbSet<CustomerDemographic> CustomerDemographics { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }
    
    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    
    public virtual DbSet<Product> Products { get; set; }
    
    public virtual DbSet<Region> Regions { get; set; }
    
    public virtual DbSet<Shipper> Shippers { get; set; }
    
    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Territory> Territories { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseSqlite($"Data Source={dbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
        });
        
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

            entity.HasMany(d => d.CustomerTypes).WithMany(p => p.Customers)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerCustomerDemo",
                    r => r.HasOne<CustomerDemographic>().WithMany()
                        .HasForeignKey("CustomerTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("CustomerId", "CustomerTypeId");
                        j.ToTable("CustomerCustomerDemo");
                        j.IndexerProperty<string>("CustomerId").HasColumnName("CustomerID");
                        j.IndexerProperty<string>("CustomerTypeId").HasColumnName("CustomerTypeID");
                    });
        });
        
        modelBuilder.Entity<CustomerDemographic>(entity =>
        {
            entity.HasKey(e => e.CustomerTypeId);

            entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.BirthDate).HasColumnType("DATE");
            entity.Property(e => e.HireDate).HasColumnType("DATE");

            entity.HasOne(d => d.ReportsToNavigation).WithMany(p => p.InverseReportsToNavigation).HasForeignKey(d => d.ReportsTo);

            entity.HasMany(d => d.Territories).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeTerritory",
                    r => r.HasOne<Territory>().WithMany()
                        .HasForeignKey("TerritoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("EmployeeId", "TerritoryId");
                        j.ToTable("EmployeeTerritories");
                        j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");
                        j.IndexerProperty<string>("TerritoryId").HasColumnName("TerritoryID");
                    });
        });
        
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Freight)
                .HasDefaultValue(0)
                .HasColumnType("NUMERIC");
            entity.Property(e => e.OrderDate).HasColumnType("DATETIME");
            entity.Property(e => e.RequiredDate).HasColumnType("DATETIME");
            entity.Property(e => e.ShippedDate).HasColumnType("DATETIME");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders).HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders).HasForeignKey(d => d.EmployeeId);

            entity.HasOne(d => d.ShipViaNavigation).WithMany(p => p.Orders).HasForeignKey(d => d.ShipVia);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId });

            entity.ToTable("Order Details");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UnitPrice).HasColumnType("NUMERIC");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Discontinued).HasDefaultValue("0");
            entity.Property(e => e.ReorderLevel).HasDefaultValue(0);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.UnitPrice)
                .HasDefaultValue(0.0)
                .HasColumnType("NUMERIC");
            entity.Property(e => e.UnitsInStock).HasDefaultValue(0);
            entity.Property(e => e.UnitsOnOrder).HasDefaultValue(0);

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasForeignKey(d => d.CategoryId);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products).HasForeignKey(d => d.SupplierId);
        });
        
        modelBuilder.Entity<Region>(entity =>
        {
            entity.Property(e => e.RegionId)
                .ValueGeneratedNever()
                .HasColumnName("RegionID");
        });
        
        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.Property(e => e.ShipperId).HasColumnName("ShipperID");
        });
        
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
        });

        modelBuilder.Entity<Territory>(entity =>
        {
            entity.Property(e => e.TerritoryId).HasColumnName("TerritoryID");
            entity.Property(e => e.RegionId).HasColumnName("RegionID");

            entity.HasOne(d => d.Region).WithMany(p => p.Territories)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

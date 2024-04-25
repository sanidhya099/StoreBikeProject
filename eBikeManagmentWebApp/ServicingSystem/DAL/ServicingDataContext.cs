﻿using Microsoft.EntityFrameworkCore;
using ServicingSystem.Entities;

namespace ServicingSystem.DAL;
public partial class ServicingDataContext :DbContext
{
    public ServicingDataContext(DbContextOptions<ServicingDataContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerVehicle> CustomerVehicles { get; set; }

    public virtual DbSet<DatabaseVersion> DatabaseVersions { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobDetail> JobDetails { get; set; }

    public virtual DbSet<JobDetailPart> JobDetailParts { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

    public virtual DbSet<ReceiveOrder> ReceiveOrders { get; set; }

    public virtual DbSet<ReceiveOrderDetail> ReceiveOrderDetails { get; set; }

    public virtual DbSet<ReturnedOrderDetail> ReturnedOrderDetails { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleDetail> SaleDetails { get; set; }

    public virtual DbSet<SaleRefund> SaleRefunds { get; set; }

    public virtual DbSet<SaleRefundDetail> SaleRefundDetails { get; set; }

    public virtual DbSet<StandardJob> StandardJobs { get; set; }

    public virtual DbSet<UnorderedPurchaseItemCart> UnorderedPurchaseItemCarts { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryID).HasName("PK_Categories_CategoryID");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponID).HasName("PK_Coupons_CouponID");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerID).HasName("PK_Customers_CustomerID");

            entity.Property(e => e.ContactPhone).IsFixedLength();
            entity.Property(e => e.PostalCode).IsFixedLength();
            entity.Property(e => e.Province).IsFixedLength();
        });

        modelBuilder.Entity<CustomerVehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleIdentification).HasName("PK_CustomerVehicles_VehicleIdentification");

            entity.Property(e => e.VehicleIdentification).IsFixedLength();
            entity.Property(e => e.Make).IsFixedLength();
            entity.Property(e => e.Model).IsFixedLength();

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerVehicles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerVehiclesCustomers_CustomerID");
        });

        modelBuilder.Entity<DatabaseVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Database__3214EC07CD6BBDD7");

            entity.Property(e => e.DateTime).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeID).HasName("PK_Employees_EmployeeID");

            entity.Property(e => e.PostalCode).IsFixedLength();
            entity.Property(e => e.Province).IsFixedLength();
            entity.Property(e => e.SocialInsuranceNumber).IsFixedLength();

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeesPositions_PositionID");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.JobID).HasName("PK_Jobs_JobID");

            entity.Property(e => e.VehicleIdentification).IsFixedLength();

            entity.HasOne(d => d.Employee).WithMany(p => p.Jobs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobsEmployees_EmployeeID");

            entity.HasOne(d => d.VehicleIdentificationNavigation).WithMany(p => p.Jobs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobsCustomerVehicles_VehicleIdentification");
        });

        modelBuilder.Entity<JobDetail>(entity =>
        {
            entity.HasKey(e => e.JobDetailID).HasName("PK_JobDetails_JobDetailID");

            entity.Property(e => e.StatusCode)
                .HasDefaultValue("I")
                .IsFixedLength();

            entity.HasOne(d => d.Coupon).WithMany(p => p.JobDetails).HasConstraintName("FK_JobDetailsCoupons_CouponID");

            entity.HasOne(d => d.Employee).WithMany(p => p.JobDetails).HasConstraintName("FK_JobDetailsEmployees_EmployeeID");

            entity.HasOne(d => d.Job).WithMany(p => p.JobDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobDetailsJobs_JobID");
        });

        modelBuilder.Entity<JobDetailPart>(entity =>
        {
            entity.HasKey(e => e.JobDetailPartID).HasName("PK_JobDetailParts_JobPartDetailsID");

            entity.HasOne(d => d.JobDetail).WithMany(p => p.JobDetailParts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobDetailPartsJobDetails_JobDetailID");

            entity.HasOne(d => d.Part).WithMany(p => p.JobDetailParts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobDetailPartsParts_PartID");
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.PartID).HasName("PK_Parts_PartID");

            entity.Property(e => e.Refundable)
                .HasDefaultValue("Y")
                .IsFixedLength();

            entity.HasOne(d => d.Category).WithMany(p => p.Parts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PartsCategories_CategoryID");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Parts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PartsVendors_VendorID");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionID).HasName("PK_Positions_PositionID");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderID).HasName("PK_PurchaseOrders_PurchaseOrderID");

            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.PurchaseOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrdersEmployees_EmployeeID");

            entity.HasOne(d => d.Vendor).WithMany(p => p.PurchaseOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrdersVednors_VendorID");
        });

        modelBuilder.Entity<PurchaseOrderDetail>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderDetailID).HasName("PK_PurchaseOrderDetails_PurchaseOrderDetailID");

            entity.HasOne(d => d.Part).WithMany(p => p.PurchaseOrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderDetailsParts_PartID");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderDetailsPurchaseOrders_PurchaseOrderID");
        });

        modelBuilder.Entity<ReceiveOrder>(entity =>
        {
            entity.HasKey(e => e.ReceiveOrderID).HasName("PK_ReceiveOrders_ReceiveOrderID");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.ReceiveOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceiveOrdersPurchaseOrders_PurchaseOrderID");
        });

        modelBuilder.Entity<ReceiveOrderDetail>(entity =>
        {
            entity.HasKey(e => e.ReceiveOrderDetailID).HasName("PK_ReceiveOrderDetails_ReceiveOrderDetailID");

            entity.HasOne(d => d.PurchaseOrderDetail).WithMany(p => p.ReceiveOrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceiveOrderDetailsPurchaseOrderDetails_OrderDetailID");

            entity.HasOne(d => d.ReceiveOrder).WithMany(p => p.ReceiveOrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceiveOrderDetailsReceiveOrders_ReceiveOrderID");
        });

        modelBuilder.Entity<ReturnedOrderDetail>(entity =>
        {
            entity.HasKey(e => e.ReturnedOrderDetailID).HasName("PK_ReturnedOrderDetails_ReturnOrderDetailID");

            entity.HasOne(d => d.PurchaseOrderDetail).WithMany(p => p.ReturnedOrderDetails).HasConstraintName("FK_ReturnedOrderDetailsPurchaseOrderDetails_OrderDetailID");

            entity.HasOne(d => d.ReceiveOrder).WithMany(p => p.ReturnedOrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReturnedOrderDetailsReceiveOrders_ReceiveOrderID");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleID).HasName("PK_Sales_SaleID");

            entity.Property(e => e.PaymentType).IsFixedLength();
            entity.Property(e => e.SaleDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Coupon).WithMany(p => p.Sales).HasConstraintName("FK_SalesCoupons_CouponID");

            entity.HasOne(d => d.Employee).WithMany(p => p.Sales)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalesEmployees_EmployeeID");
        });

        modelBuilder.Entity<SaleDetail>(entity =>
        {
            entity.HasKey(e => e.SaleDetailID).HasName("PK_SaleDetails_SaleDetailID");

            entity.HasOne(d => d.Part).WithMany(p => p.SaleDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetailsParts_PartID");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetailsSalesSaleID");
        });

        modelBuilder.Entity<SaleRefund>(entity =>
        {
            entity.HasKey(e => e.SaleRefundID).HasName("PK_SaleRefunds_SaleRefundID");

            entity.Property(e => e.SaleRefundDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.SaleRefunds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleRefundsEmployees_EmployeeID");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleRefunds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CK_SaleRefundsSales_SaleID");
        });

        modelBuilder.Entity<SaleRefundDetail>(entity =>
        {
            entity.HasKey(e => e.SaleRefundDetailID).HasName("PK_SaleRefundDetails_SaleRefundDetailID");

            entity.HasOne(d => d.Part).WithMany(p => p.SaleRefundDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleRefundDetailsParts_PartId");

            entity.HasOne(d => d.SaleRefund).WithMany(p => p.SaleRefundDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleRefundDetailsSaleRefunds_SaleRefundID");
        });

        modelBuilder.Entity<StandardJob>(entity =>
        {
            entity.HasKey(e => e.StandardJobID).HasName("PK_StandardJobs_StandardJobID");
        });

        modelBuilder.Entity<UnorderedPurchaseItemCart>(entity =>
        {
            entity.HasKey(e => e.CartID).HasName("PK__Unordere__51BCD7976B0A11B8");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.VendorID).HasName("PK_Vendors_VendorID");

            entity.Property(e => e.PostalCode).IsFixedLength();
            entity.Property(e => e.ProvinceID)
                .HasDefaultValue("AB")
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

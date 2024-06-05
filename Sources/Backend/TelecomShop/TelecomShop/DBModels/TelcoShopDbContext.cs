using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TelecomShop.DBModels;

public partial class TelcoShopDbContext : DbContext
{
    public TelcoShopDbContext()
    {
    }

    public TelcoShopDbContext(DbContextOptions<TelcoShopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActiveProduct> ActiveProducts { get; set; }

    public virtual DbSet<BillingAccount> BillingAccounts { get; set; }

    public virtual DbSet<CharInvolvement> CharInvolvements { get; set; }

    public virtual DbSet<Characteristic> Characteristics { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=postgres;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActiveProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("active_products_pk");

            entity.ToTable("active_products");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.BillingAccountId).HasColumnName("billing_account_id");
            entity.Property(e => e.ContractTerm).HasColumnName("contract_term");
            entity.Property(e => e.DataAmount).HasColumnName("data_amount");
            entity.Property(e => e.DataLeft).HasColumnName("data_left");
            entity.Property(e => e.ExtendedChars)
                .HasColumnType("json")
                .HasColumnName("extended_chars");
            entity.Property(e => e.ParentProductId).HasColumnName("parent_product_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.PurchaceDate).HasColumnName("purchace_date");
            entity.Property(e => e.SmsAmount).HasColumnName("sms_amount");
            entity.Property(e => e.SmsLeft).HasColumnName("sms_left");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VoiceAmount).HasColumnName("voice_amount");
            entity.Property(e => e.VoiceLeft).HasColumnName("voice_left");

            entity.HasOne(d => d.BillingAccount).WithMany(p => p.ActiveProducts)
                .HasForeignKey(d => d.BillingAccountId)
                .HasConstraintName("fk_active_products_billing_acc");

            entity.HasOne(d => d.Product).WithMany(p => p.ActiveProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_active_products_products");

            entity.HasOne(d => d.User).WithMany(p => p.ActiveProducts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_active_products_users");
        });

        modelBuilder.Entity<BillingAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("billing_accounts_pk");

            entity.ToTable("billing_accounts");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Balance)
                .HasDefaultValueSql("0.0")
                .HasColumnName("balance");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.BillingAccounts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_billing_account_user");
        });

        modelBuilder.Entity<CharInvolvement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("char_involvements_pk");

            entity.ToTable("char_involvements");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CharId).HasColumnName("char_id");
            entity.Property(e => e.DefaultValue)
                .HasColumnType("character varying")
                .HasColumnName("default_value");
            entity.Property(e => e.ListValues)
                .HasColumnType("character varying")
                .HasColumnName("list_values");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Char).WithMany(p => p.CharInvolvements)
                .HasForeignKey(d => d.CharId)
                .HasConstraintName("fk_char_involvements_chars");

            entity.HasOne(d => d.Product).WithMany(p => p.CharInvolvements)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_char_involvements_ptoducts");
        });

        modelBuilder.Entity<Characteristic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("characteristics_pk");

            entity.ToTable("characteristics");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pk");

            entity.ToTable("products");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ActiveFrom).HasColumnName("active_from");
            entity.Property(e => e.ActiveTo).HasColumnName("active_to");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.DowngradeOptions)
                .HasColumnType("character varying")
                .HasColumnName("downgrade_options");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.PriceOneTime)
                .HasDefaultValueSql("0.0")
                .HasColumnName("price_one_time");
           
            entity.Property(e => e.PriceRecurrent)
                .HasDefaultValueSql("0.0")
                .HasColumnName("price_recurrent");
          
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
            entity.Property(e => e.UpgradeOptions)
                .HasColumnType("character varying")
                .HasColumnName("upgrade_options");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pk");

            entity.ToTable("users");

            entity.HasIndex(e => e.Msisdn, "users_unique").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Msisdn)
                .HasColumnType("character varying")
                .HasColumnName("msisdn");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Surname)
                .HasColumnType("character varying")
                .HasColumnName("surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

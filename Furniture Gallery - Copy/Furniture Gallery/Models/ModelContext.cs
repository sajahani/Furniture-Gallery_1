using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Furniture_Gallery.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AboutU> AboutUs { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<BankAccInfo> BankAccInfos { get; set; }
        
        public virtual DbSet<ContactU> ContactUs { get; set; }
      
        public virtual DbSet<Furniturecategory> Furniturecategories { get; set; }
        public virtual DbSet<Furnitureorder> Furnitureorders { get; set; }
        public virtual DbSet<Furniturepayment> Furniturepayments { get; set; }
        public virtual DbSet<Furnitureproduct> Furnitureproducts { get; set; }
        public virtual DbSet<Furniturerole> Furnitureroles { get; set; }
        public virtual DbSet<Homepage> Homepages { get; set; }
        public virtual DbSet<Productorder> Productorders { get; set; }
        public virtual DbSet<Testimonial> Testimonials { get; set; }
        public virtual DbSet<Useraccount> Useraccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("USER ID=JOR17_User27;PASSWORD=Test1998;DATA SOURCE=94.56.229.181:3488/traindb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("JOR17_USER27")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<AboutU>(entity =>
            {
                entity.HasKey(e => e.AboutusId)
                    .HasName("SYS_C00315757");

                entity.ToTable("ABOUT_US");

                entity.Property(e => e.AboutusId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ABOUTUS_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("BANK");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Balance)
                    .HasColumnType("FLOAT")
                    .HasColumnName("BALANCE");

                entity.Property(e => e.CreditCard)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CREDIT_CARD");

                entity.Property(e => e.Cvv)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CVV");

                entity.Property(e => e.ExpireDate)
                    .HasColumnType("DATE")
                    .HasColumnName("EXPIRE_DATE");
            });

            modelBuilder.Entity<BankAccInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BANK_ACC_INFO");

                entity.Property(e => e.Cardnumber)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CARDNUMBER");

                entity.Property(e => e.Cvv)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CVV");

                entity.Property(e => e.UseraccountId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USERACCOUNT_ID");

                entity.HasOne(d => d.Useraccount)
                    .WithMany()
                    .HasForeignKey(d => d.UseraccountId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FKBANK_USERACCOUNT_ID");
            });

            

            modelBuilder.Entity<ContactU>(entity =>
            {
                entity.HasKey(e => e.ContactId)
                    .HasName("SYS_C00315754");

                entity.ToTable("CONTACT_US");

                entity.Property(e => e.ContactId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CONTACT_ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME");

                entity.Property(e => e.Message)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("MESSAGE");
            });

            

            modelBuilder.Entity<Furniturecategory>(entity =>
            {
                entity.ToTable("FURNITURECATEGORY");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Categoryname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CATEGORYNAME");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");
            });

            modelBuilder.Entity<Furnitureorder>(entity =>
            {
                entity.ToTable("FURNITUREORDER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ORDER_DATE");

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_STATUS");

                entity.Property(e => e.OrderTotal)
                    .HasColumnType("FLOAT")
                    .HasColumnName("ORDER_TOTAL");

                entity.Property(e => e.UseraccountId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USERACCOUNT_ID");

                entity.HasOne(d => d.Useraccount)
                    .WithMany(p => p.Furnitureorders)
                    .HasForeignKey(d => d.UseraccountId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_USERACCOUNT_ID");
            });

            modelBuilder.Entity<Furniturepayment>(entity =>
            {
                entity.ToTable("FURNITUREPAYMENT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.PaymentAmount)
                    .HasColumnType("FLOAT")
                    .HasColumnName("PAYMENT_AMOUNT");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PAYMENT_DATE");
            });

            modelBuilder.Entity<Furnitureproduct>(entity =>
            {
                entity.ToTable("FURNITUREPRODUCT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.Price)
                    .HasColumnType("FLOAT")
                    .HasColumnName("PRICE");

                entity.Property(e => e.Productname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTNAME");

                entity.Property(e => e.CategoryId)
                 .HasColumnType("NUMBER")
                 .HasColumnName("CATEGORY_ID");


                entity.HasOne(d => d.Category)
                   .WithMany(p => p.Furnitureproducts)
                   .HasForeignKey(d => d.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("CAT_FK");




            });

            modelBuilder.Entity<Furniturerole>(entity =>
            {
                entity.ToTable("FURNITUREROLE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Rolename)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ROLENAME");
            });

            modelBuilder.Entity<Homepage>(entity =>
            {
                entity.ToTable("HOMEPAGE");

                entity.Property(e => e.HomepageId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("HOMEPAGE_ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.EmailFacebook)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL_FACEBOOK");

                entity.Property(e => e.HomepageDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("HOMEPAGE_DESCRIPTION");

                entity.Property(e => e.HomepageImage)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("HOMEPAGE_IMAGE");

                entity.Property(e => e.HomepageLogo)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("HOMEPAGE_LOGO");

                entity.Property(e => e.HomepageTitle)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("HOMEPAGE_TITLE");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");
            });

           
            modelBuilder.Entity<Productorder>(entity =>
            {
                entity.ToTable("PRODUCTORDER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PaymentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAYMENT_ID");

                entity.Property(e => e.ProductId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_ID");

                entity.Property(e => e.Quantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("QUANTITY");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("FLOAT")
                    .HasColumnName("TOTAL_AMOUNT");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Productorders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ORDER2_ID");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Productorders)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PAYMENT");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Productorders)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PRODUCT2_ID");


				
			});

           

            modelBuilder.Entity<Testimonial>(entity =>
            {
                entity.ToTable("TESTIMONIAL");

                entity.Property(e => e.TestimonialId)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("TESTIMONIAL_ID");

                entity.Property(e => e.Message)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("MESSAGE");

                entity.Property(e => e.Rating)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("RATING");

                entity.Property(e => e.TestimonialStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TESTIMONIAL_STATUS");

                entity.Property(e => e.UseraccountId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USERACCOUNT_ID");

                entity.HasOne(d => d.Useraccount)
                    .WithMany(p => p.Testimonials)
                    .HasForeignKey(d => d.UseraccountId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("TEST_USER_FK");
            });

            
            modelBuilder.Entity<Useraccount>(entity =>
            {
                entity.ToTable("USERACCOUNT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE_PATH");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LASTNAME");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.RoleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROLE_ID");

                entity.Property(e => e.Username)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("USERNAME");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Useraccounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("ROLEID_FK");
            });

            modelBuilder.HasSequence("RESTAURANT_SEQ");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

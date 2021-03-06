using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ExampleAPI.Models
{
    public partial class EPROJECTSWEBAPIEXAMPLEAPIEXAMPLEMDFContext : DbContext
    {
        public EPROJECTSWEBAPIEXAMPLEAPIEXAMPLEMDFContext()
        {
        }

        public EPROJECTSWEBAPIEXAMPLEAPIEXAMPLEMDFContext(DbContextOptions<EPROJECTSWEBAPIEXAMPLEAPIEXAMPLEMDFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Chapters> Chapters { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ExampleDBFull");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>(entity =>
            {
                entity.Property(e => e.Published).HasColumnType("date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Chapters>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Chapters)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_Chapters_Books");
            });

            modelBuilder.Entity<Files>(entity =>
            {
                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Url).HasColumnName("url");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("Expiry_date")
                    .HasColumnType("date");

                entity.Property(e => e.Token).IsRequired();

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshToken)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_RefreshToken_User");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(user_name())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

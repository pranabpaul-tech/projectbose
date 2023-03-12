using System;
using ResourcedetailsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ResourcedetailsAPI.DBService
{
    public partial class mydbContext : DbContext
    {
        //public static string ConnectionString { get; set; }
        public mydbContext()
        {
        }

        public mydbContext(DbContextOptions<mydbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Resourcedetail> Resourcedetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException("Custom: Connection to MYSql is not established");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resourcedetail>(entity =>
            {
                entity.ToTable("resourcedetail");

                entity.Property(e => e.Resourcedetailid).HasColumnName("resourcedetailid");

                entity.Property(e => e.Leveldetailid).HasColumnName("leveldetailid");

                entity.Property(e => e.Projectname)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("projectname");

                entity.Property(e => e.Projectowneremail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("projectowneremail");

                entity.Property(e => e.Resourcegroupname)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("resourcegroupname");

                entity.Property(e => e.Subscriptionid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("subscriptionid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

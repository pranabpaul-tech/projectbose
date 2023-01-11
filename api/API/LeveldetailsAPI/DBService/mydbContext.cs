using System;
using LeveldetailsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace LeveldetailsAPI.DBService
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

        public virtual DbSet<Leveldetail> Leveldetails { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException("Custom: Connection to MYSql is not established");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Leveldetail>(entity =>
            {
                entity.ToTable("leveldetail");

                entity.Property(e => e.Leveldetailid).HasColumnName("leveldetailid");

                entity.Property(e => e.Leveldetailname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("leveldetailname");

                entity.Property(e => e.Levelid).HasColumnName("levelid");

                entity.Property(e => e.Sequenceid).HasColumnName("squenceid");

                entity.Property(e => e.Superleveldetailid).HasColumnName("superleveldetailid");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

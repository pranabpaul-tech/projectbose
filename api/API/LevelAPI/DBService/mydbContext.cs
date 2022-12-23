using System;
using LevelAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace LevelAPI.DBService
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

        public virtual DbSet<Level> Levels { get; set; }
        //public virtual DbSet<Leveldetail> Leveldetails { get; set; }
        //public virtual DbSet<Resourcedetail> Resourcedetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException("Custom: Connection to MYSql is not established");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Level>(entity =>
            {
                entity.ToTable("level");

                entity.Property(e => e.Levelid).HasColumnName("levelid");

                entity.Property(e => e.Levelname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("levelname");

                entity.Property(e => e.Superlevelid).HasColumnName("superlevelid");
            });

            

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

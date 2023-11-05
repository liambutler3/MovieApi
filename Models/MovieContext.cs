using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MovieApi.Models;

public partial class MovieContext : DbContext
{
    public MovieContext()
    {
    }

    public MovieContext(DbContextOptions<MovieContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=Movie;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Genre).HasMaxLength(100);
            entity.Property(e => e.OriginalLanguage)
                .HasMaxLength(50)
                .HasColumnName("Original_Language");
            entity.Property(e => e.Overview).HasMaxLength(900);
            entity.Property(e => e.PosterUrl)
                .HasMaxLength(100)
                .HasColumnName("Poster_Url");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("date")
                .HasColumnName("Release_Date");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.VoteAverage).HasColumnName("Vote_Average");
            entity.Property(e => e.VoteCount).HasColumnName("Vote_Count");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

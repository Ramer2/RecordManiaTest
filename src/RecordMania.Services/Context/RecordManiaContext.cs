using Microsoft.EntityFrameworkCore;
using RecordMania.Models.Models;
using Task = RecordMania.Models.Models.Task;

namespace RecordMania.Services.Context;

public partial class RecordManiaContext : DbContext
{
    public RecordManiaContext()
    {
    }

    public RecordManiaContext(DbContextOptions<RecordManiaContext> options)
        : base(options)
    {
    }
    
    public DbSet<Task> Tasks { get; set; }
    
    public DbSet<Language> Languages { get; set; }
    
    public DbSet<Student> Students { get; set; }
    
    public DbSet<Record> Records { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Task");

            entity.HasIndex(e => e.Name).IsUnique();

            entity.ToTable("Task");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(2000)
                .IsRequired();
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Language");

            entity.HasIndex(e => e.Name).IsUnique();

            entity.ToTable("Language");
            
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Student");
            
            entity.HasIndex(e => e.Email).IsUnique();
            
            entity.ToTable("Student");

            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsRequired();
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Record");
            
            entity.ToTable("Record");
            
            entity.Property(entity => entity.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.Property(entity => entity.ExecutionTime);

            entity.HasOne(e => e.Language)
                .WithMany(l => l.Records)
                .HasForeignKey(e => e.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Task)
                .WithMany(t => t.Records)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Student)
                .WithMany(s => s.Records)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        OnModelCreatingPartial(modelBuilder);
    }
    
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
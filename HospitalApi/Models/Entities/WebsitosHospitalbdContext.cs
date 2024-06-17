using Microsoft.EntityFrameworkCore;

namespace HospitalApi.Models.Entities;

public partial class WebsitosHospitalbdContext : DbContext
{
    public WebsitosHospitalbdContext()
    {
    }

    public WebsitosHospitalbdContext(DbContextOptions<WebsitosHospitalbdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Paciente> Paciente { get; set; }

    public virtual DbSet<Sala> Sala { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("paciente");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sala");

            entity.HasIndex(e => e.Doctor, "fk_Sala_Doctor_idx");

            entity.HasIndex(e => e.Paciente, "fk_Sala_Paciente_idx");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Doctor).HasColumnType("int(11)");
            entity.Property(e => e.Estado).HasColumnType("tinyint(4)");
            entity.Property(e => e.NumeroSala).HasMaxLength(45);
            entity.Property(e => e.Paciente).HasColumnType("int(11)");

            entity.HasOne(d => d.DoctorNavigation).WithMany(p => p.Sala)
                .HasForeignKey(d => d.Doctor)
                .HasConstraintName("fk_Sala_Doctor");

            entity.HasOne(d => d.PacienteNavigation).WithMany(p => p.Sala)
                .HasForeignKey(d => d.Paciente)
                .HasConstraintName("fk_Sala_Paciente");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Contraseña).HasMaxLength(128);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Rol).HasColumnType("tinyint(4)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

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

    public virtual DbSet<Estadistica> Estadistica { get; set; }

    public virtual DbSet<Paciente> Paciente { get; set; }

    public virtual DbSet<Sala> Sala { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=65.181.111.21;database=websitos_hospitalbd;user=websitos_hospital;password=k9D*1g5c1", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.7-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Estadistica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("estadistica");

            entity.HasIndex(e => e.IdDoctor, "fk_Estadistica_Doctor_idx");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdDoctor).HasColumnType("int(11)");
            entity.Property(e => e.PacientesAtendidos).HasColumnType("int(11)");
            entity.Property(e => e.TiempoPromedioEspera).HasColumnType("time");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Estadistica)
                .HasForeignKey(d => d.IdDoctor)
                .HasConstraintName("fk_Estadistica_Doctor");
        });

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

﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace personapi_dotnet.Models.Entities
{
    public partial class PersonaDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public PersonaDbContext()
        {
        }

        public PersonaDbContext(DbContextOptions<PersonaDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Estudio> Estudios { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<Profesion> Profesions { get; set; }
        public virtual DbSet<Telefono> Telefonos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estudio>(entity =>
            {
                entity.HasKey(e => new { e.IdProf, e.CcPer }).HasName("PK__estudios__FB3F71A629F9732A");

                entity.ToTable("estudios");

                entity.Property(e => e.IdProf).HasColumnName("id_prof");
                entity.Property(e => e.CcPer).HasColumnName("cc_per");
                entity.Property(e => e.Fecha).HasColumnName("fecha");
                entity.Property(e => e.Univer)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("univer");

                entity.HasOne(d => d.CcPerNavigation).WithMany(p => p.Estudios)
                    .HasForeignKey(d => d.CcPer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__estudios__cc_per__3D5E1FD2");

                entity.HasOne(d => d.IdProfNavigation).WithMany(p => p.Estudios)
                    .HasForeignKey(d => d.IdProf)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__estudios__id_pro__3C69FB99");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.Cc).HasName("PK__persona__3213666DC1F95D6D");

                entity.ToTable("persona");

                entity.Property(e => e.Cc)
                    .ValueGeneratedNever()
                    .HasColumnName("cc");
                entity.Property(e => e.Apellido)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("apellido");
                entity.Property(e => e.Edad).HasColumnName("edad");
                entity.Property(e => e.Genero)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("genero");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Profesion>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__profesio__3213E83F47440D2D");

                entity.ToTable("profesion");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Des)
                    .HasColumnType("text")
                    .HasColumnName("des");
                entity.Property(e => e.Nom)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("nom");
            });

            modelBuilder.Entity<Telefono>(entity =>
            {
                entity.HasKey(e => e.Num).HasName("PK__telefono__DF908D6525F826DF");

                entity.ToTable("telefono");

                entity.Property(e => e.Num)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("num");
                entity.Property(e => e.Dueno).HasColumnName("dueno");
                entity.Property(e => e.Oper)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("oper");

                entity.HasOne(d => d.DuenoNavigation).WithMany(p => p.Telefonos)
                    .HasForeignKey(d => d.Dueno)
                    .HasConstraintName("FK__telefono__dueno__403A8C7D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

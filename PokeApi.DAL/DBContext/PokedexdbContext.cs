using System;
using System.Collections.Generic;
using PokeApi.Model;
using Microsoft.EntityFrameworkCore;
using PokeApi.Model.Album;
using PokeApi.Model.Sticker;
using PikeApi.DTO;
using PokeApi.Model.Menu;
using PokeApi.Model.PokeApiClasses;
using PokeApi.Model.Exchange;

namespace PokeApi.DAL.DBContext;

public partial class PokedexdbContext : DbContext
{
    public PokedexdbContext()
    {
    }

    public PokedexdbContext(DbContextOptions<PokedexdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuRol> MenuRols { get; set; }

    public virtual DbSet<Pokemon> Pokemons { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Sticker_DTO> Stickers { get; set; }

    public virtual DbSet<AlbumBase> AlbumBases { get; set; }
    public virtual DbSet<Exchanges> Exchange { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__menu__C26AF483BFFF3911");

            entity.ToTable("menu");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.icon)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("icon");
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.url)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("url");
        });

        modelBuilder.Entity<AlbumBase>(entity =>
        {
            entity.ToTable("albumBase");
            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.version).HasColumnName("version");
            entity.Property(e => e.pokemonStart).HasColumnName("pokemonStart");
            entity.Property(e => e.pokemonEnd).HasColumnName("pokemonEnd");
            entity.Property(e => e.registerDate).HasColumnName("registerDate");
        });

        modelBuilder.Entity<MenuRol>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__menuRol__9D6D61A4648E22DF");

            entity.ToTable("menuRol");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.idMenu).HasColumnName("idMenu");
            entity.Property(e => e.idRol).HasColumnName("idRol");

            entity.HasOne(d => d.idMenuNavigation).WithMany(p => p.menuRols)
                .HasForeignKey(d => d.idMenu)
                .HasConstraintName("FK__menuRol__idMenu__5070F446");

            entity.HasOne(d => d.idRolNavigation).WithMany(p => p.menuRols)
                .HasForeignKey(d => d.idRol)
                .HasConstraintName("FK__menuRol__idRol__5165187F");
        });

        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.HasKey(e => e.IdPokemon).HasName("PK__pokemon__653EBD85995AA557");

            entity.ToTable("pokemon");

            entity.Property(e => e.IdPokemon).HasColumnName("id");
            entity.Property(e => e.Evolution2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("evolution2");
            entity.Property(e => e.Evolution3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("evolution3");
            entity.Property(e => e.IdPokemonApi).HasColumnName("idPokemonApi");
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");

        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__rol__3C872F7699A32DEC");

            entity.ToTable("rol");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.registerDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registerDate");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__users__3717C982EA70D20B");

            entity.ToTable("user");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.idRol).HasColumnName("idRol");
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.password)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.registerDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registerDate");
            entity.Property(e => e.secondName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("secondName");
            entity.Property(e => e.status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");

            entity.HasOne(d => d.idRolNavigation).WithMany(p => p.users)
                .HasForeignKey(d => d.idRol)
                .HasConstraintName("FK__users__idRol__403A8C7D");
        });

        modelBuilder.Entity<Stickers>(entity =>
        {
            entity.ToTable("sticker");
            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.idPokemon).HasColumnName("idPokemon");
            entity.Property(e => e.idUser).HasColumnName("idUser");
            entity.Property(e => e.status).HasColumnName("status");
            entity.Property(e => e.version).HasColumnName("version");
            entity.Property(e => e.lastModification).HasColumnName("lastModification");
            entity.Property(e => e.registerDate).HasColumnName("registerDate");

        });

        modelBuilder.Entity<Exchanges>(entity =>
        {
            entity.ToTable("exchange");
            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.idExchange).HasColumnName("idExchange");
            entity.Property(e => e.idReceivingUser).HasColumnName("idReceivingUser");
            entity.Property(e => e.idSenderUser).HasColumnName("idSenderUser");
            entity.Property(e => e.status).HasColumnName("status");
            entity.Property(e => e.idSticker).HasColumnName("idSticker");
            entity.Property(e => e.lastModification).HasColumnName("lastModification");
            entity.Property(e => e.registerDate).HasColumnName("registerDate");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

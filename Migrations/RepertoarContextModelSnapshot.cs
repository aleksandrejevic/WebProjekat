// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;

namespace Repertoar.Migrations
{
    [DbContext(typeof(RepertoarContext))]
    partial class RepertoarContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Models.Bioskop", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Mesto")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Bioskop");
                });

            modelBuilder.Entity("Models.Datum", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DatumPrikazivanja")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Datumi");
                });

            modelBuilder.Entity("Models.Film", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Godina")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OriginalniNaziv")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Reziser")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Trajanje")
                        .HasColumnType("int");

                    b.Property<string>("Uloge")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Zanr")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ZemljaPorekla")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Filmovi");
                });

            modelBuilder.Entity("Models.PocetakPrikazivanja", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("DatumID")
                        .HasColumnType("int");

                    b.Property<string>("Sati")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("DatumID");

                    b.ToTable("PoceciPrikazivanja");
                });

            modelBuilder.Entity("Models.Spoj", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("BioskopID")
                        .HasColumnType("int");

                    b.Property<int?>("FilmID")
                        .HasColumnType("int");

                    b.Property<int?>("PocetakPrikazivanjaID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("BioskopID");

                    b.HasIndex("FilmID");

                    b.HasIndex("PocetakPrikazivanjaID");

                    b.ToTable("BioskopiFilmovi");
                });

            modelBuilder.Entity("Models.PocetakPrikazivanja", b =>
                {
                    b.HasOne("Models.Datum", "Datum")
                        .WithMany()
                        .HasForeignKey("DatumID");

                    b.Navigation("Datum");
                });

            modelBuilder.Entity("Models.Spoj", b =>
                {
                    b.HasOne("Models.Bioskop", "Bioskop")
                        .WithMany("BioskopFilm")
                        .HasForeignKey("BioskopID");

                    b.HasOne("Models.Film", "Film")
                        .WithMany("FilmBioskop")
                        .HasForeignKey("FilmID");

                    b.HasOne("Models.PocetakPrikazivanja", "PocetakPrikazivanja")
                        .WithMany("BioskopiFilmovi")
                        .HasForeignKey("PocetakPrikazivanjaID");

                    b.Navigation("Bioskop");

                    b.Navigation("Film");

                    b.Navigation("PocetakPrikazivanja");
                });

            modelBuilder.Entity("Models.Bioskop", b =>
                {
                    b.Navigation("BioskopFilm");
                });

            modelBuilder.Entity("Models.Film", b =>
                {
                    b.Navigation("FilmBioskop");
                });

            modelBuilder.Entity("Models.PocetakPrikazivanja", b =>
                {
                    b.Navigation("BioskopiFilmovi");
                });
#pragma warning restore 612, 618
        }
    }
}

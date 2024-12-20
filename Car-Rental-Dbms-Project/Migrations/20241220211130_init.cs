using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Car_Rental_Dbms_Project.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adresler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sokak = table.Column<string>(type: "text", nullable: false),
                    Sehir = table.Column<string>(type: "text", nullable: false),
                    Ilce = table.Column<string>(type: "text", nullable: false),
                    PostaKodu = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adresler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "arac_kategorileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KategoriAd = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arac_kategorileri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "calisanlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Soyad = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false),
                    AdresId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calisanlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_calisanlar_adresler_AdresId",
                        column: x => x.AdresId,
                        principalTable: "adresler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "musteriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Soyad = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Telefon = table.Column<string>(type: "text", nullable: false),
                    AdresId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musteriler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_musteriler_adresler_AdresId",
                        column: x => x.AdresId,
                        principalTable: "adresler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "araclar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Marka = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Yil = table.Column<int>(type: "integer", nullable: false),
                    KategoriId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_araclar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_araclar_arac_kategorileri_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "arac_kategorileri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "arac_istatistikleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AracId = table.Column<int>(type: "integer", nullable: false),
                    KiralamaSayisi = table.Column<int>(type: "integer", nullable: false),
                    ToplamKilometre = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arac_istatistikleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_arac_istatistikleri_araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "arac_tuketim_bilgileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AracId = table.Column<int>(type: "integer", nullable: false),
                    YakitTuru = table.Column<string>(type: "text", nullable: false),
                    YakitTuketimi = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arac_tuketim_bilgileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_arac_tuketim_bilgileri_araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "binek_araclar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    AracId = table.Column<int>(type: "integer", nullable: false),
                    BagajHacmi = table.Column<decimal>(type: "numeric", nullable: false),
                    AracId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_binek_araclar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_binek_araclar_araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_binek_araclar_araclar_AracId1",
                        column: x => x.AracId1,
                        principalTable: "araclar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_binek_araclar_araclar_Id",
                        column: x => x.Id,
                        principalTable: "araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kiralamalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MusteriId = table.Column<int>(type: "integer", nullable: false),
                    AracId = table.Column<int>(type: "integer", nullable: false),
                    KiralamaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KiralamaBitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kiralamalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_kiralamalar_araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_kiralamalar_musteriler_MusteriId",
                        column: x => x.MusteriId,
                        principalTable: "musteriler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ticari_araclar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    AracId = table.Column<int>(type: "integer", nullable: false),
                    YukKapasitesi = table.Column<int>(type: "integer", nullable: false),
                    AracId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticari_araclar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ticari_araclar_araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ticari_araclar_araclar_AracId1",
                        column: x => x.AracId1,
                        principalTable: "araclar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ticari_araclar_araclar_Id",
                        column: x => x.Id,
                        principalTable: "araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "faturalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KiralamaId = table.Column<int>(type: "integer", nullable: false),
                    Tutar = table.Column<decimal>(type: "numeric", nullable: false),
                    FaturaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faturalar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_faturalar_kiralamalar_KiralamaId",
                        column: x => x.KiralamaId,
                        principalTable: "kiralamalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "odemeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FaturaId = table.Column<int>(type: "integer", nullable: false),
                    OdemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OdemeTutar = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_odemeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_odemeler_faturalar_FaturaId",
                        column: x => x.FaturaId,
                        principalTable: "faturalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_arac_istatistikleri_AracId",
                table: "arac_istatistikleri",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_arac_tuketim_bilgileri_AracId",
                table: "arac_tuketim_bilgileri",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_araclar_KategoriId",
                table: "araclar",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_binek_araclar_AracId",
                table: "binek_araclar",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_binek_araclar_AracId1",
                table: "binek_araclar",
                column: "AracId1");

            migrationBuilder.CreateIndex(
                name: "IX_calisanlar_AdresId",
                table: "calisanlar",
                column: "AdresId");

            migrationBuilder.CreateIndex(
                name: "IX_faturalar_KiralamaId",
                table: "faturalar",
                column: "KiralamaId");

            migrationBuilder.CreateIndex(
                name: "IX_kiralamalar_AracId",
                table: "kiralamalar",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_kiralamalar_MusteriId",
                table: "kiralamalar",
                column: "MusteriId");

            migrationBuilder.CreateIndex(
                name: "IX_musteriler_AdresId",
                table: "musteriler",
                column: "AdresId");

            migrationBuilder.CreateIndex(
                name: "IX_odemeler_FaturaId",
                table: "odemeler",
                column: "FaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_ticari_araclar_AracId",
                table: "ticari_araclar",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_ticari_araclar_AracId1",
                table: "ticari_araclar",
                column: "AracId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "arac_istatistikleri");

            migrationBuilder.DropTable(
                name: "arac_tuketim_bilgileri");

            migrationBuilder.DropTable(
                name: "binek_araclar");

            migrationBuilder.DropTable(
                name: "calisanlar");

            migrationBuilder.DropTable(
                name: "odemeler");

            migrationBuilder.DropTable(
                name: "ticari_araclar");

            migrationBuilder.DropTable(
                name: "faturalar");

            migrationBuilder.DropTable(
                name: "kiralamalar");

            migrationBuilder.DropTable(
                name: "araclar");

            migrationBuilder.DropTable(
                name: "musteriler");

            migrationBuilder.DropTable(
                name: "arac_kategorileri");

            migrationBuilder.DropTable(
                name: "adresler");
        }
    }
}

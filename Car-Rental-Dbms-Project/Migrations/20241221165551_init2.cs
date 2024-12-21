using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Car_Rental_Dbms_Project.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ilce",
                table: "adresler");

            migrationBuilder.DropColumn(
                name: "PostaKodu",
                table: "adresler");

            migrationBuilder.DropColumn(
                name: "Sehir",
                table: "adresler");

            migrationBuilder.DropColumn(
                name: "Sokak",
                table: "adresler");

            migrationBuilder.RenameColumn(
                name: "KiralamaSayisi",
                table: "arac_istatistikleri",
                newName: "BeygirGucu");

            migrationBuilder.AddColumn<int>(
                name: "IlceId",
                table: "adresler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MahalleId",
                table: "adresler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PostaKoduId",
                table: "adresler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SehirId",
                table: "adresler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SokakId",
                table: "adresler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ilceler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IlceAd = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ilceler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mahalleler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MahalleAd = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mahalleler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "postaKodlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostaKoduAd = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_postaKodlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sehirler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SehirAd = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sehirler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sokaklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SokakAd = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sokaklar", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_adresler_IlceId",
                table: "adresler",
                column: "IlceId");

            migrationBuilder.CreateIndex(
                name: "IX_adresler_MahalleId",
                table: "adresler",
                column: "MahalleId");

            migrationBuilder.CreateIndex(
                name: "IX_adresler_PostaKoduId",
                table: "adresler",
                column: "PostaKoduId");

            migrationBuilder.CreateIndex(
                name: "IX_adresler_SehirId",
                table: "adresler",
                column: "SehirId");

            migrationBuilder.CreateIndex(
                name: "IX_adresler_SokakId",
                table: "adresler",
                column: "SokakId");

            migrationBuilder.AddForeignKey(
                name: "FK_adresler_ilceler_IlceId",
                table: "adresler",
                column: "IlceId",
                principalTable: "ilceler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_adresler_mahalleler_MahalleId",
                table: "adresler",
                column: "MahalleId",
                principalTable: "mahalleler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_adresler_postaKodlari_PostaKoduId",
                table: "adresler",
                column: "PostaKoduId",
                principalTable: "postaKodlari",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_adresler_sehirler_SehirId",
                table: "adresler",
                column: "SehirId",
                principalTable: "sehirler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_adresler_sokaklar_SokakId",
                table: "adresler",
                column: "SokakId",
                principalTable: "sokaklar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adresler_ilceler_IlceId",
                table: "adresler");

            migrationBuilder.DropForeignKey(
                name: "FK_adresler_mahalleler_MahalleId",
                table: "adresler");

            migrationBuilder.DropForeignKey(
                name: "FK_adresler_postaKodlari_PostaKoduId",
                table: "adresler");

            migrationBuilder.DropForeignKey(
                name: "FK_adresler_sehirler_SehirId",
                table: "adresler");

            migrationBuilder.DropForeignKey(
                name: "FK_adresler_sokaklar_SokakId",
                table: "adresler");

            migrationBuilder.DropTable(
                name: "ilceler");

            migrationBuilder.DropTable(
                name: "mahalleler");

            migrationBuilder.DropTable(
                name: "postaKodlari");

            migrationBuilder.DropTable(
                name: "sehirler");

            migrationBuilder.DropTable(
                name: "sokaklar");

            migrationBuilder.DropIndex(
                name: "IX_adresler_IlceId",
                table: "adresler");

            migrationBuilder.DropIndex(
                name: "IX_adresler_MahalleId",
                table: "adresler");

            migrationBuilder.DropIndex(
                name: "IX_adresler_PostaKoduId",
                table: "adresler");

            migrationBuilder.DropIndex(
                name: "IX_adresler_SehirId",
                table: "adresler");

            migrationBuilder.DropIndex(
                name: "IX_adresler_SokakId",
                table: "adresler");

            migrationBuilder.DropColumn(
                name: "IlceId",
                table: "adresler");

            migrationBuilder.DropColumn(
                name: "MahalleId",
                table: "adresler");

            migrationBuilder.DropColumn(
                name: "PostaKoduId",
                table: "adresler");

            migrationBuilder.DropColumn(
                name: "SehirId",
                table: "adresler");

            migrationBuilder.DropColumn(
                name: "SokakId",
                table: "adresler");

            migrationBuilder.RenameColumn(
                name: "BeygirGucu",
                table: "arac_istatistikleri",
                newName: "KiralamaSayisi");

            migrationBuilder.AddColumn<string>(
                name: "Ilce",
                table: "adresler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostaKodu",
                table: "adresler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sehir",
                table: "adresler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sokak",
                table: "adresler",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

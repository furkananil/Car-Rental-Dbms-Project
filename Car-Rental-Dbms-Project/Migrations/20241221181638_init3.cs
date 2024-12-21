using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Car_Rental_Dbms_Project.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_binek_araclar_araclar_AracId1",
                table: "binek_araclar");

            migrationBuilder.DropForeignKey(
                name: "FK_ticari_araclar_araclar_AracId1",
                table: "ticari_araclar");

            migrationBuilder.DropIndex(
                name: "IX_ticari_araclar_AracId1",
                table: "ticari_araclar");

            migrationBuilder.DropIndex(
                name: "IX_binek_araclar_AracId1",
                table: "binek_araclar");

            migrationBuilder.DropColumn(
                name: "AracId1",
                table: "ticari_araclar");

            migrationBuilder.DropColumn(
                name: "AracId1",
                table: "binek_araclar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AracId1",
                table: "ticari_araclar",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AracId1",
                table: "binek_araclar",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ticari_araclar_AracId1",
                table: "ticari_araclar",
                column: "AracId1");

            migrationBuilder.CreateIndex(
                name: "IX_binek_araclar_AracId1",
                table: "binek_araclar",
                column: "AracId1");

            migrationBuilder.AddForeignKey(
                name: "FK_binek_araclar_araclar_AracId1",
                table: "binek_araclar",
                column: "AracId1",
                principalTable: "araclar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ticari_araclar_araclar_AracId1",
                table: "ticari_araclar",
                column: "AracId1",
                principalTable: "araclar",
                principalColumn: "Id");
        }
    }
}

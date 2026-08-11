using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OgrenciBilgiSistemi.Migrations
{
    /// <inheritdoc />
    public partial class OnKosulKolonuEkle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "on_kosul_ders_id",
                table: "dersler",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DersId",
                table: "DersKayitlari",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Durum",
                table: "DersKayitlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OgrenciMaliDurumlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OgrenciNo = table.Column<string>(type: "text", nullable: false),
                    HarcBorcu = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OgrenciMaliDurumlari", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DersKayitlari_DersId",
                table: "DersKayitlari",
                column: "DersId");

            migrationBuilder.AddForeignKey(
                name: "FK_DersKayitlari_dersler_DersId",
                table: "DersKayitlari",
                column: "DersId",
                principalTable: "dersler",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DersKayitlari_dersler_DersId",
                table: "DersKayitlari");

            migrationBuilder.DropTable(
                name: "OgrenciMaliDurumlari");

            migrationBuilder.DropIndex(
                name: "IX_DersKayitlari_DersId",
                table: "DersKayitlari");

            migrationBuilder.DropColumn(
                name: "on_kosul_ders_id",
                table: "dersler");

            migrationBuilder.DropColumn(
                name: "DersId",
                table: "DersKayitlari");

            migrationBuilder.DropColumn(
                name: "Durum",
                table: "DersKayitlari");
        }
    }
}

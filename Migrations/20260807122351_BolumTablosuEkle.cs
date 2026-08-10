using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OgrenciBilgiSistemi.Migrations
{
    /// <inheritdoc />
    public partial class BolumTablosuEkle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "mezuniyet_icin_gerekli_kredi",
                table: "ogrenciler",
                newName: "bolum_id");

            migrationBuilder.CreateTable(
                name: "bolumler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bolum_adi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    mezuniyet_icin_gerekli_kredi = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bolumler", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ogrenciler_bolum_id",
                table: "ogrenciler",
                column: "bolum_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ogrenciler_bolumler_bolum_id",
                table: "ogrenciler",
                column: "bolum_id",
                principalTable: "bolumler",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ogrenciler_bolumler_bolum_id",
                table: "ogrenciler");

            migrationBuilder.DropTable(
                name: "bolumler");

            migrationBuilder.DropIndex(
                name: "IX_ogrenciler_bolum_id",
                table: "ogrenciler");

            migrationBuilder.RenameColumn(
                name: "bolum_id",
                table: "ogrenciler",
                newName: "mezuniyet_icin_gerekli_kredi");
        }
    }
}

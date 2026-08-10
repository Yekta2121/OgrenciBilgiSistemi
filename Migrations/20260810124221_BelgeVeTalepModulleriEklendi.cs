using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OgrenciBilgiSistemi.Migrations
{
    /// <inheritdoc />
    public partial class BelgeVeTalepModulleriEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BelgeTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OgrenciNo = table.Column<string>(type: "text", nullable: false),
                    BelgeTuru = table.Column<string>(type: "text", nullable: false),
                    BarkodKodu = table.Column<string>(type: "text", nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Durum = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BelgeTalepleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DilekceBasvurulari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OgrenciNo = table.Column<string>(type: "text", nullable: false),
                    BasvuruTuru = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: false),
                    BasvuruTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OnayDurumu = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DilekceBasvurulari", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BelgeTalepleri");

            migrationBuilder.DropTable(
                name: "DilekceBasvurulari");
        }
    }
}

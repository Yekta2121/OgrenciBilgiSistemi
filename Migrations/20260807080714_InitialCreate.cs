using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OgrenciBilgiSistemi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dersler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ders_kodu = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ders_adi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    teori_saat = table.Column<int>(type: "integer", nullable: false),
                    uygulama_saat = table.Column<int>(type: "integer", nullable: false),
                    yerel_kredi = table.Column<int>(type: "integer", nullable: false),
                    akts = table.Column<int>(type: "integer", nullable: false),
                    yariyil = table.Column<int>(type: "integer", nullable: false),
                    zorunlu_mu = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dersler", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ogrenciler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tc_kimlik_no = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    ogrenci_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    soyad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    kurumsal_eposta = table.Column<string>(type: "text", nullable: false),
                    sinif = table.Column<int>(type: "integer", nullable: false),
                    kayit_turu = table.Column<string>(type: "text", nullable: false),
                    durum_aktif_mi = table.Column<bool>(type: "boolean", nullable: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ogrenciler", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ogrenci_notlari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ogrenci_id = table.Column<int>(type: "integer", nullable: false),
                    ders_id = table.Column<int>(type: "integer", nullable: false),
                    vize_notu = table.Column<double>(type: "double precision", nullable: true),
                    final_notu = table.Column<double>(type: "double precision", nullable: true),
                    butunleme_notu = table.Column<double>(type: "double precision", nullable: true),
                    harf_notu_katsayisi = table.Column<double>(type: "double precision", nullable: false),
                    harf_notu = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    gecti_mi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ogrenci_notlari", x => x.id);
                    table.ForeignKey(
                        name: "FK_ogrenci_notlari_dersler_ders_id",
                        column: x => x.ders_id,
                        principalTable: "dersler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ogrenci_notlari_ogrenciler_ogrenci_id",
                        column: x => x.ogrenci_id,
                        principalTable: "ogrenciler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ogrenci_notlari_ders_id",
                table: "ogrenci_notlari",
                column: "ders_id");

            migrationBuilder.CreateIndex(
                name: "IX_ogrenci_notlari_ogrenci_id",
                table: "ogrenci_notlari",
                column: "ogrenci_id");

            migrationBuilder.CreateIndex(
                name: "ux_ogrenciler_ogrenci_no",
                table: "ogrenciler",
                column: "ogrenci_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_ogrenciler_tc_kimlik_no",
                table: "ogrenciler",
                column: "tc_kimlik_no",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ogrenci_notlari");

            migrationBuilder.DropTable(
                name: "dersler");

            migrationBuilder.DropTable(
                name: "ogrenciler");
        }
    }
}

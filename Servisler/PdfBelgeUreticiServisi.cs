using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OgrenciBilgiSistemi.Servisler
{
    public class PdfBelgeUreticiServisi
    {
        public PdfBelgeUreticiServisi()
        {
            // QuestPDF topluluk lisans ayarı
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] BelgePdfUret(string ogrenciNo, string belgeTuru, string barkodKodu)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    // Header (Kurumsal Başlık)
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("T.C. BİLGİ İŞLEM MERKEZİ").Bold().FontSize(14).FontColor(Colors.Blue.Medium);
                            col.Item().Text("ÖĞRENCİ İŞLERİ DAİRE BAŞKANLIĞI").Bold().FontSize(12);
                            col.Item().Text("Resmi E-İmzalı Belge Doğrulama Sistemi").FontSize(9).FontColor(Colors.Grey.Medium);
                        });
                    });

                    // Content (İçerik)
                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
                    {
                        col.Item().AlignCenter().Text(belgeTuru.ToUpper()).Bold().FontSize(16);
                        col.Item().PaddingTop(20).Text($"İlgili makama,");
                        col.Item().PaddingTop(10).Text($"Aşağıdaki bilgileri yer alan öğrencimizin kurumumuz kayıtlarında aktif olarak kaydı bulunmaktadır. Bu belge elektronik olarak imzalanmıştır.");

                        col.Item().PaddingVertical(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);
                                columns.RelativeColumn();
                            });

                            table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).Text("Öğrenci Numarası:").Bold();
                            table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).Text(ogrenciNo);

                            table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).Text("Belge Türü:").Bold();
                            table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).Text(belgeTuru);

                            table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).Text("Veriliş Tarihi:").Bold();
                            table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).Text(DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm"));

                            table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).Text("Doğrulama Kodu:").Bold();
                            table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).Text(barkodKodu).Bold().FontColor(Colors.Red.Medium);
                        });

                        col.Item().PaddingTop(15).Text("Bu belge 5070 sayılı Elektronik İmza Kanunu gereğince güvenli elektronik imza ile imzalanmıştır. Belge doğrulama kodu ile sistem üzerinden teyit edilebilir.");
                    });

                    // Footer (Alt Bilgi ve Barkod Alanı)
                    page.Footer().AlignCenter().Column(col =>
                    {
                        col.Item().LineHorizontal(1).LineColor("#CCCCCC");
                        col.Item().PaddingTop(5).Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                        col.Item().Text($"Güvenli Doğrulama Barkodu: {barkodKodu}").FontSize(9).Italic();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
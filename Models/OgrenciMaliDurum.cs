namespace OgrenciBilgiSistemi.Models
{
    public class OgrenciMaliDurum
    {
        public int Id { get; set; }
        public string OgrenciNo { get; set; }
        public decimal HarcBorcu { get; set; } = 0; // Borç varsa ders seçimi bloke edilir
    }
}
namespace Guzellik.Models
{
    public class Uzman
    {
        public int Id { get; set; }  // Veritabanı için benzersiz ID
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Islem { get; set; }
        public decimal Maas { get; set; }

        public TimeSpan CalismaBaslangicSaati { get; set; }
        public TimeSpan CalismaBitisSaati { get; set; }

        public Uzman() { }

        // Uzman constructor'ı, gerekli tüm alanları alacak şekilde düzenlendi
        public Uzman(int id, string ad, string soyad, string islem, decimal maas, TimeSpan calismaBaslangicSaati, TimeSpan calismaBitisSaati)
        {
            Id = id;
            Ad = ad;
            Soyad = soyad;
            Islem = islem;
            Maas = maas;
            CalismaBaslangicSaati = calismaBaslangicSaati;
            CalismaBitisSaati = calismaBitisSaati;
        }
    }
}

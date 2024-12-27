using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Guzellik.Models
{
    public class Randevu
    {
        [Key]
        public int RandevuID { get; set; }


        [Required]
        [DisplayName("Müşteri Ad")]
        public string MusteriAd { get; set; }


        [Required]
        [DisplayName("Müşteri Soyad")]
        public string MusteriSoyad { get; set; }

        [Required]
        [DisplayName("Uzman Ad")]
        public string UzmanAd { get; set; }



        [Required]
        [DisplayName("İşlem Ad")]
        public string IslemAd { get; set; }


        [Required]
        [DisplayName("İşlem Süre")]
        public int IslemSure { get; set; }

        [Required]
        [DisplayName("İşlem Ucret")]
        public int IslemUcret { get; set; }

        [Required]
        [DisplayName("İşlem Tarih")]
        public DateTime IslemTarih { get; set; } = DateTime.Now;

      

        [Required]
        [DisplayName("İşlem Onay")]
        public bool Onaylandi { get; set; } // Randevu onay durumu

    }
}



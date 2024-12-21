using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental_Dbms_Project
{
    public class BaseEntity
    {
        public int Id { get; set; }
    }
    public class Musteri : BaseEntity
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public int AdresId { get; set; }

        // Navigation property
        public Adres Adres { get; set; }
    }
    public class Calisan : BaseEntity
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public int AdresId { get; set; }

        // Navigation property
        public Adres Adres { get; set; }
    }
    public class Arac : BaseEntity
    {
        public string Marka { get; set; }
        public string Model { get; set; }
        public int Yil { get; set; }
        public int KategoriId { get; set; }

        // Navigation properties
        public Arac_Kategorileri Kategori { get; set; }

        // Yeni koleksiyonlar ekliyoruz
    }

    public class TicariArac : Arac
    {
        public int AracId { get; set; }
        public int YukKapasitesi { get; set; }

        // Arac navigation property is inherited from Arac class

    }

    public class BinekArac : Arac
    {
        public int AracId { get; set; }
        public decimal BagajHacmi { get; set; }

        // Arac navigation property is inherited from Arac class
    }
    public class Kiralama : BaseEntity
    {
        public int MusteriId { get; set; }
        public int AracId { get; set; }
        public DateTime KiralamaTarihi { get; set; }
        public DateTime KiralamaBitisTarihi { get; set; }

        // Navigation properties
        public Musteri Musteri { get; set; }
        public Arac Arac { get; set; }
    }
    public class Fatura : BaseEntity
    {
        public int KiralamaId { get; set; }
        public decimal Tutar { get; set; }
        public DateTime FaturaTarihi { get; set; }

        // Navigation property
        public Kiralama Kiralama { get; set; }
    }
    public class Odeme : BaseEntity
    {
        public int FaturaId { get; set; }
        public DateTime OdemeTarihi { get; set; }
        public decimal OdemeTutar { get; set; }

        // Navigation property
        public Fatura Fatura { get; set; }
    }
    public class Sokak : BaseEntity
    {
        public string SokakAd { get; set; }

        // Navigation property
        public ICollection<Adres> Adresler { get; set; }
    }

    public class Sehir : BaseEntity
    {
        public string SehirAd { get; set; }

        // Navigation property
        public ICollection<Adres> Adresler { get; set; }
    }

    public class Ilce : BaseEntity
    {
        public string IlceAd { get; set; }

        // Navigation property
        public ICollection<Adres> Adresler { get; set; }
    }

    public class PostaKodu : BaseEntity
    {
        public string PostaKoduAd { get; set; }

        // Navigation property
        public ICollection<Adres> Adresler { get; set; }
    }
    public class Mahalle : BaseEntity
    {
        public string MahalleAd { get; set; }

        // Navigation property
        public ICollection<Adres> Adresler { get; set; }
    }
    public class Adres : BaseEntity
    {
        public int SokakId { get; set; }
        public int SehirId { get; set; }
        public int IlceId { get; set; }
        public int PostaKoduId { get; set; }
        public int MahalleId { get; set; }  // Yeni MahalleId alanı eklendi

        // Navigation properties
        public Sokak Sokak { get; set; }
        public Sehir Sehir { get; set; }
        public Ilce Ilce { get; set; }
        public PostaKodu PostaKodu { get; set; }
        public Mahalle Mahalle { get; set; }  // Yeni Mahalle ile ilişki
    }
    public class Arac_Kategorileri : BaseEntity
    {
        public string KategoriAd { get; set; }
    }
    public class Arac_Tuketim_Bilgileri : BaseEntity
    {
        public int AracId { get; set; }
        public string YakitTuru { get; set; }
        public decimal YakitTuketimi { get; set; }

        // Navigation property
        public Arac Arac { get; set; }
    }
    public class Arac_Istatistikleri : BaseEntity
    {
        public int AracId { get; set; }
        public int BeygirGucu { get; set; }
        public int ToplamKilometre { get; set; }

        // Navigation property
        public Arac Arac { get; set; }
    }
}

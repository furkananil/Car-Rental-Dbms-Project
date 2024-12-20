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
        public ICollection<BinekArac> BinekAraclar { get; set; }  // Binek araçlar
        public ICollection<TicariArac> TicariAraclar { get; set; }  // Ticari araçlar
    }

    public class TicariArac : Arac
    {
        public int AracId { get; set; }
        public int YukKapasitesi { get; set; }

        // Arac navigation property is inherited from Arac class
        public virtual Arac Arac { get; set; }
    }

    public class BinekArac : Arac
    {
        public int AracId { get; set; }
        public decimal BagajHacmi { get; set; }

        // Arac navigation property is inherited from Arac class
        public virtual Arac Arac { get; set; }
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
    public class Adres : BaseEntity
    {
        public string Sokak { get; set; }
        public string Sehir { get; set; }
        public string Ilce { get; set; }
        public string PostaKodu { get; set; }
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
        public int KiralamaSayisi { get; set; }
        public int ToplamKilometre { get; set; }

        // Navigation property
        public Arac Arac { get; set; }
    }
}

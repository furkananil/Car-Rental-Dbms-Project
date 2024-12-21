using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental_Dbms_Project
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Sokak> sokaklar { get; set; }
        public DbSet<Sehir> sehirler { get; set; }
        public DbSet<Ilce> ilceler { get; set; }
        public DbSet<PostaKodu> postaKodlari { get; set; }
        public DbSet<Mahalle> mahalleler { get; set; }  // Mahalle tablosu eklendi

        public DbSet<Musteri> musteriler { get; set; }
        public DbSet<Calisan> calisanlar { get; set; }
        public DbSet<Arac> araclar { get; set; }
        public DbSet<BinekArac> binek_araclar { get; set; }
        public DbSet<TicariArac> ticari_araclar { get; set; }
        public DbSet<Kiralama> kiralamalar { get; set; }
        public DbSet<Fatura> faturalar { get; set; }
        public DbSet<Odeme> odemeler { get; set; }
        public DbSet<Adres> adresler { get; set; }
        public DbSet<Arac_Kategorileri> arac_kategorileri { get; set; }
        public DbSet<Arac_Tuketim_Bilgileri> arac_tuketim_bilgileri { get; set; }
        public DbSet<Arac_Istatistikleri> arac_istatistikleri { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=furkanbabadb0;Database=CarRental");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Arac sınıfı için temel tablo adı
            modelBuilder.Entity<Arac>().ToTable("araclar");

            // BinekArac ve TicariArac sınıfları için ayrı tablolar
            modelBuilder.Entity<BinekArac>().ToTable("binek_araclar");
            modelBuilder.Entity<TicariArac>().ToTable("ticari_araclar");

            // BinekArac ve TicariArac sınıflarının Arac sınıfı ile ilişkisini belirtmek
            modelBuilder.Entity<BinekArac>()
                .HasBaseType<Arac>();  // TPT stratejisi için

            modelBuilder.Entity<TicariArac>()
                .HasBaseType<Arac>();  // TPT stratejisi için

            modelBuilder.Entity<TicariArac>()
                 .HasOne<Arac>()  // TicariArac'ın Arac'a sahip olduğunu belirtir
                 .WithMany()  // Arac'ın birçok TicariArac'a sahip olabileceğini belirtir
                 .HasForeignKey(ta => ta.AracId)  // AracId, foreign key olarak kullanılır
                 .OnDelete(DeleteBehavior.Restrict);  // Silme işlemi kısıtlanabilir

            modelBuilder.Entity<BinekArac>()
                .HasOne<Arac>()  // BinekArac ile Arac arasındaki ilişkiyi belirtir
                .WithMany()  // Arac'ın birçok BinekArac'a sahip olabileceğini belirtir
                .HasForeignKey(ba => ba.AracId)  // Foreign key
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Adres>()
            .HasOne(a => a.Sokak)
            .WithMany(s => s.Adresler)
            .HasForeignKey(a => a.SokakId);

            modelBuilder.Entity<Adres>()
                .HasOne(a => a.Sehir)
                .WithMany(s => s.Adresler)
                .HasForeignKey(a => a.SehirId);

            modelBuilder.Entity<Adres>()
                .HasOne(a => a.Ilce)
                .WithMany(i => i.Adresler)
                .HasForeignKey(a => a.IlceId);

            modelBuilder.Entity<Adres>()
                .HasOne(a => a.PostaKodu)
                .WithMany(p => p.Adresler)
                .HasForeignKey(a => a.PostaKoduId);

            modelBuilder.Entity<Adres>()
                .HasOne(a => a.Mahalle)
                .WithMany(m => m.Adresler)
                .HasForeignKey(a => a.MahalleId);

            // Musteri - Adres İlişkisi
            modelBuilder.Entity<Musteri>()
                .HasOne(m => m.Adres)
                .WithMany()
                .HasForeignKey(m => m.AdresId)
                .OnDelete(DeleteBehavior.Cascade);

            // Calisan - Adres İlişkisi
            modelBuilder.Entity<Calisan>()
                .HasOne(c => c.Adres)
                .WithMany()
                .HasForeignKey(c => c.AdresId)
                .OnDelete(DeleteBehavior.Cascade);

            // Kiralama - Musteri İlişkisi
            modelBuilder.Entity<Kiralama>()
                .HasOne(k => k.Musteri)
                .WithMany()
                .HasForeignKey(k => k.MusteriId)
                .OnDelete(DeleteBehavior.Restrict);

            // Kiralama - Arac İlişkisi
            modelBuilder.Entity<Kiralama>()
                .HasOne(k => k.Arac)
                .WithMany()
                .HasForeignKey(k => k.AracId)
                .OnDelete(DeleteBehavior.Restrict);

            // Fatura - Kiralama İlişkisi
            modelBuilder.Entity<Fatura>()
                .HasOne(f => f.Kiralama)
                .WithMany()
                .HasForeignKey(f => f.KiralamaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Odeme - Fatura İlişkisi
            modelBuilder.Entity<Odeme>()
                .HasOne(o => o.Fatura)
                .WithMany()
                .HasForeignKey(o => o.FaturaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Arac - AracKategori İlişkisi
            modelBuilder.Entity<Arac>()
                .HasOne(a => a.Kategori)
                .WithMany()
                .HasForeignKey(a => a.KategoriId)
                .OnDelete(DeleteBehavior.Restrict);

            // Arac_Tuketim_Bilgileri - Arac İlişkisi
            modelBuilder.Entity<Arac_Tuketim_Bilgileri>()
                .HasOne(atb => atb.Arac)
                .WithMany()
                .HasForeignKey(atb => atb.AracId)
                .OnDelete(DeleteBehavior.Cascade);

            // Arac_Istatistikleri - Arac İlişkisi
            modelBuilder.Entity<Arac_Istatistikleri>()
                .HasOne(ai => ai.Arac)
                .WithMany()
                .HasForeignKey(ai => ai.AracId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental_Dbms_Project
{
    public class ApplicationDbContext
    {
        public class ApplicationDbContext : DbContext
        {
            public DbSet<MusteriGiris> musteri { get; set; }
            public DbSet<Calisan> calisan { get; set; }
            public DbSet<Arac> arac { get; set; }
            public DbSet<BinekArac> binekAraclar { get; set; }
            public DbSet<TicariArac> ticariAraclar { get; set; }
            public DbSet<Kiralama> kiralamalar { get; set; }
            public DbSet<Fatura> faturalar { get; set; }
            public DbSet<Odeme> odeme { get; set; }
            public DbSet<Adres> adresler { get; set; }
            public DbSet<Araç_Kategorileri> araçKategorileri { get; set; }
            public DbSet<Araç_Tüketim_Bilgileri> araçTüketimBilgileri { get; set; }
            public DbSet<Araç_İstatistikleri> araçİstatistikleri { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=CarRental;Username=postgres;Password=password");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Burada, tablolar arasındaki ilişkileri, kısıtlamaları, indexleri vb. tanımlayabilirsin.
            }
        }
    }
}

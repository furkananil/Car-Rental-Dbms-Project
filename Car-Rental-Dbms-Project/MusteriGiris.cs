using Npgsql;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Car_Rental_Dbms_Project
{
    public partial class MusteriGiris : Form
    {
        public MusteriGiris()
        {
            InitializeComponent();
            LoadCarsWithDetails();
        }

        private void LoadCarsWithDetails()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental";

            string query = @"
    SELECT 
        a.""Id"" AS ""AracId"",
        a.""Marka"",
        a.""Model"",
        a.""Yil"",
        k.""KategoriAd"" AS ""Kategori"",
        COALESCE(b.""BagajHacmi"", NULL) AS ""BagajHacmi"", 
        COALESCE(t.""YukKapasitesi"", NULL) AS ""YukKapasitesi"",
        i.""BeygirGucu"", 
        i.""ToplamKilometre"", 
        t2.""YakitTuru"", 
        t2.""YakitTuketimi""
    FROM 
        araclar a
    LEFT JOIN 
        binek_araclar b ON a.""Id"" = b.""AracId""
    LEFT JOIN 
        ticari_araclar t ON a.""Id"" = t.""AracId""
    LEFT JOIN 
        arac_istatistikleri i ON a.""Id"" = i.""AracId""
    LEFT JOIN 
        arac_tuketim_bilgileri t2 ON a.""Id"" = t2.""AracId""
    LEFT JOIN 
        arac_kategorileri k ON a.""KategoriId"" = k.""Id"";";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    int panelWidth = 270;
                    int panelHeight = 340;
                    int maxColumns = 3; // Her satırda maksimum panel sayısı
                    int xOffset = (this.ClientSize.Width - ((panelWidth + 10) * maxColumns)) / 2; // Ortalamak için
                    int yOffset = 10;

                    int columnCount = 0; // Mevcut sütun sayısı

                    while (reader.Read())
                    {
                        // Verileri oku
                        int aracId = Convert.ToInt32(reader["AracId"]);
                        string marka = reader["Marka"].ToString();
                        string model = reader["Model"].ToString();
                        int yil = Convert.ToInt32(reader["Yil"]);
                        string kategori = reader["Kategori"]?.ToString() ?? "N/A";
                        string bagajHacmi = reader["BagajHacmi"]?.ToString();
                        string yukKapasitesi = reader["YukKapasitesi"]?.ToString();
                        string beygirGucu = reader["BeygirGucu"]?.ToString() ?? "N/A";
                        string toplamKilometre = reader["ToplamKilometre"]?.ToString() ?? "N/A";
                        string yakitTuru = reader["YakitTuru"]?.ToString() ?? "N/A";
                        string yakitTuketimi = reader["YakitTuketimi"]?.ToString() ?? "N/A";

                        // Araç türünü belirle
                        string aracTuru = string.IsNullOrEmpty(bagajHacmi) && !string.IsNullOrEmpty(yukKapasitesi)
                            ? "Ticari"
                            : "Binek";

                        // Resim yolu (tam yol)
                        string imagePath = $"C:\\Users\\LENOVO\\source\\repos\\deneme123\\Car-Rental-Dbms-Project\\Car-Rental-Dbms-Project\\Resources\\{aracId}.jpg";

                        // Resmi yükle, yoksa varsayılan bir resim kullan
                        Image aracResmi = File.Exists(imagePath) ? Image.FromFile(imagePath) : null;

                        // Panel oluştur
                        Panel panel = new Panel
                        {
                            Size = new Size(panelWidth, panelHeight),
                            Location = new Point(xOffset + 550, yOffset),
                            BorderStyle = BorderStyle.FixedSingle
                        };

                        // Resim Label'ı
                        PictureBox pictureBox = new PictureBox
                        {
                            Image = aracResmi,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Size = new Size(panelWidth, 150),
                            Location = new Point(0, 0)
                        };

                        // Araç bilgilerini içeren Label
                        Label infoLabel = new Label
                        {
                            Text = $"Marka: {marka}\nModel: {model}\nYıl: {yil}\nKategori: {kategori}\n" +
                                   $"Araç Türü: {aracTuru}\n" +
                                   $"Bagaj Hacmi: {bagajHacmi ?? "N/A"}\nYük Kapasitesi: {yukKapasitesi ?? "N/A"}\n" +
                                   $"Beygir Gücü: {beygirGucu}\nToplam Kilometre: {toplamKilometre}\nYakıt Türü: {yakitTuru}\nYakıt Tüketimi: {yakitTuketimi}",
                            Location = new Point(10, 160),
                            AutoSize = true,
                            Font = new Font("Arial", 8)
                        };

                        // Panel'e ekle
                        panel.Controls.Add(pictureBox);
                        panel.Controls.Add(infoLabel);

                        this.Controls.Add(panel);

                        // Sütun sayısını artır
                        columnCount++;
                        xOffset += panel.Width + 10;

                        // Eğer satır dolduysa, bir sonraki satıra geç
                        if (columnCount >= maxColumns)
                        {
                            columnCount = 0; // Sütun sayısını sıfırla
                            xOffset = (this.ClientSize.Width - ((panelWidth + 10) * maxColumns)) / 2; // X konumunu sıfırla
                            yOffset += panelHeight + 150; // Y konumunu artır (400px boşluk)
                        }
                    }
                }
            }
        }

        private void MusteriGiris_Load(object sender, EventArgs e)
        {

        }

        private void btnKiralama_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bu Aracı Kiralayıp Ödeme yapmak istediğinize emin misiniz?",
                                  "Onay",
                                  MessageBoxButtons.OKCancel,
                                  MessageBoxIcon.Question);

            if (dr == DialogResult.OK) 
            {
                string connectionString = "Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental";

                using (var context = new ApplicationDbContext())
                {
                    // TextBox'lardan alınan veriler
                    string sokakAd = txtSokak.Text;
                    string mahalleAd = txtMahalle.Text;
                    string sehirAd = txtSehir.Text;
                    string ilceAd = txtIlce.Text;
                    string postaKoduAd = txtPostaKodu.Text;

                    // 1. Sokak nesnesini oluştur ve kaydet
                    var sokak = new Sokak
                    {
                        SokakAd = sokakAd // TextBox'tan gelen sokak adı
                    };

                    context.sokaklar.Add(sokak);
                    context.SaveChanges(); // Sokak'ı kaydediyoruz, böylece ID'si oluşturulmuş oluyor

                    // 2. Mahalle nesnesini oluştur ve kaydet
                    var mahalle = new Mahalle
                    {
                        MahalleAd = mahalleAd // TextBox'tan gelen mahalle adı
                    };

                    context.mahalleler.Add(mahalle);
                    context.SaveChanges(); // Mahalle'yi kaydediyoruz, böylece ID'si oluşturulmuş oluyor

                    // 3. Sehir nesnesini oluştur ve kaydet (Eğer Sehir yoksa)
                    var sehir = new Sehir
                    {
                        SehirAd = sehirAd // TextBox'tan gelen şehir adı
                    };

                    context.sehirler.Add(sehir);
                    context.SaveChanges(); // Şehri kaydediyoruz

                    // 4. Ilce nesnesini oluştur ve kaydet (Eğer Ilce yoksa)
                    var ilce = new Ilce
                    {
                        IlceAd = ilceAd // TextBox'tan gelen ilçe adı
                    };

                    context.ilceler.Add(ilce);
                    context.SaveChanges(); // İlçe'yi kaydediyoruz

                    // 5. PostaKodu nesnesini oluştur ve kaydet (Eğer Posta Kodu yoksa)
                    var postaKodu = new PostaKodu
                    {
                        PostaKoduAd = postaKoduAd // TextBox'tan gelen posta kodu
                    };

                    context.postaKodlari.Add(postaKodu);
                    context.SaveChanges(); // Posta Kodu'nu kaydediyoruz

                    // 6. Adres nesnesini oluştur ve kaydet
                    var adres = new Adres
                    {
                        SokakId = sokak.Id, // Kaydedilen Sokak'ın ID'si
                        SehirId = sehir.Id, // Kaydedilen Sehir'in ID'si
                        IlceId = ilce.Id, // Kaydedilen Ilce'nin ID'si
                        PostaKoduId = postaKodu.Id, // Kaydedilen PostaKodu'nun ID'si
                        MahalleId = mahalle.Id // Kaydedilen Mahalle'nin ID'si
                    };

                    context.adresler.Add(adres);
                    context.SaveChanges(); // Adres'i kaydediyoruz

                    // Yeni eklenen adresin ID'sini al
                    int adresId = adres.Id;

                    // Müşteri ekleme işlemi
                    var musteri = new Musteri
                    {
                        Ad = txtAd.Text,
                        Soyad = txtSoyad.Text,
                        Email = txtEmail.Text,
                        Telefon = txtTelefon.Text,
                        AdresId = adresId // Adresin ID'si burada kullanılıyor
                    };

                    context.musteriler.Add(musteri);
                    context.SaveChanges();

                    // Seçilen RadioButton'dan aracın ID'sini al
                    int aracId = 0;
                    int gunlukFiyat = 0;

                    if (radioButton1.Checked)
                    {
                        aracId = 1;
                        gunlukFiyat = 2149; // 1. aracın günlük fiyatı
                    }
                    else if (radioButton2.Checked)
                    {
                        aracId = 2;
                        gunlukFiyat = 849; // 2. aracın günlük fiyatı
                    }
                    else if (radioButton3.Checked)
                    {
                        aracId = 3;
                        gunlukFiyat = 749; // 3. aracın günlük fiyatı
                    }
                    else if (radioButton4.Checked)
                    {
                        aracId = 4;
                        gunlukFiyat = 1699; // 4. aracın günlük fiyatı
                    }
                    else if (radioButton5.Checked)
                    {
                        aracId = 5;
                        gunlukFiyat = 899; // 5. aracın günlük fiyatı
                    }
                    else if (radioButton6.Checked)
                    {
                        aracId = 6;
                        gunlukFiyat = 1299; // 6. aracın günlük fiyatı
                    }

                    if (aracId == 0)
                    {
                        MessageBox.Show("Lütfen bir araç seçin.");
                        return;
                    }

                    // DateTimePicker ile alınan tarihi UTC'ye dönüştürme
                    DateTime kiralamaTarihi = dateTimePickerKiralamaTarihi.Value;
                    DateTime kiralamaBitisTarihi = dateTimePickerKiralamaBitisTarihi.Value;

                    // Eğer tarih yerel zaman (Local) ise UTC'ye dönüştür
                    if (kiralamaTarihi.Kind == DateTimeKind.Local)
                    {
                        kiralamaTarihi = kiralamaTarihi.ToUniversalTime();
                    }
                    else if (kiralamaTarihi.Kind == DateTimeKind.Unspecified)
                    {
                        // Zaman dilimi belirtilmemişse, UTC'ye çevirelim
                        kiralamaTarihi = DateTime.SpecifyKind(kiralamaTarihi, DateTimeKind.Utc);
                    }

                    if (kiralamaBitisTarihi.Kind == DateTimeKind.Local)
                    {
                        kiralamaBitisTarihi = kiralamaBitisTarihi.ToUniversalTime();
                    }
                    else if (kiralamaBitisTarihi.Kind == DateTimeKind.Unspecified)
                    {
                        // Zaman dilimi belirtilmemişse, UTC'ye çevirelim
                        kiralamaBitisTarihi = DateTime.SpecifyKind(kiralamaBitisTarihi, DateTimeKind.Utc);
                    }


                    var kiralamaSuresi = (kiralamaBitisTarihi - kiralamaTarihi).Days;

                    if (kiralamaSuresi <= 0)
                    {
                        MessageBox.Show("Kiralama bitiş tarihi, başlangıç tarihinden sonraki bir tarih olmalıdır.");
                        return;
                    }

                    int toplamTutar = gunlukFiyat * kiralamaSuresi; // Toplam tutar hesaplama

                    // Kiralama bilgilerini kaydet
                    var kiralama = new Kiralama
                    {
                        MusteriId = musteri.Id,
                        KiralamaTarihi = kiralamaTarihi,
                        KiralamaBitisTarihi = kiralamaBitisTarihi,
                        AracId = aracId // Seçilen aracın ID'si burada kullanılıyor
                    };

                    context.kiralamalar.Add(kiralama);
                    context.SaveChanges();

                    // Fatura bilgilerini oluştur
                    var fatura = new Fatura
                    {
                        KiralamaId = kiralama.Id,
                        FaturaTarihi = DateTime.UtcNow,
                        Tutar = toplamTutar
                    };

                    context.faturalar.Add(fatura);
                    context.SaveChanges();

                    // Ödeme bilgilerini oluştur
                    var odeme = new Odeme
                    {
                        FaturaId = fatura.Id,
                        OdemeTarihi = DateTime.UtcNow,
                        OdemeTutar = toplamTutar, // Ödeme tutarı, toplam tutar ile aynı olacak
                    };

                    context.odemeler.Add(odeme);
                    context.SaveChanges();

                    // Kiralama bilgileri ve ilişkili fatura/ödeme bilgilerini ekrana getir
                    var kiralamaBilgileri = from k in context.kiralamalar
                                            join f in context.faturalar on k.Id equals f.KiralamaId
                                            join o in context.odemeler on f.Id equals o.FaturaId
                                            where k.MusteriId == musteri.Id
                                            select new
                                            {
                                                k.KiralamaTarihi,
                                                k.KiralamaBitisTarihi,
                                                f.FaturaTarihi,
                                                f.Tutar,
                                                o.OdemeTarihi,
                                                o.OdemeTutar,
                                            };

                    // Kiralama bilgilerini ekrana yazdır
                    foreach (var bilgi in kiralamaBilgileri)
                    {
                        // Burada bilgileri ekranda göstermek için MessageBox veya DataGrid gibi bir yapı kullanabilirsiniz
                        MessageBox.Show(
                                        $"Ödeme Tarihi: {bilgi.OdemeTarihi}\n\n" +
                                        $"Ödeme Tutarı: {bilgi.OdemeTutar}\n\n", "Teşekkürler!",MessageBoxButtons.OK,           // Buton
                MessageBoxIcon.Information);

                        // Burada bilgileri ekranda göstermek için MessageBox veya DataGrid gibi bir yapı kullanabilirsiniz
                        MessageBox.Show($"Fatura Tarihi: {bilgi.FaturaTarihi}\n\n\n" +
                                        $"Kiralama Tarihi: {bilgi.KiralamaTarihi}\n" +
                                        $"Kiralama Bitiş Tarihi: {bilgi.KiralamaBitisTarihi}\n" +
                                        
                                        $"Toplam Tutar: {bilgi.Tutar}\n\n", "Fatura Bilgileri!", MessageBoxButtons.OK,           // Buton
                MessageBoxIcon.Information);
                    }

                }
            }
            else if (dr == DialogResult.Cancel)
            {
                // Cancel'e tıklanmış
                MessageBox.Show("İşlem iptal edildi!", "İptal!", MessageBoxButtons.OK,           // Buton
                MessageBoxIcon.Error);
            }

        }
    }
}

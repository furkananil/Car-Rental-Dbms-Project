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

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM get_arac_bilgileri()";

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    int panelWidth = 270;
                    int panelHeight = 340;
                    int maxColumns = 3;
                    int xOffset = (this.ClientSize.Width - ((panelWidth + 10) * maxColumns)) / 2;
                    int yOffset = 10;

                    int columnCount = 0;

                    while (reader.Read())
                    {

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


                        string aracTuru = string.IsNullOrEmpty(bagajHacmi) && !string.IsNullOrEmpty(yukKapasitesi)
                            ? "Ticari"
                            : "Binek";


                        string imagePath = $"C:\\Users\\LENOVO\\source\\repos\\deneme123\\Car-Rental-Dbms-Project\\Car-Rental-Dbms-Project\\Resources\\{aracId}.jpg";


                        Image aracResmi = File.Exists(imagePath) ? Image.FromFile(imagePath) : null;


                        Panel panel = new Panel
                        {
                            Size = new Size(panelWidth, panelHeight),
                            Location = new Point(xOffset + 550, yOffset),
                            BorderStyle = BorderStyle.FixedSingle
                        };


                        PictureBox pictureBox = new PictureBox
                        {
                            Image = aracResmi,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Size = new Size(panelWidth, 150),
                            Location = new Point(0, 0)
                        };


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


                        panel.Controls.Add(pictureBox);
                        panel.Controls.Add(infoLabel);

                        this.Controls.Add(panel);

                        columnCount++;
                        xOffset += panel.Width + 10;


                        if (columnCount >= maxColumns)
                        {
                            columnCount = 0;
                            xOffset = (this.ClientSize.Width - ((panelWidth + 10) * maxColumns)) / 2;
                            yOffset += panelHeight + 150;
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

                    string sokakAd = txtSokak.Text;
                    string mahalleAd = txtMahalle.Text;
                    string sehirAd = txtSehir.Text;
                    string ilceAd = txtIlce.Text;
                    string postaKoduAd = txtPostaKodu.Text;


                    var sokak = new Sokak
                    {
                        SokakAd = sokakAd
                    };

                    context.sokaklar.Add(sokak);
                    context.SaveChanges();


                    var mahalle = new Mahalle
                    {
                        MahalleAd = mahalleAd
                    };

                    context.mahalleler.Add(mahalle);
                    context.SaveChanges();


                    var sehir = new Sehir
                    {
                        SehirAd = sehirAd
                    };

                    context.sehirler.Add(sehir);
                    context.SaveChanges();


                    var ilce = new Ilce
                    {
                        IlceAd = ilceAd
                    };

                    context.ilceler.Add(ilce);
                    context.SaveChanges();


                    var postaKodu = new PostaKodu
                    {
                        PostaKoduAd = postaKoduAd
                    };

                    context.postaKodlari.Add(postaKodu);
                    context.SaveChanges();


                    var adres = new Adres
                    {
                        SokakId = sokak.Id,
                        SehirId = sehir.Id,
                        IlceId = ilce.Id,
                        PostaKoduId = postaKodu.Id,
                        MahalleId = mahalle.Id
                    };

                    context.adresler.Add(adres);
                    context.SaveChanges();


                    int adresId = adres.Id;


                    var musteri = new Musteri
                    {
                        Ad = txtAd.Text,
                        Soyad = txtSoyad.Text,
                        Email = txtEmail.Text,
                        Telefon = txtTelefon.Text,
                        AdresId = adresId
                    };

                    context.musteriler.Add(musteri);
                    context.SaveChanges();


                    int aracId = 0;
                    int gunlukFiyat = 0;

                    if (radioButton1.Checked)
                    {
                        aracId = 1;
                        gunlukFiyat = 2149;
                    }
                    else if (radioButton2.Checked)
                    {
                        aracId = 2;
                        gunlukFiyat = 849;
                    }
                    else if (radioButton3.Checked)
                    {
                        aracId = 3;
                        gunlukFiyat = 749;
                    }
                    else if (radioButton4.Checked)
                    {
                        aracId = 4;
                        gunlukFiyat = 1699;
                    }
                    else if (radioButton5.Checked)
                    {
                        aracId = 5;
                        gunlukFiyat = 899;
                    }
                    else if (radioButton6.Checked)
                    {
                        aracId = 6;
                        gunlukFiyat = 1299;
                    }

                    if (aracId == 0)
                    {
                        MessageBox.Show("Lütfen bir araç seçin.");
                        return;
                    }


                    DateTime kiralamaTarihi = dateTimePickerKiralamaTarihi.Value;
                    DateTime kiralamaBitisTarihi = dateTimePickerKiralamaBitisTarihi.Value;


                    if (kiralamaTarihi.Kind == DateTimeKind.Local)
                    {
                        kiralamaTarihi = kiralamaTarihi.ToUniversalTime();
                    }
                    else if (kiralamaTarihi.Kind == DateTimeKind.Unspecified)
                    {

                        kiralamaTarihi = DateTime.SpecifyKind(kiralamaTarihi, DateTimeKind.Utc);
                    }

                    if (kiralamaBitisTarihi.Kind == DateTimeKind.Local)
                    {
                        kiralamaBitisTarihi = kiralamaBitisTarihi.ToUniversalTime();
                    }
                    else if (kiralamaBitisTarihi.Kind == DateTimeKind.Unspecified)
                    {

                        kiralamaBitisTarihi = DateTime.SpecifyKind(kiralamaBitisTarihi, DateTimeKind.Utc);
                    }


                    var kiralamaSuresi = (kiralamaBitisTarihi - kiralamaTarihi).Days;

                    if (kiralamaSuresi <= 0)
                    {
                        MessageBox.Show("Kiralama bitiş tarihi, başlangıç tarihinden sonraki bir tarih olmalıdır.");
                        return;
                    }

                    int toplamTutar = gunlukFiyat * kiralamaSuresi;

                    var kiralama = new Kiralama
                    {
                        MusteriId = musteri.Id,
                        KiralamaTarihi = kiralamaTarihi,
                        KiralamaBitisTarihi = kiralamaBitisTarihi,
                        AracId = aracId
                    };

                    context.kiralamalar.Add(kiralama);
                    context.SaveChanges();


                    var fatura = new Fatura
                    {
                        KiralamaId = kiralama.Id,
                        FaturaTarihi = DateTime.UtcNow,
                        Tutar = toplamTutar
                    };

                    context.faturalar.Add(fatura);
                    context.SaveChanges();


                    var odeme = new Odeme
                    {
                        FaturaId = fatura.Id,
                        OdemeTarihi = DateTime.UtcNow,
                        OdemeTutar = toplamTutar,
                    };

                    context.odemeler.Add(odeme);
                    context.SaveChanges();

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


                    foreach (var bilgi in kiralamaBilgileri)
                    {

                        MessageBox.Show(
                                        $"Ödeme Tarihi: {bilgi.OdemeTarihi}\n\n" +
                                        $"Ödeme Tutarı: {bilgi.OdemeTutar}\n\n", "Teşekkürler!", MessageBoxButtons.OK,
                MessageBoxIcon.Information);


                        MessageBox.Show($"Fatura Tarihi: {bilgi.FaturaTarihi}\n\n\n" +
                                        $"Kiralama Tarihi: {bilgi.KiralamaTarihi}\n" +
                                        $"Kiralama Bitiş Tarihi: {bilgi.KiralamaBitisTarihi}\n" +

                                        $"Toplam Tutar: {bilgi.Tutar}\n\n", "Fatura Bilgileri!", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                    }

                }
            }
            else if (dr == DialogResult.Cancel)
            {

                MessageBox.Show("İşlem iptal edildi!", "İptal!", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }

        }

        private void yoneticiBtn_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
        }
    }
}

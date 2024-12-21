using Npgsql;
using System;
using System.Drawing;
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
            string connectionString = "Host=localhost;Username=postgres;Password=;Database=CarRental";

            // Sorgu
            string query = @"
        SELECT 
            a.""Id"" AS ""AracId"",
            a.""Marka"",
            a.""Model"",
            a.""Yil"",
            k.""KategoriAd"" AS ""Kategori"",
            COALESCE(b.""BagajHacmi"", 0) AS ""BagajHacmi"", 
            COALESCE(t.""YukKapasitesi"", 0) AS ""YukKapasitesi"",
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
                    int xOffset = 10; // X konumu için başlangıç değeri
                    int yOffset = 10; // Y konumu için başlangıç değeri
                    int panelWidth = 250; // Panel genişliği
                    int panelHeight = 340; // Panel yüksekliği
                    int maxColumns = 4; // Her satırda maksimum panel sayısı

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

                        // Resim yolu (tam yol)
                        string imagePath = $@"C:\Users\LENOVO\source\repos\deneme123\Car-Rental-Dbms-Project\Car-Rental-Dbms-Project\Resources\{aracId}.jpg";

                        // Resmi yükle, yoksa varsayılan bir resim kullan
                        Image aracResmi = File.Exists(imagePath) ? Image.FromFile(imagePath) : null;

                        // Panel oluştur
                        Panel panel = new Panel
                        {
                            Size = new Size(panelWidth, panelHeight),
                            Location = new Point(xOffset, yOffset),
                            BorderStyle = BorderStyle.FixedSingle
                        };

                        // Resim Label'ı
                        PictureBox pictureBox = new PictureBox
                        {
                            Image = aracResmi,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Size = new Size(panelWidth, 150), // Resmin boyutu
                            Location = new Point(0, 0)
                        };

                        // Araç türünü doğru şekilde belirleyin
                        string aracTuru = "Binek"; // Varsayılan olarak Binek

                        // Eğer Bagaj Hacmi varsa, Binek Araç olarak kabul et
                        if (!string.IsNullOrEmpty(bagajHacmi))
                        {
                            aracTuru = "Binek";
                        }
                        // Eğer Bagaj Hacmi yoksa ama Yük Kapasitesi varsa, Ticari Araç olarak kabul et
                        else if (!string.IsNullOrEmpty(yukKapasitesi))
                        {
                            aracTuru = "Ticari";
                        }

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

                        // Yatayda dizilim
                        xOffset += panel.Width + 10; // Sonraki panel için X konumu

                        // Eğer satır dolduysa, bir sonraki satıra geç
                        if (xOffset > this.ClientSize.Width - panelWidth)
                        {
                            xOffset = 10; // X konumunu sıfırla
                            yOffset += panelHeight + 10; // Y konumunu artır
                        }
                    }
                }
            }
        }
    }
}

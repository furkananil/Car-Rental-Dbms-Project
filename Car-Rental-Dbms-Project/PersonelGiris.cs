using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_Rental_Dbms_Project
{
    public partial class yonetici : Form
    {
        public yonetici()
        {
            InitializeComponent();
            LoadCalisanlar();
            LoadMusteriler();
        }

        private void LoadCalisanlar()
        {
            string connectionString = "Host=localhost;Port=5432;Database=CarRental;Username=postgres;Password=furkanbabadb0";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM get_calisan_bilgileri()";

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void LoadMusteriler()
        {
            string connectionString = "Host=localhost;Port=5432;Database=CarRental;Username=postgres;Password=furkanbabadb0";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM get_musteri_bilgileri()";

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView2.DataSource = dataTable;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental"))
            {
                connection.Open();

                string ad = txtAd.Text;
                string soyad = txtSoyad.Text;
                string email = txtEmail.Text;
                string telefon = txtTelefon.Text;
                string sehir = txtSehir.Text;
                string ilce = txtIlce.Text;
                string mahalle = txtMahalle.Text;
                string sokak = txtSokak.Text;
                string postaKodu = txtPostaKodu.Text;

                try
                {
                    var checkSehirCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""sehirler"" WHERE ""SehirAd"" = @Sehir", connection);
                    checkSehirCommand.Parameters.AddWithValue("@Sehir", sehir);
                    var sehirId = checkSehirCommand.ExecuteScalar();
                    if (sehirId == null)
                    {
                        var insertSehirCommand = new NpgsqlCommand(@"INSERT INTO ""sehirler"" (""SehirAd"") VALUES (@Sehir) RETURNING ""Id""", connection);
                        insertSehirCommand.Parameters.AddWithValue("@Sehir", sehir);
                        sehirId = insertSehirCommand.ExecuteScalar();
                    }

                    var checkIlceCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""ilceler"" WHERE ""IlceAd"" = @Ilce", connection);
                    checkIlceCommand.Parameters.AddWithValue("@Ilce", ilce);
                    var ilceId = checkIlceCommand.ExecuteScalar();
                    if (ilceId == null)
                    {
                        var insertIlceCommand = new NpgsqlCommand(@"INSERT INTO ""ilceler"" (""IlceAd"") VALUES (@Ilce) RETURNING ""Id""", connection);
                        insertIlceCommand.Parameters.AddWithValue("@Ilce", ilce);
                        ilceId = insertIlceCommand.ExecuteScalar();
                    }

                    var checkMahalleCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""mahalleler"" WHERE ""MahalleAd"" = @Mahalle", connection);
                    checkMahalleCommand.Parameters.AddWithValue("@Mahalle", mahalle);
                    var mahalleId = checkMahalleCommand.ExecuteScalar();
                    if (mahalleId == null)
                    {
                        var insertMahalleCommand = new NpgsqlCommand(@"INSERT INTO ""mahalleler"" (""MahalleAd"") VALUES (@Mahalle) RETURNING ""Id""", connection);
                        insertMahalleCommand.Parameters.AddWithValue("@Mahalle", mahalle);
                        mahalleId = insertMahalleCommand.ExecuteScalar();
                    }

                    var checkSokakCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""sokaklar"" WHERE ""SokakAd"" = @Sokak", connection);
                    checkSokakCommand.Parameters.AddWithValue("@Sokak", sokak);
                    var sokakId = checkSokakCommand.ExecuteScalar();
                    if (sokakId == null)
                    {
                        var insertSokakCommand = new NpgsqlCommand(@"INSERT INTO ""sokaklar"" (""SokakAd"") VALUES (@Sokak) RETURNING ""Id""", connection);
                        insertSokakCommand.Parameters.AddWithValue("@Sokak", sokak);
                        sokakId = insertSokakCommand.ExecuteScalar();
                    }

                    var checkPostaKoduCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""postaKodlari"" WHERE ""PostaKoduAd"" = @PostaKodu", connection);
                    checkPostaKoduCommand.Parameters.AddWithValue("@PostaKodu", postaKodu);
                    var postaKoduId = checkPostaKoduCommand.ExecuteScalar();
                    if (postaKoduId == null)
                    {
                        var insertPostaKoduCommand = new NpgsqlCommand(@"INSERT INTO ""postaKodlari"" (""PostaKoduAd"") VALUES (@PostaKodu) RETURNING ""Id""", connection);
                        insertPostaKoduCommand.Parameters.AddWithValue("@PostaKodu", postaKodu);
                        postaKoduId = insertPostaKoduCommand.ExecuteScalar();
                    }

                    var insertAdresCommand = new NpgsqlCommand(@"
                INSERT INTO ""adresler"" (""SehirId"", ""IlceId"", ""MahalleId"", ""SokakId"", ""PostaKoduId"")
                VALUES (@SehirId, @IlceId, @MahalleId, @SokakId, @PostaKoduId)
                RETURNING ""Id""", connection);
                    insertAdresCommand.Parameters.AddWithValue("@SehirId", sehirId);
                    insertAdresCommand.Parameters.AddWithValue("@IlceId", ilceId);
                    insertAdresCommand.Parameters.AddWithValue("@MahalleId", mahalleId);
                    insertAdresCommand.Parameters.AddWithValue("@SokakId", sokakId);
                    insertAdresCommand.Parameters.AddWithValue("@PostaKoduId", postaKoduId);
                    var adresId = insertAdresCommand.ExecuteScalar();

                    var insertCalisanCommand = new NpgsqlCommand(@"
                INSERT INTO ""calisanlar"" (""Ad"", ""Soyad"", ""Email"", ""Telefon"", ""AdresId"")
                VALUES (@Ad, @Soyad, @Email, @Telefon, @AdresId)", connection);
                    insertCalisanCommand.Parameters.AddWithValue("@Ad", ad);
                    insertCalisanCommand.Parameters.AddWithValue("@Soyad", soyad);
                    insertCalisanCommand.Parameters.AddWithValue("@Email", email);
                    insertCalisanCommand.Parameters.AddWithValue("@Telefon", telefon);
                    insertCalisanCommand.Parameters.AddWithValue("@AdresId", adresId);
                    insertCalisanCommand.ExecuteNonQuery();

                    MessageBox.Show("Personel başarıyla eklendi.");
                    VerileriYenile();
                }
                catch (NpgsqlException ex)
                {
                    if (ex.SqlState == "P0001") 
                    {
                        if (ex.Message.Contains("Email already exists"))
                        {
                            MessageBox.Show("Bu e-posta adresi zaten mevcut.");
                        }
                        else if (ex.Message.Contains("Telefon already exists"))
                        {
                            MessageBox.Show("Bu telefon numarası zaten mevcut.");
                        }
                        else
                        {
                            MessageBox.Show("Hata: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
            }
        }

        private static string connectionString = "Host=localhost;Port=5432;Database=CarRental;Username=postgres;Password=furkanbabadb0";

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                int calisanId = Convert.ToInt32(selectedRow.Cells["calisan_id"].Value);
                string ad = selectedRow.Cells["Ad"].Value.ToString();
                string soyad = selectedRow.Cells["Soyad"].Value.ToString();
                string email = selectedRow.Cells["Email"].Value.ToString();
                string telefon = selectedRow.Cells["Telefon"].Value.ToString();
                string sehir = selectedRow.Cells["sehir"].Value.ToString();
                string ilce = selectedRow.Cells["ilce"].Value.ToString();
                string mahalle = selectedRow.Cells["mahalle"].Value.ToString();
                string sokak = selectedRow.Cells["sokak"].Value.ToString();
                string postaKodu = selectedRow.Cells["postakodu"].Value.ToString();

                using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental"))
                {
                    connection.Open();

                    try
                    {
                        var command = new NpgsqlCommand("CALL update_calisan(@CalisanId, @Ad, @Soyad, @Email, @Telefon, @Sehir, @Ilce, @Mahalle, @Sokak, @PostaKodu)", connection);
                        command.Parameters.AddWithValue("@CalisanId", calisanId);
                        command.Parameters.AddWithValue("@Ad", ad);
                        command.Parameters.AddWithValue("@Soyad", soyad);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Telefon", telefon);
                        command.Parameters.AddWithValue("@Sehir", sehir);
                        command.Parameters.AddWithValue("@Ilce", ilce);
                        command.Parameters.AddWithValue("@Mahalle", mahalle);
                        command.Parameters.AddWithValue("@Sokak", sokak);
                        command.Parameters.AddWithValue("@PostaKodu", postaKodu);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Çalışan başarıyla güncellendi.");

                        LoadCalisanlar();
                    }
                    catch (NpgsqlException ex)
                    {
                        if (ex.SqlState == "P0001") 
                        {
                            if (ex.Message.Contains("Email already exists"))
                            {
                                MessageBox.Show("Bu e-posta adresi zaten mevcut.");
                            }
                            else if (ex.Message.Contains("Telefon already exists"))
                            {
                                MessageBox.Show("Bu telefon numarası zaten mevcut.");
                            }
                            else
                            {
                                MessageBox.Show("Hata: " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Hata: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz çalışanı seçin.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int calisanId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["calisan_id"].Value);

                using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental"))
                {
                    connection.Open();

          
                    var deleteCalisanCommand = new NpgsqlCommand(@"DELETE FROM ""calisanlar"" WHERE ""Id"" = @Id", connection);
                    deleteCalisanCommand.Parameters.AddWithValue("@Id", calisanId);
                    deleteCalisanCommand.ExecuteNonQuery();

        
                    var deleteAdresCommand = new NpgsqlCommand(@"
                DELETE FROM ""adresler""
                WHERE ""Id"" = (SELECT ""AdresId"" FROM ""calisanlar"" WHERE ""Id"" = @Id)", connection);
                    deleteAdresCommand.Parameters.AddWithValue("@Id", calisanId);
                    deleteAdresCommand.ExecuteNonQuery();

                    MessageBox.Show("Personel başarıyla silindi.");

           
                    VerileriYenile();
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz personeli seçin.");
            }
        }


        private void VerileriYenile()
        {
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental"))
            {
                try
                {
                    connection.Open();

                    var command = new NpgsqlCommand("SELECT * FROM get_calisan_bilgileri()", connection);
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }



        private void btnDeleteM_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int musteriId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["musteri_id"].Value);

                using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental"))
                {
                    connection.Open();

                  
                    var deleteKiralamalarCommand = new NpgsqlCommand(@"DELETE FROM ""kiralamalar"" WHERE ""MusteriId"" = @MusteriId", connection);
                    deleteKiralamalarCommand.Parameters.AddWithValue("@MusteriId", musteriId);
                    deleteKiralamalarCommand.ExecuteNonQuery();

       
                    var deleteMusteriCommand = new NpgsqlCommand(@"DELETE FROM ""musteriler"" WHERE ""Id"" = @Id", connection);
                    deleteMusteriCommand.Parameters.AddWithValue("@Id", musteriId);
                    deleteMusteriCommand.ExecuteNonQuery();

              
                    var deleteAdresCommand = new NpgsqlCommand(@"
                DELETE FROM ""adresler""
                WHERE ""Id"" = (SELECT ""AdresId"" FROM ""musteriler"" WHERE ""Id"" = @Id)", connection);
                    deleteAdresCommand.Parameters.AddWithValue("@Id", musteriId);
                    deleteAdresCommand.ExecuteNonQuery();

                    MessageBox.Show("Müşteri başarıyla silindi.");

  
                    VerileriYenileMusteri();
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz müşteriyi seçin.");
            }
        }

        private void btnUpdateM_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView2.SelectedRows[0];
                int musteriId = Convert.ToInt32(selectedRow.Cells["musteri_id"].Value);
                string ad = selectedRow.Cells["Ad"].Value.ToString();
                string soyad = selectedRow.Cells["Soyad"].Value.ToString();
                string email = selectedRow.Cells["Email"].Value.ToString();
                string telefon = selectedRow.Cells["Telefon"].Value.ToString();
                string sehir = selectedRow.Cells["sehir"].Value.ToString();
                string ilce = selectedRow.Cells["ilce"].Value.ToString();
                string mahalle = selectedRow.Cells["mahalle"].Value.ToString();
                string sokak = selectedRow.Cells["sokak"].Value.ToString();
                string postaKodu = selectedRow.Cells["postakodu"].Value.ToString();

                using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental"))
                {
                    connection.Open();

                    var command = new NpgsqlCommand("CALL update_musteri(@MusteriId, @Ad, @Soyad, @Email, @Telefon, @Sehir, @Ilce, @Mahalle, @Sokak, @PostaKodu)", connection);
                    command.Parameters.AddWithValue("@MusteriId", musteriId);
                    command.Parameters.AddWithValue("@Ad", ad);
                    command.Parameters.AddWithValue("@Soyad", soyad);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Telefon", telefon);
                    command.Parameters.AddWithValue("@Sehir", sehir);
                    command.Parameters.AddWithValue("@Ilce", ilce);
                    command.Parameters.AddWithValue("@Mahalle", mahalle);
                    command.Parameters.AddWithValue("@Sokak", sokak);
                    command.Parameters.AddWithValue("@PostaKodu", postaKodu);

                    command.ExecuteNonQuery();

                
                    VerileriYenileMusteri();
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz müşteriyi seçin.");
            }
        }

        private void VerileriYenileMusteri()
        {
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental"))
            {
                connection.Open();

                var command = new NpgsqlCommand("SELECT * FROM get_musteri_bilgileri()", connection);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void btnToplamCiro_Click(object sender, EventArgs e)
        {
            string connectionString = "Host=localhost;Port=5432;Database=CarRental;Username=postgres;Password=furkanbabadb0";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT GetToplamCiro()";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    var toplamCiro = command.ExecuteScalar();
                    MessageBox.Show("Toplam Ciro: " + toplamCiro.ToString(), "TOPLAM CİRO!",               
                MessageBoxButtons.OK,     
                MessageBoxIcon.Information);
                }
            }
        }

        private void ListenForNotifications()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                connection.Notification += (o, e) =>
                {
                    MessageBox.Show($"-> {e.Payload}", "YENİ BİR ÇALIŞAN EKLENDİ!",                  
                MessageBoxButtons.OK,   
                MessageBoxIcon.Information);
                };

                using (var command = new NpgsqlCommand("LISTEN new_calisan;", connection))
                {
                    command.ExecuteNonQuery();
                }

          
                while (true)
                {
                    connection.Wait();
                }
            }
        }
        private void ListenForNotifications2()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                connection.Notification += (o, e) =>
                {
                    MessageBox.Show($"->  {e.Payload}", "MÜŞTERİ GÜNCELLENDİ!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                using (var command = new NpgsqlCommand("LISTEN musteri_update;", connection))
                {
                    command.ExecuteNonQuery();
                }

                // Bildirimleri dinlemek için bağlantıyı açık tutun
                while (true)
                {
                    connection.Wait();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void yonetici_Load(object sender, EventArgs e)
        {
            Task.Run(() => ListenForNotifications());
            Task.Run(() => ListenForNotifications2());
        }
    }
}

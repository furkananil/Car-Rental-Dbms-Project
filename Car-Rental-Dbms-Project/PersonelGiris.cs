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
                string query = @"
                    SELECT 
                        c.""Id"" AS calisan_id, 
                        c.""Ad"", 
                        c.""Soyad"", 
                        c.""Email"", 
                        c.""Telefon"",
                        se.""SehirAd"" AS sehir,
                        il.""IlceAd"" AS ilce,
                        ma.""MahalleAd"" AS mahalle,
                        so.""SokakAd"" AS sokak,
                        pk.""PostaKoduAd"" AS postakodu
                    FROM ""calisanlar"" c
                    JOIN ""adresler"" a ON c.""AdresId"" = a.""Id""
                    JOIN ""sehirler"" se ON a.""SehirId"" = se.""Id""
                    JOIN ""ilceler"" il ON a.""IlceId"" = il.""Id""
                    JOIN ""mahalleler"" ma ON a.""MahalleId"" = ma.""Id""
                    JOIN ""sokaklar"" so ON a.""SokakId"" = so.""Id""
                    JOIN ""postaKodlari"" pk ON a.""PostaKoduId"" = pk.""Id"";
                ";

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
                string query = @"
    SELECT 
        m.""Id"" AS musteri_id, 
        m.""Ad"", 
        m.""Soyad"", 
        m.""Email"", 
        m.""Telefon"",
        se.""SehirAd"" AS sehir,
        il.""IlceAd"" AS ilce,
        ma.""MahalleAd"" AS mahalle,
        so.""SokakAd"" AS sokak,
        pk.""PostaKoduAd"" AS postakodu,
        k.""KiralamaTarihi"",
        k.""KiralamaBitisTarihi"",
        f.""Tutar"" AS toplam_tutar
    FROM ""musteriler"" m
    JOIN ""adresler"" a ON m.""AdresId"" = a.""Id""
    JOIN ""sehirler"" se ON a.""SehirId"" = se.""Id""
    JOIN ""ilceler"" il ON a.""IlceId"" = il.""Id""
    JOIN ""mahalleler"" ma ON a.""MahalleId"" = ma.""Id""
    JOIN ""sokaklar"" so ON a.""SokakId"" = so.""Id""
    JOIN ""postaKodlari"" pk ON a.""PostaKoduId"" = pk.""Id""
    LEFT JOIN ""kiralamalar"" k ON m.""Id"" = k.""MusteriId""
    LEFT JOIN ""faturalar"" f ON k.""Id"" = f.""KiralamaId"";
";


                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView2.DataSource = dataTable;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

                // Sehir ID'sini kontrol et ve ekle
                var checkSehirCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""sehirler"" WHERE ""SehirAd"" = @Sehir", connection);
                checkSehirCommand.Parameters.AddWithValue("@Sehir", sehir);
                var sehirId = checkSehirCommand.ExecuteScalar();
                if (sehirId == null)
                {
                    var insertSehirCommand = new NpgsqlCommand(@"INSERT INTO ""sehirler"" (""SehirAd"") VALUES (@Sehir) RETURNING ""Id""", connection);
                    insertSehirCommand.Parameters.AddWithValue("@Sehir", sehir);
                    sehirId = insertSehirCommand.ExecuteScalar();
                }

                // Ilce ID'sini kontrol et ve ekle
                var checkIlceCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""ilceler"" WHERE ""IlceAd"" = @Ilce", connection);
                checkIlceCommand.Parameters.AddWithValue("@Ilce", ilce);
                var ilceId = checkIlceCommand.ExecuteScalar();
                if (ilceId == null)
                {
                    var insertIlceCommand = new NpgsqlCommand(@"INSERT INTO ""ilceler"" (""IlceAd"") VALUES (@Ilce) RETURNING ""Id""", connection);
                    insertIlceCommand.Parameters.AddWithValue("@Ilce", ilce);
                    ilceId = insertIlceCommand.ExecuteScalar();
                }

                // Mahalle ID'sini kontrol et ve ekle
                var checkMahalleCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""mahalleler"" WHERE ""MahalleAd"" = @Mahalle", connection);
                checkMahalleCommand.Parameters.AddWithValue("@Mahalle", mahalle);
                var mahalleId = checkMahalleCommand.ExecuteScalar();
                if (mahalleId == null)
                {
                    var insertMahalleCommand = new NpgsqlCommand(@"INSERT INTO ""mahalleler"" (""MahalleAd"") VALUES (@Mahalle) RETURNING ""Id""", connection);
                    insertMahalleCommand.Parameters.AddWithValue("@Mahalle", mahalle);
                    mahalleId = insertMahalleCommand.ExecuteScalar();
                }

                // Sokak ID'sini kontrol et ve ekle
                var checkSokakCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""sokaklar"" WHERE ""SokakAd"" = @Sokak", connection);
                checkSokakCommand.Parameters.AddWithValue("@Sokak", sokak);
                var sokakId = checkSokakCommand.ExecuteScalar();
                if (sokakId == null)
                {
                    var insertSokakCommand = new NpgsqlCommand(@"INSERT INTO ""sokaklar"" (""SokakAd"") VALUES (@Sokak) RETURNING ""Id""", connection);
                    insertSokakCommand.Parameters.AddWithValue("@Sokak", sokak);
                    sokakId = insertSokakCommand.ExecuteScalar();
                }

                // Posta Kodu ID'sini kontrol et ve ekle
                var checkPostaKoduCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""postaKodlari"" WHERE ""PostaKoduAd"" = @PostaKodu", connection);
                checkPostaKoduCommand.Parameters.AddWithValue("@PostaKodu", postaKodu);
                var postaKoduId = checkPostaKoduCommand.ExecuteScalar();
                if (postaKoduId == null)
                {
                    var insertPostaKoduCommand = new NpgsqlCommand(@"INSERT INTO ""postaKodlari"" (""PostaKoduAd"") VALUES (@PostaKodu) RETURNING ""Id""", connection);
                    insertPostaKoduCommand.Parameters.AddWithValue("@PostaKodu", postaKodu);
                    postaKoduId = insertPostaKoduCommand.ExecuteScalar();
                }

                // Adresler tablosuna ekleme
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

                // Calisanlar tablosuna ekleme
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
        }


        private void VerileriYenile()
        {

            string query = @"
                    SELECT 
                        c.""Id"" AS calisan_id, 
                        c.""Ad"", 
                        c.""Soyad"", 
                        c.""Email"", 
                        c.""Telefon"",
                        se.""SehirAd"" AS sehir,
                        il.""IlceAd"" AS ilce,
                        ma.""MahalleAd"" AS mahalle,
                        so.""SokakAd"" AS sokak,
                        pk.""PostaKoduAd"" AS postakodu
                    FROM ""calisanlar"" c
                    JOIN ""adresler"" a ON c.""AdresId"" = a.""Id""
                    JOIN ""sehirler"" se ON a.""SehirId"" = se.""Id""
                    JOIN ""ilceler"" il ON a.""IlceId"" = il.""Id""
                    JOIN ""mahalleler"" ma ON a.""MahalleId"" = ma.""Id""
                    JOIN ""sokaklar"" so ON a.""SokakId"" = so.""Id""
                    JOIN ""postaKodlari"" pk ON a.""PostaKoduId"" = pk.""Id"";
                ";

            using (NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental"))
            {
                try
                {
                    connection.Open();

                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, connection);
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

        private static string connectionString = "Host=localhost;Port=5432;Database=CarRental;Username=postgres;Password=furkanbabadb0";

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental"))
            {
                connection.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue; // Yeni satırı atla

                    int calisanId = Convert.ToInt32(row.Cells["calisan_id"].Value);
                    string ad = row.Cells["Ad"].Value.ToString();
                    string soyad = row.Cells["Soyad"].Value.ToString();
                    string email = row.Cells["Email"].Value.ToString();
                    string telefon = row.Cells["Telefon"].Value.ToString();
                    string sehir = row.Cells["sehir"].Value.ToString();
                    string ilce = row.Cells["ilce"].Value.ToString();
                    string mahalle = row.Cells["mahalle"].Value.ToString();
                    string sokak = row.Cells["sokak"].Value.ToString();
                    string postaKodu = row.Cells["postakodu"].Value.ToString();

                    // Sehir ID'sini kontrol et ve ekle
                    var checkSehirCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""sehirler"" WHERE ""SehirAd"" = @Sehir", connection);
                    checkSehirCommand.Parameters.AddWithValue("@Sehir", sehir);
                    var sehirId = checkSehirCommand.ExecuteScalar();
                    if (sehirId == null)
                    {
                        var insertSehirCommand = new NpgsqlCommand(@"INSERT INTO ""sehirler"" (""SehirAd"") VALUES (@Sehir) RETURNING ""Id""", connection);
                        insertSehirCommand.Parameters.AddWithValue("@Sehir", sehir);
                        sehirId = insertSehirCommand.ExecuteScalar();
                    }

                    // Ilce ID'sini kontrol et ve ekle
                    var checkIlceCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""ilceler"" WHERE ""IlceAd"" = @Ilce", connection);
                    checkIlceCommand.Parameters.AddWithValue("@Ilce", ilce);
                    var ilceId = checkIlceCommand.ExecuteScalar();
                    if (ilceId == null)
                    {
                        var insertIlceCommand = new NpgsqlCommand(@"INSERT INTO ""ilceler"" (""IlceAd"") VALUES (@Ilce) RETURNING ""Id""", connection);
                        insertIlceCommand.Parameters.AddWithValue("@Ilce", ilce);
                        ilceId = insertIlceCommand.ExecuteScalar();
                    }

                    // Mahalle ID'sini kontrol et ve ekle
                    var checkMahalleCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""mahalleler"" WHERE ""MahalleAd"" = @Mahalle", connection);
                    checkMahalleCommand.Parameters.AddWithValue("@Mahalle", mahalle);
                    var mahalleId = checkMahalleCommand.ExecuteScalar();
                    if (mahalleId == null)
                    {
                        var insertMahalleCommand = new NpgsqlCommand(@"INSERT INTO ""mahalleler"" (""MahalleAd"") VALUES (@Mahalle) RETURNING ""Id""", connection);
                        insertMahalleCommand.Parameters.AddWithValue("@Mahalle", mahalle);
                        mahalleId = insertMahalleCommand.ExecuteScalar();
                    }

                    // Sokak ID'sini kontrol et ve ekle
                    var checkSokakCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""sokaklar"" WHERE ""SokakAd"" = @Sokak", connection);
                    checkSokakCommand.Parameters.AddWithValue("@Sokak", sokak);
                    var sokakId = checkSokakCommand.ExecuteScalar();
                    if (sokakId == null)
                    {
                        var insertSokakCommand = new NpgsqlCommand(@"INSERT INTO ""sokaklar"" (""SokakAd"") VALUES (@Sokak) RETURNING ""Id""", connection);
                        insertSokakCommand.Parameters.AddWithValue("@Sokak", sokak);
                        sokakId = insertSokakCommand.ExecuteScalar();
                    }

                    // Posta Kodu ID'sini kontrol et ve ekle
                    var checkPostaKoduCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""postaKodlari"" WHERE ""PostaKoduAd"" = @PostaKodu", connection);
                    checkPostaKoduCommand.Parameters.AddWithValue("@PostaKodu", postaKodu);
                    var postaKoduId = checkPostaKoduCommand.ExecuteScalar();
                    if (postaKoduId == null)
                    {
                        var insertPostaKoduCommand = new NpgsqlCommand(@"INSERT INTO ""postaKodlari"" (""PostaKoduAd"") VALUES (@PostaKodu) RETURNING ""Id""", connection);
                        insertPostaKoduCommand.Parameters.AddWithValue("@PostaKodu", postaKodu);
                        postaKoduId = insertPostaKoduCommand.ExecuteScalar();
                    }

                    // Calisanlar tablosunu güncelleme
                    var updateCalisanCommand = new NpgsqlCommand(@"
                UPDATE ""calisanlar""
                SET ""Ad"" = @Ad, ""Soyad"" = @Soyad, ""Email"" = @Email, ""Telefon"" = @Telefon
                WHERE ""Id"" = @Id", connection);
                    updateCalisanCommand.Parameters.AddWithValue("@Id", calisanId);
                    updateCalisanCommand.Parameters.AddWithValue("@Ad", ad);
                    updateCalisanCommand.Parameters.AddWithValue("@Soyad", soyad);
                    updateCalisanCommand.Parameters.AddWithValue("@Email", email);
                    updateCalisanCommand.Parameters.AddWithValue("@Telefon", telefon);
                    updateCalisanCommand.ExecuteNonQuery();

                    // Adresler tablosunu güncelleme
                    var updateAdresCommand = new NpgsqlCommand(@"
                UPDATE ""adresler""
                SET ""SehirId"" = @SehirId,
                    ""IlceId"" = @IlceId,
                    ""MahalleId"" = @MahalleId,
                    ""SokakId"" = @SokakId,
                    ""PostaKoduId"" = @PostaKoduId
                WHERE ""Id"" = (SELECT ""AdresId"" FROM ""calisanlar"" WHERE ""Id"" = @CalisanId)", connection);
                    updateAdresCommand.Parameters.AddWithValue("@SehirId", sehirId);
                    updateAdresCommand.Parameters.AddWithValue("@IlceId", ilceId);
                    updateAdresCommand.Parameters.AddWithValue("@MahalleId", mahalleId);
                    updateAdresCommand.Parameters.AddWithValue("@SokakId", sokakId);
                    updateAdresCommand.Parameters.AddWithValue("@PostaKoduId", postaKoduId);
                    updateAdresCommand.Parameters.AddWithValue("@CalisanId", calisanId);

                    updateAdresCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Güncellemeler başarılı bir şekilde kaydedildi.");
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

                    // Calisanlar tablosundan silme
                    var deleteCalisanCommand = new NpgsqlCommand(@"DELETE FROM ""calisanlar"" WHERE ""Id"" = @Id", connection);
                    deleteCalisanCommand.Parameters.AddWithValue("@Id", calisanId);
                    deleteCalisanCommand.ExecuteNonQuery();

                    // Adresler tablosundan silme
                    var deleteAdresCommand = new NpgsqlCommand(@"
                DELETE FROM ""adresler""
                WHERE ""Id"" = (SELECT ""AdresId"" FROM ""calisanlar"" WHERE ""Id"" = @Id)", connection);
                    deleteAdresCommand.Parameters.AddWithValue("@Id", calisanId);
                    deleteAdresCommand.ExecuteNonQuery();

                    MessageBox.Show("Personel başarıyla silindi.");

                    // DataGridView'i güncelle
                    VerileriYenile();
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz personeli seçin.");
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

                    // Kiralamalar tablosundaki ilgili kayıtları silme
                    var deleteKiralamalarCommand = new NpgsqlCommand(@"DELETE FROM ""kiralamalar"" WHERE ""MusteriId"" = @MusteriId", connection);
                    deleteKiralamalarCommand.Parameters.AddWithValue("@MusteriId", musteriId);
                    deleteKiralamalarCommand.ExecuteNonQuery();

                    // Musteriler tablosundan silme
                    var deleteMusteriCommand = new NpgsqlCommand(@"DELETE FROM ""musteriler"" WHERE ""Id"" = @Id", connection);
                    deleteMusteriCommand.Parameters.AddWithValue("@Id", musteriId);
                    deleteMusteriCommand.ExecuteNonQuery();

                    // Adresler tablosundan silme
                    var deleteAdresCommand = new NpgsqlCommand(@"
                DELETE FROM ""adresler""
                WHERE ""Id"" = (SELECT ""AdresId"" FROM ""musteriler"" WHERE ""Id"" = @Id)", connection);
                    deleteAdresCommand.Parameters.AddWithValue("@Id", musteriId);
                    deleteAdresCommand.ExecuteNonQuery();

                    MessageBox.Show("Müşteri başarıyla silindi.");

                    // DataGridView'i güncelle
                    VerileriYenileMusteri();
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz müşteriyi seçin.");
            }
        }

        private void VerileriYenileMusteri()
        {
            string query = @"
    SELECT 
        m.""Id"" AS musteri_id, 
        m.""Ad"", 
        m.""Soyad"", 
        m.""Email"", 
        m.""Telefon"",
        se.""SehirAd"" AS sehir,
        il.""IlceAd"" AS ilce,
        ma.""MahalleAd"" AS mahalle,
        so.""SokakAd"" AS sokak,
        pk.""PostaKoduAd"" AS postakodu,
        k.""KiralamaTarihi"",
        k.""KiralamaBitisTarihi"",
        f.""Tutar"" AS toplam_tutar
    FROM ""musteriler"" m
    JOIN ""adresler"" a ON m.""AdresId"" = a.""Id""
    JOIN ""sehirler"" se ON a.""SehirId"" = se.""Id""
    JOIN ""ilceler"" il ON a.""IlceId"" = il.""Id""
    JOIN ""mahalleler"" ma ON a.""MahalleId"" = ma.""Id""
    JOIN ""sokaklar"" so ON a.""SokakId"" = so.""Id""
    JOIN ""postaKodlari"" pk ON a.""PostaKoduId"" = pk.""Id""
    LEFT JOIN ""kiralamalar"" k ON m.""Id"" = k.""MusteriId""
    LEFT JOIN ""faturalar"" f ON k.""Id"" = f.""KiralamaId"";
";

            using (NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=furkanbabadb0;Database=CarRental"))
            {
                connection.Open();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

                    // Sehir ID'sini kontrol et ve ekle
                    var checkSehirCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""sehirler"" WHERE ""SehirAd"" = @Sehir", connection);
                    checkSehirCommand.Parameters.AddWithValue("@Sehir", sehir);
                    var sehirId = checkSehirCommand.ExecuteScalar();
                    if (sehirId == null)
                    {
                        var insertSehirCommand = new NpgsqlCommand(@"INSERT INTO ""sehirler"" (""SehirAd"") VALUES (@Sehir) RETURNING ""Id""", connection);
                        insertSehirCommand.Parameters.AddWithValue("@Sehir", sehir);
                        sehirId = insertSehirCommand.ExecuteScalar();
                    }

                    // Ilce ID'sini kontrol et ve ekle
                    var checkIlceCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""ilceler"" WHERE ""IlceAd"" = @Ilce", connection);
                    checkIlceCommand.Parameters.AddWithValue("@Ilce", ilce);
                    var ilceId = checkIlceCommand.ExecuteScalar();
                    if (ilceId == null)
                    {
                        var insertIlceCommand = new NpgsqlCommand(@"INSERT INTO ""ilceler"" (""IlceAd"") VALUES (@Ilce) RETURNING ""Id""", connection);
                        insertIlceCommand.Parameters.AddWithValue("@Ilce", ilce);
                        ilceId = insertIlceCommand.ExecuteScalar();
                    }

                    // Mahalle ID'sini kontrol et ve ekle
                    var checkMahalleCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""mahalleler"" WHERE ""MahalleAd"" = @Mahalle", connection);
                    checkMahalleCommand.Parameters.AddWithValue("@Mahalle", mahalle);
                    var mahalleId = checkMahalleCommand.ExecuteScalar();
                    if (mahalleId == null)
                    {
                        var insertMahalleCommand = new NpgsqlCommand(@"INSERT INTO ""mahalleler"" (""MahalleAd"") VALUES (@Mahalle) RETURNING ""Id""", connection);
                        insertMahalleCommand.Parameters.AddWithValue("@Mahalle", mahalle);
                        mahalleId = insertMahalleCommand.ExecuteScalar();
                    }

                    // Sokak ID'sini kontrol et ve ekle
                    var checkSokakCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""sokaklar"" WHERE ""SokakAd"" = @Sokak", connection);
                    checkSokakCommand.Parameters.AddWithValue("@Sokak", sokak);
                    var sokakId = checkSokakCommand.ExecuteScalar();
                    if (sokakId == null)
                    {
                        var insertSokakCommand = new NpgsqlCommand(@"INSERT INTO ""sokaklar"" (""SokakAd"") VALUES (@Sokak) RETURNING ""Id""", connection);
                        insertSokakCommand.Parameters.AddWithValue("@Sokak", sokak);
                        sokakId = insertSokakCommand.ExecuteScalar();
                    }

                    // Posta Kodu ID'sini kontrol et ve ekle
                    var checkPostaKoduCommand = new NpgsqlCommand(@"SELECT ""Id"" FROM ""postaKodlari"" WHERE ""PostaKoduAd"" = @PostaKodu", connection);
                    checkPostaKoduCommand.Parameters.AddWithValue("@PostaKodu", postaKodu);
                    var postaKoduId = checkPostaKoduCommand.ExecuteScalar();
                    if (postaKoduId == null)
                    {
                        var insertPostaKoduCommand = new NpgsqlCommand(@"INSERT INTO ""postaKodlari"" (""PostaKoduAd"") VALUES (@PostaKodu) RETURNING ""Id""", connection);
                        insertPostaKoduCommand.Parameters.AddWithValue("@PostaKodu", postaKodu);
                        postaKoduId = insertPostaKoduCommand.ExecuteScalar();
                    }

                    // Adresler tablosunu güncelleme
                    var updateAdresCommand = new NpgsqlCommand(@"
                UPDATE ""adresler""
                SET ""SehirId"" = @SehirId,
                    ""IlceId"" = @IlceId,
                    ""MahalleId"" = @MahalleId,
                    ""SokakId"" = @SokakId,
                    ""PostaKoduId"" = @PostaKoduId
                WHERE ""Id"" = (SELECT ""AdresId"" FROM ""musteriler"" WHERE ""Id"" = @MusteriId)", connection);
                    updateAdresCommand.Parameters.AddWithValue("@SehirId", sehirId);
                    updateAdresCommand.Parameters.AddWithValue("@IlceId", ilceId);
                    updateAdresCommand.Parameters.AddWithValue("@MahalleId", mahalleId);
                    updateAdresCommand.Parameters.AddWithValue("@SokakId", sokakId);
                    updateAdresCommand.Parameters.AddWithValue("@PostaKoduId", postaKoduId);
                    updateAdresCommand.Parameters.AddWithValue("@MusteriId", musteriId);
                    updateAdresCommand.ExecuteNonQuery();

                    // Musteriler tablosunu güncelleme
                    var updateMusteriCommand = new NpgsqlCommand(@"
                UPDATE ""musteriler""
                SET ""Ad"" = @Ad, ""Soyad"" = @Soyad, ""Email"" = @Email, ""Telefon"" = @Telefon
                WHERE ""Id"" = @Id", connection);
                    updateMusteriCommand.Parameters.AddWithValue("@Id", musteriId);
                    updateMusteriCommand.Parameters.AddWithValue("@Ad", ad);
                    updateMusteriCommand.Parameters.AddWithValue("@Soyad", soyad);
                    updateMusteriCommand.Parameters.AddWithValue("@Email", email);
                    updateMusteriCommand.Parameters.AddWithValue("@Telefon", telefon);
                    updateMusteriCommand.ExecuteNonQuery();

                    MessageBox.Show("Müşteri başarıyla güncellendi.");

                    // DataGridView'i güncelle
                    VerileriYenileMusteri();
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz müşteriyi seçin.");
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
    }
}

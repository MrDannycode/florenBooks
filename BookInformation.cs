using Microsoft.Data.SqlClient;
using System.Data;

namespace WinFormsApp2FlorenBooksV2
{
    /// <summary>
    /// BookInformation - Vizualizare, cautare, editare si stergere carti.
    /// 
    /// CONTROALE NECESARE (adauga din Toolbox):
    ///   - textBoxSearchId     : TextBox      - ID pentru cautare/selectie
    ///   - textBoxTitle        : TextBox      - Titlul cartii
    ///   - textBoxAuthor       : TextBox      - Autorul
    ///   - textBoxPublisher    : TextBox      - Editura
    ///   - textBoxYear         : TextBox      - Anul
    ///   - textBoxISBN         : TextBox      - ISBN
    ///   - textBoxCategory     : TextBox      - Categoria
    ///   - textBoxQuantity     : TextBox      - Exemplare
    ///   - textBoxPrice        : TextBox      - Pret
    ///   - textBoxShelf        : TextBox      - Raft
    ///   - dataGridView1       : DataGridView - Afisare lista carti
    ///   - buttonSearch        : Button       - Cauta dupa ID
    ///   - buttonUpdate        : Button       - Actualizeaza cartea
    ///   - buttonDelete        : Button       - Sterge cartea
    ///   - buttonClear         : Button       - Sterge campurile
    ///   - labelSearchId       : Label        - "ID Carte:"
    ///   (+ restul labelurilor corespunzatoare campurilor)
    /// 
    /// TABEL SQL: book(id, title, author, publisher, year, isbn, category, quantity, price, shelf)
    /// </summary>
    public partial class BookInformation : Form
    {
        public BookInformation()
        {
            InitializeComponent();
        }

        // =====================================================================
        // EVENT: incarca toate cartile la deschiderea formularului
        // =====================================================================
        private void BookInformation_Load(object sender, EventArgs e)
        {
            LoadAllBooks();
        }

        // =====================================================================
        // EVENT: Cauta o carte dupa ID
        // =====================================================================
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearchId.Text))
            {
                MessageBox.Show("Introdu ID-ul cartii pentru cautare!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBoxSearchId.Text.Trim(), out int bookId))
            {
                MessageBox.Show("ID-ul trebuie sa fie un numar intreg!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "SELECT title, author, publisher, year, isbn, category, quantity, price, shelf FROM book WHERE id = @id";
                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", bookId);
                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    textBoxTitle.Text      = dr.GetValue(0).ToString();
                    textBoxAuthor.Text     = dr.GetValue(1).ToString();
                    textBoxPublisher.Text  = dr.GetValue(2).ToString();
                    textBoxYear.Text       = dr.GetValue(3).ToString();
                    textBoxISBN.Text       = dr.GetValue(4).ToString();
                    textBoxCategory.Text   = dr.GetValue(5).ToString();
                    textBoxQuantity.Text   = dr.GetValue(6).ToString();
                    textBoxPrice.Text      = dr.GetValue(7).ToString();
                    textBoxShelf.Text      = dr.GetValue(8).ToString();
                }
                else
                {
                    MessageBox.Show($"Nu exista o carte cu ID-ul {bookId}.", "florenBooks",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare DB: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: Actualizeaza cartea selectata
        // =====================================================================
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearchId.Text))
            {
                MessageBox.Show("Cauta mai intai o carte dupa ID!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();

                string query = @"UPDATE book SET
                                    title     = @title,
                                    author    = @author,
                                    publisher = @publisher,
                                    year      = @year,
                                    isbn      = @isbn,
                                    category  = @category,
                                    quantity  = @quantity,
                                    price     = @price,
                                    shelf     = @shelf
                                 WHERE id = @id";

                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@title",     textBoxTitle.Text.Trim());
                cmd.Parameters.AddWithValue("@author",    textBoxAuthor.Text.Trim());
                cmd.Parameters.AddWithValue("@publisher", textBoxPublisher.Text.Trim());
                cmd.Parameters.AddWithValue("@year",      textBoxYear.Text.Trim());
                cmd.Parameters.AddWithValue("@isbn",      textBoxISBN.Text.Trim());
                cmd.Parameters.AddWithValue("@category",  textBoxCategory.Text.Trim());
                cmd.Parameters.AddWithValue("@quantity",  Convert.ToInt32(textBoxQuantity.Text.Trim()));
                cmd.Parameters.AddWithValue("@price",     Convert.ToDecimal(textBoxPrice.Text.Trim()));
                cmd.Parameters.AddWithValue("@shelf",     textBoxShelf.Text.Trim());
                cmd.Parameters.AddWithValue("@id",        Convert.ToInt32(textBoxSearchId.Text.Trim()));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Cartea a fost actualizata cu succes!", "florenBooks",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields();
                LoadAllBooks();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare la actualizare: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: Sterge cartea selectata
        // =====================================================================
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearchId.Text))
            {
                MessageBox.Show("Introdu ID-ul cartii pe care vrei sa o stergi!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Esti sigur ca vrei sa stergi cartea cu ID {textBoxSearchId.Text}?",
                "Confirmare stergere",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "DELETE FROM book WHERE id = @id";
                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBoxSearchId.Text.Trim()));
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cartea a fost stearsa cu succes!", "florenBooks",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields();
                LoadAllBooks();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare la stergere: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: Sterge campurile
        // =====================================================================
        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // =====================================================================
        // METODE PRIVATE
        // =====================================================================

        private void LoadAllBooks()
        {
            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "SELECT * FROM book ORDER BY id";
                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = new BindingSource(dt, null);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare la incarcare: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            textBoxSearchId.Text   = string.Empty;
            textBoxTitle.Text      = string.Empty;
            textBoxAuthor.Text     = string.Empty;
            textBoxPublisher.Text  = string.Empty;
            textBoxYear.Text       = string.Empty;
            textBoxISBN.Text       = string.Empty;
            textBoxCategory.Text   = string.Empty;
            textBoxQuantity.Text   = string.Empty;
            textBoxPrice.Text      = string.Empty;
            textBoxShelf.Text      = string.Empty;
        }
    }
}

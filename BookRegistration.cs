using Microsoft.Data.SqlClient;

namespace WinFormsApp2FlorenBooksV2
{
    /// <summary>
    /// BookRegistration - Inregistrarea unei carti noi in biblioteca.
    /// 
    /// CONTROALE NECESARE (adauga din Toolbox):
    ///   - textBoxBookId       : TextBox  - ID carte (readonly, auto-generat)
    ///   - textBoxTitle        : TextBox  - Titlul cartii
    ///   - textBoxAuthor       : TextBox  - Autorul
    ///   - textBoxPublisher    : TextBox  - Editura
    ///   - textBoxYear         : TextBox  - Anul aparitiei
    ///   - textBoxISBN         : TextBox  - Codul ISBN
    ///   - textBoxCategory     : TextBox  - Categoria/Genul
    ///   - textBoxQuantity     : TextBox  - Numarul de exemplare
    ///   - textBoxPrice        : TextBox  - Pretul
    ///   - textBoxShelf        : TextBox  - Locatia pe raft (ex: A1, B3)
    ///   - buttonSave          : Button   - Salveaza cartea
    ///   - buttonClear         : Button   - Sterge campurile
    ///   - labelBookId         : Label    - "ID Carte:"
    ///   - labelTitle          : Label    - "Titlu:"
    ///   - labelAuthor         : Label    - "Autor:"
    ///   - labelPublisher      : Label    - "Editura:"
    ///   - labelYear           : Label    - "Anul aparitiei:"
    ///   - labelISBN           : Label    - "ISBN:"
    ///   - labelCategory       : Label    - "Categorie:"
    ///   - labelQuantity       : Label    - "Numar exemplare:"
    ///   - labelPrice          : Label    - "Pret (RON):"
    ///   - labelShelf          : Label    - "Locatie raft:"
    /// 
    /// TABEL SQL: book(id, title, author, publisher, year, isbn, category, quantity, price, shelf)
    /// </summary>
    public partial class BookRegistration : Form
    {
        public BookRegistration()
        {
            InitializeComponent();
        }

        // =====================================================================
        // EVENT: la incarcarea formularului - genereaza urmatorul ID disponibil
        // =====================================================================
        private void BookRegistration_Load(object sender, EventArgs e)
        {
            LoadNextId();
        }

        private void LoadNextId()
        {
            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "SELECT ISNULL(MAX(id), 0) + 1 FROM book;";
                using SqlCommand cmd = new SqlCommand(query, con);
                object result = cmd.ExecuteScalar();
                textBoxBookId.Text = result.ToString();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare conexiune DB: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: Salveaza cartea in baza de date
        // =====================================================================
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();

                string query = @"INSERT INTO book (title, author, publisher, year, isbn, category, quantity, price, shelf)
                                 VALUES (@title, @author, @publisher, @year, @isbn, @category, @quantity, @price, @shelf)";

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

                cmd.ExecuteNonQuery();

                MessageBox.Show("Cartea a fost inregistrata cu succes!", "florenBooks",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields();
                LoadNextId();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare la salvare: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: Sterge toate campurile
        // =====================================================================
        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // =====================================================================
        // METODE PRIVATE
        // =====================================================================

        private void ClearFields()
        {
            textBoxTitle.Text      = string.Empty;
            textBoxAuthor.Text     = string.Empty;
            textBoxPublisher.Text  = string.Empty;
            textBoxYear.Text       = string.Empty;
            textBoxISBN.Text       = string.Empty;
            textBoxCategory.Text   = string.Empty;
            textBoxQuantity.Text   = string.Empty;
            textBoxPrice.Text      = string.Empty;
            textBoxShelf.Text      = string.Empty;
            textBoxTitle.Focus();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
            {
                MessageBox.Show("Titlul cartii este obligatoriu!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxTitle.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxAuthor.Text))
            {
                MessageBox.Show("Autorul este obligatoriu!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxAuthor.Focus();
                return false;
            }
            if (!int.TryParse(textBoxQuantity.Text, out _))
            {
                MessageBox.Show("Numarul de exemplare trebuie sa fie un numar intreg!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxQuantity.Focus();
                return false;
            }
            if (!decimal.TryParse(textBoxPrice.Text, out _))
            {
                MessageBox.Show("Pretul trebuie sa fie un numar valid!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPrice.Focus();
                return false;
            }
            return true;
        }
    }
}

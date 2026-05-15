using Microsoft.Data.SqlClient;

namespace WinFormsApp2FlorenBooksV2
{
    /// <summary>
    /// BookIssue - Imprumutarea unei carti unui membru.
    /// 
    /// CONTROALE NECESARE (adauga din Toolbox):
    ///   - textBoxIssueId      : TextBox  - ID imprumut (readonly, auto-generat)
    ///   - textBoxMemberId     : TextBox  - ID Membru (la schimbare → auto-populate)
    ///   - textBoxMemberName   : TextBox  - Numele membrului (readonly, completat auto)
    ///   - textBoxBookId       : TextBox  - ID Carte (la schimbare → auto-populate)
    ///   - textBoxBookTitle    : TextBox  - Titlul cartii (readonly, completat auto)
    ///   - textBoxBookAuthor   : TextBox  - Autorul (readonly, completat auto)
    ///   - textBoxIssueDate    : TextBox  - Data imprumutului
    ///   - textBoxDueDate      : TextBox  - Data scadenta (returnare)
    ///   - textBoxFinePerDay   : TextBox  - Penalizare zilnica (RON)
    ///   - textBoxStatus       : TextBox  - Status (implicit: "Imprumutat")
    ///   - buttonIssue         : Button   - Confirma imprumutul
    ///   - buttonClear         : Button   - Sterge campurile
    ///   + Label-uri corespunzatoare
    /// 
    /// TABEL SQL: book_issue(id, member_id, member_name, book_id, book_title, book_author,
    ///                       issue_date, due_date, fine_per_day, status)
    /// </summary>
    public partial class BookIssue : Form
    {
        public BookIssue()
        {
            InitializeComponent();
        }

        // =====================================================================
        // EVENT: la deschidere
        // =====================================================================
        private void BookIssue_Load(object sender, EventArgs e)
        {
            LoadNextId();
            textBoxIssueDate.Text  = DateTime.Now.ToString("dd/MM/yyyy");
            textBoxDueDate.Text    = DateTime.Now.AddDays(14).ToString("dd/MM/yyyy");  // 2 saptamani
            textBoxFinePerDay.Text = "1.00";
            textBoxStatus.Text     = "Imprumutat";
        }

        private void LoadNextId()
        {
            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "SELECT ISNULL(MAX(id), 0) + 1 FROM book_issue;";
                using SqlCommand cmd = new SqlCommand(query, con);
                textBoxIssueId.Text = cmd.ExecuteScalar().ToString();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare conexiune DB: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: la schimbarea ID-ului membrului → completeaza numele automat
        // =====================================================================
        private void textBoxMemberId_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxMemberId.Text)) return;
            if (!int.TryParse(textBoxMemberId.Text.Trim(), out int memberId)) return;

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "SELECT name FROM member WHERE id = @id";
                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", memberId);
                object result = cmd.ExecuteScalar();

                if (result != null)
                    textBoxMemberName.Text = result.ToString();
                else
                {
                    MessageBox.Show($"Nu exista un membru cu ID-ul {memberId}.", "florenBooks",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxMemberId.Text   = string.Empty;
                    textBoxMemberName.Text = string.Empty;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare DB: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: la schimbarea ID-ului cartii → completeaza titlul si autorul
        // =====================================================================
        private void textBoxBookId_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxBookId.Text)) return;
            if (!int.TryParse(textBoxBookId.Text.Trim(), out int bookId)) return;

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "SELECT title, author, quantity FROM book WHERE id = @id";
                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", bookId);
                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    int qty = Convert.ToInt32(dr.GetValue(2));
                    if (qty <= 0)
                    {
                        MessageBox.Show("Aceasta carte nu mai are exemplare disponibile!", "florenBooks",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBoxBookId.Text    = string.Empty;
                        textBoxBookTitle.Text = string.Empty;
                        textBoxBookAuthor.Text = string.Empty;
                        return;
                    }
                    textBoxBookTitle.Text  = dr.GetValue(0).ToString();
                    textBoxBookAuthor.Text = dr.GetValue(1).ToString();
                }
                else
                {
                    MessageBox.Show($"Nu exista o carte cu ID-ul {bookId}.", "florenBooks",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxBookId.Text    = string.Empty;
                    textBoxBookTitle.Text = string.Empty;
                    textBoxBookAuthor.Text = string.Empty;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare DB: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: Confirma imprumutul
        // =====================================================================
        private void buttonIssue_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();

                // 1) Inregistreaza imprumutul
                string insertQuery = @"INSERT INTO book_issue (member_id, member_name, book_id, book_title, book_author,
                                                               issue_date, due_date, fine_per_day, status)
                                       VALUES (@member_id, @member_name, @book_id, @book_title, @book_author,
                                               @issue_date, @due_date, @fine_per_day, @status)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@member_id",   Convert.ToInt32(textBoxMemberId.Text.Trim()));
                    cmd.Parameters.AddWithValue("@member_name", textBoxMemberName.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_id",     Convert.ToInt32(textBoxBookId.Text.Trim()));
                    cmd.Parameters.AddWithValue("@book_title",  textBoxBookTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_author", textBoxBookAuthor.Text.Trim());
                    cmd.Parameters.AddWithValue("@issue_date",  textBoxIssueDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@due_date",    textBoxDueDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@fine_per_day",Convert.ToDecimal(textBoxFinePerDay.Text.Trim()));
                    cmd.Parameters.AddWithValue("@status",      textBoxStatus.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                // 2) Scade din stocul cartii
                string updateQuery = "UPDATE book SET quantity = quantity - 1 WHERE id = @id";
                using (SqlCommand cmdUpd = new SqlCommand(updateQuery, con))
                {
                    cmdUpd.Parameters.AddWithValue("@id", Convert.ToInt32(textBoxBookId.Text.Trim()));
                    cmdUpd.ExecuteNonQuery();
                }

                MessageBox.Show(
                    $"Imprumutul a fost inregistrat cu succes!\n\n" +
                    $"Carte: {textBoxBookTitle.Text}\n" +
                    $"Membru: {textBoxMemberName.Text}\n" +
                    $"Data returnare: {textBoxDueDate.Text}",
                    "florenBooks - Imprumut confirmat",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields();
                LoadNextId();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare la inregistrare: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: Curata campurile
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
            textBoxMemberId.Text    = string.Empty;
            textBoxMemberName.Text  = string.Empty;
            textBoxBookId.Text      = string.Empty;
            textBoxBookTitle.Text   = string.Empty;
            textBoxBookAuthor.Text  = string.Empty;
            textBoxIssueDate.Text   = DateTime.Now.ToString("dd/MM/yyyy");
            textBoxDueDate.Text     = DateTime.Now.AddDays(14).ToString("dd/MM/yyyy");
            textBoxFinePerDay.Text  = "1.00";
            textBoxStatus.Text      = "Imprumutat";
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(textBoxMemberId.Text) || string.IsNullOrWhiteSpace(textBoxMemberName.Text))
            {
                MessageBox.Show("Selecteaza un membru valid!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxBookId.Text) || string.IsNullOrWhiteSpace(textBoxBookTitle.Text))
            {
                MessageBox.Show("Selecteaza o carte valida!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}

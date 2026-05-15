using Microsoft.Data.SqlClient;
using System.Data;

namespace WinFormsApp2FlorenBooksV2
{
    /// <summary>
    /// BookReturn - Returnarea unei carti imprumutate cu calculul penalizarilor.
    /// 
    /// CONTROALE NECESARE (adauga din Toolbox):
    ///   - textBoxIssueId      : TextBox  - ID imprumut (cauta dupa acesta)
    ///   - textBoxMemberName   : TextBox  - Numele membrului (readonly, completat auto)
    ///   - textBoxBookTitle    : TextBox  - Titlul cartii (readonly, completat auto)
    ///   - textBoxBookAuthor   : TextBox  - Autorul (readonly, completat auto)
    ///   - textBoxIssueDate    : TextBox  - Data imprumutului (readonly)
    ///   - textBoxDueDate      : TextBox  - Data scadenta (readonly)
    ///   - textBoxReturnDate   : TextBox  - Data returnarii (implicit: azi)
    ///   - textBoxFinePerDay   : TextBox  - Penalizare/zi (readonly)
    ///   - textBoxDaysLate     : TextBox  - Zile intarziere (readonly, calculat auto)
    ///   - textBoxTotalFine    : TextBox  - Penalizare totala (readonly, calculat auto)
    ///   - textBoxStatus       : TextBox  - Status final (implicit: "Returnat")
    ///   - buttonSearch        : Button   - Cauta imprumut dupa ID
    ///   - buttonCalculate     : Button   - Calculeaza penalizarea
    ///   - buttonReturn        : Button   - Confirma returnarea
    ///   - buttonClear         : Button   - Curata campurile
    ///   + Label-uri corespunzatoare
    /// 
    /// TABEL SQL: book_issue (id, member_name, book_title, book_author, book_id,
    ///                        issue_date, due_date, fine_per_day, status, return_date, total_fine)
    /// </summary>
    public partial class BookReturn : Form
    {
        public BookReturn()
        {
            InitializeComponent();
        }

        // =====================================================================
        // EVENT: la deschidere
        // =====================================================================
        private void BookReturn_Load(object sender, EventArgs e)
        {
            textBoxReturnDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            textBoxStatus.Text     = "Returnat";
        }

        // =====================================================================
        // EVENT: Cauta imprumutul dupa ID
        // =====================================================================
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxIssueId.Text.Trim(), out int issueId))
            {
                MessageBox.Show("Introdu un ID de imprumut valid!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = @"SELECT member_name, book_title, book_author, book_id,
                                        issue_date, due_date, fine_per_day, status
                                 FROM book_issue WHERE id = @id";
                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", issueId);
                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (dr["status"].ToString() == "Returnat")
                    {
                        MessageBox.Show("Aceasta carte a fost deja returnata!", "florenBooks",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    textBoxMemberName.Text = dr["member_name"].ToString();
                    textBoxBookTitle.Text  = dr["book_title"].ToString();
                    textBoxBookAuthor.Text = dr["book_author"].ToString();
                    textBoxBookIdHidden    = dr["book_id"].ToString();
                    textBoxIssueDate.Text  = dr["issue_date"].ToString();
                    textBoxDueDate.Text    = dr["due_date"].ToString();
                    textBoxFinePerDay.Text = dr["fine_per_day"].ToString();
                }
                else
                {
                    MessageBox.Show($"Nu exista un imprumut cu ID-ul {issueId}.", "florenBooks",
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
        // EVENT: Calculeaza penalizarea
        // =====================================================================
        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxDueDate.Text) || string.IsNullOrWhiteSpace(textBoxReturnDate.Text))
            {
                MessageBox.Show("Cauta mai intai un imprumut!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Parsam datele
            if (!DateTime.TryParseExact(textBoxDueDate.Text.Trim(), "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime dueDate))
            {
                MessageBox.Show("Formatul datei scadente este invalid! Foloseste: zz/ll/aaaa", "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!DateTime.TryParseExact(textBoxReturnDate.Text.Trim(), "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime returnDate))
            {
                MessageBox.Show("Formatul datei de returnare este invalid! Foloseste: zz/ll/aaaa", "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int daysLate = (int)(returnDate - dueDate).TotalDays;
            if (daysLate < 0) daysLate = 0;

            decimal finePerDay = decimal.TryParse(textBoxFinePerDay.Text.Trim(), out decimal fpd) ? fpd : 1m;
            decimal totalFine  = daysLate * finePerDay;

            textBoxDaysLate.Text  = daysLate.ToString();
            textBoxTotalFine.Text = totalFine.ToString("F2");
        }

        // =====================================================================
        // EVENT: Confirma returnarea
        // =====================================================================
        private void buttonReturn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxIssueId.Text) || string.IsNullOrWhiteSpace(textBoxMemberName.Text))
            {
                MessageBox.Show("Cauta mai intai un imprumut!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculeaza penalizarea daca nu s-a facut
            buttonCalculate_Click(sender, e);

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();

                // 1) Actualizeaza statusul imprumutului
                string updateIssue = @"UPDATE book_issue SET
                                           status      = 'Returnat',
                                           return_date = @return_date,
                                           total_fine  = @total_fine
                                       WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(updateIssue, con))
                {
                    cmd.Parameters.AddWithValue("@return_date", textBoxReturnDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@total_fine",  decimal.TryParse(textBoxTotalFine.Text, out decimal tf) ? tf : 0m);
                    cmd.Parameters.AddWithValue("@id",          Convert.ToInt32(textBoxIssueId.Text.Trim()));
                    cmd.ExecuteNonQuery();
                }

                // 2) Creste din nou stocul cartii
                if (!string.IsNullOrWhiteSpace(textBoxBookIdHidden))
                {
                    string updateBook = "UPDATE book SET quantity = quantity + 1 WHERE id = @id";
                    using SqlCommand cmdUpd = new SqlCommand(updateBook, con);
                    cmdUpd.Parameters.AddWithValue("@id", Convert.ToInt32(textBoxBookIdHidden));
                    cmdUpd.ExecuteNonQuery();
                }

                decimal fine = decimal.TryParse(textBoxTotalFine.Text, out decimal f) ? f : 0m;
                string msg = fine > 0
                    ? $"Cartea a fost returnata cu succes!\n\nPenalizare totala de platit: {fine:F2} RON"
                    : "Cartea a fost returnata la timp. Fara penalizare!";

                MessageBox.Show(msg, "florenBooks - Returnare confirmata",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare la returnare: " + ex.Message, "Eroare",
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
            textBoxIssueId.Text    = string.Empty;
            textBoxMemberName.Text = string.Empty;
            textBoxBookTitle.Text  = string.Empty;
            textBoxBookAuthor.Text = string.Empty;
            textBoxIssueDate.Text  = string.Empty;
            textBoxDueDate.Text    = string.Empty;
            textBoxReturnDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            textBoxFinePerDay.Text = string.Empty;
            textBoxDaysLate.Text   = string.Empty;
            textBoxTotalFine.Text  = string.Empty;
            textBoxStatus.Text     = "Returnat";
            textBoxBookIdHidden    = string.Empty;
        }

        // Camp intern (nu este un control vizibil) - stocheaza ID-ul cartii
        private string textBoxBookIdHidden = string.Empty;
    }
}

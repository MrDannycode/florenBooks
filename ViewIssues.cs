using Microsoft.Data.SqlClient;
using System.Data;

namespace WinFormsApp2FlorenBooksV2
{
    /// <summary>
    /// ViewIssues - Vizualizarea tuturor imprumuturilor (active si returnate).
    /// 
    /// CONTROALE NECESARE (adauga din Toolbox):
    ///   - textBoxSearchId     : TextBox      - ID imprumut pentru filtrare (optional)
    ///   - dataGridView1       : DataGridView - Afisare lista imprumuturi
    ///   - buttonSearch        : Button       - Cauta dupa ID
    ///   - buttonShowAll       : Button       - Afiseaza toate inregistrarile
    ///   - buttonShowActive    : Button       - Afiseaza doar imprumuturile active
    ///   - buttonShowReturned  : Button       - Afiseaza doar returnate
    ///   - labelSearchId       : Label        - "ID Imprumut:"
    ///   - labelTitle          : Label        - "Lista Imprumuturi florenBooks"
    /// 
    /// TABEL SQL: book_issue(id, member_id, member_name, book_id, book_title, book_author,
    ///                       issue_date, due_date, fine_per_day, status, return_date, total_fine)
    /// </summary>
    public partial class ViewIssues : Form
    {
        public ViewIssues()
        {
            InitializeComponent();
        }

        // =====================================================================
        // EVENT: la deschidere - incarca toate inregistrarile
        // =====================================================================
        private void ViewIssues_Load(object sender, EventArgs e)
        {
            LoadIssues("ALL");
        }

        // =====================================================================
        // EVENT: Cauta dupa ID imprumut
        // =====================================================================
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearchId.Text))
            {
                LoadIssues("ALL");
                return;
            }

            if (!int.TryParse(textBoxSearchId.Text.Trim(), out int issueId))
            {
                MessageBox.Show("Introdu un ID numeric valid!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "SELECT * FROM book_issue WHERE id = @id";
                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@id", issueId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = new BindingSource(dt, null);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare DB: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: Afiseaza toate inregistrarile
        // =====================================================================
        private void buttonShowAll_Click(object sender, EventArgs e)
        {
            textBoxSearchId.Text = string.Empty;
            LoadIssues("ALL");
        }

        // =====================================================================
        // EVENT: Afiseaza doar imprumuturile active
        // =====================================================================
        private void buttonShowActive_Click(object sender, EventArgs e)
        {
            LoadIssues("Imprumutat");
        }

        // =====================================================================
        // EVENT: Afiseaza doar returnate
        // =====================================================================
        private void buttonShowReturned_Click(object sender, EventArgs e)
        {
            LoadIssues("Returnat");
        }

        // =====================================================================
        // METODE PRIVATE
        // =====================================================================

        private void LoadIssues(string filter)
        {
            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();

                string query = filter == "ALL"
                    ? "SELECT * FROM book_issue ORDER BY id DESC"
                    : "SELECT * FROM book_issue WHERE status = @status ORDER BY id DESC";

                using SqlDataAdapter da = new SqlDataAdapter(query, con);
                if (filter != "ALL")
                    da.SelectCommand.Parameters.AddWithValue("@status", filter);

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
    }
}

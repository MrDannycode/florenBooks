using Microsoft.Data.SqlClient;
using System.Data;

namespace WinFormsApp2FlorenBooksV2
{
    /// <summary>
    /// MemberInformation - Vizualizare, cautare, editare si stergere membri.
    /// 
    /// CONTROALE NECESARE (adauga din Toolbox):
    ///   - textBoxSearchId     : TextBox      - ID pentru cautare
    ///   - textBoxName         : TextBox      - Numele
    ///   - textBoxPhone        : TextBox      - Telefon
    ///   - textBoxEmail        : TextBox      - Email
    ///   - textBoxAddress      : TextBox      - Adresa
    ///   - textBoxDateJoined   : TextBox      - Data inscrierii
    ///   - textBoxMemberType   : TextBox      - Tipul membrului
    ///   - textBoxMaxBooks     : TextBox      - Numar maxim carti
    ///   - radioButtonMale     : RadioButton  - Masculin
    ///   - radioButtonFemale   : RadioButton  - Feminin
    ///   - dataGridView1       : DataGridView - Lista membri
    ///   - buttonSearch        : Button       - Cauta
    ///   - buttonUpdate        : Button       - Actualizeaza
    ///   - buttonDelete        : Button       - Sterge
    ///   - buttonClear         : Button       - Curata campurile
    ///   + Label-uri corespunzatoare
    /// 
    /// TABEL SQL: member(id, name, gender, phone, email, address, date_joined, member_type, max_books)
    /// </summary>
    public partial class MemberInformation : Form
    {
        public MemberInformation()
        {
            InitializeComponent();
        }

        // =====================================================================
        // EVENT: Incarca toti membrii la start
        // =====================================================================
        private void MemberInformation_Load(object sender, EventArgs e)
        {
            LoadAllMembers();
        }

        // =====================================================================
        // EVENT: Cauta dupa ID
        // =====================================================================
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxSearchId.Text.Trim(), out int memberId))
            {
                MessageBox.Show("Introdu un ID numeric valid!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "SELECT name, gender, phone, email, address, date_joined, member_type, max_books FROM member WHERE id = @id";
                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", memberId);
                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    textBoxName.Text       = dr.GetValue(0).ToString();
                    string gender          = dr.GetValue(1).ToString();
                    radioButtonMale.Checked   = gender == "Masculin";
                    radioButtonFemale.Checked = gender != "Masculin";
                    textBoxPhone.Text      = dr.GetValue(2).ToString();
                    textBoxEmail.Text      = dr.GetValue(3).ToString();
                    textBoxAddress.Text    = dr.GetValue(4).ToString();
                    textBoxDateJoined.Text = dr.GetValue(5).ToString();
                    textBoxMemberType.Text = dr.GetValue(6).ToString();
                    textBoxMaxBooks.Text   = dr.GetValue(7).ToString();
                }
                else
                {
                    MessageBox.Show($"Nu exista un membru cu ID-ul {memberId}.", "florenBooks",
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
        // EVENT: Actualizeaza membrul
        // =====================================================================
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearchId.Text))
            {
                MessageBox.Show("Cauta mai intai un membru dupa ID!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string gender = radioButtonMale.Checked ? "Masculin" : "Feminin";

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = @"UPDATE member SET
                                    name        = @name,
                                    gender      = @gender,
                                    phone       = @phone,
                                    email       = @email,
                                    address     = @address,
                                    date_joined = @date_joined,
                                    member_type = @member_type,
                                    max_books   = @max_books
                                 WHERE id = @id";

                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name",        textBoxName.Text.Trim());
                cmd.Parameters.AddWithValue("@gender",      gender);
                cmd.Parameters.AddWithValue("@phone",       textBoxPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@email",       textBoxEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@address",     textBoxAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@date_joined", textBoxDateJoined.Text.Trim());
                cmd.Parameters.AddWithValue("@member_type", textBoxMemberType.Text.Trim());
                cmd.Parameters.AddWithValue("@max_books",   Convert.ToInt32(textBoxMaxBooks.Text.Trim()));
                cmd.Parameters.AddWithValue("@id",          Convert.ToInt32(textBoxSearchId.Text.Trim()));
                cmd.ExecuteNonQuery();

                MessageBox.Show($"{textBoxName.Text} a fost actualizat cu succes!", "florenBooks",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields();
                LoadAllMembers();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare la actualizare: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: Sterge membrul
        // =====================================================================
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearchId.Text))
            {
                MessageBox.Show("Introdu ID-ul membrului pe care vrei sa il stergi!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Esti sigur ca vrei sa stergi membrul cu ID {textBoxSearchId.Text}?",
                "Confirmare stergere",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "DELETE FROM member WHERE id = @id";
                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(textBoxSearchId.Text.Trim()));
                cmd.ExecuteNonQuery();

                MessageBox.Show("Membrul a fost sters cu succes!", "florenBooks",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields();
                LoadAllMembers();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare la stergere: " + ex.Message, "Eroare",
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

        private void LoadAllMembers()
        {
            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "SELECT * FROM member ORDER BY id";
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
            textBoxName.Text       = string.Empty;
            textBoxPhone.Text      = string.Empty;
            textBoxEmail.Text      = string.Empty;
            textBoxAddress.Text    = string.Empty;
            textBoxDateJoined.Text = string.Empty;
            textBoxMemberType.Text = string.Empty;
            textBoxMaxBooks.Text   = string.Empty;
            radioButtonMale.Checked = true;
        }
    }
}

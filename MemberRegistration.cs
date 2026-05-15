using Microsoft.Data.SqlClient;

namespace WinFormsApp2FlorenBooksV2
{
    /// <summary>
    /// MemberRegistration - Inregistrarea unui nou membru al bibliotecii.
    /// 
    /// CONTROALE NECESARE (adauga din Toolbox):
    ///   - textBoxMemberId     : TextBox      - ID membru (readonly, auto-generat)
    ///   - textBoxName         : TextBox      - Numele complet
    ///   - textBoxPhone        : TextBox      - Numarul de telefon
    ///   - textBoxEmail        : TextBox      - Adresa email
    ///   - textBoxAddress      : TextBox      - Adresa
    ///   - textBoxDateJoined   : TextBox      - Data inscrierii (format: zz/ll/aaaa)
    ///   - textBoxMemberType   : TextBox      - Tipul membrului (Student/Profesor/Public)
    ///   - textBoxMaxBooks     : TextBox      - Numar maxim de carti imprumutate
    ///   - radioButtonMale     : RadioButton  - Masculin
    ///   - radioButtonFemale   : RadioButton  - Feminin
    ///   - buttonSave          : Button       - Salveaza
    ///   - buttonClear         : Button       - Sterge campurile
    ///   + Label-uri corespunzatoare fiecarui camp
    /// 
    /// TABEL SQL: member(id, name, gender, phone, email, address, date_joined, member_type, max_books)
    /// </summary>
    public partial class MemberRegistration : Form
    {
        public MemberRegistration()
        {
            InitializeComponent();
        }

        // =====================================================================
        // EVENT: la incarcarea formularului
        // =====================================================================
        private void MemberRegistration_Load(object sender, EventArgs e)
        {
            LoadNextId();
            // Data de azi ca valoare implicita
            textBoxDateJoined.Text = DateTime.Now.ToString("dd/MM/yyyy");
            // Valoare implicita numar maxim carti
            textBoxMaxBooks.Text = "3";
        }

        private void LoadNextId()
        {
            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string query = "SELECT ISNULL(MAX(id), 0) + 1 FROM member;";
                using SqlCommand cmd = new SqlCommand(query, con);
                textBoxMemberId.Text = cmd.ExecuteScalar().ToString();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Eroare conexiune DB: " + ex.Message, "Eroare",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================================
        // EVENT: Salveaza membrul
        // =====================================================================
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            string gender = radioButtonMale.Checked ? "Masculin" : "Feminin";

            try
            {
                using SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();

                string query = @"INSERT INTO member (name, gender, phone, email, address, date_joined, member_type, max_books)
                                 VALUES (@name, @gender, @phone, @email, @address, @date_joined, @member_type, @max_books)";

                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name",        textBoxName.Text.Trim());
                cmd.Parameters.AddWithValue("@gender",      gender);
                cmd.Parameters.AddWithValue("@phone",       textBoxPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@email",       textBoxEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@address",     textBoxAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@date_joined", textBoxDateJoined.Text.Trim());
                cmd.Parameters.AddWithValue("@member_type", textBoxMemberType.Text.Trim());
                cmd.Parameters.AddWithValue("@max_books",   Convert.ToInt32(textBoxMaxBooks.Text.Trim()));

                cmd.ExecuteNonQuery();

                MessageBox.Show("Membrul a fost inregistrat cu succes!", "florenBooks",
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
        // EVENT: Sterge campurile
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
            textBoxName.Text        = string.Empty;
            textBoxPhone.Text       = string.Empty;
            textBoxEmail.Text       = string.Empty;
            textBoxAddress.Text     = string.Empty;
            textBoxDateJoined.Text  = DateTime.Now.ToString("dd/MM/yyyy");
            textBoxMemberType.Text  = string.Empty;
            textBoxMaxBooks.Text    = "3";
            radioButtonMale.Checked = true;
            textBoxName.Focus();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Numele membrului este obligatoriu!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxPhone.Text))
            {
                MessageBox.Show("Numarul de telefon este obligatoriu!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPhone.Focus();
                return false;
            }
            if (!int.TryParse(textBoxMaxBooks.Text, out _))
            {
                MessageBox.Show("Numarul maxim de carti trebuie sa fie un numar intreg!", "Validare",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxMaxBooks.Focus();
                return false;
            }
            return true;
        }
    }
}

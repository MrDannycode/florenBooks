namespace WinFormsApp2FlorenBooksV2
{
    /// <summary>
    /// FormLogin - Ecranul de autentificare al aplicatiei florenBooks.
    /// 
    /// CONTROALE NECESARE (adauga din Toolbox):
    ///   - textBoxUsername  : TextBox  - pentru username
    ///   - textBoxPassword  : TextBox  - pentru parola (PasswordChar = '*')
    ///   - buttonLogin      : Button   - buton Login
    ///   - buttonCancel     : Button   - buton Anulare/Iesire
    ///   - labelTitle       : Label    - titlul formularului (ex: "florenBooks - Login")
    ///   - labelUsername    : Label    - eticheta "Username:"
    ///   - labelPassword    : Label    - eticheta "Parola:"
    /// 
    /// Credentiale implicite: admin / admin123
    /// </summary>
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        // =====================================================================
        // EVENT: butonul Login apasat
        // =====================================================================
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            // Credentiale simple (fara baza de date, similar cu proiectul original)
            if (username == "admin" && password == "admin123")
            {
                MessageBox.Show("Bine ai venit, Administrator!\nEsti autentificat cu succes.",
                    "florenBooks - Autentificare", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                Home homeForm = new Home();
                homeForm.ShowDialog();

                // Cand Home se inchide, curatam campurile si reafisam Login
                textBoxUsername.Text = string.Empty;
                textBoxPassword.Text = string.Empty;
                this.Show();
            }
            else
            {
                MessageBox.Show("Username sau parola incorecta!\nIncearca din nou.",
                    "florenBooks - Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPassword.Text = string.Empty;
                textBoxPassword.Focus();
            }
        }

        // =====================================================================
        // EVENT: butonul Cancel / Iesire
        // =====================================================================
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // =====================================================================
        // EVENT: apasare Enter in campul Password → declanseaza Login
        // =====================================================================
        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                buttonLogin_Click(sender, e);
            }
        }
    }
}

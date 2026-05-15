namespace WinFormsApp2FlorenBooksV2
{
    /// <summary>
    /// Home - Fereastra principala cu meniu de navigare pentru florenBooks.
    /// 
    /// CONTROALE NECESARE (adauga din Toolbox):
    ///   - menuStrip1        : MenuStrip  - bara de meniu principala
    ///   - labelWelcome      : Label      - mesaj de bun venit (optional)
    ///   - pictureBox1       : PictureBox - logo sau imagine (optional)
    /// 
    /// STRUCTURA MENIU (creeaza in MenuStrip din Designer):
    ///   Carti
    ///     ├── Inregistrare Carte      → bookRegistrationToolStripMenuItem_Click
    ///     ├── Informatii Carti        → bookInformationToolStripMenuItem_Click
    ///     └── ─────────────────────
    ///   Membri
    ///     ├── Inregistrare Membru     → memberRegistrationToolStripMenuItem_Click
    ///     └── Informatii Membri       → memberInformationToolStripMenuItem_Click
    ///   Imprumuturi
    ///     ├── Imprumut Carte          → bookIssueToolStripMenuItem_Click
    ///     ├── Returnare Carte         → bookReturnToolStripMenuItem_Click
    ///     └── Vizualizare Imprumuturi → viewIssuesToolStripMenuItem_Click
    ///   Ajutor
    ///     ├── Despre aplicatie        → aboutToolStripMenuItem_Click
    ///     └── Inchide aplicatia       → exitToolStripMenuItem_Click
    /// </summary>
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        // =====================================================================
        // CARTI
        // =====================================================================

        private void bookRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookRegistration form = new BookRegistration();
            form.ShowDialog();
        }

        private void bookInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookInformation form = new BookInformation();
            form.ShowDialog();
        }

        // =====================================================================
        // MEMBRI
        // =====================================================================

        private void memberRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberRegistration form = new MemberRegistration();
            form.ShowDialog();
        }

        private void memberInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberInformation form = new MemberInformation();
            form.ShowDialog();
        }

        // =====================================================================
        // IMPRUMUTURI
        // =====================================================================

        private void bookIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookIssue form = new BookIssue();
            form.ShowDialog();
        }

        private void bookReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookReturn form = new BookReturn();
            form.ShowDialog();
        }

        private void viewIssuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewIssues form = new ViewIssues();
            form.ShowDialog();
        }

        // =====================================================================
        // AJUTOR
        // =====================================================================

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "florenBooks - Sistem de Management Biblioteca\n\n" +
                "Versiunea 1.0\n" +
                "Dezvoltat cu C# Windows Forms\n\n" +
                "Credentiale: admin / admin123",
                "Despre florenBooks",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Esti sigur ca vrei sa inchizi aplicatia?",
                "florenBooks - Confirmare",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                Application.Exit();
        }
    }
}

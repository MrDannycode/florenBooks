namespace WinFormsApp2FlorenBooksV2
{
    partial class BookRegistration
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 500);
            Text = "florenBooks - Inregistrare Carte";
            StartPosition = FormStartPosition.CenterParent;

            // Conecteaza evenimentele - Designer le va suprascrie cand adaugi controale
            this.Load += new EventHandler(this.BookRegistration_Load);
        }

        #endregion

        // =====================================================================
        // DECLARATII CONTROALE - adauga-le din Toolbox si leaga evenimentele
        // =====================================================================






















    }
}

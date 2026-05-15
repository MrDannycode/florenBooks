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
        internal TextBox textBoxBookId;
        internal TextBox textBoxTitle;
        internal TextBox textBoxAuthor;
        internal TextBox textBoxPublisher;
        internal TextBox textBoxYear;
        internal TextBox textBoxISBN;
        internal TextBox textBoxCategory;
        internal TextBox textBoxQuantity;
        internal TextBox textBoxPrice;
        internal TextBox textBoxShelf;
        internal Button buttonSave;
        internal Button buttonClear;
        internal Label labelBookId;
        internal Label labelTitle;
        internal Label labelAuthor;
        internal Label labelPublisher;
        internal Label labelYear;
        internal Label labelISBN;
        internal Label labelCategory;
        internal Label labelQuantity;
        internal Label labelPrice;
        internal Label labelShelf;
    }
}

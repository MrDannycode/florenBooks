namespace WinFormsApp2FlorenBooksV2
{
    partial class BookInformation
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
            ClientSize = new Size(900, 600);
            Text = "florenBooks - Informatii Carti";
            StartPosition = FormStartPosition.CenterParent;
            this.Load += new EventHandler(this.BookInformation_Load);
        }

        #endregion

        internal TextBox textBoxSearchId;
        internal TextBox textBoxTitle;
        internal TextBox textBoxAuthor;
        internal TextBox textBoxPublisher;
        internal TextBox textBoxYear;
        internal TextBox textBoxISBN;
        internal TextBox textBoxCategory;
        internal TextBox textBoxQuantity;
        internal TextBox textBoxPrice;
        internal TextBox textBoxShelf;
        internal DataGridView dataGridView1;
        internal Button buttonSearch;
        internal Button buttonUpdate;
        internal Button buttonDelete;
        internal Button buttonClear;
        internal Label labelSearchId;
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

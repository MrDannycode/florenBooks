namespace WinFormsApp2FlorenBooksV2
{
    partial class ViewIssues
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
            ClientSize = new Size(1000, 600);
            Text = "florenBooks - Vizualizare Imprumuturi";
            StartPosition = FormStartPosition.CenterParent;
            this.Load += new EventHandler(this.ViewIssues_Load);
        }

        #endregion

        internal TextBox textBoxSearchId;
        internal DataGridView dataGridView1;
        internal Button buttonSearch;
        internal Button buttonShowAll;
        internal Button buttonShowActive;
        internal Button buttonShowReturned;
        internal Label labelSearchId;
        internal Label labelTitle;
    }
}

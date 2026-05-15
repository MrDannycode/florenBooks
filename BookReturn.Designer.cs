namespace WinFormsApp2FlorenBooksV2
{
    partial class BookReturn
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
            ClientSize = new Size(560, 500);
            Text = "florenBooks - Returnare Carte";
            StartPosition = FormStartPosition.CenterParent;
            this.Load += new EventHandler(this.BookReturn_Load);
        }

        #endregion

        internal TextBox textBoxIssueId;
        internal TextBox textBoxMemberName;
        internal TextBox textBoxBookTitle;
        internal TextBox textBoxBookAuthor;
        internal TextBox textBoxIssueDate;
        internal TextBox textBoxDueDate;
        internal TextBox textBoxReturnDate;
        internal TextBox textBoxFinePerDay;
        internal TextBox textBoxDaysLate;
        internal TextBox textBoxTotalFine;
        internal TextBox textBoxStatus;
        internal Button buttonSearch;
        internal Button buttonCalculate;
        internal Button buttonReturn;
        internal Button buttonClear;
        internal Label labelIssueId;
        internal Label labelMemberName;
        internal Label labelBookTitle;
        internal Label labelBookAuthor;
        internal Label labelIssueDate;
        internal Label labelDueDate;
        internal Label labelReturnDate;
        internal Label labelFinePerDay;
        internal Label labelDaysLate;
        internal Label labelTotalFine;
        internal Label labelStatus;
    }
}

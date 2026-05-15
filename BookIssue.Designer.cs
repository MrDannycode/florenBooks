namespace WinFormsApp2FlorenBooksV2
{
    partial class BookIssue
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
            ClientSize = new Size(560, 480);
            Text = "florenBooks - Imprumut Carte";
            StartPosition = FormStartPosition.CenterParent;
            this.Load += new EventHandler(this.BookIssue_Load);
        }

        #endregion

        internal TextBox textBoxIssueId;
        internal TextBox textBoxMemberId;
        internal TextBox textBoxMemberName;
        internal TextBox textBoxBookId;
        internal TextBox textBoxBookTitle;
        internal TextBox textBoxBookAuthor;
        internal TextBox textBoxIssueDate;
        internal TextBox textBoxDueDate;
        internal TextBox textBoxFinePerDay;
        internal TextBox textBoxStatus;
        internal Button buttonIssue;
        internal Button buttonClear;
        internal Label labelIssueId;
        internal Label labelMemberId;
        internal Label labelMemberName;
        internal Label labelBookId;
        internal Label labelBookTitle;
        internal Label labelBookAuthor;
        internal Label labelIssueDate;
        internal Label labelDueDate;
        internal Label labelFinePerDay;
        internal Label labelStatus;
    }
}

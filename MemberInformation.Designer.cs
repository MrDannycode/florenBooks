namespace WinFormsApp2FlorenBooksV2
{
    partial class MemberInformation
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
            Text = "florenBooks - Informatii Membri";
            StartPosition = FormStartPosition.CenterParent;
            this.Load += new EventHandler(this.MemberInformation_Load);
        }

        #endregion

        internal TextBox textBoxSearchId;
        internal TextBox textBoxName;
        internal TextBox textBoxPhone;
        internal TextBox textBoxEmail;
        internal TextBox textBoxAddress;
        internal TextBox textBoxDateJoined;
        internal TextBox textBoxMemberType;
        internal TextBox textBoxMaxBooks;
        internal RadioButton radioButtonMale;
        internal RadioButton radioButtonFemale;
        internal DataGridView dataGridView1;
        internal Button buttonSearch;
        internal Button buttonUpdate;
        internal Button buttonDelete;
        internal Button buttonClear;
        internal Label labelSearchId;
        internal Label labelName;
        internal Label labelGender;
        internal Label labelPhone;
        internal Label labelEmail;
        internal Label labelAddress;
        internal Label labelDateJoined;
        internal Label labelMemberType;
        internal Label labelMaxBooks;
    }
}

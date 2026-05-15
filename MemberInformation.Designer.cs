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









        internal RadioButton radioButtonMale;
        internal RadioButton radioButtonFemale;














    }
}

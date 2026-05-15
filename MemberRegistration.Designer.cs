namespace WinFormsApp2FlorenBooksV2
{
    partial class MemberRegistration
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
            ClientSize = new Size(520, 480);
            Text = "florenBooks - Inregistrare Membru";
            StartPosition = FormStartPosition.CenterParent;
            this.Load += new EventHandler(this.MemberRegistration_Load);
        }

        #endregion









        internal RadioButton radioButtonMale;
        internal RadioButton radioButtonFemale;











    }
}

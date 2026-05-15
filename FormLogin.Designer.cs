namespace WinFormsApp2FlorenBooksV2
{
    partial class FormLogin
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
            ClientSize = new Size(400, 300);
            Text = "florenBooks - Autentificare";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
        }

        #endregion

        // =====================================================================
        // DECLARATII CONTROALE - vor fi completate de Designer cand adaugi din Toolbox
        // =====================================================================
        internal TextBox textBoxUsername;
        internal TextBox textBoxPassword;
        internal Button buttonLogin;
        internal Button buttonCancel;
        internal Label labelTitle;
        internal Label labelUsername;
        internal Label labelPassword;
    }
}

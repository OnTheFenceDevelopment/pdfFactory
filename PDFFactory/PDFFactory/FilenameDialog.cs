using System;
using System.IO;
using System.Windows.Forms;

namespace PDFFactory
{
    public partial class FilenameDialog : Form
    {
        public FilenameDialog()
        {
            InitializeComponent();
        }

        public string Filename
        {
            get
            {
                return txtFilename.Text.ToLower().EndsWith(".pdf") ?  txtFilename.Text : string.Format("{0}.pdf", txtFilename.Text);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilename.Text))
            {
                MessageBox.Show(this, "Please enter a filename for the merged document", "Filename Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if (txtFilename.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                MessageBox.Show(this, "You have entered an invalid filename - please try again!", "Filename Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}

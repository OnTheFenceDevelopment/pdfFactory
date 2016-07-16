using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Windows.Forms;

namespace PDFFactory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            ShowFileDialog(txtFile1);
            ValidateSelectedPath(txtFile1);
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            ShowFileDialog(txtFile2);
            ValidateSelectedPath(txtFile2);
        }

        private void ShowFileDialog(TextBox targetTextBox)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files (*.pdf) | *.pdf",
                Multiselect = false
            };

            openFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                targetTextBox.Text = openFileDialog.FileName;
            }

            btnJoin.Enabled = CanJoin;
        }

        private bool CanJoin
        {
            get
            {
                return !string.IsNullOrEmpty(txtFile1.Text) && !string.IsNullOrEmpty(txtFile2.Text);
            }
        }

        private void ValidateSelectedPath(TextBox targetTextBox)
        {
            if (txtFile1.Text == txtFile2.Text)
            {
                DialogResult result = MessageBox.Show(this, "You have selected the same path for each file - is this correct?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    targetTextBox.Text = string.Empty;
                    btnJoin.Enabled = CanJoin;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtFile1.Text = string.Empty;
            txtFile2.Text = string.Empty;
            btnJoin.Enabled = CanJoin;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] bytes = null;
                string[] array = new string[]
                {
                    txtFile1.Text,
                    txtFile2.Text
                };

                // TODO: Prompt for filename

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    using (Document document = new Document())
                    {
                        using (PdfCopy pdfCopy = new PdfCopy(document, memoryStream))
                        {
                            document.Open();
                            for (int i = 0; i < 2; i++)
                            {
                                PdfReader pdfReader = new PdfReader(array[i]);
                                int numberOfPages = pdfReader.NumberOfPages;
                                int j = 0;
                                while (j < numberOfPages)
                                {
                                    pdfCopy.AddPage(pdfCopy.GetImportedPage(pdfReader, ++j));
                                }
                            }
                        }
                    }
                    bytes = memoryStream.ToArray();
                }
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(folderPath, "MergedDocument.pdf"), bytes);
                txtFile1.Text = string.Empty;
                txtFile2.Text = string.Empty;
                btnJoin.Enabled = CanJoin;

                MessageBox.Show("File Joined Successfully");
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}

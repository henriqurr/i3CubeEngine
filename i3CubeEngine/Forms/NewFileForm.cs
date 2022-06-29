using System;
using System.IO;
using System.Windows.Forms;

namespace i3CubeEngine
{
    public partial class NewFileForm : Form
    {
        private readonly string mCurrentPath;

        public NewFileForm(string mCurrentPath)
        {
            this.mCurrentPath = mCurrentPath;
            InitializeComponent();
            localLabel.Text = $"Path: {mCurrentPath.Substring(0, 45)}";
            pictureBox.Image = ShellIcon.FolderIcon.ToBitmap();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string newFileName = mFileNameInput.Text;
                if (string.IsNullOrWhiteSpace(newFileName))
                    return;
                string newFilePath = Path.Combine(mCurrentPath, newFileName);
                if (!FileSystem.IsValidFileName(newFileName))
                {
                    CLogger.Error($"The file name can't contain any of the following characters:\n" + "\t\\/:*?\"<>|");
                }
                else if (File.Exists(newFilePath))
                {
                    CLogger.Error($"A file with the same name exists.");
                }
                else
                {
                    File.Create(newFilePath);
                    CancelBtn_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mFileNameInput_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string newFileName = mFileNameInput.Text;
                if (!string.IsNullOrEmpty(newFileName) && FileSystem.IsValidFileName(newFileName))
                    mOkBtn.Enabled = true;
                else
                    mOkBtn.Enabled = false;
            }
            catch { }
        }
    }
}
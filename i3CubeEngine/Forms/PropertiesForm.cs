using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace i3CubeEngine
{
    public partial class PropertiesForm : Form
    {
        public PropertiesForm(string filePath)
        {
            InitializeComponent();
            InitDisplay(filePath);
        }

        private void InitDisplay(string filePath)
        {
            try
            {
                if (File.GetAttributes(filePath).HasFlag(FileAttributes.Directory))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
                    nameTextBox.Text = directoryInfo.Name;
                    pictureBox.Image = ShellIcon.FolderIcon.ToBitmap();
                    typeLabel.Text = "File folder";
                    locationLabel.Text = directoryInfo.Parent?.FullName.WithoutLongPathPrefix();
                    if (string.IsNullOrEmpty(locationLabel.Text) && directoryInfo.Name.Contains(":"))
                        locationLabel.Text = directoryInfo.Name;
                    sizeLabel.Text = FileSystem.FileSizeStr(FileSystem.GetDirectorySize(filePath, false), false);
                    contains.Text = string.Format("{0} Files", FileSystem.InsertPoint(FileSystem.GetDirectorySize(filePath, true).ToString()));
                    createdTimeLabel.Text = directoryInfo.CreationTime.ToString("U", CultureInfo.CreateSpecificCulture("en-US"));
                    modifiedTimeLabel.Text = directoryInfo.LastWriteTime.ToString("U", CultureInfo.CreateSpecificCulture("en-US"));
                    hiddenCheckBox.Checked = directoryInfo.Attributes.HasFlag(FileAttributes.Hidden);
                    readonlyCheckBox.Checked = directoryInfo.Attributes.HasFlag(FileAttributes.ReadOnly);
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    nameTextBox.Text = fileInfo.Name;
                    if (FileSystem.EngineFile(fileInfo))
                    {
                        pictureBox.Image = Resource.icon2;
                        contains.Text = "Engine File";
                    }
                    else
                    {
                        pictureBox.Image = ShellIcon.GetLargeIcon(filePath.WithoutLongPathPrefix()).ToBitmap();
                        contains.Text = "Normal";
                    }
                    string extension = !string.IsNullOrEmpty(fileInfo.Extension) ? fileInfo.Extension.Split('.')[1].ToUpper() : "";
                    if (extension == "DLL")
                    {
                        extension = "Application extension";
                    }
                    else if (extension == "EXE")
                    {
                        extension = "Application";
                    }
                    else
                    {
                        extension = $"File{(string.IsNullOrEmpty(fileInfo.Extension) ? "" : " ")}{extension}";
                    }
                    typeLabel.Text = !string.IsNullOrEmpty(fileInfo.Extension) ? $"{extension} ({fileInfo.Extension})" : "File";
                    locationLabel.Text = fileInfo.DirectoryName?.WithoutLongPathPrefix();
                    sizeLabel.Text = FileSystem.FileSizeStr(fileInfo.Length, false);
                    label2.Text = "Resource:";
                    createdTimeLabel.Text = fileInfo.CreationTime.ToString("U", CultureInfo.CreateSpecificCulture("en-US"));
                    modifiedTimeLabel.Text = fileInfo.LastWriteTime.ToString("U", CultureInfo.CreateSpecificCulture("en-US"));
                    hiddenCheckBox.Checked = fileInfo.Attributes.HasFlag(FileAttributes.Hidden);
                    readonlyCheckBox.Checked = fileInfo.Attributes.HasFlag(FileAttributes.ReadOnly);
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        public long DirSize(DirectoryInfo dir)
        {
            return dir.GetFiles().Sum(fi => fi.Length) + dir.GetDirectories().Sum(di => DirSize(di));
        }

        private void DirectorySize(DirectoryInfo directoryInfo, ref long files, ref long dirs)
        {
            try
            {
                IEnumerable<FileInfo> fis = directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories);
                foreach (FileInfo fi in fis)
                {
                    if (File.GetAttributes(fi.FullName).HasFlag(FileAttributes.Directory))
                    {
                        dirs += fi.Length;
                    }
                    else
                    {
                        files += fi.Length;
                    }
                }
                fis = null;
            }
            catch { }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void locationLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", locationLabel.Text);
            }
            catch { }
        }
    }
}
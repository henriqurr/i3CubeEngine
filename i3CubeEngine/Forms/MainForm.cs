using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace i3CubeEngine
{
    public partial class MainForm : Form
    {
        /*private volatile bool mIsMove;
        private readonly List<string> mListSourcesPath;
        private volatile string mSearchFileName;
        private volatile string mCurrentPath;
        private volatile bool mShowHidden;
        private volatile TreeNode mDirViewCurrentNode;
        private volatile TreeItem mRootFileTree;
        private volatile TreeItem mTreeCurrentNode;

        public string CurrentPath
        {
            get => mCurrentPath;
            set
            {
                mCurrentPath = value;
                addressInput.Text = mCurrentPath;
            }
        }

        public MainForm()
        {
            try
            {
                InitializeComponent();
                if (openFileBrowser.ShowDialog() == DialogResult.OK)
                {
                    mListSourcesPath = new List<string>(200);
                    ThreadPool.SetMaxThreads(25, 25);
                    //ClosePreviousInstance();
                    mIsMove = false;
                    mShowHidden = false;
                    mDirViewCurrentNode = null;
                    CurrentPath = openFileBrowser.SelectedPath; //Path
                    mSearchFileName = string.Empty;
                    InitDisplay();
                }
                else
                {
                    exitToolStripMenuItem_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
                exitToolStripMenuItem_Click(null, null);
            }
        }



        private void NewFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Path.Combine(CurrentPath, "New folder");
                string newFolderPath = path;
                int num = 1;
                while (Directory.Exists(newFolderPath))
                {
                    newFolderPath = path + " (" + num + ")";
                    num++;
                }
                DirectoryInfo dirInfo = Directory.CreateDirectory(newFolderPath);
                AddItemOnListView(dirInfo.FullName, false);
                UpdateTreeView();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void NewFile_Click(object sender, EventArgs e)
        {
            try
            {
                NewFileForm newFileForm = new NewFileForm(CurrentPath);
                newFileForm.ShowDialog();
                UpdateListView();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            try
            {
                SetCopyFilesSourcePaths();
                mIsMove = false;
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            try
            {
                SetCopyFilesSourcePaths();
                mIsMove = true;
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            try
            {
                if (!mListSourcesPath.Any())
                    return;
                foreach (string path in mListSourcesPath)
                {
                    if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                        MoveToOrCopyToDirectoryBySourcePath(path); 
                    else
                        MoveToOrCopyToFileBySourcePath(path);
                }
                UpdateTreeView();
                mTreeCurrentNode.Childs.Clear();
                FileSystem.LoadFileTreeAsync(mTreeCurrentNode);
                UpdateListView();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
            finally
            {
                mListSourcesPath.Clear();
            }
        }

        private void Rename_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewFiles.SelectedItems.Count > 0)
                    listViewFiles.SelectedItems[0].BeginEdit();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                int countItems = listViewFiles.SelectedItems.Count;
                if (countItems > 0)
                {
                    DialogResult dialogResult = MessageBox.Show(string.Format("Are you sure to delete {0} file(s)?", countItems), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Queue<string> files = new Queue<string>(20);
                        foreach (ListViewItem item in listViewFiles.SelectedItems)
                        {
                            string path = item.Tag.ToString();
                            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                                Directory.Delete(path, true);
                            else
                                File.Delete(path);
                            listViewFiles.Items.Remove(item);
                            files.Enqueue(path);
                        }
                        for (int i = mTreeCurrentNode.Childs.Count - 1; i >= 0; i--)
                        {
                            TreeItem node = mTreeCurrentNode.Childs[i];
                            if (files.Contains(node.ItemData))
                                mTreeCurrentNode.Childs.Remove(node);
                        }
                        UpdateTreeView();
                        files = null;
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateListView();
                UpdateTreeView();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void HiddenFilesTsmi_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                mShowHidden = hiddenFilesTsmi.Checked;
                UpdateListView();
                UpdateTreeView();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void Properties_Click(object sender, EventArgs e)
        {
            try
            {
                PropertiesForm form;
                if (listViewFiles.SelectedItems.Count == 0)
                {
                    form = new PropertiesForm(CurrentPath);
                }
                else
                {
                    form = new PropertiesForm(listViewFiles.SelectedItems[0].Tag.ToString());
                }
                form.Show();
                //form = null;
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void MainContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                CLogger.WriteLine("MainContextMenuStrip_Opening");
                if (listViewFiles.Items.Count == 0 || sender == null && e == null)
                {
                    copyTsmi.Visible = false;
                    pasteContexMenu.Visible = false;
                    cutTsmi.Visible = false;
                    deleteTsmi.Visible = false;
                    renameTsmi.Visible = false;
                    newFolderContextMenu.Visible = false;
                    newFileContextMenu.Visible = false;
                    refreshMainTsmi.Visible = false;
                    return;
                }
                Point curPoint = listViewFiles.PointToClient(Cursor.Position);
                ListViewItem item = listViewFiles.GetItemAt(curPoint.X, curPoint.Y);
                if (item != null)
                {    
                    copyTsmi.Visible = true;
                    pasteContexMenu.Visible = mListSourcesPath.Count > 0;
                    cutTsmi.Visible = true;
                    deleteTsmi.Visible = true;
                    renameTsmi.Visible = true;
                    newFolderContextMenu.Visible = true;
                    newFileContextMenu.Visible = true;
                    refreshMainTsmi.Visible = true;
                }
                else
                {
                    copyTsmi.Visible = false;
                    pasteContexMenu.Visible = mListSourcesPath.Count > 0;
                    cutTsmi.Visible = false;
                    deleteTsmi.Visible = false;
                    renameTsmi.Visible = false;
                    newFolderContextMenu.Visible = true;
                    newFileContextMenu.Visible = true;
                    refreshMainTsmi.Visible = true;
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void AddressInput_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (string.IsNullOrWhiteSpace(addressInput.Text))
                        return;
                    string newPath = addressInput.Text.AddLongPathPrefix();
                    if (!Directory.Exists(newPath))
                        return;
                    mTreeCurrentNode = FileSystem.GetFileTreeNodeByPath(newPath, mRootFileTree);
                    UpdateListView();
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void ListViewFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(e.Label))
                {
                    e.CancelEdit = true;
                    return;
                }
                string newName = e.Label;
                ListViewItem selectedItem = listViewFiles.SelectedItems[0];
                if (newName == selectedItem.Text)
                {
                    return;
                }
                else if (!FileSystem.IsValidFileName(newName))
                {
                    CLogger.Error($"The file name can't contain any of the following characters:\n" + "\t\\/:*?\"<>|");
                    e.CancelEdit = true;
                }
                else
                {
                    Computer computer = new Computer();
                    if (File.GetAttributes(selectedItem.Tag.ToString()).HasFlag(FileAttributes.Directory))
                    {
                        if (Directory.Exists(Path.Combine(CurrentPath, newName)))
                        {
                            CLogger.Error($"Target folder already contains a folder with that name.");
                            e.CancelEdit = true;
                        }
                        else
                        {
                            computer.FileSystem.RenameDirectory(selectedItem.Tag.ToString(), newName);
                            DirectoryInfo directoryInfo = new DirectoryInfo(selectedItem.Tag.ToString());
                            string parentPath = directoryInfo.Parent.FullName;
                            string newPath = Path.Combine(parentPath, newName);
                            selectedItem.Tag = newPath;
                        }
                    }
                    else
                    {
                        if (File.Exists(Path.Combine(CurrentPath, newName)))
                        {
                            CLogger.Error($"Target folder already contains a file with that name.");
                            e.CancelEdit = true;
                        }
                        else
                        {
                            computer.FileSystem.RenameFile(selectedItem.Tag.ToString(), newName);
                            FileInfo fileInfo = new FileInfo(selectedItem.Tag.ToString());
                            string parentPath = Path.GetDirectoryName(fileInfo.FullName);
                            string newPath = Path.Combine(parentPath, newName);
                            selectedItem.Tag = newPath;
                        }
                    }
                }
                UpdateTreeView();
                mTreeCurrentNode.Childs.Clear();
                FileSystem.LoadFileTreeAsync(mTreeCurrentNode);
                UpdateListView();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void ListViewFiles_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                Open();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void DirectoryTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                mDirViewCurrentNode = e.Node;
                mTreeCurrentNode = FileSystem.GetFileTreeNodeByPath(e.Node.Tag.ToString(), mRootFileTree);
                CurrentPath = mTreeCurrentNode.ItemData.WithoutLongPathPrefix();
                UpdateListView();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void DirectoryTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                mDirViewCurrentNode = e.Node;
                UpdateTreeView();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void SearchInput_Enter(object sender, EventArgs e)
        {
            try
            {
                searchInput.Text = string.Empty;
                searchInput.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void SearchInput_Leave(object sender, EventArgs e)
        {
            try
            {
                searchInput.Text = "Search";
                searchInput.ForeColor = SystemColors.WindowFrame;
                statusBarFileNum.Text = $"{listViewFiles.Items.Count} items";
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void SearchInput_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (string.IsNullOrWhiteSpace(searchInput.Text))
                    {
                        UpdateListView();
                        return;
                    }
                    listViewFiles.Items.Clear();
                    statusBarFileNum.Text = "0 items";
                    mSearchFileName = searchInput.Text.ToLower();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SearchInTree), mTreeCurrentNode);
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void InitDisplay()
        {
            try
            {
                CLogger.WriteLine("InitDisplay");
                mRootFileTree = new TreeItem("ROOT", null);
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(CurrentPath);
                    CurrentPath = string.Empty;

                    DriveInfo drive = null;
                    string drivePrefix = directoryInfo.FullName.Split(':')[0];
                    foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
                    {
                        if (driveInfo.Name.Split(':')[0] == drivePrefix)
                        {
                            drive = driveInfo;
                            break;
                        }
                    }
                    if (drive == null)
                        throw new Exception("Drive disk null.");

                    string label = string.IsNullOrWhiteSpace(drive.VolumeLabel) ? "Local Disk" : drive.VolumeLabel;
                    label += $" ({drivePrefix})";
                    TreeNode driveNode = directoryTreeView.Nodes.Add(label);
                    driveNode.Tag = drive.Name;
                    driveNode.ImageKey = "drive";
                    driveNode.Nodes.Add(string.Empty);

                    //MessageBox.Show($"CurrentPath: {CurrentPath.AddLongPathPrefix()}; Info Name: {drive.Name.AddLongPathPrefix()}");

                    TreeItem childNode = new TreeItem(drive.Name.AddLongPathPrefix(), mRootFileTree);
                    mRootFileTree.AddChild(childNode);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(FileSystem.LoadFileTreeAsync), childNode); //Método lento: Lê todos os ITEMS de todos os diretórios como cache.

                    mTreeCurrentNode = FileSystem.GetFileTreeNodeByPath(directoryInfo.FullName.AddLongPathPrefix(), mRootFileTree);

                    UpdateListView();

                    //mTreeCurrentNode = mRootFileTree.Childs.First();
                }
                catch (Exception ex)
                {
                    CLogger.Exception(ex);
                }

                //foreach (DriveInfo info in DriveInfo.GetDrives())
                //{
                //    try
                //    {
                //        string drivePrefix = info.Name.Split('\\')[0];
                //        string label = string.IsNullOrWhiteSpace(info.VolumeLabel) ? "Local Disk" : info.VolumeLabel;
                //        label += $" ({drivePrefix})";
                //        TreeNode driveNode = directoryTreeView.Nodes.Add(label);
                //        driveNode.Tag = info.Name;
                //        MessageBox.Show("info.name: " + info.Name + " | Prefix: " + drivePrefix);
                //        driveNode.ImageKey = "drive";
                //        driveNode.Nodes.Add(string.Empty);
                //        TreeItem childNode = new TreeItem(info.Name.AddLongPathPrefix(), mRootFileTree);
                //        mRootFileTree.AddChild(childNode);
                //        ThreadPool.QueueUserWorkItem(new WaitCallback(FileSystem.LoadFileTreeAsync), childNode); //Método lento: Lê todos os ITEMS de todos os diretórios como cache.
                //    }
                //    catch (Exception ex)
                //    {
                //        CLogger.Exception(ex);
                //    }
                //}
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void UpdateTreeView()
        {
            if (mDirViewCurrentNode == null)
                return;
            lock (mDirViewCurrentNode)
            {
                try
                {
                    mDirViewCurrentNode.Nodes.Clear();
                    DirectoryInfo directoryInfo = new DirectoryInfo(mDirViewCurrentNode.Tag.ToString());
                    foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                    {
                        try
                        {
                            try
                            {
                                dir.GetAccessControl();
                            }
                            catch (UnauthorizedAccessException)
                            {
                                continue;
                            }
                            var attr = File.GetAttributes(dir.FullName);
                            if (attr.HasFlag(FileAttributes.System) || (attr.HasFlag(FileAttributes.Hidden) && !mShowHidden))
                            {
                                continue;
                            }
                            TreeNode childNode = mDirViewCurrentNode.Nodes.Add(dir.Name);
                            childNode.Tag = dir.FullName;
                            childNode.ImageKey = "folder";
                            childNode.Nodes.Add(string.Empty); //Subpastas
                        }
                        catch (Exception ex)
                        {
                            CLogger.Exception(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CLogger.Exception(ex);
                }
                finally
                {
                    Application.DoEvents();
                }
            }
        }
        

        

        private void SetCopyFilesSourcePaths()
        {
            try
            {
                if (listViewFiles.SelectedItems.Count > 0)
                {
                    mListSourcesPath.Clear();
                    foreach (ListViewItem item in listViewFiles.SelectedItems)
                        mListSourcesPath.Add(item.Tag.ToString());
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void MoveToOrCopyToFileBySourcePath(string sourcePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(sourcePath);
                string destPath = Path.Combine(CurrentPath, fileInfo.Name);
                if (destPath == sourcePath || File.Exists(destPath))
                {
                    string path = Path.Combine(fileInfo.DirectoryName, Path.GetFileNameWithoutExtension(fileInfo.FullName));
                    int num = 1;
                    while (File.Exists(destPath))
                    {
                        destPath = path + $" - Copy ({num++}){fileInfo.Extension}";
                    }
                }
                if (mIsMove)
                    fileInfo.MoveTo(destPath);
                else
                    fileInfo.CopyTo(destPath);
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void MoveToOrCopyToDirectoryBySourcePath(string sourcePath)
        {
            try
            {
                DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(sourcePath);
                string destPath = Path.Combine(CurrentPath, sourceDirectoryInfo.Name);
                if (destPath == sourcePath || Directory.Exists(destPath))
                {
                    string path = destPath;
                    int num = 1;
                    while (Directory.Exists(destPath))
                    {
                        destPath = path + $" - Copy ({num++})";
                    }
                }
                if (mIsMove)
                {
                    FileSystem.CopyAndPasteDirectory(sourceDirectoryInfo, new DirectoryInfo(destPath));
                    Directory.Delete(sourcePath, true);
                }
                else
                {
                    FileSystem.CopyAndPasteDirectory(sourceDirectoryInfo, new DirectoryInfo(destPath));
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void SearchInTree(object itemNode)
        {
            try
            {
                TreeItem node = itemNode as TreeItem;
                string nowPath = CurrentPath;
                foreach (TreeItem childNode in node.Childs)
                {
                    if (nowPath != CurrentPath)
                        break;
                    string fullName = childNode.ItemData;
                    FileAttributes attr = File.GetAttributes(fullName);
                    string name = fullName.Split('\\').Last().ToLower();
                    if (attr.HasFlag(FileAttributes.Directory))
                    {
                        if (name.Contains(mSearchFileName))
                        {
                            AddItemOnListViewAsync(fullName, false);
                        }
                        ThreadPool.QueueUserWorkItem(new WaitCallback(SearchInTree), childNode);
                    }
                    else
                    {
                        if (name.Contains(mSearchFileName))
                        {
                            AddItemOnListViewAsync(fullName, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        //public void SetProgressBarValue(double index, int count)
        //{
        //    lock (progressbar)
        //    {
        //        int value = 0;
        //        if (index == -1)
        //            value = count;
        //        else
        //            value = (int)((index / count) * 100.0);
        //        if (value > 100) value = 100;
        //        if (value < 0) value = 0;
        //        if (progressbar.ProgressBar.Parent.InvokeRequired)
        //            progressbar.ProgressBar.Parent.Invoke(new MethodInvoker(delegate { progressbar.ProgressBar.Value = value; }));
        //        else
        //            progressbar.ProgressBar.Value = value;
        //    }
        //}

        private void listViewFiles_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Modifiers == Keys.Control && e.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.C: //Copiar
                            {
                                Copy_Click(sender, e);
                                break;
                            }
                        case Keys.X: //Recortar
                            {
                                Cut_Click(sender, e);
                                break;
                            }
                        case Keys.V: //Colar
                            {
                                Paste_Click(sender, e);
                                break;
                            }
                        case Keys.Z: break; //Voltar <
                        case Keys.Y: break; //Voltar >
                    }
                }
                else if (e.Control && e.Shift) //CTRL + SHIFT
                {
                    switch (e.KeyCode)
                    {
                        case Keys.N: //Nova pasta
                            {
                                NewFolder_Click(sender, e);
                                break;
                            }
                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Delete: //Deletar
                            {
                                Delete_Click(sender, e);
                                break;
                            }
                        case Keys.F5: //Atualizar
                            {
                                Refresh_Click(sender, e);
                                break;
                            }
                    }
                }             
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Dispose(true);
                Application.Exit();
                Application.ExitThread();
                Process.GetCurrentProcess().Close();
                Environment.Exit(0);
            }
            catch { }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            exitToolStripMenuItem_Click(sender, e);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileBrowser.ShowDialog() == DialogResult.OK)
                {
                    //=========================================

                    //ThreadPool.QueueUserWorkItem(new WaitCallback(FileSystem.LoadFileTreeAsync), childNode); //Método lento: Lê todos os ITEMS de todos os diretórios como cache.

                    //InitDisplay(); 
                    return;
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }*/
    }
}
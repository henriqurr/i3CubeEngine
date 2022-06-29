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

namespace i3CubeEngine.Forms
{
    public partial class EngineForm : Form
    {
        #region Items
        private const int queueLimit = 20;
        private const int listLimit = 200;
        private volatile bool mIsMove = false;
        private volatile string mCurrentPath = string.Empty;
        private readonly List<ListViewItem> mListSourcesPath = new List<ListViewItem>(); //listLimit
        private volatile TreeItem mRootFileTree = null;
        private volatile TreeItem mTreeCurrentNode = null;
        #endregion

        #region Initialize
        public EngineForm()
        {
            try
            {
                InitializeComponent();
                OpenFile();
                ThreadPool.SetMaxThreads(25, 25);
                ClosePreviousInstance();
                Preload();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        public void Preload()
        {
            try
            {
                listIcons.Images.Add("drive", ShellIcon.DriveIcon);
                listIcons.Images.Add("folder", ShellIcon.FolderIcon);
            }
            catch { }
        }

        public void ClearCache()
        {
            try
            {
                mIsMove = false;
                mCurrentPath = string.Empty;
                mListSourcesPath.Clear();

                FileSystem.DestroyObject(mRootFileTree);
                FileSystem.DestroyObject(mTreeCurrentNode);

                //Process.GetCurrentProcess().Refresh();
                Application.DoEvents();
                //Refresh();

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch { }
        }

        public void ClosePreviousInstance()
        {
            try
            {
                Process[] pname = Process.GetProcessesByName(AppDomain.CurrentDomain.FriendlyName.Remove(AppDomain.CurrentDomain.FriendlyName.Length - 4));
                if (pname != null && pname.Length > 1)
                    pname.First(p => p.Id != Process.GetCurrentProcess().Id).Kill();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        public void Exit()
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
        #endregion

        #region Events
        protected override void OnResize(EventArgs e)
        {
            try
            {
                
                pathBox.Width = Width - 290;
                statusBar.Width = Width;
                //listViewFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None);
                //574 133 140 101
                //listViewFiles.Columns[0].Width = Width - 574;
                //listViewFiles.Columns[1].Width = Width - (574 + 133);
                //listViewFiles.Columns[2].Width = Width - (574 + 133 + 140);
                //listViewFiles.Columns[3].Width = Width - (574 + 133 + 140 + 101);
            }
            catch { }
        }

        public void UpdateListView()
        {
            lock (mTreeCurrentNode)
            {
                try
                {
                    listViewFiles.BeginUpdate();
                    listViewFiles.Items.Clear();
                    Queue<string> files = new Queue<string>(queueLimit);
                    foreach (TreeItem childNode in mTreeCurrentNode.Childs)
                    {
                        try
                        {
                            string fullName = childNode.ItemData;
                            FileAttributes attr = File.GetAttributes(fullName);
                            if (attr.HasFlag(FileAttributes.System)) //FileAttributes.Hidden
                            {
                                continue;
                            }
                            if (attr.HasFlag(FileAttributes.Directory))
                                AddItemOnListView(fullName, false);
                            else
                                files.Enqueue(fullName);
                        }
                        catch (FileNotFoundException ex)
                        {
                            CLogger.Exception(ex);
                        }
                    }
                    foreach (string fullName in files)
                    {
                        AddItemOnListView(fullName, true);
                    }
                    statusBarText.Text = $"{listViewFiles.Items.Count} items";
                    files.Clear();
                    FileSystem.DestroyObject(files);
                }
                catch (Exception ex)
                {
                    CLogger.Exception(ex);
                }
                finally
                {
                    Application.DoEvents();
                    listViewFiles.EndUpdate();
                }
            }
        }

        private void AddItemOnListView(string fullPath, bool isFile)
        {
            try
            {
                if (isFile)
                {
                    FileInfo fileInfo = new FileInfo(fullPath);
                    ListViewItem item = listViewFiles.Items.Add(fileInfo.Name);
                    if (fileInfo.Extension == ".exe" || fileInfo.Extension == ".lnk" || fileInfo.Extension == string.Empty)
                    {
                        if (!listIcons.Images.ContainsKey(fileInfo.FullName))
                        {
                            Icon fileIcon = ShellIcon.GetLargeIcon(fileInfo.FullName.WithoutLongPathPrefix());
                            listIcons.Images.Add(fileInfo.FullName, fileIcon);
                        }
                        item.ImageKey = fileInfo.FullName;
                    }
                    else
                    {
                        if (!listIcons.Images.ContainsKey(fileInfo.Extension))
                        {
                            if (FileSystem.EngineFile(fileInfo))
                            {
                                listIcons.Images.Add(fileInfo.Extension, Resource.icon);
                            }
                            else
                            {
                                Icon fileIcon = ShellIcon.GetLargeIcon(fileInfo.FullName.WithoutLongPathPrefix());
                                listIcons.Images.Add(fileInfo.Extension, fileIcon);
                            }
                        }
                        item.ImageKey = fileInfo.Extension;
                    }
                    item.Tag = fileInfo.FullName;
                    item.SubItems.Add(fileInfo.LastWriteTime.ToString("dd/MM/yyyy HH:mm"));
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
                        extension = $"File{(string.IsNullOrEmpty(extension) ? "" : " ")}{extension}";
                    }
                    item.SubItems.Add(extension);
                    item.SubItems.Add(FileSystem.FileSizeStr(fileInfo.Length, true));
                }
                else
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(fullPath);
                    ListViewItem item = listViewFiles.Items.Add(dirInfo.Name, "folder");
                    item.Tag = dirInfo.FullName;
                    item.SubItems.Add(dirInfo.LastWriteTime.ToString("dd/MM/yyyy HH:mm"));
                    item.SubItems.Add("File folder");
                    item.SubItems.Add(string.Empty);
                }
                statusBarText.Text = $"{listViewFiles.Items.Count} items";
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void AddItemOnListViewAsync(string fullPath, bool isFile)
        {
            try
            {
                if (listViewFiles.InvokeRequired)
                {
                    listViewFiles.Invoke(new MethodInvoker(delegate { AddItemOnListView(fullPath, isFile); }));
                }
                else
                {
                    AddItemOnListView(fullPath, isFile);
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void BackPath(object sender, EventArgs e)
        {
            try
            {
                TreeItem upNode = mTreeCurrentNode.ParentItem;
                if (upNode != mRootFileTree && upNode != null)
                {
                    mTreeCurrentNode = upNode;
                    CurrentPath = mTreeCurrentNode.ItemData.WithoutLongPathPrefix();
                    UpdateListView();
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void listViewFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
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
                    CLogger.Error($"The file name can't contain any of the following characters:\n" + $"\t\\/:*?\"<>|");
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
                mTreeCurrentNode.Childs.Clear();
                FileSystem.LoadFileTreeAsync(mTreeCurrentNode);
                UpdateListView();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void ShortcutKeys(object sender, KeyEventArgs e)
        {
            try
            {
                menuStrip_Opening(null, null); //Ativar funções
                //CLogger.WriteLine($"Key {e.KeyCode}");
                if (e.Alt && e.KeyCode == Keys.F4) //Fechar
                {
                    Exit();
                    return;
                }
                else if (e.Modifiers == Keys.Control && e.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.C: //Copiar
                            {
                                copyTSM_Click(sender, e);
                                break;
                            }
                        case Keys.X: //Recortar
                            {
                                cutTSM_Click(sender, e);
                                break;
                            }
                        case Keys.V: //Colar
                            {
                                pasteTSM_Click(sender, e);
                                break;
                            }
                        case Keys.O: //Abrir
                            {
                                ClearCache();
                                OpenFile();
                                break;
                            }
                        case Keys.Z: break; //Defazer
                        case Keys.Y: break; //Refazer
                    }
                }
                else if (e.Control && e.Shift) //CTRL + SHIFT
                {
                    switch (e.KeyCode)
                    {
                        case Keys.N: //Nova pasta
                            {
                                newFolderTSM_Click(sender, e);
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
                                deleteTSM_Click(sender, e);
                                break;
                            }
                        case Keys.F5: //Atualizar
                            {
                                refreshTSM_Click(sender, e);
                                break;
                            }
                        case Keys.Back: //Voltar
                            {
                                BackPath(sender, e);
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

        #region MenuStrip
        private void menuStrip_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                Point curPoint = listViewFiles.PointToClient(Cursor.Position);
                ListViewItem item = listViewFiles.GetItemAt(curPoint.X, curPoint.Y);
                if (item != null)
                {
                    newFileTSM.Enabled = false;
                    newFolderTSM.Enabled = false;
                    openTSM.Enabled = true;
                    copyTSM.Enabled = true;
                    cutTSM.Enabled = true;
                    pasteTSM.Enabled = false;
                    refreshTSM.Enabled = false;
                    deleteTSM.Enabled = true;
                    propertiesTSM.Enabled = true;
                }
                else
                {
                    newFileTSM.Enabled = true;
                    newFolderTSM.Enabled = true;
                    openTSM.Enabled = false;
                    copyTSM.Enabled = false;
                    cutTSM.Enabled = false;
                    pasteTSM.Enabled = mListSourcesPath.Count > 0;
                    refreshTSM.Enabled = true;
                    deleteTSM.Enabled = false;
                    propertiesTSM.Enabled = true;
                }
            }
            catch { }
        }

        private void newFileTSM_Click(object sender, EventArgs e)
        {
            NewFileForm newFileForm = null;
            try
            {
                if (!newFileTSM.Enabled)
                    return;
                newFileForm = new NewFileForm(CurrentPath);
                newFileForm.ShowDialog();
                UpdateListView();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
            finally
            {
                FileSystem.DestroyObject(newFileForm);
            }
        }

        private void newFolderTSM_Click(object sender, EventArgs e)
        {
            try
            {
                if (!newFolderTSM.Enabled)
                    return;
                string path = Path.Combine(CurrentPath, "New folder");
                string newFolderPath = path;
                int num = 1;
                while (Directory.Exists(newFolderPath))
                {
                    newFolderPath = $"{path} ({num})";
                    num++;
                }
                AddItemOnListView(Directory.CreateDirectory(newFolderPath).FullName, false);
                UpdateListView();
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void openTSM_Click(object sender, EventArgs e)
        {
            if (!openTSM.Enabled)
                return;
            OpenItem();
        }

        private void copyTSM_Click(object sender, EventArgs e)
        {
            try
            {
                if (!copyTSM.Enabled)
                    return;
                SetCopyFilesSourcePaths();
                mIsMove = false;
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void cutTSM_Click(object sender, EventArgs e)
        {
            try
            {
                if (!cutTSM.Enabled)
                    return;
                SetCopyFilesSourcePaths();
                mIsMove = true;
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void pasteTSM_Click(object sender, EventArgs e)
        {
            try
            {
                if (!pasteTSM.Enabled)
                    return;
                if (!mListSourcesPath.Any())
                    return;
                Queue<string> files = new Queue<string>(queueLimit);
                foreach (ListViewItem item in mListSourcesPath)
                {
                    string path = item.Tag.ToString();
                    if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                        MoveToOrCopyToDirectoryBySourcePath(path);
                    else
                        MoveToOrCopyToFileBySourcePath(path);
                    if (mIsMove)
                    {
                        listViewFiles.Items.Remove(item);
                        files.Enqueue(path);
                    }
                }
                //if (mIsMove)
                //{
                //    for (int i = mTreeCurrentNode.Childs.Count - 1; i >= 0; i--)
                //    {
                //        TreeItem node = mTreeCurrentNode.Childs[i];
                //        if (files.Contains(node.ItemData))
                //            mTreeCurrentNode.Childs.Remove(node);
                //    }
                //}
                mTreeCurrentNode.Childs.Clear();
                FileSystem.LoadFileTreeAsync(mTreeCurrentNode);
                UpdateListView();

                files.Clear();
                FileSystem.DestroyObject(files);
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
            finally
            {
                mListSourcesPath.Clear();
                mIsMove = false;
            }
        }

        private void refreshTSM_Click(object sender, EventArgs e)
        {
            if (!refreshTSM.Enabled)
                return;
            UpdateListView();
        }

        private void deleteTSM_Click(object sender, EventArgs e)
        {
            if (!deleteTSM.Enabled)
                return;
            Delete();
        }

        private void propertiesTSM_Click(object sender, EventArgs e)
        {
            try
            {
                if (!propertiesTSM.Enabled)
                    return;
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
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }
        #endregion

        #region Basic Events
        private void EngineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Exit();
        }

        private void listViewFiles_ItemActivate(object sender, EventArgs e)
        {
            OpenItem();
        }

        private void openTool_Click(object sender, EventArgs e)
        {
            ClearCache();
            OpenFile();
        }

        private void exitTool_Click(object sender, EventArgs e)
        {
            Exit();
        }
        #endregion

        #endregion

        #region File
        public string CurrentPath
        {
            get => mCurrentPath;
            set
            {
                mCurrentPath = value;
                pathBox.Text = mCurrentPath;
            }
        }

        private void OpenFile()
        {
            try
            {
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    CurrentPath = folderBrowser.SelectedPath;
                    
                    DirectoryInfo directoryInfo = new DirectoryInfo(CurrentPath);
                    CurrentPath = directoryInfo.FullName;
                    mRootFileTree = new TreeItem("ROOT", null);
                    TreeItem childNode = new TreeItem(CurrentPath.AddLongPathPrefix(), mRootFileTree);
                    mRootFileTree.AddChild(childNode);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(FileSystem.LoadFileTreeAsync), childNode);
                    mTreeCurrentNode = childNode;
                    UpdateListView();
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
                Exit();
            }
        }

        private void OpenItem()
        {
            try
            {
                if (listViewFiles.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem item in listViewFiles.SelectedItems)
                    {
                        if (item != null)
                        {
                            string path = item.Tag.ToString();
                            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                            {
                                CurrentPath = path.WithoutLongPathPrefix();
                                foreach (var childNode in mTreeCurrentNode.Childs)
                                {
                                    if (childNode.ItemData == path)
                                    {
                                        mTreeCurrentNode = childNode;
                                        break;
                                    }
                                }
                                UpdateListView();
                                break;
                            }
                            else
                            {
                                if (!FileSystem.EngineFile(path))
                                    Process.Start(path);
                                else
                                    FileSystem.StartEngineFile(path);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
            }
        }

        private void Delete()
        {
            try
            {
                int countItems = listViewFiles.SelectedItems.Count;
                if (countItems > 0)
                {
                    DialogResult dialogResult = MessageBox.Show(string.Format("Are you sure to delete {0} file(s)?", countItems), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Queue<string> files = new Queue<string>(queueLimit);
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
                        UpdateListView();
                        files.Clear();
                        FileSystem.DestroyObject(files);
                    }
                }
            }
            catch (Exception ex)
            {
                CLogger.Exception(ex);
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
                        mListSourcesPath.Add(item);
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
        #endregion
    }
}
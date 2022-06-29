using i3PackDx.Managers;
using i3PackDx.Models;
using i3PackDx.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.ListViewItem;

namespace i3PackDx
{
    public partial class i3PackDxView : Form
    {
        public Reader reader;
        public i3PackDxView(string path)
        {
            InitializeComponent();
            open(path);
        }

        private void open(string path)
        {
            FullClear();
            lbPath.Text = "Path: " + path;
            cmsPathFile.Items[0].Enabled = true;
            cmsPathFile.Items[1].Enabled = true;
            byte[] buffer = File.ReadAllBytes(path);
            reader = new Reader(buffer);
            HeaderManager.GetHeader(reader);
            StringTableManager.GetStringTables(reader, HeaderManager.header);
            NodeManager.GetNodeInfos(reader, HeaderManager.header);

            treeView1.Nodes.Clear();
            TreeNode hti = new TreeNode();
            ulong ulTreeIndex = 0;
            bool isRoot = false;
            for (int i = NodeManager.m_pvPackNodes.Count - 1; i >= 0; i--)
            {
                CSingleNode pNode = NodeManager.m_pvPackNodes[i];
                Console.WriteLine(pNode.Index);
                ulTreeIndex = (ulong)i;
                if (!isRoot)
                {
                    pNode.isRoot = isRoot = true;
                    Console.WriteLine("Root:" + pNode.NodeName);
                    hti = treeView1.Nodes.Add(pNode.Index.ToString(), pNode.NodeName);
                }
                else
                {
                    if (pNode.IsLeaf())
                    {
                        Console.WriteLine("IsLeaf:" + pNode.NodeName);
                        TreeNode node = hti.Nodes.Add(pNode.Index.ToString(), pNode.NodeName);
                        treeView1.SelectedNode = node;
                        //if (pNode.FileCount > 0)
                        //treeView1.SetItemState(_hti, TVIS_BOLD, TVIS_BOLD);
                    }
                    else
                    {
                        hti = hti.Nodes.Add(pNode.Index.ToString(), pNode.NodeName);
                        //if (rit->FileCount)
                        //    m_treeNode.SetItemState(hti, TVIS_BOLD, TVIS_BOLD);
                    }
                }
            }
            treeView1.ExpandAll();
        }

        public void FullClear()
        {
            HeaderManager.header = new CPackHeader();
            HeaderManager.m_pvHeaderDirInfo.Clear();
            StringTableManager.lastReg = null;
            StringTableManager.stringTables = new List<StringTable>();
            NodeManager.m_pvPackNodes.Clear();
            treeView1.Nodes.Clear();
            listView1.Items.Clear();
            cmsPathFile.Items[0].Enabled = false;
            cmsPathFile.Items[1].Enabled = false;
            lbPath.Text = "Path: None";
            lbFilesCount.Text = "Files Count: None";
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
                return;
            listView1.Items.Clear();
            var node = NodeManager.GetNodeById(ulong.Parse(e.Node.Name));
            if (node == null)
                return;
            for (int i = 0; i < node.Files.Count; i++)
            {
                var file = node.Files[i];
                ListViewItem lvFile = new ListViewItem(file.Filename);
                ListViewSubItem lvFileOffset = new ListViewSubItem();
                lvFileOffset.Text = file.Offset.ToString();// Utils.DecimalToHex((int)file.Offset);

                ListViewSubItem lvFileSize = new ListViewSubItem();
                lvFileSize.Text = file.Size.ToString(); // Utils.DecimalToHex((int)file.Size);

                ListViewSubItem lvFileCRC32 = new ListViewSubItem();
                //try
                //{
                //    byte[] data = new byte[file.Size];
                //    Array.Copy(reader.buffer, (int)file.Offset, data, 0, data.Length);
                //    var crc = new CrcStream(new MemoryStream(data));
                //    crc.Read(data, 0, data.Length);
                //    lvFileCRC32.Text = crc.ReadCrc.ToString("X8");
                //}
                //catch(Exception ex)
                //{
                //    MessageBox.Show(ex.ToString());
                //}

                lvFile.SubItems.Add(lvFileOffset);
                lvFile.SubItems.Add(lvFileSize);
                lvFile.SubItems.Add(lvFileCRC32);
                listView1.Items.Add(lvFile);
            }
            lbFilesCount.Text = "Files Count: " + node.Files.Count;
        }

        private void dumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = listView1.FocusedItem;
            if (item == null)
                return;
            string fileName = item.SubItems[0].Text;
            long fileOffset = int.Parse(item.SubItems[1].Text);
            int fileSize = int.Parse(item.SubItems[2].Text);
            byte[] fileData = new byte[fileSize];
            Array.Copy(reader.buffer, fileOffset, fileData, 0, fileData.Length);
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                fs.Write(fileData, 0, fileData.Length);
                fs.Close();
            }
            MessageBox.Show($"{fileName} dumped sucessfuly", "i3PackDx");
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("i3PackDx v1.0\n\nAll Credits to Exploit Network\n\nDevelopers:\nCoyote\nPISTOLA\nSpecial Thanks to Abujafar for c++ source", "Credits");
        }

        private void openPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var fileDirectory = ofdPath.FileName.Replace(ofdPath.FileName.Substring(ofdPath.FileName.LastIndexOf(@"\") + 1), "");
            //Process.Start("explorer.exe", fileDirectory);
        }

        private void copyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var fileDirectory = ofdPath.FileName.Replace(ofdPath.FileName.Substring(ofdPath.FileName.LastIndexOf(@"\") + 1), "");
            //Clipboard.SetText(fileDirectory);
            MessageBox.Show("Directory copied successfully.", "i3PackDx");
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (SettingsView settings = new SettingsView())
            //{
            //    if (settings.ShowDialog() == DialogResult.OK)
            //    {
            //        listView1.Items.Clear();
            //        if (treeView1.SelectedNode == null)
            //            return;
            //        var node = NodeManager.GetNodeById(ulong.Parse(treeView1.SelectedNode.Name));
            //        if (node == null)
            //            return;

            //        for (int i = 0; i < node.Files.Count; i++)
            //        {
            //            var file = node.Files[i];
            //            ListViewItem lvFile = new ListViewItem(file.Filename);
            //            ListViewSubItem lvFileOffset = new ListViewSubItem();
            //            lvFileOffset.Text = SettingsManager._settings.ValuesInHex ? Utils.DecimalToHex((int)file.Offset) : file.Offset.ToString();

            //            ListViewSubItem lvFileSize = new ListViewSubItem();
            //            lvFileSize.Text = SettingsManager._settings.ValuesInHex ? Utils.DecimalToHex((int)file.Size) : file.Size.ToString();

            //            ListViewSubItem lvFileCRC32 = new ListViewSubItem();
            //            try
            //            {
            //                byte[] data = new byte[file.Size];
            //                Array.Copy(_reader._buffer, (int)file.Offset, data, 0, data.Length);
            //                var crc = new CrcStream(new MemoryStream(data));
            //                crc.Read(data, 0, data.Length);
            //                lvFileCRC32.Text = crc.ReadCrc.ToString("X8");
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show(ex.ToString());
            //            }

            //            lvFile.SubItems.Add(lvFileOffset);
            //            lvFile.SubItems.Add(lvFileSize);
            //            lvFile.SubItems.Add(lvFileCRC32);
            //            listView1.Items.Add(lvFile);
            //        }
            //        lbFilesCount.Text = "Files Count: " + node.Files.Count;
            //    }
            //}
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            //ProcessStartInfo processStartInfo = new ProcessStartInfo();
            //processStartInfo.
            //Process.Start(processStartInfo);
            var item = listView1.FocusedItem;
            if (item == null)
                return;
            string fileName = item.SubItems[0].Text;
            long fileOffset = int.Parse(item.SubItems[1].Text);
            int fileSize = int.Parse(item.SubItems[2].Text);
            byte[] fileData = new byte[fileSize];
            Array.Copy(reader.buffer, fileOffset, fileData, 0, fileData.Length);
            string pathName = "";
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                fs.Write(fileData, 0, fileData.Length);
                fs.Close();
                pathName = fs.Name;
            }
            Process.Start(pathName);
        }
    }
}
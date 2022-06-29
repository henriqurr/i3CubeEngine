namespace i3CubeEngine.Forms
{
    partial class EngineForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EngineForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.openTool = new System.Windows.Forms.ToolStripMenuItem();
            this.exitTool = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusBarText = new System.Windows.Forms.ToolStripStatusLabel();
            this.listIcons = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.backPathButton = new System.Windows.Forms.ToolStripButton();
            this.pathBox = new System.Windows.Forms.ToolStripComboBox();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.nameColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastModifiedColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sizeColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newFileTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.newFolderTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorTSM1 = new System.Windows.Forms.ToolStripSeparator();
            this.openTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.copyTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.cutTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorTSM2 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesTSM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.applicationToolStrip,
            this.optionsToolStrip});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(12, 20);
            // 
            // applicationToolStrip
            // 
            this.applicationToolStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openTool,
            this.exitTool});
            this.applicationToolStrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applicationToolStrip.Name = "applicationToolStrip";
            this.applicationToolStrip.Size = new System.Drawing.Size(71, 20);
            this.applicationToolStrip.Text = "Application";
            // 
            // openTool
            // 
            this.openTool.Name = "openTool";
            this.openTool.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openTool.Size = new System.Drawing.Size(180, 22);
            this.openTool.Text = "Open Folder";
            this.openTool.Click += new System.EventHandler(this.openTool_Click);
            // 
            // exitTool
            // 
            this.exitTool.Name = "exitTool";
            this.exitTool.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitTool.Size = new System.Drawing.Size(180, 22);
            this.exitTool.Text = "Exit";
            this.exitTool.Click += new System.EventHandler(this.exitTool_Click);
            // 
            // optionsToolStrip
            // 
            this.optionsToolStrip.Name = "optionsToolStrip";
            this.optionsToolStrip.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStrip.Text = "Options";
            // 
            // folderBrowser
            // 
            this.folderBrowser.Description = "Open your folder work.";
            this.folderBrowser.ShowNewFolderButton = false;
            // 
            // statusBar
            // 
            this.statusBar.AutoSize = false;
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarText});
            this.statusBar.Location = new System.Drawing.Point(0, 499);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(984, 22);
            this.statusBar.TabIndex = 1;
            this.statusBar.Text = "statusBar";
            // 
            // statusBarText
            // 
            this.statusBarText.Name = "statusBarText";
            this.statusBarText.Size = new System.Drawing.Size(0, 17);
            // 
            // listIcons
            // 
            this.listIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.listIcons.ImageSize = new System.Drawing.Size(20, 20);
            this.listIcons.TransparentColor = System.Drawing.SystemColors.Control;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backPathButton,
            this.pathBox,
            this.progressBar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "mainToolStrip";
            // 
            // backPathButton
            // 
            this.backPathButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.backPathButton.Image = global::i3CubeEngine.Resource.left_arrow_red;
            this.backPathButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backPathButton.Name = "backPathButton";
            this.backPathButton.Size = new System.Drawing.Size(23, 22);
            this.backPathButton.Text = "Back";
            this.backPathButton.Click += new System.EventHandler(this.BackPath);
            // 
            // pathBox
            // 
            this.pathBox.AutoSize = false;
            this.pathBox.BackColor = System.Drawing.Color.White;
            this.pathBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.pathBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.pathBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathBox.Name = "pathBox";
            this.pathBox.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.pathBox.Size = new System.Drawing.Size(700, 21);
            this.pathBox.Text = "\\";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.progressBar.Size = new System.Drawing.Size(100, 22);
            this.progressBar.Step = 1;
            this.progressBar.Visible = false;
            // 
            // listViewFiles
            // 
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColHeader,
            this.lastModifiedColHeader,
            this.typeColHeader,
            this.sizeColHeader});
            this.listViewFiles.ContextMenuStrip = this.menuStrip;
            this.listViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.HideSelection = false;
            this.listViewFiles.LabelEdit = true;
            this.listViewFiles.Location = new System.Drawing.Point(0, 49);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.Size = new System.Drawing.Size(984, 450);
            this.listViewFiles.SmallImageList = this.listIcons;
            this.listViewFiles.TabIndex = 3;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            this.listViewFiles.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewFiles_AfterLabelEdit);
            this.listViewFiles.ItemActivate += new System.EventHandler(this.listViewFiles_ItemActivate);
            this.listViewFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShortcutKeys);
            // 
            // nameColHeader
            // 
            this.nameColHeader.Text = "Name";
            this.nameColHeader.Width = 574;
            // 
            // lastModifiedColHeader
            // 
            this.lastModifiedColHeader.Text = "Last modified";
            this.lastModifiedColHeader.Width = 133;
            // 
            // typeColHeader
            // 
            this.typeColHeader.Text = "Type";
            this.typeColHeader.Width = 140;
            // 
            // sizeColHeader
            // 
            this.sizeColHeader.Text = "Size                     ";
            this.sizeColHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.sizeColHeader.Width = 101;
            // 
            // menuStrip
            // 
            this.menuStrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileTSM,
            this.newFolderTSM,
            this.refreshTSM,
            this.separatorTSM1,
            this.openTSM,
            this.copyTSM,
            this.cutTSM,
            this.pasteTSM,
            this.deleteTSM,
            this.separatorTSM2,
            this.propertiesTSM});
            this.menuStrip.Name = "contextMenuStrip";
            this.menuStrip.Size = new System.Drawing.Size(192, 214);
            this.menuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.menuStrip_Opening);
            // 
            // newFileTSM
            // 
            this.newFileTSM.Enabled = false;
            this.newFileTSM.Image = global::i3CubeEngine.Resource.folder_red;
            this.newFileTSM.Name = "newFileTSM";
            this.newFileTSM.Size = new System.Drawing.Size(191, 22);
            this.newFileTSM.Text = "New File";
            this.newFileTSM.Click += new System.EventHandler(this.newFileTSM_Click);
            // 
            // newFolderTSM
            // 
            this.newFolderTSM.Enabled = false;
            this.newFolderTSM.Image = global::i3CubeEngine.Properties.Resources.folder;
            this.newFolderTSM.Name = "newFolderTSM";
            this.newFolderTSM.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.newFolderTSM.Size = new System.Drawing.Size(191, 22);
            this.newFolderTSM.Text = "New Folder";
            this.newFolderTSM.Click += new System.EventHandler(this.newFolderTSM_Click);
            // 
            // refreshTSM
            // 
            this.refreshTSM.Enabled = false;
            this.refreshTSM.Name = "refreshTSM";
            this.refreshTSM.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshTSM.Size = new System.Drawing.Size(191, 22);
            this.refreshTSM.Text = "Refresh";
            this.refreshTSM.Click += new System.EventHandler(this.refreshTSM_Click);
            // 
            // separatorTSM1
            // 
            this.separatorTSM1.Name = "separatorTSM1";
            this.separatorTSM1.Size = new System.Drawing.Size(188, 6);
            // 
            // openTSM
            // 
            this.openTSM.Enabled = false;
            this.openTSM.Name = "openTSM";
            this.openTSM.Size = new System.Drawing.Size(191, 22);
            this.openTSM.Text = "Open File";
            this.openTSM.Click += new System.EventHandler(this.openTSM_Click);
            // 
            // copyTSM
            // 
            this.copyTSM.Enabled = false;
            this.copyTSM.Name = "copyTSM";
            this.copyTSM.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyTSM.Size = new System.Drawing.Size(191, 22);
            this.copyTSM.Text = "Copy";
            this.copyTSM.Click += new System.EventHandler(this.copyTSM_Click);
            // 
            // cutTSM
            // 
            this.cutTSM.Enabled = false;
            this.cutTSM.Name = "cutTSM";
            this.cutTSM.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutTSM.Size = new System.Drawing.Size(191, 22);
            this.cutTSM.Text = "Cut";
            this.cutTSM.Click += new System.EventHandler(this.cutTSM_Click);
            // 
            // pasteTSM
            // 
            this.pasteTSM.Enabled = false;
            this.pasteTSM.Name = "pasteTSM";
            this.pasteTSM.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteTSM.Size = new System.Drawing.Size(191, 22);
            this.pasteTSM.Text = "Paste";
            this.pasteTSM.Click += new System.EventHandler(this.pasteTSM_Click);
            // 
            // deleteTSM
            // 
            this.deleteTSM.Enabled = false;
            this.deleteTSM.Name = "deleteTSM";
            this.deleteTSM.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteTSM.Size = new System.Drawing.Size(191, 22);
            this.deleteTSM.Text = "Delete";
            this.deleteTSM.Click += new System.EventHandler(this.deleteTSM_Click);
            // 
            // separatorTSM2
            // 
            this.separatorTSM2.Name = "separatorTSM2";
            this.separatorTSM2.Size = new System.Drawing.Size(188, 6);
            // 
            // propertiesTSM
            // 
            this.propertiesTSM.Enabled = false;
            this.propertiesTSM.Name = "propertiesTSM";
            this.propertiesTSM.Size = new System.Drawing.Size(191, 22);
            this.propertiesTSM.Text = "Properties";
            this.propertiesTSM.Click += new System.EventHandler(this.propertiesTSM_Click);
            // 
            // EngineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 521);
            this.Controls.Add(this.listViewFiles);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EngineForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "i3CubeEngine";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EngineForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShortcutKeys);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem applicationToolStrip;
        private System.Windows.Forms.ToolStripMenuItem openTool;
        private System.Windows.Forms.ToolStripMenuItem exitTool;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ImageList listIcons;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton backPathButton;
        private System.Windows.Forms.ToolStripComboBox pathBox;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStrip;
        public System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.ColumnHeader nameColHeader;
        private System.Windows.Forms.ColumnHeader lastModifiedColHeader;
        private System.Windows.Forms.ColumnHeader typeColHeader;
        private System.Windows.Forms.ColumnHeader sizeColHeader;
        private System.Windows.Forms.ToolStripStatusLabel statusBarText;
        private System.Windows.Forms.ContextMenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem openTSM;
        private System.Windows.Forms.ToolStripMenuItem refreshTSM;
        private System.Windows.Forms.ToolStripMenuItem deleteTSM;
        private System.Windows.Forms.ToolStripMenuItem propertiesTSM;
        public System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripMenuItem newFileTSM;
        private System.Windows.Forms.ToolStripMenuItem newFolderTSM;
        private System.Windows.Forms.ToolStripSeparator separatorTSM1;
        private System.Windows.Forms.ToolStripSeparator separatorTSM2;
        private System.Windows.Forms.ToolStripMenuItem copyTSM;
        private System.Windows.Forms.ToolStripMenuItem pasteTSM;
        private System.Windows.Forms.ToolStripMenuItem cutTSM;
    }
}
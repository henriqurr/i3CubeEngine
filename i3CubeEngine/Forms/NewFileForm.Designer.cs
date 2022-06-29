namespace i3CubeEngine
{
    partial class NewFileForm
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
            this.mLabel1 = new System.Windows.Forms.Label();
            this.mFileNameInput = new System.Windows.Forms.TextBox();
            this.mOkBtn = new System.Windows.Forms.Button();
            this.mCancelBtn = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.localLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mLabel1
            // 
            this.mLabel1.AutoSize = true;
            this.mLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mLabel1.Location = new System.Drawing.Point(9, 47);
            this.mLabel1.Name = "mLabel1";
            this.mLabel1.Size = new System.Drawing.Size(55, 13);
            this.mLabel1.TabIndex = 0;
            this.mLabel1.Text = "File name:";
            // 
            // mFileNameInput
            // 
            this.mFileNameInput.Location = new System.Drawing.Point(70, 42);
            this.mFileNameInput.Name = "mFileNameInput";
            this.mFileNameInput.Size = new System.Drawing.Size(335, 21);
            this.mFileNameInput.TabIndex = 1;
            this.mFileNameInput.TextChanged += new System.EventHandler(this.mFileNameInput_TextChanged);
            // 
            // mOkBtn
            // 
            this.mOkBtn.Enabled = false;
            this.mOkBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mOkBtn.Location = new System.Drawing.Point(355, 66);
            this.mOkBtn.Name = "mOkBtn";
            this.mOkBtn.Size = new System.Drawing.Size(50, 25);
            this.mOkBtn.TabIndex = 2;
            this.mOkBtn.Text = "OK";
            this.mOkBtn.UseVisualStyleBackColor = true;
            this.mOkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // mCancelBtn
            // 
            this.mCancelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mCancelBtn.Location = new System.Drawing.Point(294, 66);
            this.mCancelBtn.Name = "mCancelBtn";
            this.mCancelBtn.Size = new System.Drawing.Size(55, 25);
            this.mCancelBtn.TabIndex = 3;
            this.mCancelBtn.Text = "Cancel";
            this.mCancelBtn.UseVisualStyleBackColor = true;
            this.mCancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(12, 6);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(58, 38);
            this.pictureBox.TabIndex = 13;
            this.pictureBox.TabStop = false;
            // 
            // localLabel
            // 
            this.localLabel.AutoSize = true;
            this.localLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.localLabel.Location = new System.Drawing.Point(9, 78);
            this.localLabel.Name = "localLabel";
            this.localLabel.Size = new System.Drawing.Size(29, 13);
            this.localLabel.TabIndex = 14;
            this.localLabel.Text = "local";
            // 
            // NewFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 96);
            this.ControlBox = false;
            this.Controls.Add(this.localLabel);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.mCancelBtn);
            this.Controls.Add(this.mOkBtn);
            this.Controls.Add(this.mFileNameInput);
            this.Controls.Add(this.mLabel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "NewFileForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New file";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label mLabel1;
        private System.Windows.Forms.TextBox mFileNameInput;
        private System.Windows.Forms.Button mOkBtn;
        private System.Windows.Forms.Button mCancelBtn;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label localLabel;
    }
}
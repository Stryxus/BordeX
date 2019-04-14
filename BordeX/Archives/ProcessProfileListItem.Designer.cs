namespace BordeX.Forms.Controls
{
    partial class ProcessProfileListItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.windowIcon = new System.Windows.Forms.PictureBox();
            this.windowTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.windowIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // windowIcon
            // 
            this.windowIcon.BackColor = System.Drawing.Color.Gray;
            this.windowIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.windowIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.windowIcon.Location = new System.Drawing.Point(0, 0);
            this.windowIcon.Margin = new System.Windows.Forms.Padding(0);
            this.windowIcon.Name = "windowIcon";
            this.windowIcon.Size = new System.Drawing.Size(20, 20);
            this.windowIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.windowIcon.TabIndex = 0;
            this.windowIcon.TabStop = false;
            // 
            // windowTitle
            // 
            this.windowTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.windowTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowTitle.ForeColor = System.Drawing.Color.Black;
            this.windowTitle.Location = new System.Drawing.Point(20, 0);
            this.windowTitle.Name = "windowTitle";
            this.windowTitle.Size = new System.Drawing.Size(260, 20);
            this.windowTitle.TabIndex = 1;
            this.windowTitle.Text = "Window";
            this.windowTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.windowTitle.Click += new System.EventHandler(this.windowTitle_Click);
            // 
            // ProcessProfileListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.windowTitle);
            this.Controls.Add(this.windowIcon);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ProcessProfileListItem";
            this.Size = new System.Drawing.Size(280, 20);
            ((System.ComponentModel.ISupportInitialize)(this.windowIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox windowIcon;
        private System.Windows.Forms.Label windowTitle;
    }
}

namespace FuturisticAntivirus
{
    partial class DetailForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblScanDate;
        private System.Windows.Forms.Label lblDetections;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.TextBox txtHash;
        private System.Windows.Forms.ListView lvDetections;
        private System.Windows.Forms.ColumnHeader colEngine;
        private System.Windows.Forms.ColumnHeader colResult;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblPathLabel;
        private System.Windows.Forms.Label lblHashLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblScanDate = new System.Windows.Forms.Label();
            this.lblDetections = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.txtHash = new System.Windows.Forms.TextBox();
            this.lvDetections = new System.Windows.Forms.ListView();
            this.colEngine = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnClose = new System.Windows.Forms.Button();
            this.lblPathLabel = new System.Windows.Forms.Label();
            this.lblHashLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileName.Location = new System.Drawing.Point(12, 9);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(94, 25);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "File Name";
            // 
            // lblScanDate
            // 
            this.lblScanDate.AutoSize = true;
            this.lblScanDate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScanDate.Location = new System.Drawing.Point(14, 43);
            this.lblScanDate.Name = "lblScanDate";
            this.lblScanDate.Size = new System.Drawing.Size(62, 15);
            this.lblScanDate.TabIndex = 1;
            this.lblScanDate.Text = "Scan Date";
            // 
            // lblDetections
            // 
            this.lblDetections.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDetections.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetections.Location = new System.Drawing.Point(408, 13);
            this.lblDetections.Name = "lblDetections";
            this.lblDetections.Size = new System.Drawing.Size(164, 21);
            this.lblDetections.TabIndex = 2;
            this.lblDetections.Text = "Detections: 0 / 0";
            this.lblDetections.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilePath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilePath.Location = new System.Drawing.Point(17, 91);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(555, 22);
            this.txtFilePath.TabIndex = 3;
            // 
            // txtHash
            // 
            this.txtHash.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHash.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHash.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHash.Location = new System.Drawing.Point(17, 140);
            this.txtHash.Name = "txtHash";
            this.txtHash.ReadOnly = true;
            this.txtHash.Size = new System.Drawing.Size(555, 22);
            this.txtHash.TabIndex = 4;
            // 
            // lvDetections
            // 
            this.lvDetections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDetections.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvDetections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEngine,
            this.colResult});
            this.lvDetections.FullRowSelect = true;
            this.lvDetections.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvDetections.HideSelection = false;
            this.lvDetections.Location = new System.Drawing.Point(17, 182);
            this.lvDetections.Name = "lvDetections";
            this.lvDetections.Size = new System.Drawing.Size(555, 221);
            this.lvDetections.TabIndex = 5;
            this.lvDetections.UseCompatibleStateImageBehavior = false;
            this.lvDetections.View = System.Windows.Forms.View.Details;
            // 
            // colEngine
            // 
            this.colEngine.Text = "Antivirus Engine";
            this.colEngine.Width = 200;
            // 
            // colResult
            // 
            this.colResult.Text = "Result";
            this.colResult.Width = 330;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(472, 420);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblPathLabel
            // 
            this.lblPathLabel.AutoSize = true;
            this.lblPathLabel.Location = new System.Drawing.Point(14, 73);
            this.lblPathLabel.Name = "lblPathLabel";
            this.lblPathLabel.Size = new System.Drawing.Size(52, 15);
            this.lblPathLabel.TabIndex = 7;
            this.lblPathLabel.Text = "File Path";
            // 
            // lblHashLabel
            // 
            this.lblHashLabel.AutoSize = true;
            this.lblHashLabel.Location = new System.Drawing.Point(14, 122);
            this.lblHashLabel.Name = "lblHashLabel";
            this.lblHashLabel.Size = new System.Drawing.Size(79, 15);
            this.lblHashLabel.TabIndex = 8;
            this.lblHashLabel.Text = "SHA256 Hash";
            // 
            // DetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.lblHashLabel);
            this.Controls.Add(this.lblPathLabel);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lvDetections);
            this.Controls.Add(this.txtHash);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.lblDetections);
            this.Controls.Add(this.lblScanDate);
            this.Controls.Add(this.lblFileName);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DetailForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scan Details";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
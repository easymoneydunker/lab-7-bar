namespace MmfLab
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

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
            this.lblData = new System.Windows.Forms.Label();
            this.editData = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblData
            //
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(12, 15);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(33, 15);
            this.lblData.TabIndex = 0;
            this.lblData.Text = "Data";
            //
            // editData
            //
            this.editData.Location = new System.Drawing.Point(60, 12);
            this.editData.Name = "editData";
            this.editData.Size = new System.Drawing.Size(392, 23);
            this.editData.TabIndex = 1;
            //
            // btnCreate
            //
            this.btnCreate.Location = new System.Drawing.Point(12, 50);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(215, 35);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Create mapping of Data";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            //
            // btnOpen
            //
            this.btnOpen.Location = new System.Drawing.Point(237, 50);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(215, 35);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "Open mapping and get Data";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            //
            // btnClose
            //
            this.btnClose.Enabled = false;
            this.btnClose.Location = new System.Drawing.Point(12, 95);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(215, 35);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close mapping of Data";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // btnExit
            //
            this.btnExit.Location = new System.Drawing.Point(237, 95);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(215, 35);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 142);
            this.Controls.Add(this.lblData);
            this.Controls.Add(this.editData);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "MMF";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.TextBox editData;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExit;
    }
}

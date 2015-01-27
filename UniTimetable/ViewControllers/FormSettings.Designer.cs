namespace UniTimetable.ViewControllers
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableSettings = new System.Windows.Forms.TableLayoutPanel();
            this.cbReset = new System.Windows.Forms.CheckBox();
            this.cbWeekend = new System.Windows.Forms.CheckBox();
            this.cbGray = new System.Windows.Forms.CheckBox();
            this.cbLocation = new System.Windows.Forms.CheckBox();
            this.cbGhost = new System.Windows.Forms.CheckBox();
            this.ddEnd = new System.Windows.Forms.ComboBox();
            this.ddStart = new System.Windows.Forms.ComboBox();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblEnd = new System.Windows.Forms.Label();
            this.buttonColours = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableSettings.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.Location = new System.Drawing.Point(3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(125, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Location = new System.Drawing.Point(134, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(125, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // tableSettings
            // 
            this.tableSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableSettings.ColumnCount = 4;
            this.tableSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableSettings.Controls.Add(this.cbReset, 0, 7);
            this.tableSettings.Controls.Add(this.cbWeekend, 0, 2);
            this.tableSettings.Controls.Add(this.cbGray, 0, 3);
            this.tableSettings.Controls.Add(this.cbLocation, 0, 4);
            this.tableSettings.Controls.Add(this.cbGhost, 0, 1);
            this.tableSettings.Controls.Add(this.ddEnd, 1, 6);
            this.tableSettings.Controls.Add(this.ddStart, 1, 5);
            this.tableSettings.Controls.Add(this.lblStart, 0, 5);
            this.tableSettings.Controls.Add(this.lblEnd, 0, 6);
            this.tableSettings.Controls.Add(this.buttonColours, 3, 0);
            this.tableSettings.Controls.Add(this.label1, 0, 0);
            this.tableSettings.Location = new System.Drawing.Point(12, 12);
            this.tableSettings.Name = "tableSettings";
            this.tableSettings.RowCount = 8;
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableSettings.Size = new System.Drawing.Size(262, 227);
            this.tableSettings.TabIndex = 4;
            // 
            // cbReset
            // 
            this.cbReset.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.cbReset, 4);
            this.cbReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbReset.Location = new System.Drawing.Point(3, 199);
            this.cbReset.Name = "cbReset";
            this.cbReset.Size = new System.Drawing.Size(256, 25);
            this.cbReset.TabIndex = 6;
            this.cbReset.Text = "Reset window dimensions";
            this.cbReset.UseVisualStyleBackColor = true;
            // 
            // cbWeekend
            // 
            this.cbWeekend.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.cbWeekend, 4);
            this.cbWeekend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbWeekend.Location = new System.Drawing.Point(3, 59);
            this.cbWeekend.Name = "cbWeekend";
            this.cbWeekend.Size = new System.Drawing.Size(256, 22);
            this.cbWeekend.TabIndex = 0;
            this.cbWeekend.Text = "Include weekend in timetable";
            this.cbWeekend.UseVisualStyleBackColor = true;
            // 
            // cbGray
            // 
            this.cbGray.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.cbGray, 4);
            this.cbGray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbGray.Location = new System.Drawing.Point(3, 87);
            this.cbGray.Name = "cbGray";
            this.cbGray.Size = new System.Drawing.Size(256, 22);
            this.cbGray.TabIndex = 1;
            this.cbGray.Text = "Grey out times where there are no classes";
            this.cbGray.UseVisualStyleBackColor = true;
            // 
            // cbLocation
            // 
            this.cbLocation.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.cbLocation, 4);
            this.cbLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLocation.Location = new System.Drawing.Point(3, 115);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(256, 22);
            this.cbLocation.TabIndex = 2;
            this.cbLocation.Text = "Display class location (building and room)";
            this.cbLocation.UseVisualStyleBackColor = true;
            // 
            // cbGhost
            // 
            this.cbGhost.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.cbGhost, 4);
            this.cbGhost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbGhost.Location = new System.Drawing.Point(3, 31);
            this.cbGhost.Name = "cbGhost";
            this.cbGhost.Size = new System.Drawing.Size(256, 22);
            this.cbGhost.TabIndex = 6;
            this.cbGhost.Text = "Show \"ghost\" when dragging";
            this.cbGhost.UseVisualStyleBackColor = true;
            // 
            // ddEnd
            // 
            this.ddEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddEnd.FormattingEnabled = true;
            this.ddEnd.Location = new System.Drawing.Point(68, 171);
            this.ddEnd.Name = "ddEnd";
            this.ddEnd.Size = new System.Drawing.Size(59, 21);
            this.ddEnd.TabIndex = 8;
            // 
            // ddStart
            // 
            this.ddStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddStart.FormattingEnabled = true;
            this.ddStart.Location = new System.Drawing.Point(68, 143);
            this.ddStart.Name = "ddStart";
            this.ddStart.Size = new System.Drawing.Size(59, 21);
            this.ddStart.TabIndex = 7;
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStart.Location = new System.Drawing.Point(3, 140);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(59, 28);
            this.lblStart.TabIndex = 10;
            this.lblStart.Text = "Start";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEnd.Location = new System.Drawing.Point(3, 168);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(59, 28);
            this.lblEnd.TabIndex = 11;
            this.lblEnd.Text = "End";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonColours
            // 
            this.buttonColours.Location = new System.Drawing.Point(198, 3);
            this.buttonColours.Name = "buttonColours";
            this.buttonColours.Size = new System.Drawing.Size(61, 22);
            this.buttonColours.TabIndex = 6;
            this.buttonColours.Text = "Colours...";
            this.buttonColours.UseVisualStyleBackColor = true;
            this.buttonColours.Click += new System.EventHandler(this.ButtonColoursClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.label1, 3);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 28);
            this.label1.TabIndex = 6;
            this.label1.Text = "Set colours used in timetable:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 245);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(262, 29);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // FormSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(286, 286);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettingsLoad);
            this.tableSettings.ResumeLayout(false);
            this.tableSettings.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox ddEnd;
        private System.Windows.Forms.ComboBox ddStart;
        private System.Windows.Forms.CheckBox cbReset;
        private System.Windows.Forms.CheckBox cbWeekend;
        private System.Windows.Forms.CheckBox cbGray;
        private System.Windows.Forms.CheckBox cbLocation;
        private System.Windows.Forms.CheckBox cbGhost;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Button buttonColours;
        private System.Windows.Forms.Label label1;

    }
}
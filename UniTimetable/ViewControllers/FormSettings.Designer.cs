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
            this.tableSettings = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxImportUnsettable = new System.Windows.Forms.CheckBox();
            this.buttonCriteria = new System.Windows.Forms.Button();
            this.labelCriteria = new System.Windows.Forms.Label();
            this.checkBoxReset = new System.Windows.Forms.CheckBox();
            this.checkBoxWeekend = new System.Windows.Forms.CheckBox();
            this.checkBoxGray = new System.Windows.Forms.CheckBox();
            this.checkBoxLocation = new System.Windows.Forms.CheckBox();
            this.checkBoxGhost = new System.Windows.Forms.CheckBox();
            this.ddEnd = new System.Windows.Forms.ComboBox();
            this.ddStart = new System.Windows.Forms.ComboBox();
            this.labelStart = new System.Windows.Forms.Label();
            this.labelEnd = new System.Windows.Forms.Label();
            this.buttonColours = new System.Windows.Forms.Button();
            this.labelColours = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.tableSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableSettings
            // 
            this.tableSettings.AutoSize = true;
            this.tableSettings.ColumnCount = 4;
            this.tableSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableSettings.Controls.Add(this.checkBoxImportUnsettable, 0, 2);
            this.tableSettings.Controls.Add(this.buttonCriteria, 3, 1);
            this.tableSettings.Controls.Add(this.labelCriteria, 0, 1);
            this.tableSettings.Controls.Add(this.checkBoxReset, 0, 9);
            this.tableSettings.Controls.Add(this.checkBoxWeekend, 0, 4);
            this.tableSettings.Controls.Add(this.checkBoxGray, 0, 5);
            this.tableSettings.Controls.Add(this.checkBoxLocation, 0, 6);
            this.tableSettings.Controls.Add(this.checkBoxGhost, 0, 3);
            this.tableSettings.Controls.Add(this.ddEnd, 1, 8);
            this.tableSettings.Controls.Add(this.ddStart, 1, 7);
            this.tableSettings.Controls.Add(this.labelStart, 0, 7);
            this.tableSettings.Controls.Add(this.labelEnd, 0, 8);
            this.tableSettings.Controls.Add(this.buttonColours, 3, 0);
            this.tableSettings.Controls.Add(this.labelColours, 0, 0);
            this.tableSettings.Controls.Add(this.buttonCancel, 3, 10);
            this.tableSettings.Controls.Add(this.buttonOk, 2, 10);
            this.tableSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableSettings.Location = new System.Drawing.Point(5, 5);
            this.tableSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tableSettings.Name = "tableSettings";
            this.tableSettings.RowCount = 11;
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableSettings.Size = new System.Drawing.Size(342, 330);
            this.tableSettings.TabIndex = 4;
            // 
            // checkBoxImportUnsettable
            // 
            this.checkBoxImportUnsettable.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.checkBoxImportUnsettable, 4);
            this.checkBoxImportUnsettable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxImportUnsettable.Location = new System.Drawing.Point(3, 63);
            this.checkBoxImportUnsettable.Name = "checkBoxImportUnsettable";
            this.checkBoxImportUnsettable.Size = new System.Drawing.Size(336, 24);
            this.checkBoxImportUnsettable.TabIndex = 15;
            this.checkBoxImportUnsettable.Text = "Import full or unselectable streams";
            this.checkBoxImportUnsettable.UseVisualStyleBackColor = true;
            // 
            // buttonCriteria
            // 
            this.buttonCriteria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCriteria.Location = new System.Drawing.Point(258, 33);
            this.buttonCriteria.Name = "buttonCriteria";
            this.buttonCriteria.Size = new System.Drawing.Size(81, 24);
            this.buttonCriteria.TabIndex = 14;
            this.buttonCriteria.Text = "Criteria...";
            this.buttonCriteria.UseVisualStyleBackColor = true;
            this.buttonCriteria.Click += new System.EventHandler(this.ButtonCriteriaClick);
            // 
            // labelCriteria
            // 
            this.labelCriteria.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.labelCriteria, 3);
            this.labelCriteria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCriteria.Location = new System.Drawing.Point(3, 30);
            this.labelCriteria.Name = "labelCriteria";
            this.labelCriteria.Size = new System.Drawing.Size(249, 30);
            this.labelCriteria.TabIndex = 13;
            this.labelCriteria.Text = "Set criteria used in the solver:";
            this.labelCriteria.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxReset
            // 
            this.checkBoxReset.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.checkBoxReset, 4);
            this.checkBoxReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxReset.Location = new System.Drawing.Point(3, 273);
            this.checkBoxReset.Name = "checkBoxReset";
            this.checkBoxReset.Size = new System.Drawing.Size(336, 24);
            this.checkBoxReset.TabIndex = 6;
            this.checkBoxReset.Text = "Reset window dimensions";
            this.checkBoxReset.UseVisualStyleBackColor = true;
            // 
            // checkBoxWeekend
            // 
            this.checkBoxWeekend.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.checkBoxWeekend, 4);
            this.checkBoxWeekend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxWeekend.Location = new System.Drawing.Point(3, 123);
            this.checkBoxWeekend.Name = "checkBoxWeekend";
            this.checkBoxWeekend.Size = new System.Drawing.Size(336, 24);
            this.checkBoxWeekend.TabIndex = 0;
            this.checkBoxWeekend.Text = "Include weekend in timetable";
            this.checkBoxWeekend.UseVisualStyleBackColor = true;
            // 
            // checkBoxGray
            // 
            this.checkBoxGray.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.checkBoxGray, 4);
            this.checkBoxGray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxGray.Location = new System.Drawing.Point(3, 153);
            this.checkBoxGray.Name = "checkBoxGray";
            this.checkBoxGray.Size = new System.Drawing.Size(336, 24);
            this.checkBoxGray.TabIndex = 1;
            this.checkBoxGray.Text = "Grey out times where there are no classes";
            this.checkBoxGray.UseVisualStyleBackColor = true;
            // 
            // checkBoxLocation
            // 
            this.checkBoxLocation.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.checkBoxLocation, 4);
            this.checkBoxLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxLocation.Location = new System.Drawing.Point(3, 183);
            this.checkBoxLocation.Name = "checkBoxLocation";
            this.checkBoxLocation.Size = new System.Drawing.Size(336, 24);
            this.checkBoxLocation.TabIndex = 2;
            this.checkBoxLocation.Text = "Display class location (building and room)";
            this.checkBoxLocation.UseVisualStyleBackColor = true;
            // 
            // checkBoxGhost
            // 
            this.checkBoxGhost.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.checkBoxGhost, 4);
            this.checkBoxGhost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxGhost.Location = new System.Drawing.Point(3, 93);
            this.checkBoxGhost.Name = "checkBoxGhost";
            this.checkBoxGhost.Size = new System.Drawing.Size(336, 24);
            this.checkBoxGhost.TabIndex = 6;
            this.checkBoxGhost.Text = "Show \"ghost\" when dragging";
            this.checkBoxGhost.UseVisualStyleBackColor = true;
            // 
            // ddEnd
            // 
            this.ddEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddEnd.FormattingEnabled = true;
            this.ddEnd.Items.AddRange(new object[] {
            "12 am",
            "1 am",
            "2 am",
            "3 am",
            "4 am",
            "5 am",
            "6 am",
            "7 am",
            "8 am",
            "9 am",
            "10 am",
            "11 am",
            "12 pm",
            "1 pm",
            "2 pm",
            "3 pm",
            "4 pm",
            "5 pm",
            "6 pm",
            "7 pm",
            "8 pm",
            "9 pm",
            "10 pm",
            "11 pm"});
            this.ddEnd.Location = new System.Drawing.Point(88, 243);
            this.ddEnd.Name = "ddEnd";
            this.ddEnd.Size = new System.Drawing.Size(79, 21);
            this.ddEnd.TabIndex = 8;
            // 
            // ddStart
            // 
            this.ddStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddStart.FormattingEnabled = true;
            this.ddStart.Items.AddRange(new object[] {
            "12 am",
            "1 am",
            "2 am",
            "3 am",
            "4 am",
            "5 am",
            "6 am",
            "7 am",
            "8 am",
            "9 am",
            "10 am",
            "11 am",
            "12 pm",
            "1 pm",
            "2 pm",
            "3 pm",
            "4 pm",
            "5 pm",
            "6 pm",
            "7 pm",
            "8 pm",
            "9 pm",
            "10 pm",
            "11 pm"});
            this.ddStart.Location = new System.Drawing.Point(88, 213);
            this.ddStart.Name = "ddStart";
            this.ddStart.Size = new System.Drawing.Size(79, 21);
            this.ddStart.TabIndex = 7;
            // 
            // labelStart
            // 
            this.labelStart.AutoSize = true;
            this.labelStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStart.Location = new System.Drawing.Point(3, 210);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(79, 30);
            this.labelStart.TabIndex = 10;
            this.labelStart.Text = "Start";
            this.labelStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelEnd
            // 
            this.labelEnd.AutoSize = true;
            this.labelEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEnd.Location = new System.Drawing.Point(3, 240);
            this.labelEnd.Name = "labelEnd";
            this.labelEnd.Size = new System.Drawing.Size(79, 30);
            this.labelEnd.TabIndex = 11;
            this.labelEnd.Text = "End";
            this.labelEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonColours
            // 
            this.buttonColours.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonColours.Location = new System.Drawing.Point(258, 3);
            this.buttonColours.Name = "buttonColours";
            this.buttonColours.Size = new System.Drawing.Size(81, 24);
            this.buttonColours.TabIndex = 6;
            this.buttonColours.Text = "Colours...";
            this.buttonColours.UseVisualStyleBackColor = true;
            this.buttonColours.Click += new System.EventHandler(this.ButtonColoursClick);
            // 
            // labelColours
            // 
            this.labelColours.AutoSize = true;
            this.tableSettings.SetColumnSpan(this.labelColours, 3);
            this.labelColours.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelColours.Location = new System.Drawing.Point(3, 0);
            this.labelColours.Name = "labelColours";
            this.labelColours.Size = new System.Drawing.Size(249, 30);
            this.labelColours.TabIndex = 6;
            this.labelColours.Text = "Set colours used in timetable:";
            this.labelColours.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(258, 303);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(81, 24);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // buttonOk
            // 
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOk.Location = new System.Drawing.Point(173, 303);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(79, 24);
            this.buttonOk.TabIndex = 12;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(352, 340);
            this.Controls.Add(this.tableSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettingsLoad);
            this.tableSettings.ResumeLayout(false);
            this.tableSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableSettings;
        private System.Windows.Forms.ComboBox ddEnd;
        private System.Windows.Forms.ComboBox ddStart;
        private System.Windows.Forms.CheckBox checkBoxReset;
        private System.Windows.Forms.CheckBox checkBoxWeekend;
        private System.Windows.Forms.CheckBox checkBoxGray;
        private System.Windows.Forms.CheckBox checkBoxLocation;
        private System.Windows.Forms.CheckBox checkBoxGhost;
        private System.Windows.Forms.CheckBox checkBoxImportUnsettable;
        private System.Windows.Forms.Label labelStart;
        private System.Windows.Forms.Label labelEnd;
        private System.Windows.Forms.Label labelColours;
        private System.Windows.Forms.Label labelCriteria;
        private System.Windows.Forms.Button buttonColours;
        private System.Windows.Forms.Button buttonCriteria;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;

    }
}
namespace UniTimetable
{
    partial class FormImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImport));
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.panelStreams = new System.Windows.Forms.Panel();
            this.checkBoxS2 = new System.Windows.Forms.CheckBox();
            this.checkBoxS1 = new System.Windows.Forms.CheckBox();
            this.checkBoxTest = new System.Windows.Forms.CheckBox();
            this.lblClashNotice = new System.Windows.Forms.Label();
            this.lblIgnored = new System.Windows.Forms.Label();
            this.lblTitle4 = new System.Windows.Forms.Label();
            this.lblRequired = new System.Windows.Forms.Label();
            this.listViewRequired = new System.Windows.Forms.ListView();
            this.Code = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TypeName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRequire = new System.Windows.Forms.Button();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.listViewIgnored = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelLoading = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarLoading = new System.Windows.Forms.ProgressBar();
            this.panelPreview = new System.Windows.Forms.Panel();
            this.boxStreamDetails = new System.Windows.Forms.GroupBox();
            this.timetableControl1 = new UniTimetable.TimetableControl();
            this.txtTreeDetails = new System.Windows.Forms.TextBox();
            this.treePreview = new System.Windows.Forms.TreeView();
            this.lblTitle3 = new System.Windows.Forms.Label();
            this.panelBottom.SuspendLayout();
            this.panelStreams.SuspendLayout();
            this.panelLoading.SuspendLayout();
            this.panelPreview.SuspendLayout();
            this.boxStreamDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBottom.Controls.Add(this.btnCancel);
            this.panelBottom.Controls.Add(this.btnNext);
            this.panelBottom.Controls.Add(this.btnFinish);
            this.panelBottom.Location = new System.Drawing.Point(2, 485);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(540, 38);
            this.panelBottom.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(453, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Enabled = false;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(355, 8);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "&Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNextClick);
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinish.Location = new System.Drawing.Point(355, 8);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 1;
            this.btnFinish.Text = "&Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Visible = false;
            this.btnFinish.Click += new System.EventHandler(this.BtnFinishClick);
            // 
            // panelStreams
            // 
            this.panelStreams.Controls.Add(this.checkBoxS2);
            this.panelStreams.Controls.Add(this.checkBoxS1);
            this.panelStreams.Controls.Add(this.checkBoxTest);
            this.panelStreams.Controls.Add(this.lblClashNotice);
            this.panelStreams.Controls.Add(this.lblIgnored);
            this.panelStreams.Controls.Add(this.lblTitle4);
            this.panelStreams.Controls.Add(this.lblRequired);
            this.panelStreams.Controls.Add(this.listViewRequired);
            this.panelStreams.Controls.Add(this.btnRequire);
            this.panelStreams.Controls.Add(this.btnIgnore);
            this.panelStreams.Controls.Add(this.listViewIgnored);
            this.panelStreams.Location = new System.Drawing.Point(2, 89);
            this.panelStreams.Name = "panelStreams";
            this.panelStreams.Size = new System.Drawing.Size(540, 397);
            this.panelStreams.TabIndex = 6;
            // 
            // checkBoxS2
            // 
            this.checkBoxS2.AutoSize = true;
            this.checkBoxS2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxS2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxS2.Location = new System.Drawing.Point(244, 176);
            this.checkBoxS2.Name = "checkBoxS2";
            this.checkBoxS2.Size = new System.Drawing.Size(36, 18);
            this.checkBoxS2.TabIndex = 15;
            this.checkBoxS2.Text = "S2";
            this.checkBoxS2.UseVisualStyleBackColor = true;
            this.checkBoxS2.CheckedChanged += new System.EventHandler(this.CheckBoxS2CheckedChanged);
            // 
            // checkBoxS1
            // 
            this.checkBoxS1.AutoSize = true;
            this.checkBoxS1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxS1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxS1.Location = new System.Drawing.Point(244, 153);
            this.checkBoxS1.Name = "checkBoxS1";
            this.checkBoxS1.Size = new System.Drawing.Size(36, 18);
            this.checkBoxS1.TabIndex = 14;
            this.checkBoxS1.Text = "S1";
            this.checkBoxS1.UseVisualStyleBackColor = true;
            this.checkBoxS1.CheckedChanged += new System.EventHandler(this.CheckBoxS1CheckedChanged);
            // 
            // checkBoxTest
            // 
            this.checkBoxTest.AutoSize = true;
            this.checkBoxTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxTest.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxTest.Location = new System.Drawing.Point(244, 199);
            this.checkBoxTest.Name = "checkBoxTest";
            this.checkBoxTest.Size = new System.Drawing.Size(49, 18);
            this.checkBoxTest.TabIndex = 13;
            this.checkBoxTest.Text = "Tests";
            this.checkBoxTest.UseVisualStyleBackColor = true;
            this.checkBoxTest.CheckedChanged += new System.EventHandler(this.CheckBoxTestCheckedChanged);
            // 
            // lblClashNotice
            // 
            this.lblClashNotice.BackColor = System.Drawing.Color.Red;
            this.lblClashNotice.Location = new System.Drawing.Point(308, 10);
            this.lblClashNotice.Name = "lblClashNotice";
            this.lblClashNotice.Size = new System.Drawing.Size(220, 23);
            this.lblClashNotice.TabIndex = 12;
            this.lblClashNotice.Text = "Red indicates an unavoidable clash";
            this.lblClashNotice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblIgnored
            // 
            this.lblIgnored.AutoSize = true;
            this.lblIgnored.Location = new System.Drawing.Point(9, 50);
            this.lblIgnored.Name = "lblIgnored";
            this.lblIgnored.Size = new System.Drawing.Size(43, 13);
            this.lblIgnored.TabIndex = 8;
            this.lblIgnored.Text = "Ignored";
            // 
            // lblTitle4
            // 
            this.lblTitle4.AutoSize = true;
            this.lblTitle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle4.Location = new System.Drawing.Point(8, 9);
            this.lblTitle4.Name = "lblTitle4";
            this.lblTitle4.Size = new System.Drawing.Size(183, 24);
            this.lblTitle4.TabIndex = 0;
            this.lblTitle4.Text = "Streams to Include";
            // 
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.Location = new System.Drawing.Point(305, 50);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(50, 13);
            this.lblRequired.TabIndex = 6;
            this.lblRequired.Text = "Required";
            // 
            // listViewRequired
            // 
            this.listViewRequired.AllowColumnReorder = true;
            this.listViewRequired.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewRequired.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Code,
            this.TypeName});
            this.listViewRequired.FullRowSelect = true;
            this.listViewRequired.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewRequired.Location = new System.Drawing.Point(308, 69);
            this.listViewRequired.MultiSelect = false;
            this.listViewRequired.Name = "listViewRequired";
            this.listViewRequired.Size = new System.Drawing.Size(220, 316);
            this.listViewRequired.TabIndex = 7;
            this.listViewRequired.UseCompatibleStateImageBehavior = false;
            this.listViewRequired.View = System.Windows.Forms.View.Details;
            this.listViewRequired.SelectedIndexChanged += new System.EventHandler(this.ListViewRequiredSelectedIndexChanged);
            this.listViewRequired.Enter += new System.EventHandler(this.ListViewRequiredEnter);
            this.listViewRequired.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListViewRequiredMouseDoubleClick);
            // 
            // Code
            // 
            this.Code.Text = "Code";
            this.Code.Width = 30;
            // 
            // TypeName
            // 
            this.TypeName.Text = "Type";
            this.TypeName.Width = 160;
            // 
            // btnRequire
            // 
            this.btnRequire.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRequire.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRequire.Location = new System.Drawing.Point(238, 222);
            this.btnRequire.Name = "btnRequire";
            this.btnRequire.Size = new System.Drawing.Size(64, 23);
            this.btnRequire.TabIndex = 10;
            this.btnRequire.Text = ">>>>";
            this.btnRequire.UseVisualStyleBackColor = true;
            this.btnRequire.Click += new System.EventHandler(this.BtnRequireClick);
            // 
            // btnIgnore
            // 
            this.btnIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIgnore.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIgnore.Location = new System.Drawing.Point(238, 252);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(64, 23);
            this.btnIgnore.TabIndex = 11;
            this.btnIgnore.Text = "<<<<";
            this.btnIgnore.UseVisualStyleBackColor = true;
            this.btnIgnore.Click += new System.EventHandler(this.BtnIgnoreClick);
            // 
            // listViewIgnored
            // 
            this.listViewIgnored.AllowColumnReorder = true;
            this.listViewIgnored.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewIgnored.FullRowSelect = true;
            this.listViewIgnored.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewIgnored.Location = new System.Drawing.Point(12, 69);
            this.listViewIgnored.MultiSelect = false;
            this.listViewIgnored.Name = "listViewIgnored";
            this.listViewIgnored.Size = new System.Drawing.Size(220, 316);
            this.listViewIgnored.TabIndex = 9;
            this.listViewIgnored.UseCompatibleStateImageBehavior = false;
            this.listViewIgnored.View = System.Windows.Forms.View.Details;
            this.listViewIgnored.SelectedIndexChanged += new System.EventHandler(this.ListViewIgnoredSelectedIndexChanged);
            this.listViewIgnored.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListViewIgnoredMouseDoubleClick);
            this.listViewIgnored.MouseEnter += new System.EventHandler(this.ListViewIgnoredEnter);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Code";
            this.columnHeader1.Width = 30;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 160;
            // 
            // panelLoading
            // 
            this.panelLoading.Controls.Add(this.label1);
            this.panelLoading.Controls.Add(this.progressBarLoading);
            this.panelLoading.Location = new System.Drawing.Point(2, 91);
            this.panelLoading.Name = "panelLoading";
            this.panelLoading.Size = new System.Drawing.Size(540, 397);
            this.panelLoading.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(190, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Please wait. Importing...";
            // 
            // progressBarLoading
            // 
            this.progressBarLoading.Location = new System.Drawing.Point(151, 183);
            this.progressBarLoading.Name = "progressBarLoading";
            this.progressBarLoading.Size = new System.Drawing.Size(236, 23);
            this.progressBarLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarLoading.TabIndex = 0;
            // 
            // panelPreview
            // 
            this.panelPreview.Controls.Add(this.boxStreamDetails);
            this.panelPreview.Controls.Add(this.treePreview);
            this.panelPreview.Controls.Add(this.lblTitle3);
            this.panelPreview.Location = new System.Drawing.Point(2, 89);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new System.Drawing.Size(540, 397);
            this.panelPreview.TabIndex = 5;
            // 
            // boxStreamDetails
            // 
            this.boxStreamDetails.Controls.Add(this.timetableControl1);
            this.boxStreamDetails.Controls.Add(this.txtTreeDetails);
            this.boxStreamDetails.Location = new System.Drawing.Point(265, 50);
            this.boxStreamDetails.Name = "boxStreamDetails";
            this.boxStreamDetails.Size = new System.Drawing.Size(263, 335);
            this.boxStreamDetails.TabIndex = 3;
            this.boxStreamDetails.TabStop = false;
            this.boxStreamDetails.Text = "Details";
            // 
            // timetableControl1
            // 
            this.timetableControl1.EnableDrag = false;
            this.timetableControl1.Grayscale = false;
            this.timetableControl1.HourEnd = 21;
            this.timetableControl1.HourStart = 8;
            this.timetableControl1.Location = new System.Drawing.Point(22, 170);
            this.timetableControl1.Name = "timetableControl1";
            this.timetableControl1.ShowAll = true;
            this.timetableControl1.ShowDays = false;
            this.timetableControl1.ShowDragGhost = true;
            this.timetableControl1.ShowGrayArea = false;
            this.timetableControl1.ShowLocation = false;
            this.timetableControl1.ShowText = false;
            this.timetableControl1.ShowTimes = false;
            this.timetableControl1.ShowWeekend = true;
            this.timetableControl1.Size = new System.Drawing.Size(220, 155);
            this.timetableControl1.TabIndex = 3;
            this.timetableControl1.Timetable = null;
            // 
            // txtTreeDetails
            // 
            this.txtTreeDetails.AcceptsReturn = true;
            this.txtTreeDetails.AcceptsTab = true;
            this.txtTreeDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTreeDetails.Location = new System.Drawing.Point(13, 19);
            this.txtTreeDetails.Multiline = true;
            this.txtTreeDetails.Name = "txtTreeDetails";
            this.txtTreeDetails.ReadOnly = true;
            this.txtTreeDetails.Size = new System.Drawing.Size(238, 142);
            this.txtTreeDetails.TabIndex = 2;
            // 
            // treePreview
            // 
            this.treePreview.HideSelection = false;
            this.treePreview.Location = new System.Drawing.Point(12, 56);
            this.treePreview.Name = "treePreview";
            this.treePreview.Size = new System.Drawing.Size(241, 329);
            this.treePreview.TabIndex = 1;
            this.treePreview.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreePreviewAfterSelect);
            // 
            // lblTitle3
            // 
            this.lblTitle3.AutoSize = true;
            this.lblTitle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle3.Location = new System.Drawing.Point(8, 9);
            this.lblTitle3.Name = "lblTitle3";
            this.lblTitle3.Size = new System.Drawing.Size(202, 24);
            this.lblTitle3.TabIndex = 0;
            this.lblTitle3.Text = "Preview Stream Data";
            // 
            // FormImport
            // 
            this.AcceptButton = this.btnNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(544, 525);
            this.Controls.Add(this.panelStreams);
            this.Controls.Add(this.panelLoading);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelPreview);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImport";
            this.ShowInTaskbar = false;
            this.Text = "Import Wizard";
            this.Load += new System.EventHandler(this.FormImportLoad);
            this.Controls.SetChildIndex(this.panelPreview, 0);
            this.Controls.SetChildIndex(this.panelBottom, 0);
            this.Controls.SetChildIndex(this.panelLoading, 0);
            this.Controls.SetChildIndex(this.panelStreams, 0);
            this.panelBottom.ResumeLayout(false);
            this.panelStreams.ResumeLayout(false);
            this.panelStreams.PerformLayout();
            this.panelLoading.ResumeLayout(false);
            this.panelLoading.PerformLayout();
            this.panelPreview.ResumeLayout(false);
            this.panelPreview.PerformLayout();
            this.boxStreamDetails.ResumeLayout(false);
            this.boxStreamDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Panel panelStreams;
        private System.Windows.Forms.Label lblClashNotice;
        private System.Windows.Forms.Label lblIgnored;
        private System.Windows.Forms.Label lblTitle4;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.ListView listViewRequired;
        private System.Windows.Forms.ColumnHeader Code;
        private System.Windows.Forms.ColumnHeader TypeName;
        private System.Windows.Forms.Button btnRequire;
        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.ListView listViewIgnored;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel panelPreview;
        private System.Windows.Forms.GroupBox boxStreamDetails;
        private TimetableControl timetableControl1;
        private System.Windows.Forms.TextBox txtTreeDetails;
        private System.Windows.Forms.TreeView treePreview;
        private System.Windows.Forms.Label lblTitle3;
        private System.Windows.Forms.Panel panelLoading;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBarLoading;
        private System.Windows.Forms.CheckBox checkBoxS2;
        private System.Windows.Forms.CheckBox checkBoxS1;
        private System.Windows.Forms.CheckBox checkBoxTest;
    }
}
namespace UniTimetable.ViewControllers.Import
{
    partial class StreamsPanel
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
            this.checkBoxS2 = new System.Windows.Forms.CheckBox();
            this.checkBoxS1 = new System.Windows.Forms.CheckBox();
            this.checkBoxTest = new System.Windows.Forms.CheckBox();
            this.labelClashNotice = new System.Windows.Forms.Label();
            this.lblIgnored = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.lblRequired = new System.Windows.Forms.Label();
            this.listViewRequired = new System.Windows.Forms.ListView();
            this.Code = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TypeName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonRequire = new System.Windows.Forms.Button();
            this.buttonIgnore = new System.Windows.Forms.Button();
            this.listViewIgnored = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBoxOnceOffs = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxS2
            // 
            this.checkBoxS2.AutoSize = true;
            this.checkBoxS2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxS2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxS2.Location = new System.Drawing.Point(246, 157);
            this.checkBoxS2.Name = "checkBoxS2";
            this.checkBoxS2.Size = new System.Drawing.Size(36, 18);
            this.checkBoxS2.TabIndex = 26;
            this.checkBoxS2.Text = "S2";
            this.checkBoxS2.UseVisualStyleBackColor = true;
            this.checkBoxS2.CheckedChanged += new System.EventHandler(this.CheckBoxS2CheckedChanged);
            // 
            // checkBoxS1
            // 
            this.checkBoxS1.AutoSize = true;
            this.checkBoxS1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxS1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxS1.Location = new System.Drawing.Point(246, 134);
            this.checkBoxS1.Name = "checkBoxS1";
            this.checkBoxS1.Size = new System.Drawing.Size(36, 18);
            this.checkBoxS1.TabIndex = 25;
            this.checkBoxS1.Text = "S1";
            this.checkBoxS1.UseVisualStyleBackColor = true;
            this.checkBoxS1.CheckedChanged += new System.EventHandler(this.CheckBoxS1CheckedChanged);
            // 
            // checkBoxTest
            // 
            this.checkBoxTest.AutoSize = true;
            this.checkBoxTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxTest.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxTest.Location = new System.Drawing.Point(246, 180);
            this.checkBoxTest.Name = "checkBoxTest";
            this.checkBoxTest.Size = new System.Drawing.Size(49, 18);
            this.checkBoxTest.TabIndex = 24;
            this.checkBoxTest.Text = "Tests";
            this.checkBoxTest.UseVisualStyleBackColor = true;
            this.checkBoxTest.CheckedChanged += new System.EventHandler(this.CheckBoxTestCheckedChanged);
            // 
            // labelClashNotice
            // 
            this.labelClashNotice.BackColor = System.Drawing.Color.Red;
            this.labelClashNotice.Location = new System.Drawing.Point(310, 11);
            this.labelClashNotice.Name = "labelClashNotice";
            this.labelClashNotice.Size = new System.Drawing.Size(220, 23);
            this.labelClashNotice.TabIndex = 23;
            this.labelClashNotice.Text = "Red indicates an unavoidable clash";
            this.labelClashNotice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblIgnored
            // 
            this.lblIgnored.AutoSize = true;
            this.lblIgnored.Location = new System.Drawing.Point(11, 51);
            this.lblIgnored.Name = "lblIgnored";
            this.lblIgnored.Size = new System.Drawing.Size(43, 13);
            this.lblIgnored.TabIndex = 19;
            this.lblIgnored.Text = "Ignored";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(10, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(169, 22);
            this.labelTitle.TabIndex = 16;
            this.labelTitle.Text = "Streams to Include";
            // 
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.Location = new System.Drawing.Point(307, 51);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(50, 13);
            this.lblRequired.TabIndex = 17;
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
            this.listViewRequired.Location = new System.Drawing.Point(310, 70);
            this.listViewRequired.MultiSelect = false;
            this.listViewRequired.Name = "listViewRequired";
            this.listViewRequired.Size = new System.Drawing.Size(220, 316);
            this.listViewRequired.TabIndex = 18;
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
            // buttonRequire
            // 
            this.buttonRequire.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRequire.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRequire.Location = new System.Drawing.Point(240, 229);
            this.buttonRequire.Name = "buttonRequire";
            this.buttonRequire.Size = new System.Drawing.Size(64, 23);
            this.buttonRequire.TabIndex = 21;
            this.buttonRequire.Text = ">>>>";
            this.buttonRequire.UseVisualStyleBackColor = true;
            this.buttonRequire.Click += new System.EventHandler(this.ButtonRequireClick);
            // 
            // buttonIgnore
            // 
            this.buttonIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonIgnore.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonIgnore.Location = new System.Drawing.Point(240, 259);
            this.buttonIgnore.Name = "buttonIgnore";
            this.buttonIgnore.Size = new System.Drawing.Size(64, 23);
            this.buttonIgnore.TabIndex = 22;
            this.buttonIgnore.Text = "<<<<";
            this.buttonIgnore.UseVisualStyleBackColor = true;
            this.buttonIgnore.Click += new System.EventHandler(this.ButtonIgnoreClick);
            // 
            // listViewIgnored
            // 
            this.listViewIgnored.AllowColumnReorder = true;
            this.listViewIgnored.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewIgnored.FullRowSelect = true;
            this.listViewIgnored.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewIgnored.Location = new System.Drawing.Point(14, 70);
            this.listViewIgnored.MultiSelect = false;
            this.listViewIgnored.Name = "listViewIgnored";
            this.listViewIgnored.Size = new System.Drawing.Size(220, 316);
            this.listViewIgnored.TabIndex = 20;
            this.listViewIgnored.UseCompatibleStateImageBehavior = false;
            this.listViewIgnored.View = System.Windows.Forms.View.Details;
            this.listViewIgnored.SelectedIndexChanged += new System.EventHandler(this.ListViewIgnoredSelectedIndexChanged);
            this.listViewIgnored.Enter += new System.EventHandler(this.ListViewIgnoredEnter);
            this.listViewIgnored.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListViewIgnoredMouseDoubleClick);
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
            // checkBoxOnceOffs
            // 
            this.checkBoxOnceOffs.AutoSize = true;
            this.checkBoxOnceOffs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxOnceOffs.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxOnceOffs.Location = new System.Drawing.Point(246, 204);
            this.checkBoxOnceOffs.Name = "checkBoxOnceOffs";
            this.checkBoxOnceOffs.Size = new System.Drawing.Size(49, 18);
            this.checkBoxOnceOffs.TabIndex = 27;
            this.checkBoxOnceOffs.Text = "Once";
            this.checkBoxOnceOffs.UseVisualStyleBackColor = true;
            this.checkBoxOnceOffs.CheckedChanged += new System.EventHandler(this.CheckBoxOnceOffsCheckedChanged);
            // 
            // StreamsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxOnceOffs);
            this.Controls.Add(this.checkBoxS2);
            this.Controls.Add(this.checkBoxS1);
            this.Controls.Add(this.checkBoxTest);
            this.Controls.Add(this.labelClashNotice);
            this.Controls.Add(this.lblIgnored);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.lblRequired);
            this.Controls.Add(this.listViewRequired);
            this.Controls.Add(this.buttonRequire);
            this.Controls.Add(this.buttonIgnore);
            this.Controls.Add(this.listViewIgnored);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StreamsPanel";
            this.Size = new System.Drawing.Size(540, 396);
            this.Load += new System.EventHandler(this.StreamsPanelLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxS2;
        private System.Windows.Forms.CheckBox checkBoxS1;
        private System.Windows.Forms.CheckBox checkBoxTest;
        private System.Windows.Forms.Label labelClashNotice;
        private System.Windows.Forms.Label lblIgnored;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.ListView listViewRequired;
        private System.Windows.Forms.ColumnHeader Code;
        private System.Windows.Forms.ColumnHeader TypeName;
        private System.Windows.Forms.Button buttonRequire;
        private System.Windows.Forms.Button buttonIgnore;
        private System.Windows.Forms.ListView listViewIgnored;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.CheckBox checkBoxOnceOffs;

    }
}

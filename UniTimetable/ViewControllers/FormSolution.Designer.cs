namespace UniTimetable.ViewControllers
{
    partial class FormSolution
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSolution));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvbSolutions = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.timetableControl = new TimetableControl();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lvbProperties = new System.Windows.Forms.ListView();
            this.Property = new System.Windows.Forms.ColumnHeader();
            this.Value = new System.Windows.Forms.ColumnHeader();
            this.btnCriteria = new System.Windows.Forms.Button();
            this.btnUse = new System.Windows.Forms.Button();
            this.btnStar = new System.Windows.Forms.Button();
            this.chkOnlyStarred = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvbSolutions);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 404);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Solutions";
            // 
            // lvbSolutions
            // 
            this.lvbSolutions.FullRowSelect = true;
            this.lvbSolutions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvbSolutions.HideSelection = false;
            this.lvbSolutions.Location = new System.Drawing.Point(11, 19);
            this.lvbSolutions.MultiSelect = false;
            this.lvbSolutions.Name = "lvbSolutions";
            this.lvbSolutions.ShowGroups = false;
            this.lvbSolutions.Size = new System.Drawing.Size(754, 374);
            this.lvbSolutions.SmallImageList = this.imageList1;
            this.lvbSolutions.TabIndex = 0;
            this.lvbSolutions.UseCompatibleStateImageBehavior = false;
            this.lvbSolutions.View = System.Windows.Forms.View.Details;
            this.lvbSolutions.SelectedIndexChanged += new System.EventHandler(this.lvbSolutions_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Starred.gif");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.timetableControl);
            this.groupBox2.Location = new System.Drawing.Point(12, 422);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(231, 216);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Preview";
            // 
            // timetableControl
            // 
            this.timetableControl.EnableDrag = false;
            this.timetableControl.Grayscale = false;
            this.timetableControl.HourEnd = 21;
            this.timetableControl.HourStart = 8;
            this.timetableControl.Location = new System.Drawing.Point(12, 19);
            this.timetableControl.Name = "timetableControl";
            this.timetableControl.ShowAll = false;
            this.timetableControl.ShowDays = false;
            this.timetableControl.ShowDragGhost = true;
            this.timetableControl.ShowGrayArea = true;
            this.timetableControl.ShowLocation = false;
            this.timetableControl.ShowText = false;
            this.timetableControl.ShowTimes = false;
            this.timetableControl.ShowWeekend = true;
            this.timetableControl.Size = new System.Drawing.Size(207, 185);
            this.timetableControl.TabIndex = 1;
            this.timetableControl.Timetable = null;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::UniTimetable.Properties.Resources.Symbol_Delete;
            this.btnCancel.Location = new System.Drawing.Point(696, 547);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 91);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(249, 422);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(122, 216);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Streams";
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.AcceptsTab = true;
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(98, 185);
            this.textBox1.TabIndex = 0;
            this.textBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lvbProperties);
            this.groupBox4.Location = new System.Drawing.Point(377, 422);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(215, 216);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Properties";
            // 
            // lvbProperties
            // 
            this.lvbProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvbProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Property,
            this.Value});
            this.lvbProperties.FullRowSelect = true;
            this.lvbProperties.GridLines = true;
            this.lvbProperties.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvbProperties.HideSelection = false;
            this.lvbProperties.Location = new System.Drawing.Point(12, 19);
            this.lvbProperties.Name = "lvbProperties";
            this.lvbProperties.ShowGroups = false;
            this.lvbProperties.Size = new System.Drawing.Size(191, 185);
            this.lvbProperties.TabIndex = 0;
            this.lvbProperties.TabStop = false;
            this.lvbProperties.UseCompatibleStateImageBehavior = false;
            this.lvbProperties.View = System.Windows.Forms.View.Details;
            // 
            // Property
            // 
            this.Property.Text = "Property";
            this.Property.Width = 110;
            // 
            // Value
            // 
            this.Value.Text = "Value";
            this.Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnCriteria
            // 
            this.btnCriteria.Image = global::UniTimetable.Properties.Resources.Config_Tools;
            this.btnCriteria.Location = new System.Drawing.Point(696, 450);
            this.btnCriteria.Name = "btnCriteria";
            this.btnCriteria.Size = new System.Drawing.Size(92, 91);
            this.btnCriteria.TabIndex = 6;
            this.btnCriteria.Text = "Solution Criteria";
            this.btnCriteria.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCriteria.UseVisualStyleBackColor = true;
            this.btnCriteria.Click += new System.EventHandler(this.btnCriteria_Click);
            // 
            // btnUse
            // 
            this.btnUse.Image = global::UniTimetable.Properties.Resources.Symbol_Check;
            this.btnUse.Location = new System.Drawing.Point(598, 547);
            this.btnUse.Name = "btnUse";
            this.btnUse.Size = new System.Drawing.Size(92, 91);
            this.btnUse.TabIndex = 7;
            this.btnUse.Text = "Use Solution";
            this.btnUse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnUse.UseVisualStyleBackColor = true;
            this.btnUse.Click += new System.EventHandler(this.btnUse_Click);
            // 
            // btnStar
            // 
            this.btnStar.Image = global::UniTimetable.Properties.Resources.Favorites;
            this.btnStar.Location = new System.Drawing.Point(598, 450);
            this.btnStar.Name = "btnStar";
            this.btnStar.Size = new System.Drawing.Size(92, 91);
            this.btnStar.TabIndex = 5;
            this.btnStar.Text = "Star It!";
            this.btnStar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnStar.UseVisualStyleBackColor = true;
            this.btnStar.Click += new System.EventHandler(this.btnStar_Click);
            // 
            // chkOnlyStarred
            // 
            this.chkOnlyStarred.AutoSize = true;
            this.chkOnlyStarred.Location = new System.Drawing.Point(617, 427);
            this.chkOnlyStarred.Name = "chkOnlyStarred";
            this.chkOnlyStarred.Size = new System.Drawing.Size(154, 17);
            this.chkOnlyStarred.TabIndex = 4;
            this.chkOnlyStarred.Text = "Only show starred solutions";
            this.chkOnlyStarred.UseVisualStyleBackColor = true;
            this.chkOnlyStarred.CheckedChanged += new System.EventHandler(this.chkOnlyStarred_CheckedChanged);
            // 
            // FormSolution
            // 
            this.AcceptButton = this.btnUse;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(800, 650);
            this.Controls.Add(this.chkOnlyStarred);
            this.Controls.Add(this.btnUse);
            this.Controls.Add(this.btnStar);
            this.Controls.Add(this.btnCriteria);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSolution";
            this.ShowInTaskbar = false;
            this.Text = "Timetable Solver";
            this.Load += new System.EventHandler(this.FormSolution_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvbSolutions;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView lvbProperties;
        private System.Windows.Forms.ColumnHeader Property;
        private System.Windows.Forms.ColumnHeader Value;
        private System.Windows.Forms.Button btnCriteria;
        private System.Windows.Forms.Button btnUse;
        private System.Windows.Forms.Button btnStar;
        private System.Windows.Forms.CheckBox chkOnlyStarred;
        private TimetableControl timetableControl;
        private System.Windows.Forms.ImageList imageList1;
    }
}
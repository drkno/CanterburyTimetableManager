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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSolution));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonUse = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonCriteria = new System.Windows.Forms.Button();
            this.listViewSolutions = new System.Windows.Forms.ListView();
            this.checkBoxOnlyMarked = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonMark = new System.Windows.Forms.Button();
            this.listViewProperties = new System.Windows.Forms.ListView();
            this.Property = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timetableControl = new UniTimetable.ViewControllers.TimetableControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonUse);
            this.groupBox1.Controls.Add(this.buttonCancel);
            this.groupBox1.Controls.Add(this.buttonCriteria);
            this.groupBox1.Controls.Add(this.listViewSolutions);
            this.groupBox1.Controls.Add(this.checkBoxOnlyMarked);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 430);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Solutions";
            // 
            // buttonUse
            // 
            this.buttonUse.Location = new System.Drawing.Point(11, 399);
            this.buttonUse.Name = "buttonUse";
            this.buttonUse.Size = new System.Drawing.Size(157, 23);
            this.buttonUse.TabIndex = 9;
            this.buttonUse.Text = "&Use Solution";
            this.buttonUse.UseVisualStyleBackColor = true;
            this.buttonUse.Click += new System.EventHandler(this.ButtonUseClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(608, 399);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(157, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonCriteria
            // 
            this.buttonCriteria.Location = new System.Drawing.Point(445, 399);
            this.buttonCriteria.Name = "buttonCriteria";
            this.buttonCriteria.Size = new System.Drawing.Size(157, 23);
            this.buttonCriteria.TabIndex = 12;
            this.buttonCriteria.Text = "&Solution Criteria";
            this.buttonCriteria.UseVisualStyleBackColor = true;
            this.buttonCriteria.Click += new System.EventHandler(this.ButtonCriteriaClick);
            // 
            // listViewSolutions
            // 
            this.listViewSolutions.FullRowSelect = true;
            this.listViewSolutions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewSolutions.HideSelection = false;
            this.listViewSolutions.Location = new System.Drawing.Point(11, 19);
            this.listViewSolutions.MultiSelect = false;
            this.listViewSolutions.Name = "listViewSolutions";
            this.listViewSolutions.ShowGroups = false;
            this.listViewSolutions.Size = new System.Drawing.Size(754, 374);
            this.listViewSolutions.TabIndex = 0;
            this.listViewSolutions.UseCompatibleStateImageBehavior = false;
            this.listViewSolutions.View = System.Windows.Forms.View.Details;
            this.listViewSolutions.SelectedIndexChanged += new System.EventHandler(this.ListViewSolutionsSelectedIndexChanged);
            // 
            // checkBoxOnlyMarked
            // 
            this.checkBoxOnlyMarked.AutoSize = true;
            this.checkBoxOnlyMarked.Location = new System.Drawing.Point(184, 403);
            this.checkBoxOnlyMarked.Name = "checkBoxOnlyMarked";
            this.checkBoxOnlyMarked.Size = new System.Drawing.Size(157, 17);
            this.checkBoxOnlyMarked.TabIndex = 4;
            this.checkBoxOnlyMarked.Text = "Only show marked solutions";
            this.checkBoxOnlyMarked.UseVisualStyleBackColor = true;
            this.checkBoxOnlyMarked.CheckedChanged += new System.EventHandler(this.CheckboxOnlyMarkedCheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonMark);
            this.groupBox2.Controls.Add(this.listViewProperties);
            this.groupBox2.Controls.Add(this.timetableControl);
            this.groupBox2.Location = new System.Drawing.Point(794, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(231, 430);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Solution Information";
            // 
            // buttonMark
            // 
            this.buttonMark.Location = new System.Drawing.Point(40, 399);
            this.buttonMark.Name = "buttonMark";
            this.buttonMark.Size = new System.Drawing.Size(157, 23);
            this.buttonMark.TabIndex = 11;
            this.buttonMark.Text = "&Mark Solution";
            this.buttonMark.UseVisualStyleBackColor = true;
            this.buttonMark.Click += new System.EventHandler(this.ButtonMarkClick);
            // 
            // listViewProperties
            // 
            this.listViewProperties.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listViewProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Property,
            this.Value});
            this.listViewProperties.FullRowSelect = true;
            this.listViewProperties.GridLines = true;
            this.listViewProperties.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewProperties.HideSelection = false;
            this.listViewProperties.Location = new System.Drawing.Point(12, 197);
            this.listViewProperties.Name = "listViewProperties";
            this.listViewProperties.ShowGroups = false;
            this.listViewProperties.Size = new System.Drawing.Size(207, 196);
            this.listViewProperties.TabIndex = 0;
            this.listViewProperties.TabStop = false;
            this.listViewProperties.UseCompatibleStateImageBehavior = false;
            this.listViewProperties.View = System.Windows.Forms.View.Details;
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
            this.Value.Width = 70;
            // 
            // timetableControl
            // 
            this.timetableControl.EnableDrag = false;
            this.timetableControl.Font = new System.Drawing.Font("Arial Narrow", 0.0001196839F);
            this.timetableControl.Grayscale = false;
            this.timetableControl.HourEnd = 21;
            this.timetableControl.HourStart = 8;
            this.timetableControl.Location = new System.Drawing.Point(12, 19);
            this.timetableControl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.timetableControl.Name = "timetableControl";
            this.timetableControl.OutlineColour = System.Drawing.Color.LightGray;
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
            this.timetableControl.TimeslotUnavalibleColour = System.Drawing.Color.LightGray;
            this.timetableControl.Timetable = null;
            // 
            // FormSolution
            // 
            this.AcceptButton = this.buttonUse;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(1034, 447);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSolution";
            this.ShowInTaskbar = false;
            this.Text = "Timetable Solver";
            this.Load += new System.EventHandler(this.FormSolutionLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listViewSolutions;
        private System.Windows.Forms.ListView listViewProperties;
        private System.Windows.Forms.ColumnHeader Property;
        private System.Windows.Forms.ColumnHeader Value;
        private System.Windows.Forms.CheckBox checkBoxOnlyMarked;
        private TimetableControl timetableControl;
        private System.Windows.Forms.Button buttonUse;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonMark;
        private System.Windows.Forms.Button buttonCriteria;
    }
}
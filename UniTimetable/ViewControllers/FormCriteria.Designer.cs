namespace UniTimetable
{
    partial class FormCriteria
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCriteria));
            this.listBoxCriteria = new System.Windows.Forms.ListBox();
            this.listBoxFilters = new System.Windows.Forms.ListBox();
            this.btnCriteriaAdd = new System.Windows.Forms.Button();
            this.btnCriteriaEdit = new System.Windows.Forms.Button();
            this.btnCriteriaRemove = new System.Windows.Forms.Button();
            this.btnFiltersAdd = new System.Windows.Forms.Button();
            this.btnFiltersEdit = new System.Windows.Forms.Button();
            this.btnFiltersRemove = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRevert = new System.Windows.Forms.Button();
            this.lblCriteria = new System.Windows.Forms.Label();
            this.lblFilters = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ddPresets = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.etchedLine = new UniTimetable.EtchedLine();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxCriteria
            // 
            this.listBoxCriteria.AllowDrop = true;
            this.listBoxCriteria.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxCriteria.FormattingEnabled = true;
            this.listBoxCriteria.IntegralHeight = false;
            this.listBoxCriteria.ItemHeight = 50;
            this.listBoxCriteria.Location = new System.Drawing.Point(12, 30);
            this.listBoxCriteria.Name = "listBoxCriteria";
            this.listBoxCriteria.Size = new System.Drawing.Size(250, 300);
            this.listBoxCriteria.TabIndex = 0;
            this.listBoxCriteria.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxCriteria_MouseDoubleClick);
            this.listBoxCriteria.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxCriteria_DrawItem);
            this.listBoxCriteria.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxCriteria_DragDrop);
            this.listBoxCriteria.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxCriteria_MouseDown);
            this.listBoxCriteria.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBoxCriteria_DragEnter);
            this.listBoxCriteria.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxCriteria_KeyDown);
            // 
            // listBoxFilters
            // 
            this.listBoxFilters.AllowDrop = true;
            this.listBoxFilters.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxFilters.FormattingEnabled = true;
            this.listBoxFilters.IntegralHeight = false;
            this.listBoxFilters.ItemHeight = 50;
            this.listBoxFilters.Location = new System.Drawing.Point(274, 30);
            this.listBoxFilters.Name = "listBoxFilters";
            this.listBoxFilters.Size = new System.Drawing.Size(250, 300);
            this.listBoxFilters.TabIndex = 1;
            this.listBoxFilters.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxFilters_MouseDoubleClick);
            this.listBoxFilters.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxFilters_DrawItem);
            this.listBoxFilters.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxFilters_MouseDown);
            this.listBoxFilters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxFilters_KeyDown);
            // 
            // btnCriteriaAdd
            // 
            this.btnCriteriaAdd.Location = new System.Drawing.Point(12, 336);
            this.btnCriteriaAdd.Name = "btnCriteriaAdd";
            this.btnCriteriaAdd.Size = new System.Drawing.Size(79, 23);
            this.btnCriteriaAdd.TabIndex = 3;
            this.btnCriteriaAdd.Text = "Add";
            this.btnCriteriaAdd.UseVisualStyleBackColor = true;
            this.btnCriteriaAdd.Click += new System.EventHandler(this.btnCriteriaAdd_Click);
            // 
            // btnCriteriaEdit
            // 
            this.btnCriteriaEdit.Location = new System.Drawing.Point(97, 336);
            this.btnCriteriaEdit.Name = "btnCriteriaEdit";
            this.btnCriteriaEdit.Size = new System.Drawing.Size(80, 23);
            this.btnCriteriaEdit.TabIndex = 4;
            this.btnCriteriaEdit.Text = "Edit";
            this.btnCriteriaEdit.UseVisualStyleBackColor = true;
            this.btnCriteriaEdit.Click += new System.EventHandler(this.btnCriteriaEdit_Click);
            // 
            // btnCriteriaRemove
            // 
            this.btnCriteriaRemove.Location = new System.Drawing.Point(183, 336);
            this.btnCriteriaRemove.Name = "btnCriteriaRemove";
            this.btnCriteriaRemove.Size = new System.Drawing.Size(79, 23);
            this.btnCriteriaRemove.TabIndex = 5;
            this.btnCriteriaRemove.Text = "Remove";
            this.btnCriteriaRemove.UseVisualStyleBackColor = true;
            this.btnCriteriaRemove.Click += new System.EventHandler(this.btnCriteriaRemove_Click);
            // 
            // btnFiltersAdd
            // 
            this.btnFiltersAdd.Location = new System.Drawing.Point(274, 336);
            this.btnFiltersAdd.Name = "btnFiltersAdd";
            this.btnFiltersAdd.Size = new System.Drawing.Size(79, 23);
            this.btnFiltersAdd.TabIndex = 6;
            this.btnFiltersAdd.Text = "Add";
            this.btnFiltersAdd.UseVisualStyleBackColor = true;
            this.btnFiltersAdd.Click += new System.EventHandler(this.btnFiltersAdd_Click);
            // 
            // btnFiltersEdit
            // 
            this.btnFiltersEdit.Location = new System.Drawing.Point(359, 336);
            this.btnFiltersEdit.Name = "btnFiltersEdit";
            this.btnFiltersEdit.Size = new System.Drawing.Size(80, 23);
            this.btnFiltersEdit.TabIndex = 7;
            this.btnFiltersEdit.Text = "Edit";
            this.btnFiltersEdit.UseVisualStyleBackColor = true;
            this.btnFiltersEdit.Click += new System.EventHandler(this.btnFiltersEdit_Click);
            // 
            // btnFiltersRemove
            // 
            this.btnFiltersRemove.Location = new System.Drawing.Point(445, 336);
            this.btnFiltersRemove.Name = "btnFiltersRemove";
            this.btnFiltersRemove.Size = new System.Drawing.Size(79, 23);
            this.btnFiltersRemove.TabIndex = 8;
            this.btnFiltersRemove.Text = "Remove";
            this.btnFiltersRemove.UseVisualStyleBackColor = true;
            this.btnFiltersRemove.Click += new System.EventHandler(this.btnFiltersRemove_Click);
            // 
            // btnOK
            // 
            this.btnOK.Image = global::UniTimetable.Properties.Resources.Symbol_Check;
            this.btnOK.Location = new System.Drawing.Point(332, 383);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 90);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::UniTimetable.Properties.Resources.Symbol_Delete;
            this.btnCancel.Location = new System.Drawing.Point(434, 383);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 90);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRevert
            // 
            this.btnRevert.Image = global::UniTimetable.Properties.Resources.Undo;
            this.btnRevert.Location = new System.Drawing.Point(12, 383);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(90, 90);
            this.btnRevert.TabIndex = 9;
            this.btnRevert.Text = "Revert";
            this.btnRevert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // lblCriteria
            // 
            this.lblCriteria.AutoSize = true;
            this.lblCriteria.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCriteria.Location = new System.Drawing.Point(7, 9);
            this.lblCriteria.Name = "lblCriteria";
            this.lblCriteria.Size = new System.Drawing.Size(90, 25);
            this.lblCriteria.TabIndex = 2;
            this.lblCriteria.Text = "Criteria";
            // 
            // lblFilters
            // 
            this.lblFilters.AutoSize = true;
            this.lblFilters.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilters.Location = new System.Drawing.Point(269, 9);
            this.lblFilters.Name = "lblFilters";
            this.lblFilters.Size = new System.Drawing.Size(77, 25);
            this.lblFilters.TabIndex = 2;
            this.lblFilters.Text = "Filters";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(119, 411);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 34);
            this.label1.TabIndex = 11;
            this.label1.Text = "Load Preset Configuration";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ddPresets
            // 
            this.ddPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddPresets.Location = new System.Drawing.Point(114, 451);
            this.ddPresets.Name = "ddPresets";
            this.ddPresets.Size = new System.Drawing.Size(153, 21);
            this.ddPresets.TabIndex = 12;
            this.ddPresets.SelectedIndexChanged += new System.EventHandler(this.ddPresets_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::UniTimetable.Properties.Resources.My_Documents;
            this.pictureBox1.Location = new System.Drawing.Point(198, 381);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // etchedLine
            // 
            this.etchedLine.Location = new System.Drawing.Point(2, 371);
            this.etchedLine.Name = "etchedLine";
            this.etchedLine.Size = new System.Drawing.Size(534, 10);
            this.etchedLine.TabIndex = 10;
            // 
            // FormCriteria
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(536, 484);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ddPresets);
            this.Controls.Add(this.etchedLine);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRevert);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnFiltersRemove);
            this.Controls.Add(this.btnFiltersEdit);
            this.Controls.Add(this.btnFiltersAdd);
            this.Controls.Add(this.btnCriteriaRemove);
            this.Controls.Add(this.btnCriteriaEdit);
            this.Controls.Add(this.btnCriteriaAdd);
            this.Controls.Add(this.listBoxCriteria);
            this.Controls.Add(this.listBoxFilters);
            this.Controls.Add(this.lblFilters);
            this.Controls.Add(this.lblCriteria);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCriteria";
            this.ShowInTaskbar = false;
            this.Text = "Solution Criteria";
            this.Load += new System.EventHandler(this.FormCriteria_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxCriteria;
        private System.Windows.Forms.ListBox listBoxFilters;
        private System.Windows.Forms.Button btnCriteriaAdd;
        private System.Windows.Forms.Button btnCriteriaEdit;
        private System.Windows.Forms.Button btnCriteriaRemove;
        private System.Windows.Forms.Button btnFiltersAdd;
        private System.Windows.Forms.Button btnFiltersEdit;
        private System.Windows.Forms.Button btnFiltersRemove;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRevert;
        private EtchedLine etchedLine;
        private System.Windows.Forms.Label lblCriteria;
        private System.Windows.Forms.Label lblFilters;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddPresets;
        private System.Windows.Forms.PictureBox pictureBox1;

    }
}
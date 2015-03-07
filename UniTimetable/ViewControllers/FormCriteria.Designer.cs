namespace UniTimetable.ViewControllers
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
            this.buttonCriteriaAdd = new System.Windows.Forms.Button();
            this.buttonCriteriaEdit = new System.Windows.Forms.Button();
            this.buttonCriteriaRemove = new System.Windows.Forms.Button();
            this.buttonFiltersAdd = new System.Windows.Forms.Button();
            this.buttonFiltersEdit = new System.Windows.Forms.Button();
            this.buttonFiltersRemove = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.buttonRevert = new System.Windows.Forms.Button();
            this.labelPreset = new System.Windows.Forms.Label();
            this.comboBoxPresets = new System.Windows.Forms.ComboBox();
            this.groupBoxFilters = new System.Windows.Forms.GroupBox();
            this.groupBoxCriteria = new System.Windows.Forms.GroupBox();
            this.groupBoxFilters.SuspendLayout();
            this.groupBoxCriteria.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxCriteria
            // 
            this.listBoxCriteria.AllowDrop = true;
            this.listBoxCriteria.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxCriteria.FormattingEnabled = true;
            this.listBoxCriteria.IntegralHeight = false;
            this.listBoxCriteria.ItemHeight = 50;
            this.listBoxCriteria.Location = new System.Drawing.Point(15, 19);
            this.listBoxCriteria.Name = "listBoxCriteria";
            this.listBoxCriteria.Size = new System.Drawing.Size(250, 300);
            this.listBoxCriteria.TabIndex = 0;
            this.listBoxCriteria.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBoxCriteriaDrawItem);
            this.listBoxCriteria.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListBoxCriteriaDragDrop);
            this.listBoxCriteria.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListBoxCriteriaDragEnter);
            this.listBoxCriteria.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListBoxCriteriaKeyDown);
            this.listBoxCriteria.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBoxCriteriaMouseDoubleClick);
            this.listBoxCriteria.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBoxCriteriaMouseDown);
            // 
            // listBoxFilters
            // 
            this.listBoxFilters.AllowDrop = true;
            this.listBoxFilters.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxFilters.FormattingEnabled = true;
            this.listBoxFilters.IntegralHeight = false;
            this.listBoxFilters.ItemHeight = 50;
            this.listBoxFilters.Location = new System.Drawing.Point(15, 19);
            this.listBoxFilters.Name = "listBoxFilters";
            this.listBoxFilters.Size = new System.Drawing.Size(250, 300);
            this.listBoxFilters.TabIndex = 1;
            this.listBoxFilters.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBoxFiltersDrawItem);
            this.listBoxFilters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListBoxFiltersKeyDown);
            this.listBoxFilters.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBoxFiltersMouseDoubleClick);
            this.listBoxFilters.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBoxFiltersMouseDown);
            // 
            // buttonCriteriaAdd
            // 
            this.buttonCriteriaAdd.Location = new System.Drawing.Point(15, 325);
            this.buttonCriteriaAdd.Name = "buttonCriteriaAdd";
            this.buttonCriteriaAdd.Size = new System.Drawing.Size(79, 23);
            this.buttonCriteriaAdd.TabIndex = 3;
            this.buttonCriteriaAdd.Text = "Add";
            this.buttonCriteriaAdd.UseVisualStyleBackColor = true;
            this.buttonCriteriaAdd.Click += new System.EventHandler(this.ButtonCriteriaAddClick);
            // 
            // buttonCriteriaEdit
            // 
            this.buttonCriteriaEdit.Location = new System.Drawing.Point(100, 325);
            this.buttonCriteriaEdit.Name = "buttonCriteriaEdit";
            this.buttonCriteriaEdit.Size = new System.Drawing.Size(80, 23);
            this.buttonCriteriaEdit.TabIndex = 4;
            this.buttonCriteriaEdit.Text = "Edit";
            this.buttonCriteriaEdit.UseVisualStyleBackColor = true;
            this.buttonCriteriaEdit.Click += new System.EventHandler(this.ButtonCriteriaEditClick);
            // 
            // buttonCriteriaRemove
            // 
            this.buttonCriteriaRemove.Location = new System.Drawing.Point(186, 325);
            this.buttonCriteriaRemove.Name = "buttonCriteriaRemove";
            this.buttonCriteriaRemove.Size = new System.Drawing.Size(79, 23);
            this.buttonCriteriaRemove.TabIndex = 5;
            this.buttonCriteriaRemove.Text = "Remove";
            this.buttonCriteriaRemove.UseVisualStyleBackColor = true;
            this.buttonCriteriaRemove.Click += new System.EventHandler(this.ButtonCriteriaRemoveClick);
            // 
            // buttonFiltersAdd
            // 
            this.buttonFiltersAdd.Location = new System.Drawing.Point(15, 325);
            this.buttonFiltersAdd.Name = "buttonFiltersAdd";
            this.buttonFiltersAdd.Size = new System.Drawing.Size(79, 23);
            this.buttonFiltersAdd.TabIndex = 6;
            this.buttonFiltersAdd.Text = "Add";
            this.buttonFiltersAdd.UseVisualStyleBackColor = true;
            this.buttonFiltersAdd.Click += new System.EventHandler(this.ButtonFiltersAddClick);
            // 
            // buttonFiltersEdit
            // 
            this.buttonFiltersEdit.Location = new System.Drawing.Point(100, 325);
            this.buttonFiltersEdit.Name = "buttonFiltersEdit";
            this.buttonFiltersEdit.Size = new System.Drawing.Size(80, 23);
            this.buttonFiltersEdit.TabIndex = 7;
            this.buttonFiltersEdit.Text = "Edit";
            this.buttonFiltersEdit.UseVisualStyleBackColor = true;
            this.buttonFiltersEdit.Click += new System.EventHandler(this.ButtonFiltersEditClick);
            // 
            // buttonFiltersRemove
            // 
            this.buttonFiltersRemove.Location = new System.Drawing.Point(186, 325);
            this.buttonFiltersRemove.Name = "buttonFiltersRemove";
            this.buttonFiltersRemove.Size = new System.Drawing.Size(79, 23);
            this.buttonFiltersRemove.TabIndex = 8;
            this.buttonFiltersRemove.Text = "Remove";
            this.buttonFiltersRemove.UseVisualStyleBackColor = true;
            this.buttonFiltersRemove.Click += new System.EventHandler(this.ButtonFiltersRemoveClick);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(414, 376);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(79, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(499, 376);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // buttonRevert
            // 
            this.buttonRevert.Location = new System.Drawing.Point(244, 376);
            this.buttonRevert.Name = "buttonRevert";
            this.buttonRevert.Size = new System.Drawing.Size(164, 23);
            this.buttonRevert.TabIndex = 9;
            this.buttonRevert.Text = "Revert Changes";
            this.buttonRevert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonRevert.UseVisualStyleBackColor = true;
            this.buttonRevert.Click += new System.EventHandler(this.ButtonRevertClick);
            // 
            // labelPreset
            // 
            this.labelPreset.Location = new System.Drawing.Point(10, 378);
            this.labelPreset.Name = "labelPreset";
            this.labelPreset.Size = new System.Drawing.Size(45, 16);
            this.labelPreset.TabIndex = 11;
            this.labelPreset.Text = "Preset:";
            this.labelPreset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxPresets
            // 
            this.comboBoxPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPresets.Location = new System.Drawing.Point(61, 377);
            this.comboBoxPresets.Name = "comboBoxPresets";
            this.comboBoxPresets.Size = new System.Drawing.Size(153, 21);
            this.comboBoxPresets.TabIndex = 12;
            this.comboBoxPresets.SelectedIndexChanged += new System.EventHandler(this.ComboBoxPresetsSelectedIndexChanged);
            // 
            // groupBoxFilters
            // 
            this.groupBoxFilters.Controls.Add(this.listBoxFilters);
            this.groupBoxFilters.Controls.Add(this.buttonFiltersAdd);
            this.groupBoxFilters.Controls.Add(this.buttonFiltersEdit);
            this.groupBoxFilters.Controls.Add(this.buttonFiltersRemove);
            this.groupBoxFilters.Location = new System.Drawing.Point(298, 12);
            this.groupBoxFilters.Name = "groupBoxFilters";
            this.groupBoxFilters.Size = new System.Drawing.Size(280, 358);
            this.groupBoxFilters.TabIndex = 14;
            this.groupBoxFilters.TabStop = false;
            this.groupBoxFilters.Text = "Filters";
            // 
            // groupBoxCriteria
            // 
            this.groupBoxCriteria.Controls.Add(this.listBoxCriteria);
            this.groupBoxCriteria.Controls.Add(this.buttonCriteriaRemove);
            this.groupBoxCriteria.Controls.Add(this.buttonCriteriaEdit);
            this.groupBoxCriteria.Controls.Add(this.buttonCriteriaAdd);
            this.groupBoxCriteria.Location = new System.Drawing.Point(12, 12);
            this.groupBoxCriteria.Name = "groupBoxCriteria";
            this.groupBoxCriteria.Size = new System.Drawing.Size(280, 358);
            this.groupBoxCriteria.TabIndex = 15;
            this.groupBoxCriteria.TabStop = false;
            this.groupBoxCriteria.Text = "Criteria";
            // 
            // FormCriteria
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(589, 403);
            this.Controls.Add(this.groupBoxCriteria);
            this.Controls.Add(this.groupBoxFilters);
            this.Controls.Add(this.labelPreset);
            this.Controls.Add(this.comboBoxPresets);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.buttonRevert);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCriteria";
            this.ShowInTaskbar = false;
            this.Text = "Solution Criteria";
            this.Load += new System.EventHandler(this.FormCriteriaLoad);
            this.groupBoxFilters.ResumeLayout(false);
            this.groupBoxCriteria.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxCriteria;
        private System.Windows.Forms.ListBox listBoxFilters;
        private System.Windows.Forms.Button buttonCriteriaAdd;
        private System.Windows.Forms.Button buttonCriteriaEdit;
        private System.Windows.Forms.Button buttonCriteriaRemove;
        private System.Windows.Forms.Button buttonFiltersAdd;
        private System.Windows.Forms.Button buttonFiltersEdit;
        private System.Windows.Forms.Button buttonFiltersRemove;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button buttonRevert;
        private System.Windows.Forms.Label labelPreset;
        private System.Windows.Forms.ComboBox comboBoxPresets;
        private System.Windows.Forms.GroupBox groupBoxFilters;
        private System.Windows.Forms.GroupBox groupBoxCriteria;

    }
}
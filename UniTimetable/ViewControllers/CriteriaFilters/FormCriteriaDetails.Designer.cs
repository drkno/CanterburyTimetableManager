namespace UniTimetable.ViewControllers.CriteriaFilters
{
    partial class FormCriteriaDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCriteriaDetails));
            this.ddField = new System.Windows.Forms.ComboBox();
            this.lblPref = new System.Windows.Forms.Label();
            this.ddPreference = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRevert = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.etchedLine1 = new EtchedLine();
            this.SuspendLayout();
            // 
            // ddField
            // 
            this.ddField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddField.FormattingEnabled = true;
            this.ddField.Location = new System.Drawing.Point(12, 12);
            this.ddField.Name = "ddField";
            this.ddField.Size = new System.Drawing.Size(120, 21);
            this.ddField.TabIndex = 1;
            this.ddField.SelectedIndexChanged += new System.EventHandler(this.ddField_SelectedIndexChanged);
            // 
            // lblPref
            // 
            this.lblPref.AutoSize = true;
            this.lblPref.Location = new System.Drawing.Point(150, 15);
            this.lblPref.Name = "lblPref";
            this.lblPref.Size = new System.Drawing.Size(62, 13);
            this.lblPref.TabIndex = 2;
            this.lblPref.Text = "Preference:";
            // 
            // ddPreference
            // 
            this.ddPreference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddPreference.FormattingEnabled = true;
            this.ddPreference.Location = new System.Drawing.Point(218, 12);
            this.ddPreference.Name = "ddPreference";
            this.ddPreference.Size = new System.Drawing.Size(100, 21);
            this.ddPreference.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(238, 57);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRevert
            // 
            this.btnRevert.Location = new System.Drawing.Point(12, 57);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(80, 23);
            this.btnRevert.TabIndex = 11;
            this.btnRevert.Text = "Revert";
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(146, 57);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // etchedLine1
            // 
            this.etchedLine1.Location = new System.Drawing.Point(2, 45);
            this.etchedLine1.Name = "etchedLine1";
            this.etchedLine1.Size = new System.Drawing.Size(326, 10);
            this.etchedLine1.TabIndex = 10;
            // 
            // FormCriteriaDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(330, 92);
            this.Controls.Add(this.etchedLine1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRevert);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.ddPreference);
            this.Controls.Add(this.lblPref);
            this.Controls.Add(this.ddField);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCriteriaDetails";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Criteria Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ddField;
        private System.Windows.Forms.Label lblPref;
        private System.Windows.Forms.ComboBox ddPreference;
        private EtchedLine etchedLine1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.Button btnOK;
    }
}
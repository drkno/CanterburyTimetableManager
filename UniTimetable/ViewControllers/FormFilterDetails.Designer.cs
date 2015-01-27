namespace UniTimetable.ViewControllers
{
    partial class FormFilterDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFilterDetails));
            this.ddTest = new System.Windows.Forms.ComboBox();
            this.ddExclude = new System.Windows.Forms.ComboBox();
            this.ddField = new System.Windows.Forms.ComboBox();
            this.txtInteger = new System.Windows.Forms.TextBox();
            this.timeOfDayPicker = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtTimeLength = new System.Windows.Forms.MaskedTextBox();
            this.btnRevert = new System.Windows.Forms.Button();
            this.etchedLine1 = new EtchedLine();
            this.SuspendLayout();
            // 
            // ddTest
            // 
            this.ddTest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddTest.FormattingEnabled = true;
            this.ddTest.Location = new System.Drawing.Point(248, 12);
            this.ddTest.Name = "ddTest";
            this.ddTest.Size = new System.Drawing.Size(100, 21);
            this.ddTest.TabIndex = 2;
            // 
            // ddExclude
            // 
            this.ddExclude.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddExclude.FormattingEnabled = true;
            this.ddExclude.Items.AddRange(new object[] {
            "must be",
            "must not be"});
            this.ddExclude.Location = new System.Drawing.Point(144, 12);
            this.ddExclude.Name = "ddExclude";
            this.ddExclude.Size = new System.Drawing.Size(92, 21);
            this.ddExclude.TabIndex = 1;
            // 
            // ddField
            // 
            this.ddField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddField.FormattingEnabled = true;
            this.ddField.Location = new System.Drawing.Point(12, 12);
            this.ddField.Name = "ddField";
            this.ddField.Size = new System.Drawing.Size(120, 21);
            this.ddField.TabIndex = 0;
            this.ddField.SelectedIndexChanged += new System.EventHandler(this.ddField_SelectedIndexChanged);
            // 
            // txtInteger
            // 
            this.txtInteger.Location = new System.Drawing.Point(360, 12);
            this.txtInteger.Name = "txtInteger";
            this.txtInteger.Size = new System.Drawing.Size(80, 20);
            this.txtInteger.TabIndex = 5;
            this.txtInteger.Text = "0";
            this.txtInteger.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // timeOfDayPicker
            // 
            this.timeOfDayPicker.CustomFormat = "h:mm tt";
            this.timeOfDayPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timeOfDayPicker.Location = new System.Drawing.Point(365, 12);
            this.timeOfDayPicker.Name = "timeOfDayPicker";
            this.timeOfDayPicker.ShowUpDown = true;
            this.timeOfDayPicker.Size = new System.Drawing.Size(75, 20);
            this.timeOfDayPicker.TabIndex = 4;
            this.timeOfDayPicker.Value = new System.DateTime(1989, 11, 3, 12, 0, 0, 0);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(360, 57);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(268, 57);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtTimeLength
            // 
            this.txtTimeLength.AsciiOnly = true;
            this.txtTimeLength.Location = new System.Drawing.Point(360, 12);
            this.txtTimeLength.Mask = "00 hr 00 min";
            this.txtTimeLength.Name = "txtTimeLength";
            this.txtTimeLength.PromptChar = '0';
            this.txtTimeLength.Size = new System.Drawing.Size(80, 20);
            this.txtTimeLength.TabIndex = 3;
            this.txtTimeLength.Text = " 1";
            this.txtTimeLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnRevert
            // 
            this.btnRevert.Location = new System.Drawing.Point(12, 57);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(80, 23);
            this.btnRevert.TabIndex = 7;
            this.btnRevert.Text = "Revert";
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // etchedLine1
            // 
            this.etchedLine1.Location = new System.Drawing.Point(2, 45);
            this.etchedLine1.Name = "etchedLine1";
            this.etchedLine1.Size = new System.Drawing.Size(448, 10);
            this.etchedLine1.TabIndex = 6;
            // 
            // FormFilterDetails
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(452, 92);
            this.Controls.Add(this.etchedLine1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.ddTest);
            this.Controls.Add(this.btnRevert);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.ddExclude);
            this.Controls.Add(this.ddField);
            this.Controls.Add(this.txtInteger);
            this.Controls.Add(this.txtTimeLength);
            this.Controls.Add(this.timeOfDayPicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFilterDetails";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ddField;
        private System.Windows.Forms.ComboBox ddExclude;
        private System.Windows.Forms.ComboBox ddTest;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtInteger;
        public System.Windows.Forms.DateTimePicker timeOfDayPicker;
        private System.Windows.Forms.MaskedTextBox txtTimeLength;
        private System.Windows.Forms.Button btnRevert;
        private EtchedLine etchedLine1;
    }
}
namespace UniTimetable.ViewControllers
{
    partial class FormLogin
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
            this.fieldsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.inputFieldsTable = new System.Windows.Forms.TableLayoutPanel();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelPrivicy = new System.Windows.Forms.Label();
            this.inputFieldsTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Size = new System.Drawing.Size(279, 63);
            this.labelTitle.Text = "Import Timetable";
            // 
            // fieldsLayoutPanel
            // 
            this.fieldsLayoutPanel.AutoSize = true;
            this.fieldsLayoutPanel.ColumnCount = 2;
            this.inputFieldsTable.SetColumnSpan(this.fieldsLayoutPanel, 2);
            this.fieldsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.23729F));
            this.fieldsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.76271F));
            this.fieldsLayoutPanel.Font = new System.Drawing.Font("Arial", 8.25F);
            this.fieldsLayoutPanel.Location = new System.Drawing.Point(31, 5);
            this.fieldsLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.fieldsLayoutPanel.Name = "fieldsLayoutPanel";
            this.fieldsLayoutPanel.RowCount = 1;
            this.inputFieldsTable.SetRowSpan(this.fieldsLayoutPanel, 3);
            this.fieldsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.fieldsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.fieldsLayoutPanel.Size = new System.Drawing.Size(0, 0);
            this.fieldsLayoutPanel.TabIndex = 10;
            // 
            // inputFieldsTable
            // 
            this.inputFieldsTable.AutoSize = true;
            this.inputFieldsTable.ColumnCount = 4;
            this.inputFieldsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.inputFieldsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.inputFieldsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 169F));
            this.inputFieldsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.inputFieldsTable.Controls.Add(this.buttonImport, 3, 0);
            this.inputFieldsTable.Controls.Add(this.fieldsLayoutPanel, 1, 0);
            this.inputFieldsTable.Controls.Add(this.buttonCancel, 3, 1);
            this.inputFieldsTable.Controls.Add(this.labelPrivicy, 0, 3);
            this.inputFieldsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputFieldsTable.Location = new System.Drawing.Point(2, 91);
            this.inputFieldsTable.Margin = new System.Windows.Forms.Padding(0);
            this.inputFieldsTable.Name = "inputFieldsTable";
            this.inputFieldsTable.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.inputFieldsTable.RowCount = 4;
            this.inputFieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.inputFieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.inputFieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.inputFieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.inputFieldsTable.Size = new System.Drawing.Size(397, 74);
            this.inputFieldsTable.TabIndex = 11;
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImport.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonImport.Location = new System.Drawing.Point(299, 7);
            this.buttonImport.Margin = new System.Windows.Forms.Padding(0);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(75, 22);
            this.buttonImport.TabIndex = 12;
            this.buttonImport.Text = "[acceptBtn]";
            this.buttonImport.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.ButtonImportClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(299, 34);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 22);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // labelPrivicy
            // 
            this.labelPrivicy.AutoSize = true;
            this.inputFieldsTable.SetColumnSpan(this.labelPrivicy, 4);
            this.labelPrivicy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrivicy.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPrivicy.Location = new System.Drawing.Point(3, 59);
            this.labelPrivicy.Name = "labelPrivicy";
            this.labelPrivicy.Size = new System.Drawing.Size(395, 20);
            this.labelPrivicy.TabIndex = 10;
            this.labelPrivicy.Text = "[privacyMessage]";
            this.labelPrivicy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormLogin
            // 
            this.AcceptButton = this.buttonImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(401, 167);
            this.Controls.Add(this.inputFieldsTable);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowIcon = false;
            this.Text = "Import Timetable";
            this.TopMost = true;
            this.Controls.SetChildIndex(this.inputFieldsTable, 0);
            this.inputFieldsTable.ResumeLayout(false);
            this.inputFieldsTable.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel fieldsLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel inputFieldsTable;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelPrivicy;

    }
}
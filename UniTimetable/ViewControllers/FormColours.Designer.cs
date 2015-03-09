namespace UniTimetable.ViewControllers
{
    partial class FormColours
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormColours));
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.buttonColour = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanelColours = new System.Windows.Forms.TableLayoutPanel();
            this.colourSchemes = new System.Windows.Forms.ComboBox();
            this.listBoxColours = new UniTimetable.ViewControllers.ListBoxBuffered();
            this.tableLayoutPanelColours.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonColour
            // 
            this.buttonColour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonColour.Enabled = false;
            this.buttonColour.Location = new System.Drawing.Point(160, 250);
            this.buttonColour.Margin = new System.Windows.Forms.Padding(0);
            this.buttonColour.Name = "buttonColour";
            this.buttonColour.Size = new System.Drawing.Size(100, 25);
            this.buttonColour.TabIndex = 2;
            this.buttonColour.Text = "&Colour Picker";
            this.buttonColour.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonColour.UseVisualStyleBackColor = true;
            this.buttonColour.Click += new System.EventHandler(this.ButtonColorClick);
            // 
            // buttonOk
            // 
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOk.Location = new System.Drawing.Point(260, 250);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(100, 25);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "&OK";
            this.buttonOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOkClick);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(360, 250);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 25);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancelClick);
            // 
            // tableLayoutPanelColours
            // 
            this.tableLayoutPanelColours.ColumnCount = 4;
            this.tableLayoutPanelColours.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanelColours.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelColours.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelColours.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelColours.Controls.Add(this.listBoxColours, 0, 0);
            this.tableLayoutPanelColours.Controls.Add(this.colourSchemes, 0, 1);
            this.tableLayoutPanelColours.Controls.Add(this.buttonCancel, 3, 1);
            this.tableLayoutPanelColours.Controls.Add(this.buttonOk, 2, 1);
            this.tableLayoutPanelColours.Controls.Add(this.buttonColour, 1, 1);
            this.tableLayoutPanelColours.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelColours.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanelColours.Name = "tableLayoutPanelColours";
            this.tableLayoutPanelColours.RowCount = 2;
            this.tableLayoutPanelColours.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelColours.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanelColours.Size = new System.Drawing.Size(460, 275);
            this.tableLayoutPanelColours.TabIndex = 4;
            // 
            // colourSchemes
            // 
            this.colourSchemes.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.colourSchemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colourSchemes.FormattingEnabled = true;
            this.colourSchemes.Location = new System.Drawing.Point(0, 252);
            this.colourSchemes.Margin = new System.Windows.Forms.Padding(0);
            this.colourSchemes.Name = "colourSchemes";
            this.colourSchemes.Size = new System.Drawing.Size(160, 21);
            this.colourSchemes.TabIndex = 4;
            this.colourSchemes.SelectedIndexChanged += new System.EventHandler(this.DdSchemesSelectedIndexChanged);
            // 
            // listBoxColours
            // 
            this.tableLayoutPanelColours.SetColumnSpan(this.listBoxColours, 4);
            this.listBoxColours.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxColours.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxColours.FormattingEnabled = true;
            this.listBoxColours.IntegralHeight = false;
            this.listBoxColours.ItemHeight = 40;
            this.listBoxColours.Location = new System.Drawing.Point(6, 6);
            this.listBoxColours.Margin = new System.Windows.Forms.Padding(6);
            this.listBoxColours.Name = "listBoxColours";
            this.listBoxColours.Size = new System.Drawing.Size(448, 238);
            this.listBoxColours.TabIndex = 3;
            this.listBoxColours.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBoxDrawItem);
            this.listBoxColours.SelectedIndexChanged += new System.EventHandler(this.ListBoxSelectedIndexChanged);
            // 
            // FormColours
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(472, 287);
            this.Controls.Add(this.tableLayoutPanelColours);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormColours";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Colours";
            this.Load += new System.EventHandler(this.FormStyleLoad);
            this.tableLayoutPanelColours.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button buttonColour;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private ListBoxBuffered listBoxColours;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelColours;
        private System.Windows.Forms.ComboBox colourSchemes;
    }
}
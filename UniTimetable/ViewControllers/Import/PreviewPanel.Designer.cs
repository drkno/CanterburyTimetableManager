namespace UniTimetable.ViewControllers.Import
{
    partial class PreviewPanel
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
            this.boxStreamDetails = new System.Windows.Forms.GroupBox();
            this.textBoxTreeDetails = new System.Windows.Forms.TextBox();
            this.treePreview = new System.Windows.Forms.TreeView();
            this.labelTitle = new System.Windows.Forms.Label();
            this.timetableControl = new UniTimetable.ViewControllers.TimetableControl();
            this.labelImportNotice = new System.Windows.Forms.Label();
            this.boxStreamDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // boxStreamDetails
            // 
            this.boxStreamDetails.Controls.Add(this.textBoxTreeDetails);
            this.boxStreamDetails.Controls.Add(this.timetableControl);
            this.boxStreamDetails.Location = new System.Drawing.Point(267, 51);
            this.boxStreamDetails.Name = "boxStreamDetails";
            this.boxStreamDetails.Size = new System.Drawing.Size(263, 335);
            this.boxStreamDetails.TabIndex = 6;
            this.boxStreamDetails.TabStop = false;
            this.boxStreamDetails.Text = "Details";
            // 
            // textBoxTreeDetails
            // 
            this.textBoxTreeDetails.AcceptsReturn = true;
            this.textBoxTreeDetails.AcceptsTab = true;
            this.textBoxTreeDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTreeDetails.Location = new System.Drawing.Point(13, 19);
            this.textBoxTreeDetails.Multiline = true;
            this.textBoxTreeDetails.Name = "textBoxTreeDetails";
            this.textBoxTreeDetails.ReadOnly = true;
            this.textBoxTreeDetails.Size = new System.Drawing.Size(238, 142);
            this.textBoxTreeDetails.TabIndex = 2;
            // 
            // treePreview
            // 
            this.treePreview.HideSelection = false;
            this.treePreview.Location = new System.Drawing.Point(14, 57);
            this.treePreview.Name = "treePreview";
            this.treePreview.Size = new System.Drawing.Size(241, 329);
            this.treePreview.TabIndex = 5;
            this.treePreview.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreePreviewAfterSelect);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(11, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(187, 22);
            this.labelTitle.TabIndex = 4;
            this.labelTitle.Text = "Preview Stream Data";
            // 
            // timetableControl
            // 
            this.timetableControl.BackColor = System.Drawing.Color.Transparent;
            this.timetableControl.EnableDrag = false;
            this.timetableControl.Font = new System.Drawing.Font("Arial Narrow", 0.004216973F);
            this.timetableControl.Grayscale = false;
            this.timetableControl.HourEnd = 21;
            this.timetableControl.HourStart = 8;
            this.timetableControl.Location = new System.Drawing.Point(12, 172);
            this.timetableControl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.timetableControl.Name = "timetableControl";
            this.timetableControl.OutlineColour = System.Drawing.Color.LightGray;
            this.timetableControl.ShowAll = true;
            this.timetableControl.ShowDays = false;
            this.timetableControl.ShowDragGhost = true;
            this.timetableControl.ShowGrayArea = false;
            this.timetableControl.ShowLocation = false;
            this.timetableControl.ShowText = false;
            this.timetableControl.ShowTimes = false;
            this.timetableControl.ShowWeekend = true;
            this.timetableControl.Size = new System.Drawing.Size(239, 178);
            this.timetableControl.TabIndex = 3;
            this.timetableControl.TimeslotUnavalibleColour = System.Drawing.Color.LightGray;
            this.timetableControl.Timetable = null;
            // 
            // lblClashNotice
            // 
            this.labelImportNotice.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelImportNotice.ForeColor = System.Drawing.Color.Red;
            this.labelImportNotice.Location = new System.Drawing.Point(191, 9);
            this.labelImportNotice.Name = "lblClashNotice";
            this.labelImportNotice.Size = new System.Drawing.Size(339, 36);
            this.labelImportNotice.TabIndex = 24;
            this.labelImportNotice.Text = "NOTE: no unselectable streams are included in this data.\r\nTo import unselectable/" +
    "full streams change your settings in preferences.";
            this.labelImportNotice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelImportNotice.Visible = false;
            // 
            // PreviewPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelImportNotice);
            this.Controls.Add(this.boxStreamDetails);
            this.Controls.Add(this.treePreview);
            this.Controls.Add(this.labelTitle);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PreviewPanel";
            this.Size = new System.Drawing.Size(540, 396);
            this.boxStreamDetails.ResumeLayout(false);
            this.boxStreamDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox boxStreamDetails;
        private TimetableControl timetableControl;
        private System.Windows.Forms.TextBox textBoxTreeDetails;
        private System.Windows.Forms.TreeView treePreview;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelImportNotice;
    }
}

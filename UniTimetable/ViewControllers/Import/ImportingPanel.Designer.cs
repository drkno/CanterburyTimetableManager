namespace UniTimetable.ViewControllers.Import
{
    partial class ImportingPanel
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
            this.labelImportingMessage = new System.Windows.Forms.Label();
            this.progressBarLoading = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // labelImportingMessage
            // 
            this.labelImportingMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelImportingMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelImportingMessage.Location = new System.Drawing.Point(0, 172);
            this.labelImportingMessage.Name = "labelImportingMessage";
            this.labelImportingMessage.Size = new System.Drawing.Size(540, 16);
            this.labelImportingMessage.TabIndex = 1;
            this.labelImportingMessage.Text = "Please wait. Importing...";
            this.labelImportingMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBarLoading
            // 
            this.progressBarLoading.Location = new System.Drawing.Point(152, 191);
            this.progressBarLoading.Name = "progressBarLoading";
            this.progressBarLoading.Size = new System.Drawing.Size(236, 23);
            this.progressBarLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarLoading.TabIndex = 0;
            // 
            // ImportingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelImportingMessage);
            this.Controls.Add(this.progressBarLoading);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ImportingPanel";
            this.Size = new System.Drawing.Size(540, 396);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelImportingMessage;
        private System.Windows.Forms.ProgressBar progressBarLoading;
    }
}

namespace PassportMeetReservator.Forms
{
    partial class ReservedListForm
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
            this.ReservedListWrapper = new System.Windows.Forms.Panel();
            this.CopyAllButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ReservedListWrapper
            // 
            this.ReservedListWrapper.AutoScroll = true;
            this.ReservedListWrapper.Location = new System.Drawing.Point(10, 36);
            this.ReservedListWrapper.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ReservedListWrapper.Name = "ReservedListWrapper";
            this.ReservedListWrapper.Size = new System.Drawing.Size(413, 375);
            this.ReservedListWrapper.TabIndex = 0;
            // 
            // CopyAllButton
            // 
            this.CopyAllButton.Location = new System.Drawing.Point(9, 10);
            this.CopyAllButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CopyAllButton.Name = "CopyAllButton";
            this.CopyAllButton.Size = new System.Drawing.Size(414, 21);
            this.CopyAllButton.TabIndex = 0;
            this.CopyAllButton.Text = "Copy All";
            this.CopyAllButton.UseVisualStyleBackColor = true;
            this.CopyAllButton.Click += new System.EventHandler(this.CopyAllButton_Click);
            // 
            // ReservedListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 420);
            this.Controls.Add(this.CopyAllButton);
            this.Controls.Add(this.ReservedListWrapper);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ReservedListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReservedListForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ReservedListWrapper;
        private System.Windows.Forms.Button CopyAllButton;
    }
}
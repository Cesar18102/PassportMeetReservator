namespace PassportMeetReservator.Controls
{
    partial class ReservedListItemView
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
            this.InfoLabel = new System.Windows.Forms.Label();
            this.CopyInfoButton = new System.Windows.Forms.Button();
            this.ForgetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point(2, 6);
            this.InfoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(0, 13);
            this.InfoLabel.TabIndex = 0;
            // 
            // CopyInfoButton
            // 
            this.CopyInfoButton.Location = new System.Drawing.Point(271, 2);
            this.CopyInfoButton.Margin = new System.Windows.Forms.Padding(2);
            this.CopyInfoButton.Name = "CopyInfoButton";
            this.CopyInfoButton.Size = new System.Drawing.Size(61, 20);
            this.CopyInfoButton.TabIndex = 1;
            this.CopyInfoButton.Text = "Copy Info";
            this.CopyInfoButton.UseVisualStyleBackColor = true;
            // 
            // ForgetButton
            // 
            this.ForgetButton.Location = new System.Drawing.Point(336, 2);
            this.ForgetButton.Margin = new System.Windows.Forms.Padding(2);
            this.ForgetButton.Name = "ForgetButton";
            this.ForgetButton.Size = new System.Drawing.Size(61, 20);
            this.ForgetButton.TabIndex = 2;
            this.ForgetButton.Text = "Forget";
            this.ForgetButton.UseVisualStyleBackColor = true;
            // 
            // ReservedListItemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ForgetButton);
            this.Controls.Add(this.CopyInfoButton);
            this.Controls.Add(this.InfoLabel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ReservedListItemView";
            this.Size = new System.Drawing.Size(402, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Button CopyInfoButton;
        private System.Windows.Forms.Button ForgetButton;
    }
}

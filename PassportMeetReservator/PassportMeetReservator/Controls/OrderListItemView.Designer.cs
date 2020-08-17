namespace PassportMeetReservator.Controls
{
    partial class OrderListItemView
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
            this.OrderInfoLabel = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OrderInfoLabel
            // 
            this.OrderInfoLabel.AutoSize = true;
            this.OrderInfoLabel.Location = new System.Drawing.Point(3, 10);
            this.OrderInfoLabel.Name = "OrderInfoLabel";
            this.OrderInfoLabel.Size = new System.Drawing.Size(0, 17);
            this.OrderInfoLabel.TabIndex = 0;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.DeleteButton.Location = new System.Drawing.Point(574, 0);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(100, 36);
            this.DeleteButton.TabIndex = 1;
            this.DeleteButton.Text = "DELETE";
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // DownButton
            // 
            this.DownButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.DownButton.Location = new System.Drawing.Point(544, 0);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(30, 36);
            this.DownButton.TabIndex = 2;
            this.DownButton.Text = "↓";
            this.DownButton.UseVisualStyleBackColor = false;
            // 
            // UpButton
            // 
            this.UpButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.UpButton.Location = new System.Drawing.Point(514, 0);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(30, 36);
            this.UpButton.TabIndex = 3;
            this.UpButton.Text = "↑";
            this.UpButton.UseVisualStyleBackColor = false;
            // 
            // OrderListItemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.UpButton);
            this.Controls.Add(this.DownButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.OrderInfoLabel);
            this.Name = "OrderListItemView";
            this.Size = new System.Drawing.Size(674, 36);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label OrderInfoLabel;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button UpButton;
    }
}

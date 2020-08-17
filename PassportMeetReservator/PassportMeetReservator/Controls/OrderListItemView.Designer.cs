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
            this.EditButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OrderInfoLabel
            // 
            this.OrderInfoLabel.AutoSize = true;
            this.OrderInfoLabel.Location = new System.Drawing.Point(2, 8);
            this.OrderInfoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OrderInfoLabel.Name = "OrderInfoLabel";
            this.OrderInfoLabel.Size = new System.Drawing.Size(0, 13);
            this.OrderInfoLabel.TabIndex = 0;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(378, 2);
            this.DeleteButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 29);
            this.DeleteButton.TabIndex = 1;
            this.DeleteButton.Text = "DELETE";
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // DownButton
            // 
            this.DownButton.Location = new System.Drawing.Point(352, 2);
            this.DownButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(22, 29);
            this.DownButton.TabIndex = 2;
            this.DownButton.Text = "↓";
            this.DownButton.UseVisualStyleBackColor = false;
            // 
            // UpButton
            // 
            this.UpButton.Location = new System.Drawing.Point(326, 2);
            this.UpButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(22, 29);
            this.UpButton.TabIndex = 3;
            this.UpButton.Text = "↑";
            this.UpButton.UseVisualStyleBackColor = false;
            // 
            // EditButton
            // 
            this.EditButton.Location = new System.Drawing.Point(457, 2);
            this.EditButton.Margin = new System.Windows.Forms.Padding(2);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(47, 29);
            this.EditButton.TabIndex = 4;
            this.EditButton.Text = "EDIT";
            this.EditButton.UseVisualStyleBackColor = true;
            // 
            // OrderListItemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.UpButton);
            this.Controls.Add(this.DownButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.OrderInfoLabel);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "OrderListItemView";
            this.Size = new System.Drawing.Size(506, 33);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label OrderInfoLabel;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button EditButton;
    }
}

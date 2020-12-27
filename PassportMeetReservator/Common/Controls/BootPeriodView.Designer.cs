namespace PassportMeetReservator.Controls
{
    partial class BootPeriodView
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
            this.FromLabel = new System.Windows.Forms.Label();
            this.FromTime = new System.Windows.Forms.DateTimePicker();
            this.ToTime = new System.Windows.Forms.DateTimePicker();
            this.ToLabel = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.SplitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FromLabel
            // 
            this.FromLabel.AutoSize = true;
            this.FromLabel.Location = new System.Drawing.Point(2, 6);
            this.FromLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FromLabel.Name = "FromLabel";
            this.FromLabel.Size = new System.Drawing.Size(33, 13);
            this.FromLabel.TabIndex = 0;
            this.FromLabel.Text = "From:";
            // 
            // FromTime
            // 
            this.FromTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.FromTime.Location = new System.Drawing.Point(40, 2);
            this.FromTime.Margin = new System.Windows.Forms.Padding(2);
            this.FromTime.Name = "FromTime";
            this.FromTime.Size = new System.Drawing.Size(79, 20);
            this.FromTime.TabIndex = 1;
            this.FromTime.ValueChanged += new System.EventHandler(this.FromTime_ValueChanged);
            // 
            // ToTime
            // 
            this.ToTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.ToTime.Location = new System.Drawing.Point(182, 2);
            this.ToTime.Margin = new System.Windows.Forms.Padding(2);
            this.ToTime.Name = "ToTime";
            this.ToTime.Size = new System.Drawing.Size(79, 20);
            this.ToTime.TabIndex = 3;
            this.ToTime.ValueChanged += new System.EventHandler(this.ToTime_ValueChanged);
            // 
            // ToLabel
            // 
            this.ToLabel.AutoSize = true;
            this.ToLabel.Location = new System.Drawing.Point(157, 6);
            this.ToLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ToLabel.Name = "ToLabel";
            this.ToLabel.Size = new System.Drawing.Size(23, 13);
            this.ToLabel.TabIndex = 2;
            this.ToLabel.Text = "To:";
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(397, 1);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 4;
            this.DeleteButton.Text = "DELETE";
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // SplitButton
            // 
            this.SplitButton.Location = new System.Drawing.Point(316, 1);
            this.SplitButton.Name = "SplitButton";
            this.SplitButton.Size = new System.Drawing.Size(75, 23);
            this.SplitButton.TabIndex = 5;
            this.SplitButton.Text = "SPLIT";
            this.SplitButton.UseVisualStyleBackColor = true;
            // 
            // BootPeriodView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SplitButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.ToTime);
            this.Controls.Add(this.ToLabel);
            this.Controls.Add(this.FromTime);
            this.Controls.Add(this.FromLabel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "BootPeriodView";
            this.Size = new System.Drawing.Size(477, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FromLabel;
        private System.Windows.Forms.DateTimePicker FromTime;
        private System.Windows.Forms.DateTimePicker ToTime;
        private System.Windows.Forms.Label ToLabel;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button SplitButton;
    }
}

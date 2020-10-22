namespace PassportMeetReservator.Forms
{
    partial class BootPeriodSplitForm
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
            this.ToTime = new System.Windows.Forms.DateTimePicker();
            this.ToLabel = new System.Windows.Forms.Label();
            this.FromTime = new System.Windows.Forms.DateTimePicker();
            this.FromLabel = new System.Windows.Forms.Label();
            this.SplitButton = new System.Windows.Forms.Button();
            this.DurationLabel = new System.Windows.Forms.Label();
            this.CountLabel = new System.Windows.Forms.Label();
            this.CountSelector = new System.Windows.Forms.NumericUpDown();
            this.DurationSelector = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.CountSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DurationSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // ToTime
            // 
            this.ToTime.Enabled = false;
            this.ToTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.ToTime.Location = new System.Drawing.Point(49, 35);
            this.ToTime.Margin = new System.Windows.Forms.Padding(2);
            this.ToTime.Name = "ToTime";
            this.ToTime.Size = new System.Drawing.Size(79, 20);
            this.ToTime.TabIndex = 7;
            // 
            // ToLabel
            // 
            this.ToLabel.AutoSize = true;
            this.ToLabel.Location = new System.Drawing.Point(11, 39);
            this.ToLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ToLabel.Name = "ToLabel";
            this.ToLabel.Size = new System.Drawing.Size(23, 13);
            this.ToLabel.TabIndex = 6;
            this.ToLabel.Text = "To:";
            // 
            // FromTime
            // 
            this.FromTime.Enabled = false;
            this.FromTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.FromTime.Location = new System.Drawing.Point(49, 11);
            this.FromTime.Margin = new System.Windows.Forms.Padding(2);
            this.FromTime.Name = "FromTime";
            this.FromTime.Size = new System.Drawing.Size(79, 20);
            this.FromTime.TabIndex = 5;
            // 
            // FromLabel
            // 
            this.FromLabel.AutoSize = true;
            this.FromLabel.Location = new System.Drawing.Point(11, 15);
            this.FromLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FromLabel.Name = "FromLabel";
            this.FromLabel.Size = new System.Drawing.Size(33, 13);
            this.FromLabel.TabIndex = 4;
            this.FromLabel.Text = "From:";
            // 
            // SplitButton
            // 
            this.SplitButton.Location = new System.Drawing.Point(14, 60);
            this.SplitButton.Name = "SplitButton";
            this.SplitButton.Size = new System.Drawing.Size(398, 23);
            this.SplitButton.TabIndex = 8;
            this.SplitButton.Text = "SPLIT";
            this.SplitButton.UseVisualStyleBackColor = true;
            this.SplitButton.Click += new System.EventHandler(this.SplitButton_Click);
            // 
            // DurationLabel
            // 
            this.DurationLabel.AutoSize = true;
            this.DurationLabel.Location = new System.Drawing.Point(218, 39);
            this.DurationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DurationLabel.Name = "DurationLabel";
            this.DurationLabel.Size = new System.Drawing.Size(73, 13);
            this.DurationLabel.TabIndex = 10;
            this.DurationLabel.Text = "Duration, sec:";
            // 
            // CountLabel
            // 
            this.CountLabel.AutoSize = true;
            this.CountLabel.Location = new System.Drawing.Point(218, 15);
            this.CountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CountLabel.Name = "CountLabel";
            this.CountLabel.Size = new System.Drawing.Size(38, 13);
            this.CountLabel.TabIndex = 9;
            this.CountLabel.Text = "Count:";
            // 
            // CountSelector
            // 
            this.CountSelector.Location = new System.Drawing.Point(292, 12);
            this.CountSelector.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.CountSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CountSelector.Name = "CountSelector";
            this.CountSelector.Size = new System.Drawing.Size(120, 20);
            this.CountSelector.TabIndex = 11;
            this.CountSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // DurationSelector
            // 
            this.DurationSelector.Location = new System.Drawing.Point(292, 38);
            this.DurationSelector.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.DurationSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DurationSelector.Name = "DurationSelector";
            this.DurationSelector.Size = new System.Drawing.Size(120, 20);
            this.DurationSelector.TabIndex = 12;
            this.DurationSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // BootPeriodSplitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 88);
            this.Controls.Add(this.DurationSelector);
            this.Controls.Add(this.CountSelector);
            this.Controls.Add(this.DurationLabel);
            this.Controls.Add(this.CountLabel);
            this.Controls.Add(this.SplitButton);
            this.Controls.Add(this.ToTime);
            this.Controls.Add(this.ToLabel);
            this.Controls.Add(this.FromTime);
            this.Controls.Add(this.FromLabel);
            this.Name = "BootPeriodSplitForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BootPeriodSplitForm";
            ((System.ComponentModel.ISupportInitialize)(this.CountSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DurationSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker ToTime;
        private System.Windows.Forms.Label ToLabel;
        private System.Windows.Forms.DateTimePicker FromTime;
        private System.Windows.Forms.Label FromLabel;
        private System.Windows.Forms.Button SplitButton;
        private System.Windows.Forms.Label DurationLabel;
        private System.Windows.Forms.Label CountLabel;
        private System.Windows.Forms.NumericUpDown CountSelector;
        private System.Windows.Forms.NumericUpDown DurationSelector;
    }
}
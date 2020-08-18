namespace PassportMeetReservator.Forms
{
    partial class DelayInfoForm
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
            this.OrderLoadingDelayLabel = new System.Windows.Forms.Label();
            this.OrderLoadingDelay = new System.Windows.Forms.NumericUpDown();
            this.BrowserIterationDelay = new System.Windows.Forms.NumericUpDown();
            this.BrowserIterationDelayLabel = new System.Windows.Forms.Label();
            this.ActionResultDelay = new System.Windows.Forms.NumericUpDown();
            this.ActionResultDelayLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OrderLoadingDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrowserIterationDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActionResultDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // OrderLoadingDelayLabel
            // 
            this.OrderLoadingDelayLabel.AutoSize = true;
            this.OrderLoadingDelayLabel.Location = new System.Drawing.Point(16, 17);
            this.OrderLoadingDelayLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OrderLoadingDelayLabel.Name = "OrderLoadingDelayLabel";
            this.OrderLoadingDelayLabel.Size = new System.Drawing.Size(136, 16);
            this.OrderLoadingDelayLabel.TabIndex = 0;
            this.OrderLoadingDelayLabel.Text = "Order Loading Delay:";
            // 
            // OrderLoadingDelay
            // 
            this.OrderLoadingDelay.Location = new System.Drawing.Point(204, 15);
            this.OrderLoadingDelay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OrderLoadingDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.OrderLoadingDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.OrderLoadingDelay.Name = "OrderLoadingDelay";
            this.OrderLoadingDelay.Size = new System.Drawing.Size(160, 22);
            this.OrderLoadingDelay.TabIndex = 1;
            this.OrderLoadingDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // BrowserIterationDelay
            // 
            this.BrowserIterationDelay.Location = new System.Drawing.Point(203, 47);
            this.BrowserIterationDelay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BrowserIterationDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.BrowserIterationDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.BrowserIterationDelay.Name = "BrowserIterationDelay";
            this.BrowserIterationDelay.Size = new System.Drawing.Size(160, 22);
            this.BrowserIterationDelay.TabIndex = 3;
            this.BrowserIterationDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // BrowserIterationDelayLabel
            // 
            this.BrowserIterationDelayLabel.AutoSize = true;
            this.BrowserIterationDelayLabel.Location = new System.Drawing.Point(15, 49);
            this.BrowserIterationDelayLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BrowserIterationDelayLabel.Name = "BrowserIterationDelayLabel";
            this.BrowserIterationDelayLabel.Size = new System.Drawing.Size(149, 16);
            this.BrowserIterationDelayLabel.TabIndex = 2;
            this.BrowserIterationDelayLabel.Text = "Browser Iteration Delay:";
            // 
            // ActionResultDelay
            // 
            this.ActionResultDelay.Location = new System.Drawing.Point(203, 79);
            this.ActionResultDelay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ActionResultDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ActionResultDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ActionResultDelay.Name = "ActionResultDelay";
            this.ActionResultDelay.Size = new System.Drawing.Size(160, 22);
            this.ActionResultDelay.TabIndex = 5;
            this.ActionResultDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // ActionResultDelayLabel
            // 
            this.ActionResultDelayLabel.AutoSize = true;
            this.ActionResultDelayLabel.Location = new System.Drawing.Point(15, 81);
            this.ActionResultDelayLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActionResultDelayLabel.Name = "ActionResultDelayLabel";
            this.ActionResultDelayLabel.Size = new System.Drawing.Size(128, 16);
            this.ActionResultDelayLabel.TabIndex = 4;
            this.ActionResultDelayLabel.Text = "Action Result Delay:";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(16, 111);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(347, 28);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "SAVE";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // DelayInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 148);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ActionResultDelay);
            this.Controls.Add(this.ActionResultDelayLabel);
            this.Controls.Add(this.BrowserIterationDelay);
            this.Controls.Add(this.BrowserIterationDelayLabel);
            this.Controls.Add(this.OrderLoadingDelay);
            this.Controls.Add(this.OrderLoadingDelayLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DelayInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DelayInfoForm";
            ((System.ComponentModel.ISupportInitialize)(this.OrderLoadingDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrowserIterationDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActionResultDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label OrderLoadingDelayLabel;
        private System.Windows.Forms.NumericUpDown OrderLoadingDelay;
        private System.Windows.Forms.NumericUpDown BrowserIterationDelay;
        private System.Windows.Forms.Label BrowserIterationDelayLabel;
        private System.Windows.Forms.NumericUpDown ActionResultDelay;
        private System.Windows.Forms.Label ActionResultDelayLabel;
        private System.Windows.Forms.Button SaveButton;
    }
}
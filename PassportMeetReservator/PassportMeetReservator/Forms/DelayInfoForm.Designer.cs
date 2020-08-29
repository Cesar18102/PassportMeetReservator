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
            this.PostInputDelay = new System.Windows.Forms.NumericUpDown();
            this.PostInputDelayLabel = new System.Windows.Forms.Label();
            this.DiscreteWaitDelay = new System.Windows.Forms.NumericUpDown();
            this.DiscreteWaitDelayLabel = new System.Windows.Forms.Label();
            this.ManualReactionWaitDelay = new System.Windows.Forms.NumericUpDown();
            this.ReserveRefreshDelayLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.OrderLoadingDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BrowserIterationDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActionResultDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PostInputDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiscreteWaitDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ManualReactionWaitDelay)).BeginInit();
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
            1000000,
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
            1000000,
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
            1000000,
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
            this.SaveButton.Location = new System.Drawing.Point(16, 207);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(347, 28);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "SAVE";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // PostInputDelay
            // 
            this.PostInputDelay.Location = new System.Drawing.Point(204, 111);
            this.PostInputDelay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PostInputDelay.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.PostInputDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.PostInputDelay.Name = "PostInputDelay";
            this.PostInputDelay.Size = new System.Drawing.Size(160, 22);
            this.PostInputDelay.TabIndex = 8;
            this.PostInputDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // PostInputDelayLabel
            // 
            this.PostInputDelayLabel.AutoSize = true;
            this.PostInputDelayLabel.Location = new System.Drawing.Point(16, 113);
            this.PostInputDelayLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PostInputDelayLabel.Name = "PostInputDelayLabel";
            this.PostInputDelayLabel.Size = new System.Drawing.Size(108, 16);
            this.PostInputDelayLabel.TabIndex = 7;
            this.PostInputDelayLabel.Text = "Post Input Delay:";
            // 
            // DiscreteWaitDelay
            // 
            this.DiscreteWaitDelay.Location = new System.Drawing.Point(204, 143);
            this.DiscreteWaitDelay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DiscreteWaitDelay.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.DiscreteWaitDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.DiscreteWaitDelay.Name = "DiscreteWaitDelay";
            this.DiscreteWaitDelay.Size = new System.Drawing.Size(160, 22);
            this.DiscreteWaitDelay.TabIndex = 10;
            this.DiscreteWaitDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // DiscreteWaitDelayLabel
            // 
            this.DiscreteWaitDelayLabel.AutoSize = true;
            this.DiscreteWaitDelayLabel.Location = new System.Drawing.Point(16, 145);
            this.DiscreteWaitDelayLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DiscreteWaitDelayLabel.Name = "DiscreteWaitDelayLabel";
            this.DiscreteWaitDelayLabel.Size = new System.Drawing.Size(130, 16);
            this.DiscreteWaitDelayLabel.TabIndex = 9;
            this.DiscreteWaitDelayLabel.Text = "Discrete Wait Delay:";
            // 
            // ManualReactionWaitDelay
            // 
            this.ManualReactionWaitDelay.Location = new System.Drawing.Point(204, 175);
            this.ManualReactionWaitDelay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ManualReactionWaitDelay.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.ManualReactionWaitDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ManualReactionWaitDelay.Name = "ManualReactionWaitDelay";
            this.ManualReactionWaitDelay.Size = new System.Drawing.Size(160, 22);
            this.ManualReactionWaitDelay.TabIndex = 12;
            this.ManualReactionWaitDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // ReserveRefreshDelayLabel
            // 
            this.ReserveRefreshDelayLabel.AutoSize = true;
            this.ReserveRefreshDelayLabel.Location = new System.Drawing.Point(16, 177);
            this.ReserveRefreshDelayLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ReserveRefreshDelayLabel.Name = "ReserveRefreshDelayLabel";
            this.ReserveRefreshDelayLabel.Size = new System.Drawing.Size(142, 16);
            this.ReserveRefreshDelayLabel.TabIndex = 11;
            this.ReserveRefreshDelayLabel.Text = "Manual Reaction Wait:";
            // 
            // DelayInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 244);
            this.Controls.Add(this.ManualReactionWaitDelay);
            this.Controls.Add(this.ReserveRefreshDelayLabel);
            this.Controls.Add(this.DiscreteWaitDelay);
            this.Controls.Add(this.DiscreteWaitDelayLabel);
            this.Controls.Add(this.PostInputDelay);
            this.Controls.Add(this.PostInputDelayLabel);
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
            ((System.ComponentModel.ISupportInitialize)(this.PostInputDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiscreteWaitDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ManualReactionWaitDelay)).EndInit();
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
        private System.Windows.Forms.NumericUpDown PostInputDelay;
        private System.Windows.Forms.Label PostInputDelayLabel;
        private System.Windows.Forms.NumericUpDown DiscreteWaitDelay;
        private System.Windows.Forms.Label DiscreteWaitDelayLabel;
        private System.Windows.Forms.NumericUpDown ManualReactionWaitDelay;
        private System.Windows.Forms.Label ReserveRefreshDelayLabel;
    }
}
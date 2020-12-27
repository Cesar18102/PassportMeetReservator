namespace Common.Forms
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
            this.ActionResultDelay = new System.Windows.Forms.NumericUpDown();
            this.ActionResultDelayLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.RefreshSessionUpdateDelay = new System.Windows.Forms.NumericUpDown();
            this.RefreshSessionUpdateDelayLabel = new System.Windows.Forms.Label();
            this.DiscreteWaitDelay = new System.Windows.Forms.NumericUpDown();
            this.DiscreteWaitDelayLabel = new System.Windows.Forms.Label();
            this.ManualReactionWaitDelay = new System.Windows.Forms.NumericUpDown();
            this.ReserveRefreshDelayLabel = new System.Windows.Forms.Label();
            this.PostInputDelay = new System.Windows.Forms.NumericUpDown();
            this.PostInputDelayLabel = new System.Windows.Forms.Label();
            this.DateCheckDelay = new System.Windows.Forms.NumericUpDown();
            this.DateCheckDelayLabel = new System.Windows.Forms.Label();
            this.RequestTimeout = new System.Windows.Forms.NumericUpDown();
            this.RequestTimeoutLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ActionResultDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RefreshSessionUpdateDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiscreteWaitDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ManualReactionWaitDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PostInputDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateCheckDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequestTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // ActionResultDelay
            // 
            this.ActionResultDelay.Location = new System.Drawing.Point(176, 12);
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
            this.ActionResultDelay.Size = new System.Drawing.Size(112, 20);
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
            this.ActionResultDelayLabel.Location = new System.Drawing.Point(11, 14);
            this.ActionResultDelayLabel.Name = "ActionResultDelayLabel";
            this.ActionResultDelayLabel.Size = new System.Drawing.Size(103, 13);
            this.ActionResultDelayLabel.TabIndex = 4;
            this.ActionResultDelayLabel.Text = "Action Result Delay:";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(12, 194);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(276, 23);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "SAVE";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // RefreshSessionUpdateDelay
            // 
            this.RefreshSessionUpdateDelay.Location = new System.Drawing.Point(176, 38);
            this.RefreshSessionUpdateDelay.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.RefreshSessionUpdateDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.RefreshSessionUpdateDelay.Name = "RefreshSessionUpdateDelay";
            this.RefreshSessionUpdateDelay.Size = new System.Drawing.Size(112, 20);
            this.RefreshSessionUpdateDelay.TabIndex = 8;
            this.RefreshSessionUpdateDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // RefreshSessionUpdateDelayLabel
            // 
            this.RefreshSessionUpdateDelayLabel.AutoSize = true;
            this.RefreshSessionUpdateDelayLabel.Location = new System.Drawing.Point(12, 40);
            this.RefreshSessionUpdateDelayLabel.Name = "RefreshSessionUpdateDelayLabel";
            this.RefreshSessionUpdateDelayLabel.Size = new System.Drawing.Size(155, 13);
            this.RefreshSessionUpdateDelayLabel.TabIndex = 7;
            this.RefreshSessionUpdateDelayLabel.Text = "Refresh Session Update Delay:";
            // 
            // DiscreteWaitDelay
            // 
            this.DiscreteWaitDelay.Location = new System.Drawing.Point(176, 64);
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
            this.DiscreteWaitDelay.Size = new System.Drawing.Size(112, 20);
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
            this.DiscreteWaitDelayLabel.Location = new System.Drawing.Point(12, 66);
            this.DiscreteWaitDelayLabel.Name = "DiscreteWaitDelayLabel";
            this.DiscreteWaitDelayLabel.Size = new System.Drawing.Size(104, 13);
            this.DiscreteWaitDelayLabel.TabIndex = 9;
            this.DiscreteWaitDelayLabel.Text = "Discrete Wait Delay:";
            // 
            // ManualReactionWaitDelay
            // 
            this.ManualReactionWaitDelay.Location = new System.Drawing.Point(176, 90);
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
            this.ManualReactionWaitDelay.Size = new System.Drawing.Size(112, 20);
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
            this.ReserveRefreshDelayLabel.Location = new System.Drawing.Point(12, 92);
            this.ReserveRefreshDelayLabel.Name = "ReserveRefreshDelayLabel";
            this.ReserveRefreshDelayLabel.Size = new System.Drawing.Size(116, 13);
            this.ReserveRefreshDelayLabel.TabIndex = 11;
            this.ReserveRefreshDelayLabel.Text = "Manual Reaction Wait:";
            // 
            // PostInputDelay
            // 
            this.PostInputDelay.Location = new System.Drawing.Point(176, 116);
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
            this.PostInputDelay.Size = new System.Drawing.Size(112, 20);
            this.PostInputDelay.TabIndex = 14;
            this.PostInputDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // PostInputDelayLabel
            // 
            this.PostInputDelayLabel.AutoSize = true;
            this.PostInputDelayLabel.Location = new System.Drawing.Point(12, 118);
            this.PostInputDelayLabel.Name = "PostInputDelayLabel";
            this.PostInputDelayLabel.Size = new System.Drawing.Size(88, 13);
            this.PostInputDelayLabel.TabIndex = 13;
            this.PostInputDelayLabel.Text = "Post Input Delay:";
            // 
            // DateCheckDelay
            // 
            this.DateCheckDelay.Location = new System.Drawing.Point(176, 142);
            this.DateCheckDelay.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.DateCheckDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.DateCheckDelay.Name = "DateCheckDelay";
            this.DateCheckDelay.Size = new System.Drawing.Size(112, 20);
            this.DateCheckDelay.TabIndex = 16;
            this.DateCheckDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // DateCheckDelayLabel
            // 
            this.DateCheckDelayLabel.AutoSize = true;
            this.DateCheckDelayLabel.Location = new System.Drawing.Point(12, 144);
            this.DateCheckDelayLabel.Name = "DateCheckDelayLabel";
            this.DateCheckDelayLabel.Size = new System.Drawing.Size(97, 13);
            this.DateCheckDelayLabel.TabIndex = 15;
            this.DateCheckDelayLabel.Text = "Date Check Delay:";
            // 
            // RequestTimeout
            // 
            this.RequestTimeout.Location = new System.Drawing.Point(176, 168);
            this.RequestTimeout.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.RequestTimeout.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.RequestTimeout.Name = "RequestTimeout";
            this.RequestTimeout.Size = new System.Drawing.Size(112, 20);
            this.RequestTimeout.TabIndex = 18;
            this.RequestTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // RequestTimeoutLabel
            // 
            this.RequestTimeoutLabel.AutoSize = true;
            this.RequestTimeoutLabel.Location = new System.Drawing.Point(12, 170);
            this.RequestTimeoutLabel.Name = "RequestTimeoutLabel";
            this.RequestTimeoutLabel.Size = new System.Drawing.Size(87, 13);
            this.RequestTimeoutLabel.TabIndex = 17;
            this.RequestTimeoutLabel.Text = "Request timeout:";
            // 
            // DelayInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 229);
            this.Controls.Add(this.RequestTimeout);
            this.Controls.Add(this.RequestTimeoutLabel);
            this.Controls.Add(this.DateCheckDelay);
            this.Controls.Add(this.DateCheckDelayLabel);
            this.Controls.Add(this.PostInputDelay);
            this.Controls.Add(this.PostInputDelayLabel);
            this.Controls.Add(this.ManualReactionWaitDelay);
            this.Controls.Add(this.ReserveRefreshDelayLabel);
            this.Controls.Add(this.DiscreteWaitDelay);
            this.Controls.Add(this.DiscreteWaitDelayLabel);
            this.Controls.Add(this.RefreshSessionUpdateDelay);
            this.Controls.Add(this.RefreshSessionUpdateDelayLabel);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ActionResultDelay);
            this.Controls.Add(this.ActionResultDelayLabel);
            this.Name = "DelayInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DelayInfoForm";
            ((System.ComponentModel.ISupportInitialize)(this.ActionResultDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RefreshSessionUpdateDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiscreteWaitDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ManualReactionWaitDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PostInputDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateCheckDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RequestTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown ActionResultDelay;
        private System.Windows.Forms.Label ActionResultDelayLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.NumericUpDown RefreshSessionUpdateDelay;
        private System.Windows.Forms.Label RefreshSessionUpdateDelayLabel;
        private System.Windows.Forms.NumericUpDown DiscreteWaitDelay;
        private System.Windows.Forms.Label DiscreteWaitDelayLabel;
        private System.Windows.Forms.NumericUpDown ManualReactionWaitDelay;
        private System.Windows.Forms.Label ReserveRefreshDelayLabel;
        private System.Windows.Forms.NumericUpDown PostInputDelay;
        private System.Windows.Forms.Label PostInputDelayLabel;
        private System.Windows.Forms.NumericUpDown DateCheckDelay;
        private System.Windows.Forms.Label DateCheckDelayLabel;
        private System.Windows.Forms.NumericUpDown RequestTimeout;
        private System.Windows.Forms.Label RequestTimeoutLabel;
    }
}
namespace Common.Forms
{
    partial class BootScheduleForm
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
            this.BootPeriodsListWrapper = new System.Windows.Forms.Panel();
            this.AddPeriodButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BootPeriodsListWrapper
            // 
            this.BootPeriodsListWrapper.AutoScroll = true;
            this.BootPeriodsListWrapper.Location = new System.Drawing.Point(10, 40);
            this.BootPeriodsListWrapper.Margin = new System.Windows.Forms.Padding(2);
            this.BootPeriodsListWrapper.Name = "BootPeriodsListWrapper";
            this.BootPeriodsListWrapper.Size = new System.Drawing.Size(537, 244);
            this.BootPeriodsListWrapper.TabIndex = 0;
            // 
            // AddPeriodButton
            // 
            this.AddPeriodButton.Location = new System.Drawing.Point(12, 12);
            this.AddPeriodButton.Name = "AddPeriodButton";
            this.AddPeriodButton.Size = new System.Drawing.Size(280, 23);
            this.AddPeriodButton.TabIndex = 1;
            this.AddPeriodButton.Text = "Add period";
            this.AddPeriodButton.UseVisualStyleBackColor = true;
            this.AddPeriodButton.Click += new System.EventHandler(this.AddPeriodButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(298, 12);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(248, 23);
            this.ClearButton.TabIndex = 2;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // BootScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 293);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.AddPeriodButton);
            this.Controls.Add(this.BootPeriodsListWrapper);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "BootScheduleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BootScheduleForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BootPeriodsListWrapper;
        private System.Windows.Forms.Button AddPeriodButton;
        private System.Windows.Forms.Button ClearButton;
    }
}
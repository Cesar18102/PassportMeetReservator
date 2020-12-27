namespace PassportMeetBlocker
{
    partial class MainForm
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
            this.PlatformSelector = new System.Windows.Forms.ComboBox();
            this.CitySelector = new System.Windows.Forms.ComboBox();
            this.OperationSelector = new System.Windows.Forms.ComboBox();
            this.DelaySettings = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.LogWindow = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // PlatformSelector
            // 
            this.PlatformSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlatformSelector.FormattingEnabled = true;
            this.PlatformSelector.Location = new System.Drawing.Point(11, 11);
            this.PlatformSelector.Margin = new System.Windows.Forms.Padding(2);
            this.PlatformSelector.Name = "PlatformSelector";
            this.PlatformSelector.Size = new System.Drawing.Size(1084, 21);
            this.PlatformSelector.TabIndex = 39;
            this.PlatformSelector.SelectedIndexChanged += new System.EventHandler(this.CommonPlatformSelector_SelectedIndexChanged);
            // 
            // CitySelector
            // 
            this.CitySelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CitySelector.FormattingEnabled = true;
            this.CitySelector.Location = new System.Drawing.Point(11, 36);
            this.CitySelector.Margin = new System.Windows.Forms.Padding(2);
            this.CitySelector.Name = "CitySelector";
            this.CitySelector.Size = new System.Drawing.Size(1084, 21);
            this.CitySelector.TabIndex = 38;
            this.CitySelector.SelectedIndexChanged += new System.EventHandler(this.CommonCityChecker_SelectedIndexChanged);
            // 
            // OperationSelector
            // 
            this.OperationSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OperationSelector.FormattingEnabled = true;
            this.OperationSelector.Location = new System.Drawing.Point(11, 61);
            this.OperationSelector.Margin = new System.Windows.Forms.Padding(2);
            this.OperationSelector.Name = "OperationSelector";
            this.OperationSelector.Size = new System.Drawing.Size(1084, 21);
            this.OperationSelector.TabIndex = 37;
            this.OperationSelector.SelectedIndexChanged += new System.EventHandler(this.CommonOperationSelector_SelectedIndexChanged);
            // 
            // DelaySettings
            // 
            this.DelaySettings.Location = new System.Drawing.Point(11, 86);
            this.DelaySettings.Margin = new System.Windows.Forms.Padding(2);
            this.DelaySettings.Name = "DelaySettings";
            this.DelaySettings.Size = new System.Drawing.Size(1084, 34);
            this.DelaySettings.TabIndex = 36;
            this.DelaySettings.Text = "Delay Settings";
            this.DelaySettings.UseVisualStyleBackColor = true;
            this.DelaySettings.Click += new System.EventHandler(this.DelaySettings_Click);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(11, 124);
            this.StartButton.Margin = new System.Windows.Forms.Padding(2);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(1084, 34);
            this.StartButton.TabIndex = 40;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // LogWindow
            // 
            this.LogWindow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LogWindow.HideSelection = false;
            this.LogWindow.Location = new System.Drawing.Point(0, 162);
            this.LogWindow.Margin = new System.Windows.Forms.Padding(2);
            this.LogWindow.Name = "LogWindow";
            this.LogWindow.ReadOnly = true;
            this.LogWindow.Size = new System.Drawing.Size(1106, 277);
            this.LogWindow.TabIndex = 41;
            this.LogWindow.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 439);
            this.Controls.Add(this.LogWindow);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.PlatformSelector);
            this.Controls.Add(this.CitySelector);
            this.Controls.Add(this.OperationSelector);
            this.Controls.Add(this.DelaySettings);
            this.Name = "MainForm";
            this.Text = "Blocker";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox PlatformSelector;
        private System.Windows.Forms.ComboBox CitySelector;
        private System.Windows.Forms.ComboBox OperationSelector;
        private System.Windows.Forms.Button DelaySettings;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.RichTextBox LogWindow;
    }
}


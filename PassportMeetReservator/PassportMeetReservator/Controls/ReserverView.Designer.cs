namespace PassportMeetReservator.Controls
{
    partial class ReserverView
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
            this.BrowserPanel = new System.Windows.Forms.Panel();
            this.BrowserInfo = new PassportMeetReservator.Controls.ReserverInfoView();
            this.TimeSelectStrategyInfo = new System.Windows.Forms.GroupBox();
            this.TimePeriodStrategyChecker = new System.Windows.Forms.RadioButton();
            this.BrowserNumberStrategyChecker = new System.Windows.Forms.RadioButton();
            this.VirtualBrowserNumber = new System.Windows.Forms.NumericUpDown();
            this.ReserveTimeMax = new System.Windows.Forms.DateTimePicker();
            this.ReserveTimeMin = new System.Windows.Forms.DateTimePicker();
            this.PlatformSelector = new System.Windows.Forms.ComboBox();
            this.CitySelector = new System.Windows.Forms.ComboBox();
            this.Auto = new System.Windows.Forms.CheckBox();
            this.ReserveDateMax = new System.Windows.Forms.DateTimePicker();
            this.ReserveDateMin = new System.Windows.Forms.DateTimePicker();
            this.OperationSelector = new System.Windows.Forms.ComboBox();
            this.DoneButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.PauseChangeButton = new System.Windows.Forms.Button();
            this.UrlInput = new PassportMeetReservator.Controls.NamedInput();
            this.BrowserInfo.SuspendLayout();
            this.TimeSelectStrategyInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VirtualBrowserNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // BrowserPanel
            // 
            this.BrowserPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.BrowserPanel.Location = new System.Drawing.Point(2, 218);
            this.BrowserPanel.Margin = new System.Windows.Forms.Padding(2);
            this.BrowserPanel.Name = "BrowserPanel";
            this.BrowserPanel.Size = new System.Drawing.Size(376, 416);
            this.BrowserPanel.TabIndex = 9;
            // 
            // BrowserInfo
            // 
            this.BrowserInfo.Controls.Add(this.TimeSelectStrategyInfo);
            this.BrowserInfo.Controls.Add(this.VirtualBrowserNumber);
            this.BrowserInfo.Controls.Add(this.ReserveTimeMax);
            this.BrowserInfo.Controls.Add(this.ReserveTimeMin);
            this.BrowserInfo.Controls.Add(this.PlatformSelector);
            this.BrowserInfo.Controls.Add(this.CitySelector);
            this.BrowserInfo.Controls.Add(this.Auto);
            this.BrowserInfo.Controls.Add(this.ReserveDateMax);
            this.BrowserInfo.Controls.Add(this.ReserveDateMin);
            this.BrowserInfo.Controls.Add(this.OperationSelector);
            this.BrowserInfo.Controls.Add(this.DoneButton);
            this.BrowserInfo.Controls.Add(this.ResetButton);
            this.BrowserInfo.Controls.Add(this.PauseChangeButton);
            this.BrowserInfo.Controls.Add(this.UrlInput);
            this.BrowserInfo.Location = new System.Drawing.Point(2, 2);
            this.BrowserInfo.Margin = new System.Windows.Forms.Padding(2);
            this.BrowserInfo.Name = "BrowserInfo";
            this.BrowserInfo.Padding = new System.Windows.Forms.Padding(2);
            this.BrowserInfo.Size = new System.Drawing.Size(376, 212);
            this.BrowserInfo.TabIndex = 10;
            this.BrowserInfo.TabStop = false;
            this.BrowserInfo.UrlInput = this.UrlInput;
            // 
            // TimeSelectStrategyInfo
            // 
            this.TimeSelectStrategyInfo.Controls.Add(this.TimePeriodStrategyChecker);
            this.TimeSelectStrategyInfo.Controls.Add(this.BrowserNumberStrategyChecker);
            this.TimeSelectStrategyInfo.Location = new System.Drawing.Point(249, 40);
            this.TimeSelectStrategyInfo.Name = "TimeSelectStrategyInfo";
            this.TimeSelectStrategyInfo.Size = new System.Drawing.Size(126, 64);
            this.TimeSelectStrategyInfo.TabIndex = 36;
            this.TimeSelectStrategyInfo.TabStop = false;
            this.TimeSelectStrategyInfo.Text = "Time select strategy";
            // 
            // TimePeriodStrategyChecker
            // 
            this.TimePeriodStrategyChecker.AutoSize = true;
            this.TimePeriodStrategyChecker.Checked = true;
            this.TimePeriodStrategyChecker.Location = new System.Drawing.Point(6, 44);
            this.TimePeriodStrategyChecker.Name = "TimePeriodStrategyChecker";
            this.TimePeriodStrategyChecker.Size = new System.Drawing.Size(91, 17);
            this.TimePeriodStrategyChecker.TabIndex = 34;
            this.TimePeriodStrategyChecker.TabStop = true;
            this.TimePeriodStrategyChecker.Text = "By time period";
            this.TimePeriodStrategyChecker.UseVisualStyleBackColor = true;
            this.TimePeriodStrategyChecker.CheckedChanged += new System.EventHandler(this.StrategyChecker_CheckedChanged);
            // 
            // BrowserNumberStrategyChecker
            // 
            this.BrowserNumberStrategyChecker.AutoSize = true;
            this.BrowserNumberStrategyChecker.Location = new System.Drawing.Point(6, 19);
            this.BrowserNumberStrategyChecker.Name = "BrowserNumberStrategyChecker";
            this.BrowserNumberStrategyChecker.Size = new System.Drawing.Size(115, 17);
            this.BrowserNumberStrategyChecker.TabIndex = 33;
            this.BrowserNumberStrategyChecker.Text = "By browser number";
            this.BrowserNumberStrategyChecker.UseVisualStyleBackColor = true;
            this.BrowserNumberStrategyChecker.CheckedChanged += new System.EventHandler(this.StrategyChecker_CheckedChanged);
            // 
            // VirtualBrowserNumber
            // 
            this.VirtualBrowserNumber.Location = new System.Drawing.Point(250, 110);
            this.VirtualBrowserNumber.Name = "VirtualBrowserNumber";
            this.VirtualBrowserNumber.Size = new System.Drawing.Size(121, 20);
            this.VirtualBrowserNumber.TabIndex = 30;
            this.VirtualBrowserNumber.ValueChanged += new System.EventHandler(this.VirtualBrowserNumber_ValueChanged);
            // 
            // ReserveTimeMax
            // 
            this.ReserveTimeMax.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.ReserveTimeMax.Location = new System.Drawing.Point(131, 161);
            this.ReserveTimeMax.Name = "ReserveTimeMax";
            this.ReserveTimeMax.Size = new System.Drawing.Size(114, 20);
            this.ReserveTimeMax.TabIndex = 29;
            this.ReserveTimeMax.ValueChanged += new System.EventHandler(this.ReserveTimeMax_ValueChanged);
            // 
            // ReserveTimeMin
            // 
            this.ReserveTimeMin.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.ReserveTimeMin.Location = new System.Drawing.Point(7, 161);
            this.ReserveTimeMin.Name = "ReserveTimeMin";
            this.ReserveTimeMin.Size = new System.Drawing.Size(120, 20);
            this.ReserveTimeMin.TabIndex = 28;
            this.ReserveTimeMin.ValueChanged += new System.EventHandler(this.ReserveTimeMin_ValueChanged);
            // 
            // PlatformSelector
            // 
            this.PlatformSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlatformSelector.FormattingEnabled = true;
            this.PlatformSelector.Location = new System.Drawing.Point(8, 59);
            this.PlatformSelector.Margin = new System.Windows.Forms.Padding(2);
            this.PlatformSelector.Name = "PlatformSelector";
            this.PlatformSelector.Size = new System.Drawing.Size(237, 21);
            this.PlatformSelector.TabIndex = 27;
            this.PlatformSelector.SelectedIndexChanged += new System.EventHandler(this.PlatformSelector_SelectedIndexChanged);
            // 
            // CitySelector
            // 
            this.CitySelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CitySelector.FormattingEnabled = true;
            this.CitySelector.Location = new System.Drawing.Point(7, 84);
            this.CitySelector.Margin = new System.Windows.Forms.Padding(2);
            this.CitySelector.Name = "CitySelector";
            this.CitySelector.Size = new System.Drawing.Size(237, 21);
            this.CitySelector.TabIndex = 25;
            this.CitySelector.SelectedIndexChanged += new System.EventHandler(this.CitySelector_SelectedIndexChanged);
            // 
            // Auto
            // 
            this.Auto.AutoSize = true;
            this.Auto.Location = new System.Drawing.Point(264, 17);
            this.Auto.Name = "Auto";
            this.Auto.Size = new System.Drawing.Size(48, 17);
            this.Auto.TabIndex = 24;
            this.Auto.Text = "Auto";
            this.Auto.UseVisualStyleBackColor = true;
            this.Auto.CheckedChanged += new System.EventHandler(this.Auto_CheckedChanged);
            // 
            // ReserveDateMax
            // 
            this.ReserveDateMax.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ReserveDateMax.Location = new System.Drawing.Point(131, 135);
            this.ReserveDateMax.Name = "ReserveDateMax";
            this.ReserveDateMax.Size = new System.Drawing.Size(114, 20);
            this.ReserveDateMax.TabIndex = 23;
            this.ReserveDateMax.ValueChanged += new System.EventHandler(this.ReserveDateMax_ValueChanged);
            // 
            // ReserveDateMin
            // 
            this.ReserveDateMin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ReserveDateMin.Location = new System.Drawing.Point(7, 135);
            this.ReserveDateMin.Name = "ReserveDateMin";
            this.ReserveDateMin.Size = new System.Drawing.Size(120, 20);
            this.ReserveDateMin.TabIndex = 22;
            this.ReserveDateMin.ValueChanged += new System.EventHandler(this.ReserveDateMin_ValueChanged);
            // 
            // OperationSelector
            // 
            this.OperationSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OperationSelector.FormattingEnabled = true;
            this.OperationSelector.Location = new System.Drawing.Point(7, 109);
            this.OperationSelector.Margin = new System.Windows.Forms.Padding(2);
            this.OperationSelector.Name = "OperationSelector";
            this.OperationSelector.Size = new System.Drawing.Size(237, 21);
            this.OperationSelector.TabIndex = 9;
            this.OperationSelector.SelectedIndexChanged += new System.EventHandler(this.OperationSelector_SelectedIndexChanged);
            // 
            // DoneButton
            // 
            this.DoneButton.Location = new System.Drawing.Point(169, 17);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 39);
            this.DoneButton.TabIndex = 8;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(88, 17);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 39);
            this.ResetButton.TabIndex = 7;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // PauseChangeButton
            // 
            this.PauseChangeButton.Location = new System.Drawing.Point(7, 17);
            this.PauseChangeButton.Name = "PauseChangeButton";
            this.PauseChangeButton.Size = new System.Drawing.Size(75, 39);
            this.PauseChangeButton.TabIndex = 6;
            this.PauseChangeButton.Text = "Continue";
            this.PauseChangeButton.UseVisualStyleBackColor = true;
            this.PauseChangeButton.Click += new System.EventHandler(this.Continue_Click);
            // 
            // UrlInput
            // 
            this.UrlInput.Editable = false;
            this.UrlInput.InputLeft = 75;
            this.UrlInput.InputSize = new System.Drawing.Size(160, 20);
            this.UrlInput.InputText = "";
            this.UrlInput.LabelLeft = 0;
            this.UrlInput.LabelText = "URL:";
            this.UrlInput.Location = new System.Drawing.Point(4, 186);
            this.UrlInput.Margin = new System.Windows.Forms.Padding(2);
            this.UrlInput.Name = "UrlInput";
            this.UrlInput.Size = new System.Drawing.Size(240, 19);
            this.UrlInput.TabIndex = 5;
            this.UrlInput.Text = "namedInput1";
            this.UrlInput.TopPosition = 0;
            // 
            // ReserverView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BrowserPanel);
            this.Controls.Add(this.BrowserInfo);
            this.Name = "ReserverView";
            this.Size = new System.Drawing.Size(380, 636);
            this.BrowserInfo.ResumeLayout(false);
            this.BrowserInfo.PerformLayout();
            this.TimeSelectStrategyInfo.ResumeLayout(false);
            this.TimeSelectStrategyInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VirtualBrowserNumber)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BrowserPanel;
        private ReserverInfoView BrowserInfo;
        private System.Windows.Forms.DateTimePicker ReserveTimeMax;
        private System.Windows.Forms.DateTimePicker ReserveTimeMin;
        private System.Windows.Forms.ComboBox PlatformSelector;
        private System.Windows.Forms.ComboBox CitySelector;
        private System.Windows.Forms.CheckBox Auto;
        private System.Windows.Forms.DateTimePicker ReserveDateMax;
        private System.Windows.Forms.DateTimePicker ReserveDateMin;
        private System.Windows.Forms.ComboBox OperationSelector;
        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button PauseChangeButton;
        private NamedInput UrlInput;
        private System.Windows.Forms.NumericUpDown VirtualBrowserNumber;
        private System.Windows.Forms.GroupBox TimeSelectStrategyInfo;
        private System.Windows.Forms.RadioButton TimePeriodStrategyChecker;
        private System.Windows.Forms.RadioButton BrowserNumberStrategyChecker;
    }
}

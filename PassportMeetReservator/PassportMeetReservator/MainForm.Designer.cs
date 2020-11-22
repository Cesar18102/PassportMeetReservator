namespace PassportMeetReservator
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
            this.OrdersInfo = new System.Windows.Forms.RichTextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.ReservedListButton = new System.Windows.Forms.Button();
            this.ScheduleButton = new System.Windows.Forms.Button();
            this.StartScheduledButton = new System.Windows.Forms.Button();
            this.UnbindScheduleButton = new System.Windows.Forms.Button();
            this.DelaySettings = new System.Windows.Forms.Button();
            this.BotNumberLabel = new System.Windows.Forms.Label();
            this.BotNumber = new System.Windows.Forms.NumericUpDown();
            this.ReserveDateMinPicker = new System.Windows.Forms.DateTimePicker();
            this.OperationSelector = new System.Windows.Forms.ComboBox();
            this.ReserveDateMaxPicker = new System.Windows.Forms.DateTimePicker();
            this.LogChatIdLabel = new System.Windows.Forms.Label();
            this.LogChatId = new System.Windows.Forms.TextBox();
            this.OrderListButton = new System.Windows.Forms.Button();
            this.AddOrderButton = new System.Windows.Forms.Button();
            this.ResetAllButton = new System.Windows.Forms.Button();
            this.CitySelector = new System.Windows.Forms.ComboBox();
            this.ForceRollBrowserUp = new System.Windows.Forms.Button();
            this.PlatformSelector = new System.Windows.Forms.ComboBox();
            this.ReserversPanel = new System.Windows.Forms.Panel();
            this.ReserversCount = new System.Windows.Forms.NumericUpDown();
            this.ReserversCountLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.BotNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReserversCount)).BeginInit();
            this.SuspendLayout();
            // 
            // OrdersInfo
            // 
            this.OrdersInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.OrdersInfo.HideSelection = false;
            this.OrdersInfo.Location = new System.Drawing.Point(0, 760);
            this.OrdersInfo.Margin = new System.Windows.Forms.Padding(2);
            this.OrdersInfo.Name = "OrdersInfo";
            this.OrdersInfo.ReadOnly = true;
            this.OrdersInfo.Size = new System.Drawing.Size(1828, 144);
            this.OrdersInfo.TabIndex = 4;
            this.OrdersInfo.Text = "";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(9, 646);
            this.StartButton.Margin = new System.Windows.Forms.Padding(2);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(150, 34);
            this.StartButton.TabIndex = 11;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Location = new System.Drawing.Point(9, 684);
            this.PauseButton.Margin = new System.Windows.Forms.Padding(2);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(150, 34);
            this.PauseButton.TabIndex = 12;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // ReservedListButton
            // 
            this.ReservedListButton.Location = new System.Drawing.Point(407, 722);
            this.ReservedListButton.Margin = new System.Windows.Forms.Padding(2);
            this.ReservedListButton.Name = "ReservedListButton";
            this.ReservedListButton.Size = new System.Drawing.Size(150, 34);
            this.ReservedListButton.TabIndex = 15;
            this.ReservedListButton.Text = "Reserved List";
            this.ReservedListButton.UseVisualStyleBackColor = true;
            this.ReservedListButton.Click += new System.EventHandler(this.ReservedListButton_Click);
            // 
            // ScheduleButton
            // 
            this.ScheduleButton.Location = new System.Drawing.Point(209, 646);
            this.ScheduleButton.Margin = new System.Windows.Forms.Padding(2);
            this.ScheduleButton.Name = "ScheduleButton";
            this.ScheduleButton.Size = new System.Drawing.Size(150, 34);
            this.ScheduleButton.TabIndex = 16;
            this.ScheduleButton.Text = "Schedule";
            this.ScheduleButton.UseVisualStyleBackColor = true;
            this.ScheduleButton.Click += new System.EventHandler(this.ScheduleButton_Click);
            // 
            // StartScheduledButton
            // 
            this.StartScheduledButton.Location = new System.Drawing.Point(209, 684);
            this.StartScheduledButton.Margin = new System.Windows.Forms.Padding(2);
            this.StartScheduledButton.Name = "StartScheduledButton";
            this.StartScheduledButton.Size = new System.Drawing.Size(150, 34);
            this.StartScheduledButton.TabIndex = 17;
            this.StartScheduledButton.Text = "Start Scheduled";
            this.StartScheduledButton.UseVisualStyleBackColor = true;
            this.StartScheduledButton.Click += new System.EventHandler(this.StartScheduledButton_Click);
            // 
            // UnbindScheduleButton
            // 
            this.UnbindScheduleButton.Location = new System.Drawing.Point(209, 722);
            this.UnbindScheduleButton.Margin = new System.Windows.Forms.Padding(2);
            this.UnbindScheduleButton.Name = "UnbindScheduleButton";
            this.UnbindScheduleButton.Size = new System.Drawing.Size(150, 34);
            this.UnbindScheduleButton.TabIndex = 18;
            this.UnbindScheduleButton.Text = "Unbind Schedule";
            this.UnbindScheduleButton.UseVisualStyleBackColor = true;
            this.UnbindScheduleButton.Click += new System.EventHandler(this.UnbindScheduleButton_Click);
            // 
            // DelaySettings
            // 
            this.DelaySettings.Location = new System.Drawing.Point(1238, 724);
            this.DelaySettings.Margin = new System.Windows.Forms.Padding(2);
            this.DelaySettings.Name = "DelaySettings";
            this.DelaySettings.Size = new System.Drawing.Size(183, 34);
            this.DelaySettings.TabIndex = 19;
            this.DelaySettings.Text = "Delay Settings";
            this.DelaySettings.UseVisualStyleBackColor = true;
            this.DelaySettings.Click += new System.EventHandler(this.DelaySettings_Click);
            // 
            // BotNumberLabel
            // 
            this.BotNumberLabel.AutoSize = true;
            this.BotNumberLabel.Location = new System.Drawing.Point(642, 649);
            this.BotNumberLabel.Name = "BotNumberLabel";
            this.BotNumberLabel.Size = new System.Drawing.Size(66, 13);
            this.BotNumberLabel.TabIndex = 20;
            this.BotNumberLabel.Text = "Bot Number:";
            // 
            // BotNumber
            // 
            this.BotNumber.Location = new System.Drawing.Point(732, 647);
            this.BotNumber.Name = "BotNumber";
            this.BotNumber.Size = new System.Drawing.Size(120, 20);
            this.BotNumber.TabIndex = 21;
            this.BotNumber.ValueChanged += new System.EventHandler(this.BotNumber_ValueChanged);
            // 
            // ReserveDateMinPicker
            // 
            this.ReserveDateMinPicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ReserveDateMinPicker.Location = new System.Drawing.Point(914, 735);
            this.ReserveDateMinPicker.Name = "ReserveDateMinPicker";
            this.ReserveDateMinPicker.Size = new System.Drawing.Size(148, 20);
            this.ReserveDateMinPicker.TabIndex = 26;
            this.ReserveDateMinPicker.ValueChanged += new System.EventHandler(this.ReserveDateMinPicker_ValueChanged);
            // 
            // OperationSelector
            // 
            this.OperationSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OperationSelector.FormattingEnabled = true;
            this.OperationSelector.Location = new System.Drawing.Point(914, 699);
            this.OperationSelector.Margin = new System.Windows.Forms.Padding(2);
            this.OperationSelector.Name = "OperationSelector";
            this.OperationSelector.Size = new System.Drawing.Size(507, 21);
            this.OperationSelector.TabIndex = 25;
            this.OperationSelector.SelectedIndexChanged += new System.EventHandler(this.CommonOperationSelector_SelectedIndexChanged);
            // 
            // ReserveDateMaxPicker
            // 
            this.ReserveDateMaxPicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ReserveDateMaxPicker.Location = new System.Drawing.Point(1071, 735);
            this.ReserveDateMaxPicker.Name = "ReserveDateMaxPicker";
            this.ReserveDateMaxPicker.Size = new System.Drawing.Size(148, 20);
            this.ReserveDateMaxPicker.TabIndex = 27;
            this.ReserveDateMaxPicker.ValueChanged += new System.EventHandler(this.ReserveDateMaxPicker_ValueChanged);
            // 
            // LogChatIdLabel
            // 
            this.LogChatIdLabel.AutoSize = true;
            this.LogChatIdLabel.Location = new System.Drawing.Point(642, 678);
            this.LogChatIdLabel.Name = "LogChatIdLabel";
            this.LogChatIdLabel.Size = new System.Drawing.Size(65, 13);
            this.LogChatIdLabel.TabIndex = 28;
            this.LogChatIdLabel.Text = "Log Chat Id:";
            // 
            // LogChatId
            // 
            this.LogChatId.Location = new System.Drawing.Point(732, 673);
            this.LogChatId.Name = "LogChatId";
            this.LogChatId.Size = new System.Drawing.Size(120, 20);
            this.LogChatId.TabIndex = 29;
            this.LogChatId.TextChanged += new System.EventHandler(this.LogChatId_TextChanged);
            // 
            // OrderListButton
            // 
            this.OrderListButton.Location = new System.Drawing.Point(407, 684);
            this.OrderListButton.Margin = new System.Windows.Forms.Padding(2);
            this.OrderListButton.Name = "OrderListButton";
            this.OrderListButton.Size = new System.Drawing.Size(150, 34);
            this.OrderListButton.TabIndex = 31;
            this.OrderListButton.Text = "Order List";
            this.OrderListButton.UseVisualStyleBackColor = true;
            this.OrderListButton.Click += new System.EventHandler(this.OrderListButton_Click);
            // 
            // AddOrderButton
            // 
            this.AddOrderButton.Location = new System.Drawing.Point(407, 646);
            this.AddOrderButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddOrderButton.Name = "AddOrderButton";
            this.AddOrderButton.Size = new System.Drawing.Size(150, 34);
            this.AddOrderButton.TabIndex = 30;
            this.AddOrderButton.Text = "Add Order";
            this.AddOrderButton.UseVisualStyleBackColor = true;
            this.AddOrderButton.Click += new System.EventHandler(this.AddOrderButton_Click);
            // 
            // ResetAllButton
            // 
            this.ResetAllButton.Location = new System.Drawing.Point(9, 722);
            this.ResetAllButton.Margin = new System.Windows.Forms.Padding(2);
            this.ResetAllButton.Name = "ResetAllButton";
            this.ResetAllButton.Size = new System.Drawing.Size(150, 34);
            this.ResetAllButton.TabIndex = 32;
            this.ResetAllButton.Text = "Reset ALL";
            this.ResetAllButton.UseVisualStyleBackColor = true;
            this.ResetAllButton.Click += new System.EventHandler(this.ResetAllButton_Click);
            // 
            // CitySelector
            // 
            this.CitySelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CitySelector.FormattingEnabled = true;
            this.CitySelector.Location = new System.Drawing.Point(914, 674);
            this.CitySelector.Margin = new System.Windows.Forms.Padding(2);
            this.CitySelector.Name = "CitySelector";
            this.CitySelector.Size = new System.Drawing.Size(507, 21);
            this.CitySelector.TabIndex = 33;
            this.CitySelector.SelectedIndexChanged += new System.EventHandler(this.CommonCityChecker_SelectedIndexChanged);
            // 
            // ForceRollBrowserUp
            // 
            this.ForceRollBrowserUp.Location = new System.Drawing.Point(1612, 870);
            this.ForceRollBrowserUp.Margin = new System.Windows.Forms.Padding(2);
            this.ForceRollBrowserUp.Name = "ForceRollBrowserUp";
            this.ForceRollBrowserUp.Size = new System.Drawing.Size(150, 34);
            this.ForceRollBrowserUp.TabIndex = 34;
            this.ForceRollBrowserUp.Text = "Force Roll Browser Up";
            this.ForceRollBrowserUp.UseVisualStyleBackColor = true;
            this.ForceRollBrowserUp.Click += new System.EventHandler(this.ForceRollBrowserUp_Click);
            // 
            // PlatformSelector
            // 
            this.PlatformSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlatformSelector.FormattingEnabled = true;
            this.PlatformSelector.Location = new System.Drawing.Point(914, 649);
            this.PlatformSelector.Margin = new System.Windows.Forms.Padding(2);
            this.PlatformSelector.Name = "PlatformSelector";
            this.PlatformSelector.Size = new System.Drawing.Size(507, 21);
            this.PlatformSelector.TabIndex = 35;
            this.PlatformSelector.SelectedIndexChanged += new System.EventHandler(this.CommonPlatformSelector_SelectedIndexChanged);
            // 
            // ReserversPanel
            // 
            this.ReserversPanel.AutoScroll = true;
            this.ReserversPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ReserversPanel.Location = new System.Drawing.Point(0, 0);
            this.ReserversPanel.Name = "ReserversPanel";
            this.ReserversPanel.Size = new System.Drawing.Size(1828, 641);
            this.ReserversPanel.TabIndex = 36;
            // 
            // ReserversCount
            // 
            this.ReserversCount.Location = new System.Drawing.Point(732, 700);
            this.ReserversCount.Name = "ReserversCount";
            this.ReserversCount.Size = new System.Drawing.Size(120, 20);
            this.ReserversCount.TabIndex = 38;
            this.ReserversCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ReserversCount.ValueChanged += new System.EventHandler(this.ReserversCount_ValueChanged);
            // 
            // ReserversCountLabel
            // 
            this.ReserversCountLabel.AutoSize = true;
            this.ReserversCountLabel.Location = new System.Drawing.Point(642, 702);
            this.ReserversCountLabel.Name = "ReserversCountLabel";
            this.ReserversCountLabel.Size = new System.Drawing.Size(89, 13);
            this.ReserversCountLabel.TabIndex = 37;
            this.ReserversCountLabel.Text = "Reservers Count:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1828, 904);
            this.Controls.Add(this.ReserversCount);
            this.Controls.Add(this.ReserversCountLabel);
            this.Controls.Add(this.ReserversPanel);
            this.Controls.Add(this.PlatformSelector);
            this.Controls.Add(this.ForceRollBrowserUp);
            this.Controls.Add(this.CitySelector);
            this.Controls.Add(this.ResetAllButton);
            this.Controls.Add(this.OrderListButton);
            this.Controls.Add(this.AddOrderButton);
            this.Controls.Add(this.LogChatId);
            this.Controls.Add(this.LogChatIdLabel);
            this.Controls.Add(this.ReserveDateMaxPicker);
            this.Controls.Add(this.ReserveDateMinPicker);
            this.Controls.Add(this.OperationSelector);
            this.Controls.Add(this.BotNumber);
            this.Controls.Add(this.BotNumberLabel);
            this.Controls.Add(this.DelaySettings);
            this.Controls.Add(this.UnbindScheduleButton);
            this.Controls.Add(this.StartScheduledButton);
            this.Controls.Add(this.ScheduleButton);
            this.Controls.Add(this.ReservedListButton);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.OrdersInfo);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.BotNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReserversCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox OrdersInfo;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.Button ReservedListButton;
        private System.Windows.Forms.Button ScheduleButton;
        private System.Windows.Forms.Button StartScheduledButton;
        private System.Windows.Forms.Button UnbindScheduleButton;
        private System.Windows.Forms.Button DelaySettings;
        private System.Windows.Forms.Label BotNumberLabel;
        private System.Windows.Forms.NumericUpDown BotNumber;
        private System.Windows.Forms.DateTimePicker ReserveDateMinPicker;
        private System.Windows.Forms.ComboBox OperationSelector;
        private System.Windows.Forms.DateTimePicker ReserveDateMaxPicker;
        private System.Windows.Forms.Label LogChatIdLabel;
        private System.Windows.Forms.TextBox LogChatId;
        private System.Windows.Forms.Button OrderListButton;
        private System.Windows.Forms.Button AddOrderButton;
        private System.Windows.Forms.Button ResetAllButton;
        private System.Windows.Forms.ComboBox CitySelector;
        private System.Windows.Forms.Button ForceRollBrowserUp;
        private System.Windows.Forms.ComboBox PlatformSelector;
        private System.Windows.Forms.Panel ReserversPanel;
        private System.Windows.Forms.NumericUpDown ReserversCount;
        private System.Windows.Forms.Label ReserversCountLabel;
    }
}


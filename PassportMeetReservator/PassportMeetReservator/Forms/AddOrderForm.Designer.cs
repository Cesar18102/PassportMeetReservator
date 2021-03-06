﻿namespace PassportMeetReservator.Forms
{
    partial class AddOrderForm
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
            this.OperationLabel = new System.Windows.Forms.Label();
            this.OperationSelector = new System.Windows.Forms.ComboBox();
            this.AddFirstButton = new System.Windows.Forms.Button();
            this.AddLastButton = new System.Windows.Forms.Button();
            this.CitySelector = new System.Windows.Forms.ComboBox();
            this.CityLabel = new System.Windows.Forms.Label();
            this.EmailInput = new Common.Controls.NamedInput();
            this.NameInput = new Common.Controls.NamedInput();
            this.SurnameInput = new Common.Controls.NamedInput();
            this.PlatformSelector = new System.Windows.Forms.ComboBox();
            this.PlatformLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OperationLabel
            // 
            this.OperationLabel.AutoSize = true;
            this.OperationLabel.Location = new System.Drawing.Point(9, 134);
            this.OperationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OperationLabel.Name = "OperationLabel";
            this.OperationLabel.Size = new System.Drawing.Size(56, 13);
            this.OperationLabel.TabIndex = 3;
            this.OperationLabel.Text = "Operation:";
            // 
            // OperationSelector
            // 
            this.OperationSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OperationSelector.FormattingEnabled = true;
            this.OperationSelector.Location = new System.Drawing.Point(121, 131);
            this.OperationSelector.Margin = new System.Windows.Forms.Padding(2);
            this.OperationSelector.Name = "OperationSelector";
            this.OperationSelector.Size = new System.Drawing.Size(580, 21);
            this.OperationSelector.TabIndex = 4;
            // 
            // AddFirstButton
            // 
            this.AddFirstButton.Location = new System.Drawing.Point(12, 161);
            this.AddFirstButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddFirstButton.Name = "AddFirstButton";
            this.AddFirstButton.Size = new System.Drawing.Size(346, 19);
            this.AddFirstButton.TabIndex = 5;
            this.AddFirstButton.Text = "Add First";
            this.AddFirstButton.UseVisualStyleBackColor = true;
            this.AddFirstButton.Click += new System.EventHandler(this.AddFirstButton_Click);
            // 
            // AddLastButton
            // 
            this.AddLastButton.Location = new System.Drawing.Point(362, 161);
            this.AddLastButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddLastButton.Name = "AddLastButton";
            this.AddLastButton.Size = new System.Drawing.Size(339, 19);
            this.AddLastButton.TabIndex = 6;
            this.AddLastButton.Text = "Add Last";
            this.AddLastButton.UseVisualStyleBackColor = true;
            this.AddLastButton.Click += new System.EventHandler(this.AddLastButton_Click);
            // 
            // CitySelector
            // 
            this.CitySelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CitySelector.FormattingEnabled = true;
            this.CitySelector.Location = new System.Drawing.Point(121, 106);
            this.CitySelector.Margin = new System.Windows.Forms.Padding(2);
            this.CitySelector.Name = "CitySelector";
            this.CitySelector.Size = new System.Drawing.Size(579, 21);
            this.CitySelector.TabIndex = 8;
            // 
            // CityLabel
            // 
            this.CityLabel.AutoSize = true;
            this.CityLabel.Location = new System.Drawing.Point(8, 109);
            this.CityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CityLabel.Name = "CityLabel";
            this.CityLabel.Size = new System.Drawing.Size(27, 13);
            this.CityLabel.TabIndex = 7;
            this.CityLabel.Text = "City:";
            // 
            // EmailInput
            // 
            this.EmailInput.Editable = true;
            this.EmailInput.InputLeft = 112;
            this.EmailInput.InputSize = new System.Drawing.Size(600, 20);
            this.EmailInput.InputText = "";
            this.EmailInput.LabelLeft = 0;
            this.EmailInput.LabelText = "Email:";
            this.EmailInput.Location = new System.Drawing.Point(10, 58);
            this.EmailInput.Margin = new System.Windows.Forms.Padding(2);
            this.EmailInput.Name = "EmailInput";
            this.EmailInput.Size = new System.Drawing.Size(690, 19);
            this.EmailInput.TabIndex = 2;
            this.EmailInput.Text = "namedInput1";
            this.EmailInput.TopPosition = 0;
            // 
            // NameInput
            // 
            this.NameInput.Editable = true;
            this.NameInput.InputLeft = 112;
            this.NameInput.InputSize = new System.Drawing.Size(600, 20);
            this.NameInput.InputText = "";
            this.NameInput.LabelLeft = 0;
            this.NameInput.LabelText = "Name:";
            this.NameInput.Location = new System.Drawing.Point(10, 34);
            this.NameInput.Margin = new System.Windows.Forms.Padding(2);
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(690, 19);
            this.NameInput.TabIndex = 1;
            this.NameInput.Text = "namedInput1";
            this.NameInput.TopPosition = 0;
            // 
            // SurnameInput
            // 
            this.SurnameInput.Editable = true;
            this.SurnameInput.InputLeft = 112;
            this.SurnameInput.InputSize = new System.Drawing.Size(600, 20);
            this.SurnameInput.InputText = "";
            this.SurnameInput.LabelLeft = 0;
            this.SurnameInput.LabelText = "Surname:";
            this.SurnameInput.Location = new System.Drawing.Point(10, 11);
            this.SurnameInput.Margin = new System.Windows.Forms.Padding(2);
            this.SurnameInput.Name = "SurnameInput";
            this.SurnameInput.Size = new System.Drawing.Size(690, 19);
            this.SurnameInput.TabIndex = 0;
            this.SurnameInput.Text = "namedInput1";
            this.SurnameInput.TopPosition = 0;
            // 
            // PlatformSelector
            // 
            this.PlatformSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PlatformSelector.FormattingEnabled = true;
            this.PlatformSelector.Location = new System.Drawing.Point(121, 81);
            this.PlatformSelector.Margin = new System.Windows.Forms.Padding(2);
            this.PlatformSelector.Name = "PlatformSelector";
            this.PlatformSelector.Size = new System.Drawing.Size(579, 21);
            this.PlatformSelector.TabIndex = 10;
            // 
            // PlatformLabel
            // 
            this.PlatformLabel.AutoSize = true;
            this.PlatformLabel.Location = new System.Drawing.Point(8, 84);
            this.PlatformLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PlatformLabel.Name = "PlatformLabel";
            this.PlatformLabel.Size = new System.Drawing.Size(48, 13);
            this.PlatformLabel.TabIndex = 9;
            this.PlatformLabel.Text = "Platform:";
            // 
            // AddOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 188);
            this.Controls.Add(this.PlatformSelector);
            this.Controls.Add(this.PlatformLabel);
            this.Controls.Add(this.CitySelector);
            this.Controls.Add(this.CityLabel);
            this.Controls.Add(this.AddLastButton);
            this.Controls.Add(this.AddFirstButton);
            this.Controls.Add(this.OperationSelector);
            this.Controls.Add(this.OperationLabel);
            this.Controls.Add(this.EmailInput);
            this.Controls.Add(this.NameInput);
            this.Controls.Add(this.SurnameInput);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AddOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddOrderForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Common.Controls.NamedInput SurnameInput;
        private Common.Controls.NamedInput NameInput;
        private Common.Controls.NamedInput EmailInput;
        private System.Windows.Forms.Label OperationLabel;
        private System.Windows.Forms.ComboBox OperationSelector;
        private System.Windows.Forms.Button AddFirstButton;
        private System.Windows.Forms.Button AddLastButton;
        private System.Windows.Forms.ComboBox CitySelector;
        private System.Windows.Forms.Label CityLabel;
        private System.Windows.Forms.ComboBox PlatformSelector;
        private System.Windows.Forms.Label PlatformLabel;
    }
}
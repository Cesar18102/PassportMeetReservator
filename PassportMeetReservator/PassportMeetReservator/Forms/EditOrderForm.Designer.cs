namespace PassportMeetReservator.Forms
{
    partial class EditOrderForm
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
            this.EmailInput = new PassportMeetReservator.Controls.NamedInput();
            this.NameInput = new PassportMeetReservator.Controls.NamedInput();
            this.SurnameInput = new PassportMeetReservator.Controls.NamedInput();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CitySelector = new System.Windows.Forms.ComboBox();
            this.CityLabel = new System.Windows.Forms.Label();
            this.OperationSelector = new System.Windows.Forms.ComboBox();
            this.OperationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // EmailInput
            // 
            this.EmailInput.Editable = true;
            this.EmailInput.InputLeft = 112;
            this.EmailInput.InputSize = new System.Drawing.Size(625, 20);
            this.EmailInput.InputText = "";
            this.EmailInput.LabelLeft = 0;
            this.EmailInput.LabelText = "Email:";
            this.EmailInput.Location = new System.Drawing.Point(12, 58);
            this.EmailInput.Margin = new System.Windows.Forms.Padding(2);
            this.EmailInput.Name = "EmailInput";
            this.EmailInput.Size = new System.Drawing.Size(725, 19);
            this.EmailInput.TabIndex = 9;
            this.EmailInput.Text = "namedInput1";
            this.EmailInput.TopPosition = 0;
            // 
            // NameInput
            // 
            this.NameInput.Editable = true;
            this.NameInput.InputLeft = 112;
            this.NameInput.InputSize = new System.Drawing.Size(625, 20);
            this.NameInput.InputText = "";
            this.NameInput.LabelLeft = 0;
            this.NameInput.LabelText = "Name:";
            this.NameInput.Location = new System.Drawing.Point(12, 34);
            this.NameInput.Margin = new System.Windows.Forms.Padding(2);
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(725, 19);
            this.NameInput.TabIndex = 8;
            this.NameInput.Text = "namedInput1";
            this.NameInput.TopPosition = 0;
            // 
            // SurnameInput
            // 
            this.SurnameInput.Editable = true;
            this.SurnameInput.InputLeft = 112;
            this.SurnameInput.InputSize = new System.Drawing.Size(625, 20);
            this.SurnameInput.InputText = "";
            this.SurnameInput.LabelLeft = 0;
            this.SurnameInput.LabelText = "Surname:";
            this.SurnameInput.Location = new System.Drawing.Point(12, 11);
            this.SurnameInput.Margin = new System.Windows.Forms.Padding(2);
            this.SurnameInput.Name = "SurnameInput";
            this.SurnameInput.Size = new System.Drawing.Size(725, 19);
            this.SurnameInput.TabIndex = 7;
            this.SurnameInput.Text = "namedInput1";
            this.SurnameInput.TopPosition = 0;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(12, 132);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(726, 23);
            this.SaveButton.TabIndex = 12;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CitySelector
            // 
            this.CitySelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CitySelector.FormattingEnabled = true;
            this.CitySelector.Location = new System.Drawing.Point(123, 81);
            this.CitySelector.Margin = new System.Windows.Forms.Padding(2);
            this.CitySelector.Name = "CitySelector";
            this.CitySelector.Size = new System.Drawing.Size(614, 21);
            this.CitySelector.TabIndex = 16;
            // 
            // CityLabel
            // 
            this.CityLabel.AutoSize = true;
            this.CityLabel.Location = new System.Drawing.Point(9, 84);
            this.CityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CityLabel.Name = "CityLabel";
            this.CityLabel.Size = new System.Drawing.Size(27, 13);
            this.CityLabel.TabIndex = 15;
            this.CityLabel.Text = "City:";
            // 
            // OperationSelector
            // 
            this.OperationSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OperationSelector.FormattingEnabled = true;
            this.OperationSelector.Location = new System.Drawing.Point(123, 106);
            this.OperationSelector.Margin = new System.Windows.Forms.Padding(2);
            this.OperationSelector.Name = "OperationSelector";
            this.OperationSelector.Size = new System.Drawing.Size(614, 21);
            this.OperationSelector.TabIndex = 14;
            // 
            // OperationLabel
            // 
            this.OperationLabel.AutoSize = true;
            this.OperationLabel.Location = new System.Drawing.Point(10, 109);
            this.OperationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OperationLabel.Name = "OperationLabel";
            this.OperationLabel.Size = new System.Drawing.Size(56, 13);
            this.OperationLabel.TabIndex = 13;
            this.OperationLabel.Text = "Operation:";
            // 
            // EditOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 162);
            this.Controls.Add(this.CitySelector);
            this.Controls.Add(this.CityLabel);
            this.Controls.Add(this.OperationSelector);
            this.Controls.Add(this.OperationLabel);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.EmailInput);
            this.Controls.Add(this.NameInput);
            this.Controls.Add(this.SurnameInput);
            this.Name = "EditOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EditOrderForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Controls.NamedInput EmailInput;
        private Controls.NamedInput NameInput;
        private Controls.NamedInput SurnameInput;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.ComboBox CitySelector;
        private System.Windows.Forms.Label CityLabel;
        private System.Windows.Forms.ComboBox OperationSelector;
        private System.Windows.Forms.Label OperationLabel;
    }
}
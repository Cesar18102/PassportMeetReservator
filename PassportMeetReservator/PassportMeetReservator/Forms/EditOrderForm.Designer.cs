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
            this.OrderTypeSelector = new System.Windows.Forms.ComboBox();
            this.OrderTypeLabel = new System.Windows.Forms.Label();
            this.EmailInput = new PassportMeetReservator.Controls.NamedInput();
            this.NameInput = new PassportMeetReservator.Controls.NamedInput();
            this.SurnameInput = new PassportMeetReservator.Controls.NamedInput();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OrderTypeSelector
            // 
            this.OrderTypeSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OrderTypeSelector.FormattingEnabled = true;
            this.OrderTypeSelector.Items.AddRange(new object[] {
            "PASZPORTY - Składanie wniosków o paszport",
            "PASZPORTY - Odbiór paszportu",
            "CUDZOZIEMCY - Odbiór karty pobytu",
            "CUDZOZIEMCY - Złożenie wniosku: pobyt czasowy / stały / rezydenta UE, wydanie / w" +
                "ymiana karty pobytu, świadczenie pieniężne dla posiadaczy Karty Polaka",
            "CUDZOZIEMCY - Złożenie odcisków palców (do wniosków przesłanych pocztą / złożonyc" +
                "h w delegaturach)",
            "CUDZOZIEMCY - Uzyskanie stempla (pieczątki) w paszporcie (wyłącznie wnioski ze st" +
                "atusem pozytywna weryfikacja formalna)",
            "Obywatelstwo polskie"});
            this.OrderTypeSelector.Location = new System.Drawing.Point(124, 81);
            this.OrderTypeSelector.Margin = new System.Windows.Forms.Padding(2);
            this.OrderTypeSelector.Name = "OrderTypeSelector";
            this.OrderTypeSelector.Size = new System.Drawing.Size(176, 21);
            this.OrderTypeSelector.TabIndex = 11;
            // 
            // OrderTypeLabel
            // 
            this.OrderTypeLabel.AutoSize = true;
            this.OrderTypeLabel.Location = new System.Drawing.Point(11, 84);
            this.OrderTypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OrderTypeLabel.Name = "OrderTypeLabel";
            this.OrderTypeLabel.Size = new System.Drawing.Size(34, 13);
            this.OrderTypeLabel.TabIndex = 10;
            this.OrderTypeLabel.Text = "Type:";
            // 
            // EmailInput
            // 
            this.EmailInput.Editable = true;
            this.EmailInput.InputLeft = 112;
            this.EmailInput.InputSize = new System.Drawing.Size(174, 20);
            this.EmailInput.InputText = "";
            this.EmailInput.LabelLeft = 0;
            this.EmailInput.LabelText = "Email:";
            this.EmailInput.Location = new System.Drawing.Point(12, 58);
            this.EmailInput.Margin = new System.Windows.Forms.Padding(2);
            this.EmailInput.Name = "EmailInput";
            this.EmailInput.Size = new System.Drawing.Size(287, 19);
            this.EmailInput.TabIndex = 9;
            this.EmailInput.Text = "namedInput1";
            this.EmailInput.TopPosition = 0;
            // 
            // NameInput
            // 
            this.NameInput.Editable = true;
            this.NameInput.InputLeft = 112;
            this.NameInput.InputSize = new System.Drawing.Size(174, 20);
            this.NameInput.InputText = "";
            this.NameInput.LabelLeft = 0;
            this.NameInput.LabelText = "Name:";
            this.NameInput.Location = new System.Drawing.Point(12, 34);
            this.NameInput.Margin = new System.Windows.Forms.Padding(2);
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(287, 19);
            this.NameInput.TabIndex = 8;
            this.NameInput.Text = "namedInput1";
            this.NameInput.TopPosition = 0;
            // 
            // SurnameInput
            // 
            this.SurnameInput.Editable = true;
            this.SurnameInput.InputLeft = 112;
            this.SurnameInput.InputSize = new System.Drawing.Size(174, 20);
            this.SurnameInput.InputText = "";
            this.SurnameInput.LabelLeft = 0;
            this.SurnameInput.LabelText = "Surname:";
            this.SurnameInput.Location = new System.Drawing.Point(12, 11);
            this.SurnameInput.Margin = new System.Windows.Forms.Padding(2);
            this.SurnameInput.Name = "SurnameInput";
            this.SurnameInput.Size = new System.Drawing.Size(287, 19);
            this.SurnameInput.TabIndex = 7;
            this.SurnameInput.Text = "namedInput1";
            this.SurnameInput.TopPosition = 0;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(12, 107);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(288, 23);
            this.SaveButton.TabIndex = 12;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // EditOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 136);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.OrderTypeSelector);
            this.Controls.Add(this.OrderTypeLabel);
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

        private System.Windows.Forms.ComboBox OrderTypeSelector;
        private System.Windows.Forms.Label OrderTypeLabel;
        private Controls.NamedInput EmailInput;
        private Controls.NamedInput NameInput;
        private Controls.NamedInput SurnameInput;
        private System.Windows.Forms.Button SaveButton;
    }
}
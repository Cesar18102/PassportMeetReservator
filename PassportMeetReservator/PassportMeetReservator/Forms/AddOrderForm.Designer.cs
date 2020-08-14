namespace PassportMeetReservator.Forms
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
            this.SurnameInput = new PassportMeetReservator.Controls.NamedInput();
            this.NameInput = new PassportMeetReservator.Controls.NamedInput();
            this.EmailInput = new PassportMeetReservator.Controls.NamedInput();
            this.OrderTypeLabel = new System.Windows.Forms.Label();
            this.OrderTypeSelector = new System.Windows.Forms.ComboBox();
            this.AddOrderButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SurnameInput
            // 
            this.SurnameInput.Editable = true;
            this.SurnameInput.InputLeft = 150;
            this.SurnameInput.InputSize = new System.Drawing.Size(230, 22);
            this.SurnameInput.InputText = "";
            this.SurnameInput.LabelLeft = 0;
            this.SurnameInput.LabelText = "Surname:";
            this.SurnameInput.Location = new System.Drawing.Point(13, 13);
            this.SurnameInput.Name = "SurnameInput";
            this.SurnameInput.Size = new System.Drawing.Size(383, 23);
            this.SurnameInput.TabIndex = 0;
            this.SurnameInput.Text = "namedInput1";
            this.SurnameInput.TopPosition = 0;
            // 
            // NameInput
            // 
            this.NameInput.Editable = true;
            this.NameInput.InputLeft = 150;
            this.NameInput.InputSize = new System.Drawing.Size(230, 22);
            this.NameInput.InputText = "";
            this.NameInput.LabelLeft = 0;
            this.NameInput.LabelText = "Name:";
            this.NameInput.Location = new System.Drawing.Point(13, 42);
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(383, 23);
            this.NameInput.TabIndex = 1;
            this.NameInput.Text = "namedInput1";
            this.NameInput.TopPosition = 0;
            // 
            // EmailInput
            // 
            this.EmailInput.Editable = true;
            this.EmailInput.InputLeft = 150;
            this.EmailInput.InputSize = new System.Drawing.Size(230, 22);
            this.EmailInput.InputText = "";
            this.EmailInput.LabelLeft = 0;
            this.EmailInput.LabelText = "Email:";
            this.EmailInput.Location = new System.Drawing.Point(13, 71);
            this.EmailInput.Name = "EmailInput";
            this.EmailInput.Size = new System.Drawing.Size(383, 23);
            this.EmailInput.TabIndex = 2;
            this.EmailInput.Text = "namedInput1";
            this.EmailInput.TopPosition = 0;
            // 
            // OrderTypeLabel
            // 
            this.OrderTypeLabel.AutoSize = true;
            this.OrderTypeLabel.Location = new System.Drawing.Point(12, 103);
            this.OrderTypeLabel.Name = "OrderTypeLabel";
            this.OrderTypeLabel.Size = new System.Drawing.Size(44, 17);
            this.OrderTypeLabel.TabIndex = 3;
            this.OrderTypeLabel.Text = "Type:";
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
            this.OrderTypeSelector.Location = new System.Drawing.Point(163, 100);
            this.OrderTypeSelector.Name = "OrderTypeSelector";
            this.OrderTypeSelector.Size = new System.Drawing.Size(233, 24);
            this.OrderTypeSelector.TabIndex = 4;
            // 
            // AddOrderButton
            // 
            this.AddOrderButton.Location = new System.Drawing.Point(12, 131);
            this.AddOrderButton.Name = "AddOrderButton";
            this.AddOrderButton.Size = new System.Drawing.Size(384, 23);
            this.AddOrderButton.TabIndex = 5;
            this.AddOrderButton.Text = "Add Order";
            this.AddOrderButton.UseVisualStyleBackColor = true;
            this.AddOrderButton.Click += new System.EventHandler(this.AddOrderButton_Click);
            // 
            // AddOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 162);
            this.Controls.Add(this.AddOrderButton);
            this.Controls.Add(this.OrderTypeSelector);
            this.Controls.Add(this.OrderTypeLabel);
            this.Controls.Add(this.EmailInput);
            this.Controls.Add(this.NameInput);
            this.Controls.Add(this.SurnameInput);
            this.Name = "AddOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddOrderForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.NamedInput SurnameInput;
        private Controls.NamedInput NameInput;
        private Controls.NamedInput EmailInput;
        private System.Windows.Forms.Label OrderTypeLabel;
        private System.Windows.Forms.ComboBox OrderTypeSelector;
        private System.Windows.Forms.Button AddOrderButton;
    }
}
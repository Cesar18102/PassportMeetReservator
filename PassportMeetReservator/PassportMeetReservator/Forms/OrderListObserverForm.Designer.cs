namespace PassportMeetReservator.Forms
{
    partial class OrderListObserverForm
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
            this.OrderListWrapper = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // OrderListWrapper
            // 
            this.OrderListWrapper.Location = new System.Drawing.Point(12, 12);
            this.OrderListWrapper.Name = "OrderListWrapper";
            this.OrderListWrapper.Size = new System.Drawing.Size(549, 426);
            this.OrderListWrapper.TabIndex = 0;
            // 
            // OrderListObserverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 450);
            this.Controls.Add(this.OrderListWrapper);
            this.Name = "OrderListObserverForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order List Observer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel OrderListWrapper;
    }
}
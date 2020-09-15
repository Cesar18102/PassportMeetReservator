namespace PassportMeetReservator.Forms
{
    partial class OrderListForm
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
            this.OrderListWrapper.AutoScroll = true;
            this.OrderListWrapper.Location = new System.Drawing.Point(9, 10);
            this.OrderListWrapper.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OrderListWrapper.Name = "OrderListWrapper";
            this.OrderListWrapper.Size = new System.Drawing.Size(567, 346);
            this.OrderListWrapper.TabIndex = 0;
            // 
            // OrderListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 366);
            this.Controls.Add(this.OrderListWrapper);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "OrderListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order List Observer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel OrderListWrapper;
    }
}
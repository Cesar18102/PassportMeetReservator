using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace PassportMeetReservator.Controls
{
    public class NamedInput : Control
    {
        private Label Label = new Label() { AutoSize = true }; 
        private TextBox Input = new TextBox();

        public NamedInput()
        {
            Label.SizeChanged += (sender, e) => UpdateLabelTop();
            Input.SizeChanged += (sender, e) => UpdateLabelTop();

            Controls.Add(Label);
            Controls.Add(Input);

            this.GotFocus += (sender, e) => Input.Focus();
        }

        [Category("CustomLayout")]
        public int TopPosition
        {
            get => Input.Top;
            set
            {
                Input.Top = value;
                UpdateLabelTop();
            }
        }

        [Category("CustomLayout")]
        public int LabelLeft
        {
            get => Label.Left;
            set => Label.Left = value;
        }

        [Category("CustomLayout")]
        public int InputLeft
        {
            get => Input.Left;
            set => Input.Left = value;
        }

        [Category("CustomLayout")]
        public Size InputSize
        {
            get => Input.Size;
            set => Input.Size = value;
        }

        [Category("CustomData")]
        public string InputText
        {
            get => Input.Text;
            set => Input.Text = value;
        }

        [Category("CustomData")]
        public string LabelText
        {
            get => Label.Text;
            set => Label.Text = value;
        }

        [Category("CustomInteraction")]
        public bool Editable
        {
            get => !Input.ReadOnly;
            set => Input.ReadOnly = !value;
        }

        private void UpdateLabelTop()
        {
            Label.Top = Input.Top + (Input.Height - Label.Height) / 2;
        }
    }
}

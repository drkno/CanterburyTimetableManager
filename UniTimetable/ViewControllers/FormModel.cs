using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UniTimetable
{
    public partial class FormModel : Form
    {
        public FormModel()
        {
            InitializeComponent();
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                labelTitle.Text = value;
            }
        }

        private static readonly Pen OutlinePen = new Pen(Color.Red, 2);
        private void ModelFormPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(OutlinePen, 1, 1, Width - 2, Height - 2);
        }

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void ModelFormMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(Handle, 0xA1, 0x2, 0);
        }
    }
}

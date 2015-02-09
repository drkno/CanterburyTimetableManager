#region

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace UniTimetable.ViewControllers
{
    // EtchedLine control, modified from original version at
    // http://www.differentpla.net/content/2005/02/wizard-csharp1
    public partial class EtchedLine : UserControl
    {
        private Color DarkColor_ = SystemColors.ControlDark;
        private Color LightColor_ = SystemColors.ControlLightLight;

        public EtchedLine()
        {
            InitializeComponent();

            // don't allow control to receive focus
            SetStyle(ControlStyles.Selectable, false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawLine(new Pen(DarkColor_), 0, 0, Width, 0);
            e.Graphics.DrawLine(new Pen(LightColor_), 0, 1, Width, 1);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        #region Properties

        [Category("Appearance")]
        private Color DarkColor
        {
            get { return DarkColor_; }
            set
            {
                DarkColor_ = value;
                Refresh();
            }
        }

        [Category("Appearance")]
        private Color LightColor
        {
            get { return LightColor_; }
            set
            {
                LightColor_ = value;
                Refresh();
            }
        }

        #endregion
    }
}
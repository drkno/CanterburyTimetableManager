#region

using System.Drawing;
using System.Windows.Forms;

#endregion

namespace UniTimetable.ViewControllers
{
    // Adapted from Flicker Free ListBox by Les Potter
    // http://yacsharpblog.blogspot.com/2008/07/listbox-flicker.html
    // 
    // Note: First item and selected item (if any) in list seem
    // to still flicker if the listbox has focus. This even
    // happens when the OnPaint override is left empty.
    public class ListBoxBuffered : ListBox
    {
        private DrawMode DrawMode_ = DrawMode.Normal;

        public ListBoxBuffered()
        {
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);
            base.DrawMode = DrawMode.OwnerDrawFixed;
        }

        public override DrawMode DrawMode
        {
            get { return DrawMode_; }
            set
            {
                if (value != DrawMode.OwnerDrawVariable)
                    DrawMode_ = value;
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (DrawMode_ == DrawMode.Normal)
            {
                if (Items.Count > 0)
                {
                    e.DrawBackground();
                    e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, new SolidBrush(ForeColor),
                        new PointF(e.Bounds.X, e.Bounds.Y));
                }
            }
            else
            {
                base.OnDrawItem(e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Region region = new Region(e.ClipRectangle);
            Graphics g = e.Graphics;
            g.FillRegion(new SolidBrush(BackColor), region);

            for (int i = 0; i < Items.Count; i++)
            {
                Rectangle rect = GetItemRectangle(i);
                if (e.ClipRectangle.IntersectsWith(rect))
                {
                    bool selected = (SelectionMode == SelectionMode.One && SelectedIndex == i)
                                    || (SelectionMode == SelectionMode.MultiSimple && SelectedIndices.Contains(i))
                                    || (SelectionMode == SelectionMode.MultiExtended && SelectedIndices.Contains(i));
                    OnDrawItem(new DrawItemEventArgs(
                        g, Font, rect, i,
                        selected ? DrawItemState.Selected : DrawItemState.Default,
                        ForeColor, BackColor));
                }
                region.Complement(rect);
            }

            base.OnPaint(e);
        }
    }
}
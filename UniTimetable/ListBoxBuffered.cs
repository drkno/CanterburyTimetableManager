using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UniTimetable
{
    // Adapted from Flicker Free ListBox by Les Potter
    // http://yacsharpblog.blogspot.com/2008/07/listbox-flicker.html
    // 
    // Note: First item and selected item (if any) in list seem
    // to still flicker if the listbox has focus. This even
    // happens when the OnPaint override is left empty.
    public class ListBoxBuffered : ListBox
    {
        DrawMode DrawMode_ = DrawMode.Normal;

        public ListBoxBuffered()
            : base()
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
            get
            {
                return DrawMode_;
            }
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
                if (this.Items.Count > 0)
                {
                    e.DrawBackground();
                    e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, new SolidBrush(this.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));
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
            g.FillRegion(new SolidBrush(this.BackColor), region);

            for (int i = 0; i < this.Items.Count; i++)
            {
                Rectangle rect = GetItemRectangle(i);
                if (e.ClipRectangle.IntersectsWith(rect))
                {
                    bool selected = (this.SelectionMode == SelectionMode.One && this.SelectedIndex == i)
                        || (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i))
                        || (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i));
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

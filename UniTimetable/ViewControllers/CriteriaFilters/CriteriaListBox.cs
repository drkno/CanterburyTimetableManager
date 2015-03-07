using System.Drawing;
using System.Windows.Forms;
using UniTimetable.Model.Solver;

namespace UniTimetable.ViewControllers.CriteriaFilters
{
    public class CriteriaListBox : ListBox
    {
        private readonly Font _headingFont;
        private readonly Font _subheadingFont;
        private const int DrawMargin = 1;
        private const int HeadingLeftMargin = 5;
        private const int HeadingTopMargin = 5;
        private const int ItemNumberRightMargin = 3;
        private const int ItemNumberTopMargin = 3;
        private const int SubheadingLeftMargin = 20;
        private const int SubheadingTopMargin = 25;
        private const float HeadingFontSize = 10f;
        private const float SubheadingFontSize = 8f;
        private const string FontFamily = "Arial";
        private readonly Brush _headingBrush = Brushes.Black;
        private readonly Brush _subheadingBrush = Brushes.Black;
        private readonly Brush _itemNumberBrush = Brushes.Black;
        private readonly Pen _borderPen = Pens.Gray;

        public bool ShowItemNumber { get; set; }
        public CriteriaFilter CriteriaOrFilter { get; set; }

        public CriteriaListBox()
        {
            _headingFont = new Font(FontFamily, HeadingFontSize, FontStyle.Bold);
            _subheadingFont = new Font(FontFamily, SubheadingFontSize, FontStyle.Regular);
        }

        public enum CriteriaFilter
        {
            None = 0,
            Critera = 1,
            Filter = 2
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (CriteriaOrFilter == CriteriaFilter.None || Items.Count == 0)
            {
                return;
            }

            string heading, subheading;
            if (CriteriaOrFilter == CriteriaFilter.Critera)
            {
                var criteria = (Solver.Criteria)Items[e.Index];
                heading = criteria.Field.ToString();
                subheading = "Preference: " + Solver.Criteria.FieldSpecificPreference(criteria);
            }
            else
            {
                var filter = (Solver.Filter)Items[e.Index];
                heading = filter.Field.ToString();
                subheading = (filter.Exclude ? "Must not" : "Must") + " be " +
                          Solver.Filter.FieldSpecificTest(filter) + " " +
                          filter.ValueToString();
            }

            var r = new Rectangle(e.Bounds.X + DrawMargin, e.Bounds.Y + DrawMargin, e.Bounds.Width - 2 * DrawMargin, e.Bounds.Height - 2 * DrawMargin);

            var format = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            DrawHeading(e.Graphics, r, format, heading);
            DrawSubheading(e.Graphics, r, format, subheading);
            if (ShowItemNumber)
            {
                DrawItemNumber(e.Graphics, r, format, e.Index + 1);
            }
            e.Graphics.DrawRectangle(_borderPen, r);
        }

        private void DrawHeading(Graphics g, Rectangle rect, StringFormat format, string heading)
        {
            
            var q = new Rectangle(rect.X + HeadingLeftMargin, rect.Y + HeadingTopMargin, rect.Width - HeadingLeftMargin, rect.Height - HeadingTopMargin);
            g.DrawString(heading, _headingFont, _headingBrush, q, format);
        }

        private void DrawSubheading(Graphics g, Rectangle rect, StringFormat format, string text)
        {
            var q = new Rectangle(rect.X + SubheadingLeftMargin, rect.Y + SubheadingTopMargin, rect.Width - SubheadingLeftMargin, rect.Height - SubheadingTopMargin);
            g.DrawString(text, _subheadingFont, _subheadingBrush, q, format);
        }

        private void DrawItemNumber(Graphics g, Rectangle rect, StringFormat format, int number)
        {
            format.Alignment = StringAlignment.Far;
            var q = new Rectangle(rect.X, rect.Y + ItemNumberTopMargin, rect.Width - ItemNumberRightMargin, rect.Height - ItemNumberTopMargin);
            g.DrawString(number.ToString(), _subheadingFont, _itemNumberBrush, q, format);
        }
    }
}

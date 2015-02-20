#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniTimetable.Model;
using UniTimetable.Model.Timetable;

#endregion

namespace UniTimetable.ViewControllers
{
    partial class FormStyle : Form
    {
        private readonly List<Color> ColorList_ = new List<Color>();
        private Timetable Timetable_;

        public FormStyle()
        {
            InitializeComponent();

            ddSchemes.Items.Clear();
            ddSchemes.Items.AddRange(ColorScheme.Schemes);
        }

        public DialogResult ShowDialog(Timetable timetable)
        {
            Timetable_ = timetable;
            return base.ShowDialog();
        }

        public new DialogResult ShowDialog()
        {
            throw new Exception("No input was provided.");
        }

        public void UpdateButton()
        {
            if (listBox1.SelectedIndex == -1)
            {
                btnColor.Enabled = false;
            }
            else
            {
                btnColor.Enabled = true;
            }
        }

        private void FormStyle_Load(object sender, EventArgs e)
        {
            // copy colors into color list and subject names to list box
            ColorList_.Clear();
            listBox1.Items.Clear();
            foreach (Subject subject in Timetable_.SubjectList)
            {
                ColorList_.Add(subject.Color);
                listBox1.Items.Add(subject);
            }
            ddSchemes.SelectedIndex = -1;
            UpdateButton();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButton();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.Cancel)
            {
                ColorList_[listBox1.SelectedIndex] = colorDialog1.Color;
                listBox1.Invalidate();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool changed = false;
            // copy the colours back
            for (int i = 0; i < ColorList_.Count; i++)
            {
                if (Timetable_.SubjectList[i].Color != ColorList_[i])
                {
                    changed = true;
                    Timetable_.SubjectList[i].Color = ColorList_[i];
                }
            }
            DialogResult = (changed ? DialogResult.OK : DialogResult.Cancel);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            const int margin = 2;

            e.DrawBackground();
            e.DrawFocusRectangle();

            Graphics g = e.Graphics;
            Subject subject = (Subject) listBox1.Items[e.Index];

            Rectangle r = new Rectangle(e.Bounds.X + margin, e.Bounds.Y + margin, e.Bounds.Width - 2*margin - 1,
                e.Bounds.Height - 2*margin - 1);
            g.FillRectangle(TimetableControl.LinearGradient(r.Location, r.Width, r.Height, ColorList_[e.Index]), r);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            g.DrawString(subject.ToString(), listBox1.Font, Brushes.Black, r, format);

            g.DrawRectangle(Pens.Black, r);
        }

        private void ddSchemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddSchemes.SelectedIndex == -1)
                return;
            ColorScheme scheme = (ColorScheme) ddSchemes.SelectedItem;
            LoadScheme(scheme);
        }

        private void LoadScheme(ColorScheme scheme)
        {
            int n = scheme.Colors.Count;
            for (int i = 0; i < ColorList_.Count; i++)
            {
                ColorList_[i] = scheme.Colors[i%n];
            }
            listBox1.Invalidate();
        }
    }
}
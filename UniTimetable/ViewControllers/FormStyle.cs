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
        private readonly List<Color> _colorList = new List<Color>();
        private Timetable _timetable;

        public FormStyle()
        {
            InitializeComponent();

            ddSchemes.Items.Clear();
            ddSchemes.Items.AddRange(ColorScheme.Schemes);
        }

        public DialogResult ShowDialog(Timetable timetable)
        {
            _timetable = timetable;
            return base.ShowDialog();
        }

        private void UpdateButton()
        {
            btnColor.Enabled = listBox1.SelectedIndex != -1;
        }

        private void FormStyleLoad(object sender, EventArgs e)
        {
            // copy colors into color list and subject names to list box
            _colorList.Clear();
            listBox1.Items.Clear();
            foreach (var subject in _timetable.SubjectList)
            {
                _colorList.Add(subject.Color);
                listBox1.Items.Add(subject);
            }
            ddSchemes.SelectedIndex = -1;
            UpdateButton();
        }

        private void ListBox1SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButton();
        }

        private void BtnColorClick(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel) return;
            _colorList[listBox1.SelectedIndex] = colorDialog1.Color;
            listBox1.Invalidate();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            bool changed = false;
            // copy the colours back
            for (int i = 0; i < _colorList.Count; i++)
            {
                if (_timetable.SubjectList[i].Color != _colorList[i])
                {
                    changed = true;
                    _timetable.SubjectList[i].Color = _colorList[i];
                }
            }
            DialogResult = (changed ? DialogResult.OK : DialogResult.Cancel);
            Close();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ListBox1DrawItem(object sender, DrawItemEventArgs e)
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
            g.FillRectangle(TimetableControl.LinearGradient(r.Location, r.Width, r.Height, _colorList[e.Index]), r);

            var format = new StringFormat
                                  {
                                      Alignment = StringAlignment.Center,
                                      LineAlignment = StringAlignment.Center
                                  };
            g.DrawString(subject.ToString(), listBox1.Font, Brushes.Black, r, format);

            g.DrawRectangle(Pens.Black, r);
        }

        private void DdSchemesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddSchemes.SelectedIndex == -1)
                return;
            ColorScheme scheme = (ColorScheme) ddSchemes.SelectedItem;
            LoadScheme(scheme);
        }

        private void LoadScheme(ColorScheme scheme)
        {
            int n = scheme.Colours.Count;
            for (int i = 0; i < _colorList.Count; i++)
            {
                _colorList[i] = scheme.Colours[i%n];
            }
            listBox1.Invalidate();
        }
    }
}
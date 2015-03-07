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
        private readonly List<Color> _colourList = new List<Color>();
        private Timetable _timetable;

        public FormStyle()
        {
            InitializeComponent();

            colourSchemes.Items.Clear();
            colourSchemes.Items.AddRange(ColorScheme.Schemes);
        }

        public DialogResult ShowDialog(Timetable timetable)
        {
            _timetable = timetable;
            return base.ShowDialog();
        }

        private void UpdateButton()
        {
            buttonColour.Enabled = listBoxColours.SelectedIndex != -1;
        }

        private void FormStyleLoad(object sender, EventArgs e)
        {
            // copy colors into color list and subject names to list box
            _colourList.Clear();
            listBoxColours.Items.Clear();
            foreach (var subject in _timetable.SubjectList)
            {
                _colourList.Add(subject.Color);
                listBoxColours.Items.Add(subject);
            }
            colourSchemes.SelectedIndex = -1;
            UpdateButton();
        }

        private void ListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButton();
        }

        private void ButtonColorClick(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.Cancel) return;
            _colourList[listBoxColours.SelectedIndex] = colorDialog.Color;
            listBoxColours.Invalidate();
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            var changed = false;
            // copy the colours back
            for (var i = 0; i < _colourList.Count; i++)
            {
                if (_timetable.SubjectList[i].Color == _colourList[i]) continue;
                changed = true;
                _timetable.SubjectList[i].Color = _colourList[i];
            }
            DialogResult = (changed ? DialogResult.OK : DialogResult.Cancel);
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            const int margin = 2;

            e.DrawBackground();
            e.DrawFocusRectangle();

            var g = e.Graphics;
            var subject = (Subject) listBoxColours.Items[e.Index];

            var r = new Rectangle(e.Bounds.X + margin, e.Bounds.Y + margin, e.Bounds.Width - 2*margin - 1,
                e.Bounds.Height - 2*margin - 1);
            g.FillRectangle(TimetableControl.LinearGradient(r.Location, r.Width, r.Height, _colourList[e.Index]), r);

            var format = new StringFormat
                                  {
                                      Alignment = StringAlignment.Center,
                                      LineAlignment = StringAlignment.Center
                                  };
            g.DrawString(subject.ToString(), listBoxColours.Font, Brushes.Black, r, format);

            g.DrawRectangle(Pens.Black, r);
        }

        private void DdSchemesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (colourSchemes.SelectedIndex == -1)
                return;
            var scheme = (ColorScheme) colourSchemes.SelectedItem;
            LoadScheme(scheme);
        }

        private void LoadScheme(ColorScheme scheme)
        {
            var n = scheme.Colours.Count;
            for (var i = 0; i < _colourList.Count; i++)
            {
                _colourList[i] = scheme.Colours[i%n];
            }
            listBoxColours.Invalidate();
        }
    }
}
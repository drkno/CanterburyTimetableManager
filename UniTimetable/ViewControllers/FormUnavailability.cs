#region

using System;
using System.Windows.Forms;
using UniTimetable.Model.Time;
using UniTimetable.Model.Timetable;

#endregion

namespace UniTimetable.ViewControllers
{
    partial class FormUnavailability : Form
    {
        private int _earliest = 8;
        private int _latest = 21;
        private Timetable _timetable;
        private Unavailability _unavail;

        public FormUnavailability()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(Timetable timetable, Unavailability unavail, int earliest, int latest)
        {
            _timetable = timetable;
            _unavail = unavail;
            _earliest = earliest;
            _latest = latest;
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(Timetable timetable, Timeslot timeslot, int earliest, int latest)
        {
            return ShowDialog(timetable, new Unavailability("", timeslot), earliest, latest);
        }

        private void FormUnavailabilityLoad(object sender, EventArgs e)
        {
            // if we're editing
            if (_unavail != null)
            {
                // copy the data out from the timeslot into the inputs
                textBoxName.Text = _unavail.Name;
                dateTimePickerStart.Value = dateTimePickerStart.Value.AddMinutes(_unavail.StartTotalMinutes);
                dateTimePickerEnd.Value = dateTimePickerEnd.Value.AddMinutes(_unavail.EndTotalMinutes);
            }
            else
            {
                // reset inputs to default/blank
                textBoxName.Clear();
                var now = DateTime.Now;
                dateTimePickerStart.Value = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
                dateTimePickerEnd.Value = dateTimePickerStart.Value;
            }
            textBoxName.Focus();
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            // take focus away from time pickers
            buttonOK.Focus();

            // get start and end times from input
            var start = TimeOfDay.FromDateTime(dateTimePickerStart.Value);
            var end = TimeOfDay.FromDateTime(dateTimePickerEnd.Value);
            // if it starts before it ends
            if (start >= end)
            {
                MessageBox.Show("Start time must be before end time.", "Unavailable Timeslot", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            // if it's not within the scope of the timetable
            if (end <= new TimeOfDay(_earliest, 0))
            {
                MessageBox.Show("End time must be after " + _earliest + "am.", "Unavailable Timeslot", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                dateTimePickerEnd.Focus();
                return;
            }
            if (start >= new TimeOfDay(_latest, 0))
            {
                MessageBox.Show("Start time must be before " + _latest + "pm.", "Unavailable Timeslot", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                dateTimePickerStart.Focus();
                return;
            }

            // clip the timeslot to be within the bounds of the timetable
            if (start < new TimeOfDay(_earliest, 0))
                start = new TimeOfDay(_earliest, 0);
            if (end > new TimeOfDay(_latest, 0))
                end = new TimeOfDay(_latest, 0);

            // build new unavailability from input
            var unavail = new Unavailability(textBoxName.Text, (int)dateTimePickerStart.Value.DayOfWeek,
                dateTimePickerStart.Value.DayOfYear, start.Hour, start.Minute, end.Hour, end.Minute);

            if (_unavail != null && _unavail.EquivalentTo(unavail) && _unavail.Name == unavail.Name)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            // if editing, remove the old timeslot
            if (_unavail != null)
                _timetable.UnavailableList.Remove(_unavail);

            // if it doesn't fit in the current timetable
            if (!_timetable.FreeDuring(unavail, true))
            {
                MessageBox.Show("Selected time slot is currently occupied.", "Unavailable Timeslot",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                // if editing, add the old timeslot back in
                if (_unavail != null)
                    _timetable.UnavailableList.Add(_unavail);
                return;
            }

            // insert the edited timeslot
            _timetable.UnavailableList.Add(unavail);

            // recompute solutions
            _timetable.RecomputeSolutions = true;
            // return
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
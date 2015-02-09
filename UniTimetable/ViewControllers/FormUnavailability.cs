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
        private int Earliest_ = 8;
        private int Latest_ = 21;
        private Timetable Timetable_;
        private Unavailability Unavail_;

        public FormUnavailability()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(Timetable timetable, Unavailability unavail, int earliest, int latest)
        {
            Timetable_ = timetable;
            Unavail_ = unavail;
            Earliest_ = earliest;
            Latest_ = latest;
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(Timetable timetable, Timeslot timeslot, int earliest, int latest)
        {
            return ShowDialog(timetable, new Unavailability("", timeslot), earliest, latest);
        }

        public new DialogResult ShowDialog()
        {
            throw new Exception("No input was provided.");
        }

        private void FormUnavailability_Load(object sender, EventArgs e)
        {
            // if we're editing
            if (Unavail_ != null)
            {
                // copy the data out from the timeslot into the inputs
                txtName.Text = Unavail_.Name;
                ddDay.SelectedIndex = Unavail_.Day;
                timeStart.Value = timeStart.Value.Date.AddMinutes(Unavail_.StartTotalMinutes);
                timeEnd.Value = timeEnd.Value.Date.AddMinutes(Unavail_.EndTotalMinutes);
            }
            else
            {
                // reset inputs to default/blank
                txtName.Clear();
                ddDay.SelectedIndex = -1;
                timeStart.Value = timeStart.MinDate.Date.AddHours(9);
                timeEnd.Value = timeEnd.MinDate.Date.AddHours(17);
            }
            txtName.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // take focus away from time pickers
            btnOK.Focus();

            // if no day is selected
            if (ddDay.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a day.", "Unavailable Timeslot", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                ddDay.Focus();
                return;
            }

            // get start and end times from input
            TimeOfDay start = TimeOfDay.FromDateTime(timeStart.Value);
            TimeOfDay end = TimeOfDay.FromDateTime(timeEnd.Value);
            // if it starts before it ends
            if (start >= end)
            {
                MessageBox.Show("Start time must be before end time.", "Unavailable Timeslot", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            // if it's not within the scope of the timetable
            if (end <= new TimeOfDay(Earliest_, 0))
            {
                MessageBox.Show("End time must be after 8am.", "Unavailable Timeslot", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                timeEnd.Focus();
                return;
            }
            if (start >= new TimeOfDay(Latest_, 0))
            {
                MessageBox.Show("Start time must be before 9pm.", "Unavailable Timeslot", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                timeStart.Focus();
                return;
            }

            // clip the timeslot to be within the bounds of the timetable
            if (start < new TimeOfDay(Earliest_, 0))
                start = new TimeOfDay(Earliest_, 0);
            if (end > new TimeOfDay(Latest_, 0))
                end = new TimeOfDay(Latest_, 0);

            // build new unavailability from input
            Unavailability unavail = new Unavailability(txtName.Text, ddDay.SelectedIndex, start.Hour, start.Minute,
                end.Hour, end.Minute);

            if (Unavail_ != null && Unavail_.EquivalentTo(unavail) && Unavail_.Name == unavail.Name)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            // if editing, remove the old timeslot
            if (Unavail_ != null)
                Timetable_.UnavailableList.Remove(Unavail_);

            // if it doesn't fit in the current timetable
            if (!Timetable_.FreeDuring(unavail, true))
            {
                MessageBox.Show("Selected time slot is currently occupied.", "Unavailable Timeslot",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                // if editing, add the old timeslot back in
                if (Unavail_ != null)
                    Timetable_.UnavailableList.Add(Unavail_);
                return;
            }

            // insert the edited timeslot
            Timetable_.UnavailableList.Add(unavail);

            // recompute solutions
            Timetable_.RecomputeSolutions = true;
            // return
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
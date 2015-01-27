#region

using System;
using System.Windows.Forms;
using UniTimetable.Model;

#endregion

namespace UniTimetable.ViewControllers
{
    public partial class FormSettings : Form
    {
        private FormMain _form;
        private Settings _settings;

        public FormSettings()
        {
            InitializeComponent();
            for (var i = 0; i <= 23; i++)
            {
                var hr = i%24;
                var time = (hr < 12 ? "am" : "pm");
                hr = i%12;
                time = (hr == 0 ? 12 : hr) + time;

                ddStart.Items.Add(time);
                ddEnd.Items.Add(time);
            }
        }

        private void FormSettingsLoad(object sender, EventArgs e)
        {
            cbGhost.Checked = _settings.ShowGhost;
            cbWeekend.Checked = _settings.ShowWeekend;
            cbGray.Checked = _settings.ShowGray;
            cbLocation.Checked = _settings.ShowLocation;
            ddStart.SelectedIndex = _settings.HourStart;
            ddEnd.SelectedIndex = _settings.HourEnd;
            cbReset.Checked = _settings.ResetWindow;
        }

        public Settings ShowDialog(Settings settings, FormMain formMain)
        {
            _form = formMain;
            _settings = settings;
            if (base.ShowDialog() != DialogResult.OK)
                return null;
            return new Settings(
                cbGhost.Checked,
                cbWeekend.Checked,
                cbGray.Checked,
                cbLocation.Checked,
                ddStart.SelectedIndex,
                ddEnd.SelectedIndex,
                cbReset.Checked);
        }

        public new DialogResult ShowDialog()
        {
            throw new Exception("No input was provided.");
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (ddStart.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a start time!", "Settings", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                ddStart.Focus();
                return;
            }
            if (ddEnd.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an end time!", "Settings", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                ddEnd.Focus();
                return;
            }
            if (ddStart.SelectedIndex >= ddEnd.SelectedIndex)
            {
                MessageBox.Show("Start time must be before end time!", "Settings", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonColoursClick(object sender, EventArgs e)
        {
            if (_form == null || _form.Timetable == null)
                return;

            var formStyle = new FormStyle();
            if (formStyle.ShowDialog(_form.Timetable) == DialogResult.Cancel)
                return;

            _form.MadeChanges(false);
        }
    }
}
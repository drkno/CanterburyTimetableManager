#region

using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using UniTimetable.Model;
using UniTimetable.ViewControllers.CriteriaFilters;

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
        }

        private void FormSettingsLoad(object sender, EventArgs e)
        {
            checkBoxImportUnsettable.Checked = _settings.ImportUnselectable;
            checkBoxGhost.Checked = _settings.ShowGhost;
            checkBoxWeekend.Checked = _settings.ShowWeekend;
            checkBoxGray.Checked = _settings.ShowGray;
            checkBoxLocation.Checked = _settings.ShowLocation;
            ddStart.SelectedIndex = _settings.HourStart;
            ddEnd.SelectedIndex = _settings.HourEnd;
            checkBoxReset.Checked = _settings.ResetWindow;
        }

        public Settings ShowDialog(Settings settings, FormMain formMain)
        {
            _form = formMain;
            _settings = settings;
            if (base.ShowDialog() != DialogResult.OK)
            {
                return null;
            }
            return new Settings(
                checkBoxImportUnsettable.Checked,
                checkBoxGhost.Checked,
                checkBoxWeekend.Checked,
                checkBoxGray.Checked,
                checkBoxLocation.Checked,
                ddStart.SelectedIndex,
                ddEnd.SelectedIndex,
                checkBoxReset.Checked);
        }

        private void ButtonOkClick(object sender, EventArgs e)
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

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonColoursClick(object sender, EventArgs e)
        {
            if (_form == null || _form.Timetable == null)
            {
                MessageBox.Show("Please import a timetable before setting colours.", "Colours",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var formStyle = new FormColours();
            if (formStyle.ShowDialog(_form.Timetable) == DialogResult.Cancel)
                return;

            _form.MadeChanges(false);
        }

        private void ButtonCriteriaClick(object sender, EventArgs e)
        {
            if (_form == null || _form.Timetable == null)
            {
                MessageBox.Show("Please import a timetable before setting solver criteria.", "Criteria",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var formCriteria = new FormCriteria();
            if (formCriteria.ShowDialog(_form.Solver) != DialogResult.OK)
            {
                return;
            }
            _form.Timetable.RecomputeSolutions = true;
        }
    }
}
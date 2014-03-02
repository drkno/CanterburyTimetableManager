using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UniTimetable.Properties;

namespace UniTimetable
{
    public partial class FormSettings : Form
    {
        Settings Settings_ = null;

        public FormSettings()
        {
            InitializeComponent();
            for (int i = 0; i <= 23; i++)
            {
                int hr = i % 24;
                string time = (hr < 12 ? "am" : "pm");
                hr = i % 12;
                time = (hr == 0 ? 12 : hr).ToString() + time;

                ddStart.Items.Add(time);
                ddEnd.Items.Add(time);
            }
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            cbLarge.Checked = Settings_.UseLargeIcons;
            cbGhost.Checked = Settings_.ShowGhost;
            cbWeekend.Checked = Settings_.ShowWeekend;
            cbGray.Checked = Settings_.ShowGray;
            cbLocation.Checked = Settings_.ShowLocation;
            ddStart.SelectedIndex = Settings_.HourStart;
            ddEnd.SelectedIndex = Settings_.HourEnd;
            cbReset.Checked = Settings_.ResetWindow;
        }

        public Settings ShowDialog(Settings settings)
        {
            Settings_ = settings;
            if (base.ShowDialog() != DialogResult.OK)
                return null;
            return new Settings(
                cbLarge.Checked,
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ddStart.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a start time!", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ddStart.Focus();
                return;
            }
            if (ddEnd.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an end time!", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ddEnd.Focus();
                return;
            }
            if (ddStart.SelectedIndex >= ddEnd.SelectedIndex)
            {
                MessageBox.Show("Start time must be before end time!", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

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
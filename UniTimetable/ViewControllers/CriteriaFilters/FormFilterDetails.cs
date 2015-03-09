#region

using System;
using System.Windows.Forms;
using UniTimetable.Model.Solver;
using UniTimetable.Model.Time;

#endregion

namespace UniTimetable.ViewControllers.CriteriaFilters
{
    partial class FormFilterDetails : Form
    {
        private Solver.Filter Filter_;

        public FormFilterDetails()
        {
            InitializeComponent();
            ddField.Items.AddRange(Solver.Fields);
        }

        public Solver.Filter ShowDialog(Solver.Filter filter)
        {
            if (filter == null)
            {
                Filter_ = null;
                Text = "Add Filter";
                btnRevert.Enabled = false;
            }
            else
            {
                Filter_ = filter.DeepCopy();
                Text = "Edit Filter";
                btnRevert.Enabled = true;
            }

            LoadFromFilter();

            DialogResult result = base.ShowDialog();
            if (result == DialogResult.OK)
                return Filter_;
            return null;
        }

        public new DialogResult ShowDialog()
        {
            throw new Exception("No input was provided.");
        }

        private void LoadFromFilter()
        {
            if (Filter_ == null)
            {
                ddField.SelectedIndex = -1;
                ddExclude.SelectedIndex = -1;
                UpdateTestItems();

                ShowValueControls();
            }
            else
            {
                ddField.SelectedIndex = (int) Filter_.FieldIndex;
                ddExclude.SelectedIndex = Filter_.Exclude ? 1 : 0;
                UpdateTestItems();
                ddTest.SelectedIndex = (int) Filter_.Test;

                ShowValueControls();
                LoadValue();
            }
        }

        private void ddField_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowValueControls();
            UpdateTestItems();
        }

        private void UpdateTestItems()
        {
            int index = ddTest.SelectedIndex;
            ddTest.Items.Clear();
            if (ddField.SelectedIndex == -1)
                return;
            Solver.Field field = Solver.Fields[ddField.SelectedIndex];
            if (field.Type == Solver.FieldType.Unknown)
                return;
            ddTest.Items.AddRange(Solver.Filter.FieldSpecificTests(field.Type));
            ddTest.SelectedIndex = index;
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            LoadFromFilter();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ddField.SelectedIndex == -1)
            {
                MessageBox.Show("Please select which criteria the filter applies to.", "Filter Details",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ddField.Focus();
                return;
            }
            if (ddExclude.SelectedIndex == -1)
            {
                MessageBox.Show("Please select the filter type.", "Filter Details", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                ddExclude.Focus();
                return;
            }
            if (ddTest.SelectedIndex == -1)
            {
                MessageBox.Show("Please select the filter test.", "Filter Details", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                ddTest.Focus();
                return;
            }

            int value = ParseValue();
            if (value == -1)
                return;

            Filter_ = new Solver.Filter(
                ddExclude.SelectedIndex == 1,
                (Solver.FieldIndex) ddField.SelectedIndex,
                value,
                (Solver.FilterTest) ddTest.SelectedIndex);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #region Show/load/parse different value types

        private void ShowValueControls()
        {
            if (ddField.SelectedIndex == -1)
            {
                // hide all
                timeOfDayPicker.Visible = false;
                txtTimeLength.Visible = false;
                txtInteger.Visible = false;
                return;
            }
            switch (Solver.Fields[ddField.SelectedIndex].Type)
            {
                case Solver.FieldType.Int:
                    // hide TimeOfDay
                    timeOfDayPicker.Visible = false;
                    // hide TimeLength
                    txtTimeLength.Visible = false;
                    // show Int
                    txtInteger.Visible = true;
                    break;

                case Solver.FieldType.TimeLength:
                    // hide Int
                    txtInteger.Visible = false;
                    // hide TimeOfDay
                    timeOfDayPicker.Visible = false;
                    // show TimeLength
                    txtTimeLength.Visible = true;
                    break;

                case Solver.FieldType.TimeOfDay:
                    // hide Int
                    txtInteger.Visible = false;
                    // hide TimeLength
                    txtTimeLength.Visible = false;
                    // show TimeOfDay
                    timeOfDayPicker.Visible = true;
                    break;

                default:
                    // bugger it, hide all
                    timeOfDayPicker.Visible = false;
                    txtTimeLength.Visible = false;
                    txtInteger.Visible = false;
                    break;
            }
        }

        private void LoadValue()
        {
            switch (Solver.Fields[ddField.SelectedIndex].Type)
            {
                case Solver.FieldType.Int:
                    txtInteger.Text = Filter_.ValueAsInt.ToString();
                    break;

                case Solver.FieldType.TimeLength:
                    TimeLength timeLength = Filter_.ValueAsTimeLength;
                    string text = timeLength.Hours.ToString("00") + timeLength.Minutes.ToString("00");
                    txtTimeLength.Text = text;
                    break;

                case Solver.FieldType.TimeOfDay:
                    TimeOfDay timeOfDay = Filter_.ValueAsTimeOfDay;
                    timeOfDayPicker.Value = timeOfDayPicker.MinDate.Date.AddMinutes(timeOfDay.DayMinutes);
                    break;

                default:
                    break;
            }
        }

        private int ParseValue()
        {
            int value;
            switch (Solver.Fields[ddField.SelectedIndex].Type)
            {
                case Solver.FieldType.Int:
                    try
                    {
                        value = Convert.ToInt32(txtInteger.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid value specified.", "Filter Details", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        txtInteger.Focus();
                        return -1;
                    }
                    break;

                case Solver.FieldType.TimeLength:
                    string input;
                    int hour, min;

                    input = txtTimeLength.Text.Substring(0, 2);
                    input = input.Replace(' ', '0');
                    try
                    {
                        hour = Convert.ToInt32(input);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid hour value specified.", "Filter Details", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        txtTimeLength.Focus();
                        return -1;
                    }

                    input = txtTimeLength.Text.Substring("00 hr ".Length, 2);
                    if (input == "  ")
                    {
                        min = 0;
                    }
                    else
                    {
                        try
                        {
                            min = Convert.ToInt32(input);
                        }
                        catch
                        {
                            MessageBox.Show("Invalid minute value specified.", "Filter Details", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                            txtTimeLength.Focus();
                            return -1;
                        }
                    }

                    value = hour*60 + min;
                    break;

                case Solver.FieldType.TimeOfDay:
                    value = (int) timeOfDayPicker.Value.TimeOfDay.TotalMinutes;
                    break;

                default:
                    return -1;
            }
            return value;
        }

        #endregion
    }
}
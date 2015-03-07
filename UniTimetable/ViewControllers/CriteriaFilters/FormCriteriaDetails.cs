#region

using System;
using System.Windows.Forms;
using UniTimetable.Model.Solver;

#endregion

namespace UniTimetable.ViewControllers.CriteriaFilters
{
    public partial class FormCriteriaDetails : Form
    {
        private Solver.Criteria Criteria_;

        public FormCriteriaDetails()
        {
            InitializeComponent();
            ddField.Items.AddRange(Solver.Fields);
        }

        public Solver.Criteria ShowDialog(Solver.Criteria criteria)
        {
            if (criteria == null)
            {
                Criteria_ = null;
                Text = "Add Criteria";
                btnRevert.Enabled = false;
            }
            else
            {
                Criteria_ = criteria.DeepCopy();
                Text = "Edit Criteria";
                btnRevert.Enabled = true;
            }

            LoadFromCriteria();

            DialogResult result = base.ShowDialog();
            if (result == DialogResult.OK)
                return Criteria_;
            return null;
        }

        public new DialogResult ShowDialog()
        {
            throw new Exception("No input was provided.");
        }

        private void LoadFromCriteria()
        {
            if (Criteria_ == null)
            {
                ddField.SelectedIndex = -1;
                UpdatePreferenceItems();
            }
            else
            {
                ddField.SelectedIndex = (int) Criteria_.FieldIndex;
                UpdatePreferenceItems();
                ddPreference.SelectedIndex = (int) Criteria_.Preference;
            }
        }

        private void ddField_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePreferenceItems();
            if (ddField.SelectedIndex != -1)
            {
                ddPreference.SelectedIndex = (int) Solver.Fields[ddField.SelectedIndex].DefaultPreference;
            }
        }

        private void UpdatePreferenceItems()
        {
            int index = ddPreference.SelectedIndex;
            ddPreference.Items.Clear();
            if (ddField.SelectedIndex == -1)
                return;
            Solver.Field field = Solver.Fields[ddField.SelectedIndex];
            if (field.Type == Solver.FieldType.Unknown)
                return;
            ddPreference.Items.AddRange(Solver.Criteria.FieldSpecificPreferences(field.Type));
            ddPreference.SelectedIndex = index;
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            LoadFromCriteria();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ddField.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a criteria.", "Criteria Details", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                ddField.Focus();
                return;
            }
            if (ddPreference.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a preference.", "Criteria Details", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                ddPreference.Focus();
                return;
            }

            Criteria_ = new Solver.Criteria(
                (Solver.FieldIndex) ddField.SelectedIndex,
                (Solver.Preference) ddPreference.SelectedIndex);

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
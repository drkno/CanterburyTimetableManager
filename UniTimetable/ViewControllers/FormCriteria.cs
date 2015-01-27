#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniTimetable.Model.Solver;

#endregion

namespace UniTimetable.ViewControllers
{
    public partial class FormCriteria : Form
    {
        private Solver.Criteria DragCriteria_;
        private int DragTarget_;
        private Solver Solver_;

        public FormCriteria()
        {
            InitializeComponent();

            ddPresets.Items.Clear();
            ddPresets.Items.AddRange(Solver.Presets);
        }

        private void ddPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ddPresets.SelectedIndex;
            if (index == -1)
                return;

            Solver.Preset preset = Solver.Presets[index];
            LoadLists(preset.Criteria, preset.Filters);
        }

        #region Opening form

        public DialogResult ShowDialog(Solver solver)
        {
            Solver_ = solver;
            return base.ShowDialog();
        }

        public new DialogResult ShowDialog()
        {
            throw new Exception("No input was provided.");
        }

        private void FormCriteria_Load(object sender, EventArgs e)
        {
            LoadLists(Solver_);
            ddPresets.SelectedIndex = -1;
        }

        private void LoadLists(Solver solver)
        {
            LoadLists(solver.Comparer.Criteria, solver.Filters);
        }

        private void LoadLists(IEnumerable<Solver.Criteria> criteria, IEnumerable<Solver.Filter> filters)
        {
            listBoxCriteria.Items.Clear();
            foreach (Solver.Criteria c in criteria)
                listBoxCriteria.Items.Add(c.DeepCopy());

            listBoxFilters.Items.Clear();
            foreach (Solver.Filter f in filters)
                listBoxFilters.Items.Add(f.DeepCopy());

            UpdateCriteriaButtons();
            UpdateFilterButtons();
        }

        #endregion

        #region Criteria list

        private void listBoxCriteria_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (listBoxCriteria.Items.Count == 0)
                return;

            Graphics g = e.Graphics;
            Solver.Criteria criteria = (Solver.Criteria) listBoxCriteria.Items[e.Index];

            const int margin = 2;
            Rectangle r = new Rectangle(e.Bounds.X + margin, e.Bounds.Y + margin, e.Bounds.Width - 2*margin,
                e.Bounds.Height - 2*margin);

            Font font;
            Rectangle q;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Near;

            /*const int numTop = 2;
            font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
            q = new Rectangle(r.X, r.Y + numTop, r.Width, r.Height - numTop);
            g.DrawString((e.Index + 1).ToString() + ".", font, Brushes.Black, q, format);*/

            // heading: name
            const int nameLeft = 5, nameTop = 5;
            font = new Font("Microsoft Sans Serif", 11f, FontStyle.Bold);
            q = new Rectangle(r.X + nameLeft, r.Y + nameTop, r.Width - nameLeft, r.Height - nameTop);
            g.DrawString(criteria.Field.ToString(), font, Brushes.Black, q, format);

            // subheading: preference
            const int prefLeft = 20, prefTop = 25;
            font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
            q = new Rectangle(r.X + prefLeft, r.Y + prefTop, r.Width - prefLeft, r.Height - prefTop);
            string text = "Preference: " + Solver.Criteria.FieldSpecificPreference(criteria);
            g.DrawString(text, font, Brushes.Black, q, format);

            // top right corner: criteria number
            const int numRight = 3, numTop = 3;
            format.Alignment = StringAlignment.Far;
            font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
            q = new Rectangle(r.X, r.Y + numTop, r.Width - numRight, r.Height - numTop);
            g.DrawString((e.Index + 1).ToString(), font, Brushes.Black, q, format);

            g.DrawRectangle(Pens.Black, r);
        }

        private void listBoxCriteria_MouseDown(object sender, MouseEventArgs e)
        {
            int index = listBoxCriteria.IndexFromPoint(e.Location);
            if (index == -1)
            {
                listBoxCriteria.SelectedIndex = -1;
                return;
            }

            if (e.Clicks == 2 && e.Button == MouseButtons.Left)
            {
                EditCriteria();
                return;
            }

            Solver.Criteria criteria = (Solver.Criteria) listBoxCriteria.Items[index];
            DragTarget_ = -1;
            DragCriteria_ = null;
            // drag drop failed
            if (listBoxCriteria.DoDragDrop(criteria, DragDropEffects.Move) != DragDropEffects.Move)
                return;
            // some sort of conflict
            if (DragTarget_ == -1 || DragCriteria_ != criteria)
                return;
            // moving back to same position
            if (DragTarget_ == index)
                return;
            if (DragTarget_ > index)
            {
                listBoxCriteria.Items.Insert(DragTarget_ + 1, criteria);
                listBoxCriteria.Items.RemoveAt(index);
            }
            else
            {
                listBoxCriteria.Items.RemoveAt(index);
                listBoxCriteria.Items.Insert(DragTarget_, criteria);
            }
            listBoxCriteria.SelectedIndex = DragTarget_;

            // not preset anymore
            ddPresets.SelectedIndex = -1;
        }

        private void listBoxCriteria_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof (Solver.Criteria)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listBoxCriteria_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof (Solver.Criteria)))
                return;

            Point location = listBoxCriteria.PointToClient(new Point(e.X, e.Y));
            DragTarget_ = listBoxCriteria.IndexFromPoint(location);
            DragCriteria_ = (Solver.Criteria) e.Data.GetData(typeof (Solver.Criteria));
        }

        private void listBoxCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                RemoveCriteria();
                // not preset anymore
                ddPresets.SelectedIndex = -1;
            }
        }

        private void listBoxCriteria_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBoxCriteria.IndexFromPoint(e.Location);
            if (index < 0)
                return;
            EditCriteria();
        }

        private void btnCriteriaAdd_Click(object sender, EventArgs e)
        {
            AddCriteria();
        }

        private void btnCriteriaEdit_Click(object sender, EventArgs e)
        {
            EditCriteria();
        }

        private void btnCriteriaRemove_Click(object sender, EventArgs e)
        {
            RemoveCriteria();
        }

        private void AddCriteria()
        {
            FormCriteriaDetails formDetails = new FormCriteriaDetails();
            Solver.Criteria criteria = formDetails.ShowDialog(null);
            if (criteria == null)
                return;
            int index = listBoxCriteria.SelectedIndex;
            if (index == -1)
                index = listBoxCriteria.Items.Count;
            listBoxCriteria.Items.Insert(index, criteria);

            ddPresets.SelectedIndex = -1;
        }

        private void EditCriteria()
        {
            int index = listBoxCriteria.SelectedIndex;
            if (index == -1)
                return;

            Solver.Criteria criteria = (Solver.Criteria) listBoxCriteria.SelectedItem;
            FormCriteriaDetails formDetails = new FormCriteriaDetails();
            criteria = formDetails.ShowDialog(criteria);
            if (criteria == null)
                return;
            listBoxCriteria.Items.RemoveAt(index);
            listBoxCriteria.Items.Insert(index, criteria);
            listBoxCriteria.SelectedIndex = index;

            ddPresets.SelectedIndex = -1;
        }

        private void RemoveCriteria()
        {
            if (listBoxCriteria.SelectedIndex == -1)
                return;
            int index = listBoxCriteria.SelectedIndex;
            listBoxCriteria.Items.RemoveAt(index);
            listBoxCriteria.SelectedIndex = Math.Min(index, listBoxCriteria.Items.Count - 1);
            UpdateCriteriaButtons();

            ddPresets.SelectedIndex = -1;
        }

        private void UpdateCriteriaButtons()
        {
            bool enabled = (listBoxCriteria.Items.Count > 0);
            btnCriteriaEdit.Enabled = enabled;
            btnCriteriaRemove.Enabled = enabled;
        }

        #endregion

        #region Filter list

        private void listBoxFilters_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (listBoxFilters.Items.Count == 0)
                return;

            Graphics g = e.Graphics;
            Solver.Filter filter = (Solver.Filter) listBoxFilters.Items[e.Index];

            const int margin = 2;
            Rectangle r = new Rectangle(e.Bounds.X + margin, e.Bounds.Y + margin, e.Bounds.Width - 2*margin,
                e.Bounds.Height - 2*margin);

            Font font;
            Rectangle q;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Near;

            // header: field name
            const int nameLeft = 5, nameTop = 5;
            font = new Font("Microsoft Sans Serif", 11f, FontStyle.Bold);
            q = new Rectangle(r.X + nameLeft, r.Y + nameTop, r.Width - nameLeft, r.Height - nameTop);
            g.DrawString(filter.Field.ToString(), font, Brushes.Black, q, format);

            // subheading: details of filter
            const int specLeft = 20, specTop = 25;
            font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
            q = new Rectangle(r.X + specLeft, r.Y + specTop, r.Width - specLeft, r.Height - specTop);
            string text = (filter.Exclude ? "Must not" : "Must") + " be " +
                          Solver.Filter.FieldSpecificTest(filter) + " " +
                          filter.ValueToString();
            g.DrawString(text, font, Brushes.Black, q, format);

            g.DrawRectangle(Pens.Black, r);
        }

        private void listBoxFilters_MouseDown(object sender, MouseEventArgs e)
        {
            // didn't click on an item, so deselect
            int index = listBoxFilters.IndexFromPoint(e.Location);
            if (index == -1)
            {
                listBoxFilters.SelectedIndex = -1;
            }
        }

        private void listBoxFilters_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBoxFilters.IndexFromPoint(e.Location);
            if (index < 0)
                return;
            EditFilter();
        }

        private void listBoxFilters_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                RemoveFilter();
            }
        }

        private void btnFiltersAdd_Click(object sender, EventArgs e)
        {
            AddFilter();
        }

        private void btnFiltersEdit_Click(object sender, EventArgs e)
        {
            EditFilter();
        }

        private void btnFiltersRemove_Click(object sender, EventArgs e)
        {
            RemoveFilter();
        }

        private void AddFilter()
        {
            FormFilterDetails formDetails = new FormFilterDetails();
            Solver.Filter filter = formDetails.ShowDialog(null);
            if (filter == null)
                return;
            /*int index = listBoxFilters.SelectedIndex;
            if (index == -1)
                index = listBoxFilters.Items.Count;
            listBoxFilters.Items.Insert(index, filter);*/
            listBoxFilters.Items.Add(filter);
            listBoxFilters.SelectedIndex = listBoxFilters.Items.Count - 1;

            // not preset anymore
            ddPresets.SelectedIndex = -1;
        }

        private void EditFilter()
        {
            int index = listBoxFilters.SelectedIndex;
            if (index == -1)
                return;

            Solver.Filter filter = (Solver.Filter) listBoxFilters.SelectedItem;
            FormFilterDetails formDetails = new FormFilterDetails();
            filter = formDetails.ShowDialog(filter);
            if (filter == null)
                return;
            listBoxFilters.Items.RemoveAt(index);
            listBoxFilters.Items.Insert(index, filter);
            listBoxFilters.SelectedIndex = index;

            // not preset anymore
            ddPresets.SelectedIndex = -1;
        }

        private void RemoveFilter()
        {
            if (listBoxFilters.SelectedIndex == -1)
                return;
            int index = listBoxFilters.SelectedIndex;
            listBoxFilters.Items.RemoveAt(index);
            listBoxFilters.SelectedIndex = Math.Min(index, listBoxFilters.Items.Count - 1);
            UpdateFilterButtons();

            // not preset anymore
            ddPresets.SelectedIndex = -1;
        }

        private void UpdateFilterButtons()
        {
            bool enabled = (listBoxFilters.Items.Count > 0);
            btnFiltersEdit.Enabled = enabled;
            btnFiltersRemove.Enabled = enabled;
        }

        #endregion

        #region Main buttons

        private void btnRevert_Click(object sender, EventArgs e)
        {
            LoadLists(Solver_);
            // not preset anymore
            ddPresets.SelectedIndex = -1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // check if there were any changes
            bool changed = false;
            if (Solver_.Comparer.Criteria.Count == listBoxCriteria.Items.Count)
            {
                for (int i = 0; i < Solver_.Comparer.Criteria.Count; i++)
                {
                    Solver.Criteria criteria = Solver_.Comparer.Criteria[i];
                    Solver.Criteria other = (Solver.Criteria) listBoxCriteria.Items[i];
                    if (criteria.FieldIndex != other.FieldIndex || criteria.Preference != other.Preference)
                    {
                        changed = false;
                        break;
                    }
                }
            }
            else
            {
                changed = true;
            }
            if (!changed && Solver_.Filters.Count == listBoxFilters.Items.Count)
            {
                for (int i = 0; i < Solver_.Filters.Count; i++)
                {
                    Solver.Filter filter = Solver_.Filters[i];
                    Solver.Filter other = (Solver.Filter) listBoxFilters.Items[i];
                    if (filter.FieldIndex == other.FieldIndex
                        && filter.Exclude == other.Exclude
                        && filter.Test == other.Test
                        && filter.ValueAsInt == other.ValueAsInt)
                    {
                        changed = false;
                        break;
                    }
                }
            }
            else
            {
                changed = true;
            }

            if (!changed)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            // copy changes across
            Solver_.Comparer.Criteria.Clear();
            foreach (Solver.Criteria criteria in listBoxCriteria.Items)
                Solver_.Comparer.Criteria.Add(criteria);

            Solver_.Filters.Clear();
            foreach (Solver.Filter filter in listBoxFilters.Items)
                Solver_.Filters.Add(filter);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion
    }
}
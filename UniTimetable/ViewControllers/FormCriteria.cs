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
        private Solver.Criteria _dragCriteria;
        private int _dragTarget;
        private Solver _solver;

        public FormCriteria()
        {
            InitializeComponent();

            comboBoxPresets.Items.Clear();
            comboBoxPresets.Items.AddRange(Solver.Presets);
        }

        private void ComboBoxPresetsSelectedIndexChanged(object sender, EventArgs e)
        {
            var index = comboBoxPresets.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            var preset = Solver.Presets[index];
            LoadLists(preset.Criteria, preset.Filters);
        }

        #region Opening form

        public DialogResult ShowDialog(Solver solver)
        {
            _solver = solver;
            return base.ShowDialog();
        }

        private void FormCriteriaLoad(object sender, EventArgs e)
        {
            LoadLists(_solver);
            comboBoxPresets.SelectedIndex = -1;
        }

        private void LoadLists(Solver solver)
        {
            LoadLists(solver.Comparer.Criteria, solver.Filters);
        }

        private void LoadLists(IEnumerable<Solver.Criteria> criteria, IEnumerable<Solver.Filter> filters)
        {
            listBoxCriteria.Items.Clear();
            foreach (var c in criteria)
            {
                listBoxCriteria.Items.Add(c.DeepCopy());
            }

            listBoxFilters.Items.Clear();
            foreach (var f in filters)
            {
                listBoxFilters.Items.Add(f.DeepCopy());
            }

            UpdateCriteriaButtons();
            UpdateFilterButtons();
        }

        #endregion

        #region Criteria list

        private void ListBoxCriteriaDrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (listBoxCriteria.Items.Count == 0)
                return;

            var g = e.Graphics;
            var criteria = (Solver.Criteria) listBoxCriteria.Items[e.Index];

            const int margin = 2;
            var r = new Rectangle(e.Bounds.X + margin, e.Bounds.Y + margin, e.Bounds.Width - 2*margin,
                e.Bounds.Height - 2*margin);

            var format = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near};

            // heading: name
            const int nameLeft = 5, nameTop = 5;
            var font = new Font("Microsoft Sans Serif", 11f, FontStyle.Bold);
            var q = new Rectangle(r.X + nameLeft, r.Y + nameTop, r.Width - nameLeft, r.Height - nameTop);
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

        private void ListBoxCriteriaMouseDown(object sender, MouseEventArgs e)
        {
            var index = listBoxCriteria.IndexFromPoint(e.Location);
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

            var criteria = (Solver.Criteria) listBoxCriteria.Items[index];
            _dragTarget = -1;
            _dragCriteria = null;
            // drag drop failed
            if (listBoxCriteria.DoDragDrop(criteria, DragDropEffects.Move) != DragDropEffects.Move)
            {
                return;
            }
            // some sort of conflict
            if (_dragTarget == -1 || _dragCriteria != criteria)
            {
                return;
            }
            // moving back to same position
            if (_dragTarget == index)
            {
                return;
            }
            if (_dragTarget > index)
            {
                listBoxCriteria.Items.Insert(_dragTarget + 1, criteria);
                listBoxCriteria.Items.RemoveAt(index);
            }
            else
            {
                listBoxCriteria.Items.RemoveAt(index);
                listBoxCriteria.Items.Insert(_dragTarget, criteria);
            }
            listBoxCriteria.SelectedIndex = _dragTarget;

            // not preset anymore
            comboBoxPresets.SelectedIndex = -1;
        }

        private void ListBoxCriteriaDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(typeof (Solver.Criteria)) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void ListBoxCriteriaDragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof (Solver.Criteria)))
            {
                return;
            }

            var location = listBoxCriteria.PointToClient(new Point(e.X, e.Y));
            _dragTarget = listBoxCriteria.IndexFromPoint(location);
            _dragCriteria = (Solver.Criteria) e.Data.GetData(typeof (Solver.Criteria));
        }

        private void ListBoxCriteriaKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete) return;
            RemoveCriteria();
            // not preset anymore
            comboBoxPresets.SelectedIndex = -1;
        }

        private void ListBoxCriteriaMouseDoubleClick(object sender, MouseEventArgs e)
        {
            var index = listBoxCriteria.IndexFromPoint(e.Location);
            if (index < 0)
            {
                return;
            }
            EditCriteria();
        }

        private void ButtonCriteriaAddClick(object sender, EventArgs e)
        {
            AddCriteria();
        }

        private void ButtonCriteriaEditClick(object sender, EventArgs e)
        {
            EditCriteria();
        }

        private void ButtonCriteriaRemoveClick(object sender, EventArgs e)
        {
            RemoveCriteria();
        }

        private void AddCriteria()
        {
            var formDetails = new FormCriteriaDetails();
            var criteria = formDetails.ShowDialog(null);
            if (criteria == null)
                return;
            var index = listBoxCriteria.SelectedIndex;
            if (index == -1)
                index = listBoxCriteria.Items.Count;
            listBoxCriteria.Items.Insert(index, criteria);

            comboBoxPresets.SelectedIndex = -1;
        }

        private void EditCriteria()
        {
            var index = listBoxCriteria.SelectedIndex;
            if (index == -1)
            {
                return;
            }

            var criteria = (Solver.Criteria) listBoxCriteria.SelectedItem;
            var formDetails = new FormCriteriaDetails();
            criteria = formDetails.ShowDialog(criteria);
            if (criteria == null)
            {
                return;
            }

            _solver.Comparer.Criteria[index] = criteria;

            listBoxCriteria.Items.RemoveAt(index);
            listBoxCriteria.Items.Insert(index, criteria);
            listBoxCriteria.SelectedIndex = index;

            comboBoxPresets.SelectedIndex = -1;
        }

        private void RemoveCriteria()
        {
            if (listBoxCriteria.SelectedIndex == -1)
                return;
            var index = listBoxCriteria.SelectedIndex;
            listBoxCriteria.Items.RemoveAt(index);
            listBoxCriteria.SelectedIndex = Math.Min(index, listBoxCriteria.Items.Count - 1);
            UpdateCriteriaButtons();

            comboBoxPresets.SelectedIndex = -1;
        }

        private void UpdateCriteriaButtons()
        {
            var enabled = listBoxCriteria.Items.Count > 0;
            buttonCriteriaEdit.Enabled = enabled;
            buttonCriteriaRemove.Enabled = enabled;
        }

        #endregion

        #region Filter list

        private void ListBoxFiltersDrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (listBoxFilters.Items.Count == 0)
            {
                return;
            }

            var g = e.Graphics;
            var filter = (Solver.Filter) listBoxFilters.Items[e.Index];

            const int margin = 2;
            var r = new Rectangle(e.Bounds.X + margin, e.Bounds.Y + margin, e.Bounds.Width - 2*margin,
                e.Bounds.Height - 2*margin);

            var format = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near};

            // header: field name
            const int nameLeft = 5, nameTop = 5;
            var font = new Font("Microsoft Sans Serif", 11f, FontStyle.Bold);
            var q = new Rectangle(r.X + nameLeft, r.Y + nameTop, r.Width - nameLeft, r.Height - nameTop);
            g.DrawString(filter.Field.ToString(), font, Brushes.Black, q, format);

            // subheading: details of filter
            const int specLeft = 20, specTop = 25;
            font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
            q = new Rectangle(r.X + specLeft, r.Y + specTop, r.Width - specLeft, r.Height - specTop);
            var text = (filter.Exclude ? "Must not" : "Must") + " be " +
                          Solver.Filter.FieldSpecificTest(filter) + " " +
                          filter.ValueToString();
            g.DrawString(text, font, Brushes.Black, q, format);

            g.DrawRectangle(Pens.Black, r);
        }

        private void ListBoxFiltersMouseDown(object sender, MouseEventArgs e)
        {
            // didn't click on an item, so deselect
            var index = listBoxFilters.IndexFromPoint(e.Location);
            if (index == -1)
            {
                listBoxFilters.SelectedIndex = -1;
            }
        }

        private void ListBoxFiltersMouseDoubleClick(object sender, MouseEventArgs e)
        {
            var index = listBoxFilters.IndexFromPoint(e.Location);
            if (index < 0)
                return;
            EditFilter();
        }

        private void ListBoxFiltersKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                RemoveFilter();
            }
        }

        private void ButtonFiltersAddClick(object sender, EventArgs e)
        {
            AddFilter();
        }

        private void ButtonFiltersEditClick(object sender, EventArgs e)
        {
            EditFilter();
        }

        private void ButtonFiltersRemoveClick(object sender, EventArgs e)
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
            comboBoxPresets.SelectedIndex = -1;
        }

        private void EditFilter()
        {
            var index = listBoxFilters.SelectedIndex;
            if (index == -1)
            {
                return;
            }

            var filter = (Solver.Filter) listBoxFilters.SelectedItem;
            var formDetails = new FormFilterDetails();
            filter = formDetails.ShowDialog(filter);
            if (filter == null)
            {
                return;
            }

            _solver.Filters[index] = filter;

            listBoxFilters.Items.RemoveAt(index);
            listBoxFilters.Items.Insert(index, filter);
            listBoxFilters.SelectedIndex = index;

            // not preset anymore
            comboBoxPresets.SelectedIndex = -1;
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
            comboBoxPresets.SelectedIndex = -1;
        }

        private void UpdateFilterButtons()
        {
            var enabled = (listBoxFilters.Items.Count > 0);
            buttonFiltersEdit.Enabled = enabled;
            buttonFiltersRemove.Enabled = enabled;
        }

        #endregion

        #region Main buttons

        private void ButtonRevertClick(object sender, EventArgs e)
        {
            LoadLists(_solver);
            // not preset anymore
            comboBoxPresets.SelectedIndex = -1;
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            // check if there were any changes
            var changed = false;
            if (_solver.Comparer.Criteria.Count == listBoxCriteria.Items.Count)
            {
                for (var i = 0; i < _solver.Comparer.Criteria.Count; i++)
                {
                    var criteria = _solver.Comparer.Criteria[i];
                    var other = (Solver.Criteria) listBoxCriteria.Items[i];
                    if (criteria.FieldIndex != other.FieldIndex || criteria.Preference != other.Preference)
                    {
                        break;
                    }
                }
            }
            else
            {
                changed = true;
            }
            if (!changed && _solver.Filters.Count == listBoxFilters.Items.Count)
            {
                for (var i = 0; i < _solver.Filters.Count; i++)
                {
                    var filter = _solver.Filters[i];
                    var other = (Solver.Filter) listBoxFilters.Items[i];
                    if (filter.FieldIndex == other.FieldIndex
                        && filter.Exclude == other.Exclude
                        && filter.Test == other.Test
                        && filter.ValueAsInt == other.ValueAsInt)
                    {
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
            _solver.Comparer.Criteria.Clear();
            foreach (Solver.Criteria criteria in listBoxCriteria.Items)
                _solver.Comparer.Criteria.Add(criteria);

            _solver.Filters.Clear();
            foreach (Solver.Filter filter in listBoxFilters.Items)
                _solver.Filters.Add(filter);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion
    }
}
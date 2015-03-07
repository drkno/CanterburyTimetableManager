#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniTimetable.Model.Solver;
using UniTimetable.Model.Timetable;

#endregion

namespace UniTimetable.ViewControllers
{
    partial class FormSolution : Form
    {
        private const string MarkText = "&Mark Solution";
        private const string UnmarkText = "Un&mark Solution";
        private readonly Color _solutionColour1 = Color.White;
        private readonly Color _solutionColour2 = Color.FromArgb(0xF0, 0xF0, 0xF0);
        private readonly Color _markColour = Color.Yellow;
        private ListViewItem[] _fullListBackup;
        private Color[] _originalColors;
        private Solver _solver;

        public FormSolution()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(Solver solver)
        {
            _solver = solver;
            return base.ShowDialog();
        }

        private void FormSolutionLoad(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            StartPosition = FormStartPosition.Manual;

            // rebuild list
            RebuildList();

            // disable buttons which can't be used yet
            buttonMark.Enabled = false;
            buttonUse.Enabled = false;
            SetMarkButton(false);
            checkBoxOnlyMarked.Checked = false;
        }

        /// <summary>
        ///     Update preview and information for a new selected solution
        /// </summary>
        private void ListViewSolutionsSelectedIndexChanged(object sender, EventArgs e)
        {
            // update preview panel
            UpdatePreview();
            // clear properties box
            listViewProperties.Items.Clear();
            // if nothing is selected, return
            if (listViewSolutions.SelectedIndices.Count == 0 || listViewSolutions.SelectedIndices[0] == -1)
            {
                buttonMark.Enabled = false;
                buttonUse.Enabled = false;
                SetMarkButton(false);
                return;
            }

            buttonMark.Enabled = true;
            buttonUse.Enabled = true;
            // set mark button text accordingly
            SetMarkButton(listViewSolutions.SelectedItems[0].BackColor == _markColour);

            // set the currently selected solution
            var solution = (Solver.Solution) listViewSolutions.SelectedItems[0].Tag;

            // a list of lists of streams selected for each subject
            var groupedStreams = new List<List<Stream>>();
            // an ordered list of subjects
            var orderedSubjects = new List<Subject>();
            // fill the lists
            foreach (var subject in _solver.Timetable.SubjectList)
            {
                groupedStreams.Add(new List<Stream>());
                orderedSubjects.Add(subject);
            }
            // sort the subjects
            orderedSubjects.Sort();
            //orderedSubjects.Sort(new IndexComparer(m_Timetable.SubjectList));
            // fill the grouped streams list
            foreach (var stream in solution.Streams)
            {
                groupedStreams[orderedSubjects.IndexOf(stream.Type.Subject)].Add(stream);
            }

            // properties list - for each property (field)
            for (var i = 0; i < Solver.Fields.Length; i++)
            {
                var item = new ListViewItem(new[]
                                                     {
                                                         // with the name of the field
                                                         Solver.Fields[i].Name,
                                                         // and the selected solution's value
                                                         solution.FieldValueToString((Solver.FieldIndex) i)
                                                     });

                listViewProperties.Items.Add(item);
            }
        }

        private void ButtonUseClick(object sender, EventArgs e)
        {
            // nothing selected
            if (listViewSolutions.SelectedIndices.Count <= 0 || listViewSolutions.SelectedIndices[0] == -1)
                return;

            // set the currently selected solution
            var solution = (Solver.Solution) listViewSolutions.SelectedItems[0].Tag;
            _solver.Timetable.LoadSolution(solution);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCriteriaClick(object sender, EventArgs e)
        {
            var formCriteria = new FormCriteria();
            // if the criteria dialog was shown and nothing changed, all good
            if (formCriteria.ShowDialog(_solver) != DialogResult.OK)
                return;

            _solver.Timetable.RecomputeSolutions = true;
            var formProgress = new FormProgress();
            formProgress.ShowDialog(_solver);

            Reset();
        }

        private void ButtonMarkClick(object sender, EventArgs e)
        {
            // nothing selected
            if (listViewSolutions.SelectedIndices.Count == 0 || listViewSolutions.SelectedIndices[0] == -1)
                return;
            // get the selected item
            var item = listViewSolutions.SelectedItems[0];

            // if we're only viewing marked
            if (checkBoxOnlyMarked.Checked)
            {
                // get the solution
                var solution = (Solver.Solution) item.Tag;
                // remove the item from the list
                listViewSolutions.Items.RemoveAt(listViewSolutions.SelectedIndices[0]);
                // search for the item in the backup list and unmark it
                for (var i = 0; i < _fullListBackup.Length; i++)
                {
                    if ((Solver.Solution) _fullListBackup[i].Tag != solution) continue;
                    _fullListBackup[i].ImageIndex = -1;
                    _fullListBackup[i].BackColor = _originalColors[i];
                    break;
                }
            }
            // if it is mark already
            if (item.BackColor == _markColour)
            {
                listViewSolutions.SelectedItems[0].ImageIndex = -1;
                listViewSolutions.SelectedItems[0].BackColor = _originalColors[listViewSolutions.SelectedIndices[0]];
                // set button text
                SetMarkButton(false);
            }
            else
            {
                item.ImageIndex = 0;
                item.BackColor = _markColour;
                // set button text
                SetMarkButton(true);
            }
            // reselect list view box
            listViewSolutions.Select();
        }

        private void RebuildList()
        {
            listViewSolutions.Items.Clear();
            UpdatePreview();
            listViewProperties.Items.Clear();
            listViewSolutions.Columns.Clear();
            _originalColors = new Color[_solver.Solutions.Count];

            var solutionNumberColumn = new ColumnHeader {Text = "#", Width = 25};
            listViewSolutions.Columns.Add(solutionNumberColumn);

            int i;
            for (i = 0; i < _solver.Comparer.Criteria.Count; i++)
            {
                var columnHeader = new ColumnHeader
                                            {
                                                Text = _solver.Comparer.Criteria[i].Field.Name,
                                                TextAlign = HorizontalAlignment.Center,
                                                Width = 100
                                            };
                listViewSolutions.Columns.Add(columnHeader);
            }

            // rebuild the list of solutions
            Solver.Solution prevSolution = null;
            var prevColor = _solutionColour1;
            i = 0;
            foreach (var solution in _solver.Solutions)
            {
                // make a list of data for each entry
                var data = new string[_solver.Comparer.Criteria.Count + 1];
                data[0] = i.ToString();
                // for each field we're using
                for (var j = 0; j < _solver.Comparer.Criteria.Count; j++)
                {
                    // get a string representing the value for that criteria in the solution
                    data[j + 1] = solution.FieldValueToString(_solver.Comparer.Criteria[j].FieldIndex);
                }
                // create the item
                var item = new ListViewItem(data) {Tag = solution, ImageIndex = -1};

                // make the first result white
                if (prevSolution == null)
                {
                    prevColor = _solutionColour1;
                }
                // else if the following solution is different (by the current criteria)
                else if (solution.CompareTo(prevSolution, _solver.Comparer) != 0)
                {
                    // switch colours
                    prevColor = prevColor == _solutionColour1 ? _solutionColour2 : _solutionColour1;
                }
                item.BackColor = prevColor;
                _originalColors[i++] = item.BackColor;

                // add the item to the list
                listViewSolutions.Items.Add(item);
                prevSolution = solution;
            }
        }

        private void UpdatePreview()
        {
            // nothing selected
            if (listViewSolutions.SelectedIndices.Count == 0 || listViewSolutions.SelectedIndices[0] == -1)
            {
                timetableControl.Timetable = null;
                return;
            }

            var solution = (Solver.Solution) listViewSolutions.SelectedItems[0].Tag;
            timetableControl.Timetable = _solver.Timetable.PreviewSolution(solution);
        }

        private void SetMarkButton(bool unmark)
        {
            buttonMark.Text = checkBoxOnlyMarked.Checked || unmark ? UnmarkText : MarkText;
        }

        private void CheckboxOnlyMarkedCheckedChanged(object sender, EventArgs e)
        {
            // checkbox just got checked
            if (checkBoxOnlyMarked.Checked)
            {
                // disable mark button
                buttonMark.Enabled = false;
                // back-up full list
                _fullListBackup = new ListViewItem[listViewSolutions.Items.Count];
                listViewSolutions.Items.CopyTo(_fullListBackup, 0);
                // start with fresh list
                listViewSolutions.Items.Clear();
                // add marked items to ListView
                foreach (var item in _fullListBackup.Where(item => item.BackColor == _markColour))
                {
                    // clone the item and add to the list
                    listViewSolutions.Items.Add((ListViewItem) item.Clone());
                }
                // now set the colours
                foreach (ListViewItem item in listViewSolutions.Items)
                {
                    item.BackColor = _solutionColour1;
                }
            }
            // checkbox just got unchecked
            else
            {
                // enable start button
                buttonMark.Enabled = true;
                // refer back to the backup
                listViewSolutions.Items.Clear();
                listViewSolutions.Items.AddRange(_fullListBackup);
                // restore selection
                listViewSolutions.Select();
            }
        }
    }
}
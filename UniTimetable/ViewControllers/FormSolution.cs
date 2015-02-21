#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniTimetable.Model.Solver;
using UniTimetable.Model.Timetable;

#endregion

namespace UniTimetable.ViewControllers
{
    partial class FormSolution : Form
    {
        private const string StarText = "Star It!";
        private const string DeStarText = "De-Star It!";
        private readonly Image _deStarImage = Image.FromStream(Resources.Resources.GetEmbeddedResourceStream("UniTimetable.Resources.DeStar.png"));
        private readonly Color _solutionColor1 = Color.White;
        private readonly Color _solutionColor2 = Color.LightGray;
        private readonly Color _starColor = Color.Yellow;
        private readonly Image _starImage = Image.FromStream(Resources.Resources.GetEmbeddedResourceStream("UniTimetable.Resources.Favorites.png"));
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
            btnStar.Enabled = false;
            btnUse.Enabled = false;
            SetStarButton(false);
            chkOnlyStarred.Checked = false;
        }

        /// <summary>
        ///     Update preview and information for a new selected solution
        /// </summary>
        private void LvbSolutionsSelectedIndexChanged(object sender, EventArgs e)
        {
            // update preview panel
            UpdatePreview();
            // clear streams box
            textBox1.Clear();
            // clear properties box
            lvbProperties.Items.Clear();
            // if nothing is selected, return
            if (lvbSolutions.SelectedIndices.Count == 0 || lvbSolutions.SelectedIndices[0] == -1)
            {
                btnStar.Enabled = false;
                btnUse.Enabled = false;
                SetStarButton(false);
                return;
            }

            btnStar.Enabled = true;
            btnUse.Enabled = true;
            // set star button text accordingly
            SetStarButton(lvbSolutions.SelectedItems[0].BackColor == _starColor);

            // set the currently selected solution
            Solver.Solution solution = (Solver.Solution) lvbSolutions.SelectedItems[0].Tag;

            // a list of lists of streams selected for each subject
            List<List<Stream>> groupedStreams = new List<List<Stream>>();
            // an ordered list of subjects
            List<Subject> orderedSubjects = new List<Subject>();
            // fill the lists
            foreach (Subject subject in _solver.Timetable.SubjectList)
            {
                groupedStreams.Add(new List<Stream>());
                orderedSubjects.Add(subject);
            }
            // sort the subjects
            orderedSubjects.Sort();
            //orderedSubjects.Sort(new IndexComparer(m_Timetable.SubjectList));
            // fill the grouped streams list
            foreach (Stream stream in solution.Streams)
            {
                groupedStreams[orderedSubjects.IndexOf(stream.Type.Subject)].Add(stream);
            }
            // sort each sublist and fill up the streams text box
            for (int i = 0; i < groupedStreams.Count; i++)
            {
                // sort the sublist
                groupedStreams[i].Sort();
                // first get the name of the subject
                textBox1.Text += orderedSubjects[i].Name + ":\r\n  ";
                // for each stream in the subject sublist
                for (int j = 0; j < groupedStreams[i].Count; j++)
                {
                    // print the stream
                    textBox1.Text += groupedStreams[i][j].ToString();
                    if (j != groupedStreams[i].Count - 1)
                        textBox1.Text += ", ";
                }
                if (i != groupedStreams.Count - 1)
                    textBox1.Text += "\r\n";
            }

            // properties list - for each property (field)
            for (int i = 0; i < Solver.Fields.Length; i++)
            {
                ListViewItem item = new ListViewItem(new[]
                                                     {
                                                         // with the name of the field
                                                         Solver.Fields[i].Name,
                                                         // and the selected solution's value
                                                         solution.FieldValueToString((Solver.FieldIndex) i)
                                                     });

                lvbProperties.Items.Add(item);
            }
        }

        private void BtnUseClick(object sender, EventArgs e)
        {
            // nothing selected
            if (lvbSolutions.SelectedIndices.Count <= 0 || lvbSolutions.SelectedIndices[0] == -1)
                return;

            // set the currently selected solution
            Solver.Solution solution = (Solver.Solution) lvbSolutions.SelectedItems[0].Tag;
            _solver.Timetable.LoadSolution(solution);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCriteriaClick(object sender, EventArgs e)
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

        private void BtnStarClick(object sender, EventArgs e)
        {
            // nothing selected
            if (lvbSolutions.SelectedIndices.Count == 0 || lvbSolutions.SelectedIndices[0] == -1)
                return;
            // get the selected item
            ListViewItem item = lvbSolutions.SelectedItems[0];

            // if we're only viewing starred
            if (chkOnlyStarred.Checked)
            {
                // get the solution
                Solver.Solution solution = (Solver.Solution) item.Tag;
                // remove the item from the list
                lvbSolutions.Items.RemoveAt(lvbSolutions.SelectedIndices[0]);
                // search for the item in the backup list and destar it
                for (int i = 0; i < _fullListBackup.Length; i++)
                {
                    if ((Solver.Solution) _fullListBackup[i].Tag == solution)
                    {
                        _fullListBackup[i].ImageIndex = -1;
                        _fullListBackup[i].BackColor = _originalColors[i];
                        break;
                    }
                }
            }
            // if it is starred already
            if (item.BackColor == _starColor)
            {
                lvbSolutions.SelectedItems[0].ImageIndex = -1;
                lvbSolutions.SelectedItems[0].BackColor = _originalColors[lvbSolutions.SelectedIndices[0]];
                // set button text
                SetStarButton(false);
            }
            else
            {
                item.ImageIndex = 0;
                item.BackColor = _starColor;
                // set button text
                SetStarButton(true);
            }
            // reselect list view box
            lvbSolutions.Select();
        }

        private void RebuildList()
        {
            // clear the outputs
            // TODO: clean up at all?
            lvbSolutions.Items.Clear();
            UpdatePreview();
            textBox1.Clear();
            lvbProperties.Items.Clear();
            lvbSolutions.Columns.Clear();
            _originalColors = new Color[_solver.Solutions.Count];
            int i;

            // create a dummy column - first column must be left-aligned
            var dummy = new ColumnHeader {Text = "", Width = 24};
            lvbSolutions.Columns.Add(dummy);
            // redo all the columns
            for (i = 0; i < _solver.Comparer.Criteria.Count; i++)
            {
                var columnHeader = new ColumnHeader
                                            {
                                                Text = _solver.Comparer.Criteria[i].Field.Name,
                                                TextAlign = HorizontalAlignment.Right,
                                                Width = 100
                                            };
                lvbSolutions.Columns.Add(columnHeader);
            }

            // rebuild the list of solutions
            Solver.Solution prevSolution = null;
            Color prevColor = _solutionColor1;
            i = 0;
            foreach (Solver.Solution solution in _solver.Solutions)
            {
                // make a list of data for each entry
                string[] data = new string[_solver.Comparer.Criteria.Count + 1];
                data[0] = "";
                // for each field we're using
                for (int j = 0; j < _solver.Comparer.Criteria.Count; j++)
                {
                    // get a string representing the value for that criteria in the solution
                    data[j + 1] = solution.FieldValueToString(_solver.Comparer.Criteria[j].FieldIndex);
                }
                // create the item
                var item = new ListViewItem(data) {Tag = solution, ImageIndex = -1};

                // make the first result white
                if (prevSolution == null)
                {
                    prevColor = _solutionColor1;
                }
                // else if the following solution is different (by the current criteria)
                else if (solution.CompareTo(prevSolution, _solver.Comparer) != 0)
                {
                    // switch colours
                    prevColor = prevColor == _solutionColor1 ? _solutionColor2 : _solutionColor1;
                }
                item.BackColor = prevColor;
                _originalColors[i++] = item.BackColor;

                // add the item to the list
                lvbSolutions.Items.Add(item);
                prevSolution = solution;
            }
        }

        private void UpdatePreview()
        {
            // nothing selected
            if (lvbSolutions.SelectedIndices.Count == 0 || lvbSolutions.SelectedIndices[0] == -1)
            {
                timetableControl.Timetable = null;
                return;
            }

            Solver.Solution solution = (Solver.Solution) lvbSolutions.SelectedItems[0].Tag;
            timetableControl.Timetable = _solver.Timetable.PreviewSolution(solution);
        }

        private void SetStarButton(bool destar)
        {
            // if starring is disabled
            if (chkOnlyStarred.Checked || destar)
            {
                btnStar.Image = _deStarImage;
                btnStar.Text = DeStarText;
            }
            else
            {
                btnStar.Image = _starImage;
                btnStar.Text = StarText;
            }
        }

        private void ChkOnlyStarredCheckedChanged(object sender, EventArgs e)
        {
            // checkbox just got checked
            if (chkOnlyStarred.Checked)
            {
                // disable star button
                btnStar.Enabled = false;
                // back-up full list
                _fullListBackup = new ListViewItem[lvbSolutions.Items.Count];
                lvbSolutions.Items.CopyTo(_fullListBackup, 0);
                // start with fresh list
                lvbSolutions.Items.Clear();
                // add starred items to ListView
                foreach (ListViewItem item in _fullListBackup)
                {
                    // if it is starred
                    if (item.BackColor == _starColor)
                    {
                        // clone the item and add to the list
                        lvbSolutions.Items.Add((ListViewItem) item.Clone());
                    }
                }
                // now set the colours
                foreach (ListViewItem item in lvbSolutions.Items)
                {
                    item.BackColor = _solutionColor1;
                }
            }
            // checkbox just got unchecked
            else
            {
                // enable start button
                btnStar.Enabled = true;
                // refer back to the backup
                lvbSolutions.Items.Clear();
                lvbSolutions.Items.AddRange(_fullListBackup);
                // restore selection
                lvbSolutions.Select();
            }
        }
    }
}
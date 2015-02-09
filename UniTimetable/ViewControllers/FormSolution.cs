#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniTimetable.Model.Solver;
using UniTimetable.Model.Timetable;
using UniTimetable.Properties;

#endregion

namespace UniTimetable.ViewControllers
{
    partial class FormSolution : Form
    {
        private const string StarText_ = "Star It!";
        private const string DeStarText_ = "De-Star It!";
        private readonly Image DeStarImage_ = Resources.DeStar;
        private readonly Color SolutionColor1_ = Color.White;
        private readonly Color SolutionColor2_ = Color.LightGray;
        private readonly Color StarColor_ = Color.Yellow;
        private readonly Image StarImage_ = Resources.Favorites;
        private ListViewItem[] FullListBackup_;
        private Color[] OriginalColors_;
        private Solver Solver_;

        public FormSolution()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(Solver solver)
        {
            Solver_ = solver;
            return base.ShowDialog();
        }

        public new DialogResult ShowDialog()
        {
            throw new Exception("No input was provided.");
        }

        private void FormSolution_Load(object sender, EventArgs e)
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
        private void lvbSolutions_SelectedIndexChanged(object sender, EventArgs e)
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
            if (lvbSolutions.SelectedItems[0].BackColor == StarColor_)
                SetStarButton(true);
            else
                SetStarButton(false);

            // set the currently selected solution
            Solver.Solution solution = (Solver.Solution) lvbSolutions.SelectedItems[0].Tag;

            // a list of lists of streams selected for each subject
            List<List<Stream>> groupedStreams = new List<List<Stream>>();
            // an ordered list of subjects
            List<Subject> orderedSubjects = new List<Subject>();
            // fill the lists
            foreach (Subject subject in Solver_.Timetable.SubjectList)
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

        private void btnUse_Click(object sender, EventArgs e)
        {
            // nothing selected
            if (lvbSolutions.SelectedIndices.Count <= 0 || lvbSolutions.SelectedIndices[0] == -1)
                return;

            // set the currently selected solution
            Solver.Solution solution = (Solver.Solution) lvbSolutions.SelectedItems[0].Tag;
            Solver_.Timetable.LoadSolution(solution);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCriteria_Click(object sender, EventArgs e)
        {
            FormCriteria formCriteria = new FormCriteria();
            // if the criteria dialog was shown and nothing changed, all good
            if (formCriteria.ShowDialog(Solver_) != DialogResult.OK)
                return;

            Solver_.Timetable.RecomputeSolutions = true;
            FormProgress formProgress = new FormProgress();
            formProgress.ShowDialog(Solver_);

            Reset();
        }

        private void btnStar_Click(object sender, EventArgs e)
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
                for (int i = 0; i < FullListBackup_.Length; i++)
                {
                    if ((Solver.Solution) FullListBackup_[i].Tag == solution)
                    {
                        FullListBackup_[i].ImageIndex = -1;
                        FullListBackup_[i].BackColor = OriginalColors_[i];
                        break;
                    }
                }
            }
            // if it is starred already
            if (item.BackColor == StarColor_)
            {
                lvbSolutions.SelectedItems[0].ImageIndex = -1;
                lvbSolutions.SelectedItems[0].BackColor = OriginalColors_[lvbSolutions.SelectedIndices[0]];
                // set button text
                SetStarButton(false);
            }
            else
            {
                item.ImageIndex = 0;
                item.BackColor = StarColor_;
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
            OriginalColors_ = new Color[Solver_.Solutions.Count];
            int i;

            // create a dummy column - first column must be left-aligned
            ColumnHeader dummy = new ColumnHeader();
            dummy.Text = "";
            dummy.Width = 24;
            lvbSolutions.Columns.Add(dummy);
            // redo all the columns
            for (i = 0; i < Solver_.Comparer.Criteria.Count; i++)
            {
                ColumnHeader columnHeader = new ColumnHeader();
                columnHeader.Text = Solver_.Comparer.Criteria[i].Field.Name;
                columnHeader.TextAlign = HorizontalAlignment.Right;
                columnHeader.Width = 100;
                lvbSolutions.Columns.Add(columnHeader);
            }

            // rebuild the list of solutions
            Solver.Solution prevSolution = null;
            Color prevColor = SolutionColor1_;
            i = 0;
            foreach (Solver.Solution solution in Solver_.Solutions)
            {
                // make a list of data for each entry
                string[] data = new string[Solver_.Comparer.Criteria.Count + 1];
                data[0] = "";
                // for each field we're using
                for (int j = 0; j < Solver_.Comparer.Criteria.Count; j++)
                {
                    // get a string representing the value for that criteria in the solution
                    data[j + 1] = solution.FieldValueToString(Solver_.Comparer.Criteria[j].FieldIndex);
                }
                // create the item
                ListViewItem item = new ListViewItem(data);
                item.Tag = solution;
                item.ImageIndex = -1;

                // make the first result white
                if (prevSolution == null)
                {
                    prevColor = SolutionColor1_;
                }
                // else if the following solution is different (by the current criteria)
                else if (!(solution.CompareTo(prevSolution, Solver_.Comparer) == 0))
                {
                    // switch colours
                    if (prevColor == SolutionColor1_)
                        prevColor = SolutionColor2_;
                    else
                        prevColor = SolutionColor1_;
                }
                item.BackColor = prevColor;
                OriginalColors_[i++] = item.BackColor;

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
            timetableControl.Timetable = Solver_.Timetable.PreviewSolution(solution);
        }

        private void SetStarButton(bool destar)
        {
            // if starring is disabled
            if (chkOnlyStarred.Checked || destar)
            {
                btnStar.Image = DeStarImage_;
                btnStar.Text = DeStarText_;
            }
            else
            {
                btnStar.Image = StarImage_;
                btnStar.Text = StarText_;
            }
        }

        private void chkOnlyStarred_CheckedChanged(object sender, EventArgs e)
        {
            // checkbox just got checked
            if (chkOnlyStarred.Checked)
            {
                // disable star button
                btnStar.Enabled = false;
                // back-up full list
                FullListBackup_ = new ListViewItem[lvbSolutions.Items.Count];
                lvbSolutions.Items.CopyTo(FullListBackup_, 0);
                // start with fresh list
                lvbSolutions.Items.Clear();
                // add starred items to ListView
                foreach (ListViewItem item in FullListBackup_)
                {
                    // if it is starred
                    if (item.BackColor == StarColor_)
                    {
                        // clone the item and add to the list
                        lvbSolutions.Items.Add((ListViewItem) item.Clone());
                    }
                }
                // now set the colours
                foreach (ListViewItem item in lvbSolutions.Items)
                {
                    item.BackColor = SolutionColor1_;
                }
            }
            // checkbox just got unchecked
            else
            {
                // enable start button
                btnStar.Enabled = true;
                // refer back to the backup
                lvbSolutions.Items.Clear();
                lvbSolutions.Items.AddRange(FullListBackup_);
                // restore selection
                lvbSolutions.Select();
            }
        }
    }
}
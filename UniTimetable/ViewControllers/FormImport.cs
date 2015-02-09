#region

using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UniTimetable.Model;
using UniTimetable.Model.Import;
using UniTimetable.Model.Timetable;
using Type = UniTimetable.Model.Timetable.Type;

#endregion

namespace UniTimetable.ViewControllers
{
    partial class FormImport : FormModel
    {
        private readonly Importer _importer;
        private Timetable _timetable;

        public FormImport()
        {
            InitializeComponent();

            // Set Importer Type
            _importer = new UnocImporter();

            if (_importer.RequiresPassword)
            {
                string username, password;
                var loginForm = new FormLogin();
                var result = loginForm.ShowDialog(out username, out password);
                if (result == DialogResult.Cancel)
                {
                    DialogResult = DialogResult.Cancel;
                }
                _importer.SetLogin(username, password);
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
            if (DialogResult == DialogResult.Cancel)
            {
                Close();
            }
        }

        public new Timetable ShowDialog()
        {
            return base.ShowDialog() != DialogResult.OK ? null : _timetable;
        }

        private void FormImportLoad(object sender, EventArgs e)
        {
            var thread = new Thread(ImportThread);
            thread.Start();
        }

        private void ImportThread()
        {
            _timetable = _importer.Import();
            Invoke(new MethodInvoker(delegate
                                     {
                                         if (_timetable != null)
                                         {
                                             panelLoading.Visible = false;
                                             panelStreams.Visible = false;
                                             btnNext.Enabled = true;

                                             // build relational data
                                             _timetable.BuildEquivalency();
                                             _timetable.BuildCompatibility();
                                             //Timetable_.UpdateStates();

                                             // build tree
                                             _timetable.BuildTreeView(treePreview);
                                             // and scroll back to the top
                                             treePreview.Nodes[0].EnsureVisible();
                                             // clear details box
                                             txtTreeDetails.Text = "";
                                             timetableControl1.Clear();
                                         }
                                         else
                                         {
                                             MessageBox.Show("Failed to import/retrieve timetable data.", "Import",
                                                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                             Close();
                                         }
                                     }));
        }

        #region Page 3 Event Handlers

        private void TreePreviewAfterSelect(object sender, TreeViewEventArgs e)
        {
            // clear textbox
            txtTreeDetails.Text = "";
            // if nothing selected, done already
            if (treePreview.SelectedNode == null)
                return;

            // level 0: subject
            if (treePreview.SelectedNode.Level == 0)
            {
                // get subject
                var subject = (Subject) treePreview.SelectedNode.Tag;
                // print subject name
                txtTreeDetails.Text += subject.Name + "\r\n";
                // print all the types within the subject
                foreach (var type in subject.Types)
                {
                    txtTreeDetails.Text += "\r\n\t" + type.Name + " (" + type.Streams.Count + ")";
                }
                // preview pane
                timetableControl1.Timetable = Timetable.From(subject);
            }
            // level 1: type
            else if (treePreview.SelectedNode.Level == 1)
            {
                // get type
                var type = (Type) treePreview.SelectedNode.Tag;
                // print type name
                txtTreeDetails.Text += type.Subject.Name + " " + type.Name + "\r\n";
                // print all the streams within the type
                foreach (var stream in type.Streams)
                {
                    txtTreeDetails.Text += "\r\n\t" + stream;
                }
                // preview pane
                timetableControl1.Timetable = Timetable.From(type);
            }
            // level 2: stream
            else
            {
                // get stream
                var stream = (Stream) treePreview.SelectedNode.Tag;
                // print stream name
                txtTreeDetails.Text += stream.Type.Subject.Name + " " + stream;
                // print all the classes within the type
                foreach (var session in stream.Classes)
                {
                    txtTreeDetails.Text += "\r\n\t\r\n\t" + session;
                }
                // preview pane
                timetableControl1.Timetable = Timetable.From(stream);
            }
        }

        #endregion

        private void CheckBoxTestCheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTest.Checked)
            {
                foreach (
                    var item in
                        listViewIgnored.Items.Cast<ListViewItem>().Where(item => item.SubItems[1].Text.Contains("Test"))
                    )
                {
                    item.Selected = true;
                    MoveRight();
                }
            }
            else
            {
                foreach (
                    var item in
                        listViewRequired.Items.Cast<ListViewItem>()
                            .Where(item => item.SubItems[1].Text.Contains("Test")))
                {
                    item.Selected = true;
                    MoveLeft();
                }
            }
        }

        private void CheckBoxS1CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxS1.Checked)
            {
                foreach (ListViewItem item in listViewIgnored.Items)
                {
                    if (!item.Group.ToString().Contains("S1")) continue;
                    item.Selected = true;
                    MoveRight();
                }
            }
            else
            {
                foreach (ListViewItem item in listViewRequired.Items)
                {
                    if (!item.Group.ToString().Contains("S1")) continue;
                    item.Selected = true;
                    MoveLeft();
                }
            }
        }

        private void CheckBoxS2CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxS2.Checked)
            {
                foreach (ListViewItem item in listViewIgnored.Items)
                {
                    if (!item.Group.ToString().Contains("S2")) continue;
                    item.Selected = true;
                    MoveRight();
                }
            }
            else
            {
                foreach (ListViewItem item in listViewRequired.Items)
                {
                    if (!item.Group.ToString().Contains("S2")) continue;
                    item.Selected = true;
                    MoveLeft();
                }
            }
        }

        #region Wizard Control Flow Buttons

        private void BtnNextClick(object sender, EventArgs e)
        {
            Next();
        }

        private void BtnFinishClick(object sender, EventArgs e)
        {
            /*if (TimetableShadow_.HasClashingTypes())
            {
                MessageBox.Show("Please remove clashing streams until none remain.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }*/
            _timetable.Update();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Next()
        {
            if (!panelLoading.Visible && panelPreview.Visible)
            {
                panelStreams.Visible = true;

                // clear ignored/required lists
                listViewIgnored.Items.Clear();
                listViewIgnored.Groups.Clear();
                listViewRequired.Items.Clear();
                listViewRequired.Groups.Clear();

                // populate ignored/required lists
                foreach (var subject in _timetable.SubjectList)
                {
                    // create and add groups for the subjects
                    var ignoredSubjectGroup = new ListViewGroup(subject.Name) {Tag = subject};
                    listViewIgnored.Groups.Add(ignoredSubjectGroup);

                    var requiredSubjectGroup = new ListViewGroup(subject.Name) {Tag = subject};
                    listViewRequired.Groups.Add(requiredSubjectGroup);

                    // add stream types to subject groups
                    foreach (var type in subject.Types)
                    {
                        // create ListViewItem without group
                        var item = new ListViewItem(new[] {type.Code, type.Name}) {Tag = type};

                        // add it to the current group in the correct box
                        if (type.Required)
                        {
                            // set group and add to list
                            item.Group = requiredSubjectGroup;
                            listViewRequired.Items.Add(item);
                        }
                        else
                        {
                            // set group and add to list
                            item.Group = ignoredSubjectGroup;
                            listViewIgnored.Items.Add(item);
                        }
                    }
                }

                checkBoxS2.Checked = DateTime.Now.Month > 6;
                checkBoxS1.Checked = !checkBoxS2.Checked;

                CheckBoxS1CheckedChanged(null, null);
                CheckBoxS2CheckedChanged(null, null);
                CheckBoxTestCheckedChanged(null, null);

                UpdateClashHighlight();

                btnRequire.Enabled = false;
                btnIgnore.Enabled = false;

                // bring up panel 4
                panelLoading.Visible = false;
                panelPreview.Visible = false;

                // swap next button for finish
                btnNext.Visible = false;
                btnFinish.Visible = true;

                // need to refresh to get red highlighter
                listViewIgnored.Refresh();
                listViewRequired.Refresh();
            }

            /*if (_currentPage == 1)
            {
                // if there's no selected format, skip
                if (listBoxFormats.SelectedItem == null)
                    return;

                // load selected importer
                _importer = (Importer)listBoxFormats.SelectedItem;
                // clear importer files
                _importer.File1Dialog.FileName = "";
                _importer.File2Dialog.FileName = "";
                _importer.File3Dialog.FileName = "";

                // bring up panel 2 (file import) information
                // file 1
                /*if (_importer.File1Description != null)
                {
                    lblFile1.Text = _importer.File1Description;
                    lblFile1.Visible = true;
                    btnBrowse1.Visible = true;
                    txtFile1.Visible = true;
                }
                else
                {
                    lblFile1.Visible = false;
                    btnBrowse1.Visible = false;
                    txtFile1.Visible = false;
                }
                // file 2
                if (_importer.File2Description != null)
                {
                    lblFile2.Text = _importer.File2Description;
                    lblFile2.Visible = true;
                    btnBrowse2.Visible = true;
                    txtFile2.Visible = true;
                }
                else
                {
                    lblFile2.Visible = false;
                    btnBrowse2.Visible = false;
                    txtFile2.Visible = false;
                }
                // file 3
                if (_importer.File3Description != null)
                {
                    lblFile3.Text = _importer.File3Description;
                    lblFile3.Visible = true;
                    btnBrowse3.Visible = true;
                    txtFile3.Visible = true;
                }
                else
                {
                    lblFile3.Visible = false;
                    btnBrowse3.Visible = false;
                    txtFile3.Visible = false;
                }
                // file instructions
                if (_importer.FileInstructions != null)
                {
                    txtFileInstructions.Text = _importer.FileInstructions;
                }
                else
                {
                    txtFileInstructions.Text = "No instructions provided for " + _importer.FormatName + ".";
                }

                // bring up panel 2
                panel1.Visible = false;
                panel2.Visible = true;
                if(lblFile1.Text.ToLower() == "login")
                {
                    panelLogin.Visible = true;
                }* /
                // enable back button
                btnBack.Enabled = true;
            }
            else if (_currentPage == 2)
            {
                // internet login specifics
                labelWait.Visible = true;
                panelLogin.Cursor = Cursors.WaitCursor;
                _importer.SetLogin(textBoxUsername.Text, textBoxPassword.Text);
                panelLogin.Refresh();

                // try and parse files
                _timetable = _importer.Import();

                // if it failed, alert the user and stay on the current page
                if (_timetable == null)
                {
                    MessageBox.Show("Failed to import/retrieve timetable data.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    // Hide "loading" prompts from login page
                    //labelWait.Visible = false;
                    Cursor = Cursors.Default;
                    Refresh();

                    return;
                }

                // build relational data
                _timetable.BuildEquivalency();
                _timetable.BuildCompatibility();
                //Timetable_.UpdateStates();

                // build tree
                _timetable.BuildTreeView(treePreview);
                // and scroll back to the top
                treePreview.Nodes[0].EnsureVisible();
                // clear details box
                txtTreeDetails.Text = "";
                timetableControl1.Clear();

                // bring up panel 3
                //panel2.Visible = false;
                //panelLogin.Visible = false;
                panel3.Visible = true;
            }
            else if (_currentPage == 3)
            {
                // clear ignored/required lists
                listViewIgnored.Items.Clear();
                listViewIgnored.Groups.Clear();
                listViewRequired.Items.Clear();
                listViewRequired.Groups.Clear();

                // populate ignored/required lists
                foreach (var subject in _timetable.SubjectList)
                {
                    // create and add groups for the subjects
                    var ignoredSubjectGroup = new ListViewGroup(subject.Name) {Tag = subject};
                    listViewIgnored.Groups.Add(ignoredSubjectGroup);

                    var requiredSubjectGroup = new ListViewGroup(subject.Name) {Tag = subject};
                    listViewRequired.Groups.Add(requiredSubjectGroup);

                    // add stream types to subject groups
                    foreach (var type in subject.Types)
                    {
                        // create ListViewItem without group
                        var item = new ListViewItem(new [] { type.Code, type.Name }) {Tag = type};

                        // add it to the current group in the correct box
                        if (type.Required)
                        {
                            // set group and add to list
                            item.Group = requiredSubjectGroup;
                            listViewRequired.Items.Add(item);
                        }
                        else
                        {
                            // set group and add to list
                            item.Group = ignoredSubjectGroup;
                            listViewIgnored.Items.Add(item);
                        }
                    }
                }

                UpdateClashHighlight();

                btnRequire.Enabled = false;
                btnIgnore.Enabled = false;

                // bring up panel 4
                panel3.Visible = false;
                panel4.Visible = true;
                // swap next button for finish
                btnNext.Visible = false;
                btnFinish.Visible = true;

                // need to refresh to get red highlighter
                listViewIgnored.Refresh();
                listViewRequired.Refresh();
            }
            _currentPage++;*/
        }

        #endregion

        #region Page 4 Event Handlers

        private void ListViewRequiredSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateIgnoreButton();
        }

        private void ListViewIgnoredSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRequireButton();
        }

        private void ListViewRequiredEnter(object sender, EventArgs e)
        {
            UpdateIgnoreButton();
        }

        private void ListViewIgnoredEnter(object sender, EventArgs e)
        {
            UpdateRequireButton();
        }

        private void UpdateRequireButton()
        {
            btnRequire.Enabled = false;
            btnIgnore.Enabled = false;
            if (listViewIgnored.SelectedIndices.Count == 0 || listViewIgnored.SelectedIndices[0] == -1)
                return;
            btnRequire.Enabled = true;
        }

        private void UpdateIgnoreButton()
        {
            btnRequire.Enabled = false;
            btnIgnore.Enabled = false;
            if (listViewRequired.SelectedIndices.Count == 0 || listViewRequired.SelectedIndices[0] == -1)
                return;
            btnIgnore.Enabled = true;
        }

        private void UpdateClashHighlight()
        {
            var clash = false;
            foreach (ListViewItem item in listViewRequired.Items)
            {
                var type = (Type) item.Tag;
                if (_timetable.CheckDirectClash(type))
                {
                    clash = true;
                    item.BackColor = Color.Red;
                }
                else
                {
                    item.BackColor = SystemColors.Window;
                }
            }
            lblClashNotice.Visible = clash;

            listViewIgnored.Invalidate();
            listViewRequired.Invalidate();
        }

        private void BtnIgnoreClick(object sender, EventArgs e)
        {
            MoveLeft();
        }

        private void BtnRequireClick(object sender, EventArgs e)
        {
            MoveRight();
        }

        private void ListViewRequiredMouseDoubleClick(object sender, MouseEventArgs e)
        {
            MoveLeft();
        }

        private void ListViewIgnoredMouseDoubleClick(object sender, MouseEventArgs e)
        {
            MoveRight();
        }

        private void MoveLeft()
        {
            if (listViewRequired.SelectedItems.Count == 0 || listViewRequired.SelectedItems[0] == null)
                return;

            var item = listViewRequired.SelectedItems[0];
            item.BackColor = SystemColors.Window;
            var type = (Type) item.Tag;
            type.Required = false;
            _timetable.BuildCompatibility();

            var index = listViewRequired.SelectedIndices[0];
            listViewRequired.Items.RemoveAt(index);

            // look through each subject group in the ignored list
            foreach (ListViewGroup group in listViewIgnored.Groups)
            {
                // if we've found the subject group
                if (@group.Tag != type.Subject) continue;
                // set group and add to list
                item.Group = @group;
                listViewIgnored.Items.Add(item);
                break;
            }

            // select the next item in the list
            if (index == listViewRequired.Items.Count)
                index--;
            if (index >= 0)
            {
                listViewRequired.Items[index].Selected = true;
                listViewRequired.Select();
            }
            else
            {
                btnRequire.Enabled = false;
                btnIgnore.Enabled = false;
            }

            UpdateClashHighlight();
        }

        private void MoveRight()
        {
            if (listViewIgnored.SelectedItems.Count == 0 || listViewIgnored.SelectedItems[0] == null)
                return;

            var item = listViewIgnored.SelectedItems[0];
            var type = (Type) item.Tag;
            type.Required = true;
            _timetable.BuildCompatibility();

            var index = listViewIgnored.SelectedIndices[0];
            listViewIgnored.Items.RemoveAt(index);

            // look through each subject group in the required list
            foreach (ListViewGroup group in listViewRequired.Groups)
            {
                // if we've found the subject group
                if (@group.Tag != type.Subject) continue;
                // set group and add to list
                item.Group = @group;
                listViewRequired.Items.Add(item);
                break;
            }

            // select the next item in the list
            if (index == listViewIgnored.Items.Count)
                index--;
            if (index >= 0)
            {
                listViewIgnored.Items[index].Selected = true;
                listViewIgnored.Select();
            }
            else
            {
                btnRequire.Enabled = false;
                btnIgnore.Enabled = false;
            }

            UpdateClashHighlight();
        }

        #endregion
    }
}
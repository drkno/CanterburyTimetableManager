#region

using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UniTimetable.Model.ImportExport;
using UniTimetable.Model.ImportExport.Login;
using UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury;
using UniTimetable.Model.Timetable;
using Type = UniTimetable.Model.Timetable.Type;

#endregion

namespace UniTimetable.ViewControllers
{
    partial class FormImport : FormModel
    {
        private readonly IImporter _importer;
        private Timetable _timetable;

        public FormImport(bool importUnselectable)
        {
            InitializeComponent();

            _importer = new CanterburyImporter {ImportUnselectableStreams = importUnselectable};
            ILoginRequired loginRequired = _importer as ILoginRequired;
            if (loginRequired != null)
            {
                var loginHandle = loginRequired.CreateNewLoginHandle();
                var login = new FormLogin(ref loginHandle, "Import Timetable");
                var result = login.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    DialogResult = DialogResult.Cancel;
                }
                loginRequired.SetLoginHandle(ref loginHandle);
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
            _timetable = _importer.ImportTimetable();
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
            if (panelLoading.Visible || !panelPreview.Visible) return;
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

        private bool CheckDirectClash(Type a)
        {
            return a.Required && _timetable.TypeList.Where(b => a != b).Where(b => b.Required).Any(a.ClashesWith);
        }

        private void UpdateClashHighlight()
        {
            var clash = false;
            foreach (ListViewItem item in listViewRequired.Items)
            {
                var type = (Type) item.Tag;
                if (CheckDirectClash(type))
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
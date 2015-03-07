using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniTimetable.Model.Timetable;
using Type = UniTimetable.Model.Timetable.Type;

namespace UniTimetable.ViewControllers.Import
{
    public partial class StreamsPanel : UserControl
    {
        private readonly Timetable _timetable;

        public StreamsPanel(Timetable timetable)
        {
            InitializeComponent();
            _timetable = timetable;

            // clear ignored/required lists
            listViewIgnored.Items.Clear();
            listViewIgnored.Groups.Clear();
            listViewRequired.Items.Clear();
            listViewRequired.Groups.Clear();

            // populate ignored/required lists
            foreach (var subject in _timetable.SubjectList)
            {
                // create and add groups for the subjects
                var ignoredSubjectGroup = new ListViewGroup(subject.Name) { Tag = subject };
                listViewIgnored.Groups.Add(ignoredSubjectGroup);

                var requiredSubjectGroup = new ListViewGroup(subject.Name) { Tag = subject };
                listViewRequired.Groups.Add(requiredSubjectGroup);

                // add stream types to subject groups
                foreach (var type in subject.Types)
                {
                    // create ListViewItem without group
                    var item = new ListViewItem(new[] { type.Code, type.Name }) { Tag = type };

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

            UpdateClashHighlight();

            buttonRequire.Enabled = false;
            buttonIgnore.Enabled = false;

            // need to refresh to get red highlighter
            listViewIgnored.Refresh();
            listViewRequired.Refresh();
        }

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
            var type = (Type)item.Tag;
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
                buttonRequire.Enabled = false;
                buttonIgnore.Enabled = false;
            }

            UpdateClashHighlight();
        }

        private void MoveRight()
        {
            if (listViewIgnored.SelectedItems.Count == 0 || listViewIgnored.SelectedItems[0] == null)
                return;

            var item = listViewIgnored.SelectedItems[0];
            var type = (Type)item.Tag;
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
                buttonRequire.Enabled = false;
                buttonIgnore.Enabled = false;
            }

            UpdateClashHighlight();
        }

        private void UpdateClashHighlight()
        {
            var clash = false;
            foreach (ListViewItem item in listViewRequired.Items)
            {
                var type = (Type)item.Tag;
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
            labelClashNotice.Visible = clash;

            listViewIgnored.Invalidate();
            listViewRequired.Invalidate();
        }

        private bool CheckDirectClash(Type a)
        {
            return a.Required && _timetable.TypeList.Where(b => a != b).Where(b => b.Required).Any(a.ClashesWith);
        }

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
            buttonRequire.Enabled = false;
            buttonIgnore.Enabled = false;
            if (listViewIgnored.SelectedIndices.Count == 0 || listViewIgnored.SelectedIndices[0] == -1)
                return;
            buttonRequire.Enabled = true;
        }

        private void UpdateIgnoreButton()
        {
            buttonRequire.Enabled = false;
            buttonIgnore.Enabled = false;
            if (listViewRequired.SelectedIndices.Count == 0 || listViewRequired.SelectedIndices[0] == -1)
                return;
            buttonIgnore.Enabled = true;
        }

        private void StreamsPanelLoad(object sender, EventArgs e)
        {
            CheckBoxS1CheckedChanged(null, null);
            CheckBoxS2CheckedChanged(null, null);
            CheckBoxTestCheckedChanged(null, null);
        }
    }
}

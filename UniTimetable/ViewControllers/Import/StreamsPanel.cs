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
            var listView = checkBoxTest.Checked ? listViewIgnored : listViewRequired;
            foreach (var item in listView.Items.Cast<ListViewItem>()
                    .Where(item => item.SubItems[1].Text.Contains("Test")))
            {
                item.Selected = true;
                MoveItems(!checkBoxTest.Checked);
            }
        }

        private void MoveItems(bool right, string contains)
        {
            var items = right ? listViewIgnored.Items : listViewRequired.Items;
            foreach (ListViewItem item in items)
            {
                if (!item.Group.ToString().Contains(contains)) continue;
                item.Selected = true;
                MoveItems(!right);
            }
        }

        private void CheckBoxS1CheckedChanged(object sender, EventArgs e)
        {
            MoveItems(checkBoxS1.Checked, "S1");

            var listView = checkBoxS1.Checked ? listViewIgnored : listViewRequired;
            foreach (ListViewItem item in listView.Items)
            {
                var type = item.Tag as Type;
                if (type == null) continue;
                var once = false;
                foreach (var stream in type.Streams)
                {
                    foreach (var session in stream.Classes)
                    {
                        var index = session.WeekPattern.LastIndexOf('1');
                        if (index < 0) continue;
                        var day = (session.StartYearDay + index * 7) % 365;
                        if (day > 182) continue;
                        once = true;
                        break;
                    }
                    if (once)
                    {
                        break;
                    }
                }
                if (once)
                {
                    item.Selected = true;
                }
            }
            MoveItems(!checkBoxS1.Checked);
        }

        private void CheckBoxS2CheckedChanged(object sender, EventArgs e)
        {
            MoveItems(checkBoxS2.Checked, "S2");

            var listView = checkBoxS2.Checked ? listViewIgnored : listViewRequired;
            foreach (ListViewItem item in listView.Items)
            {
                var type = item.Tag as Type;
                if (type == null) continue;
                var once = false;
                foreach (var stream in type.Streams)
                {
                    foreach (var session in stream.Classes)
                    {
                        var index = session.WeekPattern.IndexOf('1');
                        if (index < 0) continue;
                        var day = (session.StartYearDay + index * 7) % 365;
                        if (day <= 182) continue;
                        once = true;
                        break;
                    }
                    if (once)
                    {
                        break;
                    }
                }
                if (once)
                {
                    item.Selected = true;
                }
            }
            MoveItems(!checkBoxS2.Checked);
        }

        private void ButtonIgnoreClick(object sender, EventArgs e)
        {
            MoveItems(true);
        }

        private void ButtonRequireClick(object sender, EventArgs e)
        {
            MoveItems(false);
        }

        private void ListViewRequiredMouseDoubleClick(object sender, MouseEventArgs e)
        {
            MoveItems(true);
        }

        private void ListViewIgnoredMouseDoubleClick(object sender, MouseEventArgs e)
        {
            MoveItems(false);
        }

        private void MoveItems(bool left)
        {
            ListView origional, next;
            if (left)
            {
                origional = listViewRequired;
                next = listViewIgnored;
            }
            else
            {
                origional = listViewIgnored;
                next = listViewRequired;
            }

            if (origional.SelectedItems.Count == 0 || origional.SelectedItems[0] == null)
                return;

            var item = origional.SelectedItems[0];
            item.BackColor = SystemColors.Window;
            var type = (Type)item.Tag;
            type.Required = false;
            _timetable.BuildCompatibility();

            var index = origional.SelectedIndices[0];
            origional.Items.RemoveAt(index);

            // look through each subject group in the ignored list
            foreach (ListViewGroup group in next.Groups)
            {
                // if we've found the subject group
                if (@group.Tag != type.Subject) continue;
                // set group and add to list
                item.Group = @group;
                next.Items.Add(item);
                break;
            }

            // select the next item in the list
            if (index == origional.Items.Count) index--;
            if (index >= 0)
            {
                origional.Items[index].Selected = true;
                origional.Select();
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

        private void CheckBoxOnceOffsCheckedChanged(object sender, EventArgs e)
        {
            var listView = checkBoxOnceOffs.Checked ? listViewIgnored : listViewRequired;
            foreach (ListViewItem item in listView.Items)
            {
                var type = item.Tag as Type;
                if (type == null) continue;
                var once = type.Streams.Aggregate(true, (current, stream) =>
                    current && stream.Classes.All(aClass => aClass.OccursOnce()));
                if (once)
                {
                    item.Selected = true;
                }
            }
            MoveItems(!checkBoxTest.Checked);
        }
    }
}

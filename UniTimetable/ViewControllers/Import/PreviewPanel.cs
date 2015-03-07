using System.Windows.Forms;
using UniTimetable.Model.Timetable;
using Type = UniTimetable.Model.Timetable.Type;

namespace UniTimetable.ViewControllers.Import
{
    public partial class PreviewPanel : UserControl
    {
        public PreviewPanel(Timetable timetable)
        {
            InitializeComponent();

            if (!Properties.Settings.Default.ImportUnselectable)
            {
                labelImportNotice.Visible = true;
            }

            // build tree
            timetable.BuildTreeView(treePreview);
            // and scroll back to the top
            treePreview.Nodes[0].EnsureVisible();
            // clear details box
            textBoxTreeDetails.Text = "";
            timetableControl.Clear();
        }

        private void TreePreviewAfterSelect(object sender, TreeViewEventArgs e)
        {
            // clear textbox
            textBoxTreeDetails.Text = "";
            // if nothing selected, done already
            if (treePreview.SelectedNode == null) return;

            // level 0: subject
            if (treePreview.SelectedNode.Level == 0)
            {
                // get subject
                var subject = (Subject)treePreview.SelectedNode.Tag;
                // print subject name
                textBoxTreeDetails.Text += subject.Name + "\r\n";
                // print all the types within the subject
                foreach (var type in subject.Types)
                {
                    textBoxTreeDetails.Text += "\r\n\t" + type.Name + " (" + type.Streams.Count + ")";
                }
                // preview pane
                timetableControl.Timetable = Timetable.From(subject);
            }
            // level 1: type
            else if (treePreview.SelectedNode.Level == 1)
            {
                // get type
                var type = (Type)treePreview.SelectedNode.Tag;
                // print type name
                textBoxTreeDetails.Text += type.Subject.Name + " " + type.Name + "\r\n";
                // print all the streams within the type
                foreach (var stream in type.Streams)
                {
                    textBoxTreeDetails.Text += "\r\n\t" + stream;
                }
                // preview pane
                timetableControl.Timetable = Timetable.From(type);
            }
            // level 2: stream
            else
            {
                // get stream
                var stream = (Stream)treePreview.SelectedNode.Tag;
                // print stream name
                textBoxTreeDetails.Text += stream.Type.Subject.Name + " " + stream;
                // print all the classes within the type
                foreach (var session in stream.Classes)
                {
                    textBoxTreeDetails.Text += "\r\n\t\r\n\t" + session;
                }
                // preview pane
                timetableControl.Timetable = Timetable.From(stream);
            }
        }
    }
}

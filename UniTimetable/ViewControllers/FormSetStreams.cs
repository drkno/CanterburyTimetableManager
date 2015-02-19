#region

using System;
using System.Threading;
using System.Windows.Forms;
using UniTimetable.Model;
using UniTimetable.Model.Import;
using UniTimetable.Model.Import.CanterburyData;
using UniTimetable.Model.Timetable;

#endregion

namespace UniTimetable.ViewControllers
{
    public partial class FormSetStreams : FormModel
    {
        private readonly Timetable _timetable;

        public FormSetStreams(ref Timetable timetable)
        {
            InitializeComponent();
            _timetable = timetable;
            var thread = new Thread(SetThread);
            thread.Start();
        }

        private void SetThread()
        {
            var importer = new CanterburyImporter();
            if (importer.RequiresPassword)
            {
                /* Login */
                ModifyList("Logging In...");
                string username, password;
                var login = new FormLogin();
                login.ShowDialog(out username, out password, "Login", "Set Course");
                importer.SetLogin(username, password);
            }

            var res = importer.Export(_timetable, ModifyList);
            if (!res)
            {
                MessageBox.Show(
                    "One or more errors occured while setting the desired timetable.\nPlease review the log for details.",
                    "Error(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ModifyList("Complete!");
            EnableOkButton();
        }

        private void ModifyList(string text, bool overwriteLast = false)
        {
            Invoke(new MethodInvoker(delegate
                                     {
                                         if (overwriteLast)
                                         {
                                             listBoxActions.Items[listBoxActions.Items.Count - 1] = text;
                                         }
                                         else
                                         {
                                             listBoxActions.Items.Add(text);
                                         }
                                     }));
        }

        private void EnableOkButton()
        {
            Invoke(new MethodInvoker(delegate { buttonOK.Enabled = true; }));
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
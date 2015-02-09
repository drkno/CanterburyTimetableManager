#region

using System;
using System.Windows.Forms;

#endregion

namespace UniTimetable.ViewControllers
{
    public partial class FormLogin : FormModel
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public DialogResult ShowDialog(out string username, out string password, string title = "Import Timetable",
            string acceptText = "Import")
        {
            Text = title;
            buttonImport.Text = acceptText;
            var result = ShowDialog();
            username = textBoxUsername.Text;
            password = textBoxPassword.Text;
            return result;
        }

        private void ButtonImportClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
using System;
using System.Windows.Forms;

namespace UniTimetable
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

        public DialogResult ShowDialog(out string username, out string password, string title = "Import Timetable")
        {
            Text = title;
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

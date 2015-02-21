#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniTimetable.Model.ImportExport.Login;

#endregion

namespace UniTimetable.ViewControllers
{
    public partial class FormLogin : FormModel
    {
        private readonly ILoginHandle _handle;
        private readonly IEnumerable<LoginField> _loginFields;
 
        public FormLogin(ref ILoginHandle loginHandle, string title)
        {
            InitializeComponent();
            _handle = loginHandle;
            _loginFields = loginHandle.GetLoginFields();
            SetupDialog(title);
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            SetHandleValues();
            Close();
        }

        private void ButtonImportClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            SetHandleValues();
            Close();
        }

        private void SetupDialog(string title)
        {
            Text = _handle.GetLoginPrompt();
            buttonImport.Text = _handle.GetLoginAction();
            labelPrivicy.Text = _handle.GetPrivacyPromise();
            labelTitle.Text = title;

            var i = 0;
            foreach (var loginField in _loginFields)
            {
                var label = new Label
                {
                    Text = loginField.Name,
                    Visible = true,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill
                };
                fieldsLayoutPanel.Controls.Add(label, 0, i);
                label.Show();
                Control ctrl = null;
                switch (loginField.Type)
                {
                    case LoginFieldType.String: ctrl = new TextBox {TextAlign = HorizontalAlignment.Center, AcceptsReturn = true}; break;
                    case LoginFieldType.PasswordString: ctrl = new TextBox { UseSystemPasswordChar = true, PasswordChar = '*', TextAlign = HorizontalAlignment.Center, AcceptsReturn = true }; break;
                    case LoginFieldType.Boolean:
                    {
                        ctrl = new CheckBox {ForeColor = BackColor};
                        ((CheckBox) ctrl).CheckedChanged += (sender, args) =>
                                                            {
                                                                ctrl.Text = ((CheckBox) ctrl).Checked.ToString();
                                                            };
                        break;
                    }
                    case LoginFieldType.List:
                        {
                            ctrl = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
                            if (loginField.Options == null)
                            {
                                throw new ArgumentNullException("LoginField.Options cannot be null.", new Exception());
                            }
                            ((ComboBox)ctrl).Items.AddRange(loginField.Options);
                            ((ComboBox)ctrl).SelectedIndex = 0;
                            break;
                        }
                }
                ctrl.TabIndex = i;
                ctrl.Dock = DockStyle.Fill;

                fieldsLayoutPanel.Controls.Add(ctrl, 1, i);
                i++;
            }
            ActiveControl = fieldsLayoutPanel.GetControlFromPosition(1, 0);
            buttonImport.TabIndex = i++;
            buttonCancel.TabIndex = i;
        }

        private void SetHandleValues()
        {
            var i = 0;
            foreach (var loginField in _loginFields)
            {
                loginField.Value = fieldsLayoutPanel.GetControlFromPosition(1, i++).Text;
            }
            _handle.SetLoginFields(_loginFields);
        }
    }
}
#region

using System;
using System.Threading;
using System.Windows.Forms;
using UniTimetable.Model.ImportExport;
using UniTimetable.Model.ImportExport.Login;
using UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury;
using UniTimetable.Model.Timetable;

#endregion

namespace UniTimetable.ViewControllers.Import
{
    partial class FormImport : FormModel
    {
        private int _currentPanel = -1;
        private Timetable _timetable;
        private readonly IImporter _importer;
        private readonly Thread _importerThread;
        private const string NextButtonText = "&Next >";
        private const string FinishButtonText = "&Finish";
        private const string CancelButtonText = "&Cancel";

        public FormImport(bool importUnselectable)
        {
            InitializeComponent();
            _importer = new CanterburyImporter { ImportUnselectableStreams = importUnselectable };
            _importerThread = new Thread(ImportThread);
            Next();
        }

        private void FormImportLoad(object sender, EventArgs e)
        {
            Import();
        }

        private void Import()
        {
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
                return;
            }

            _importerThread.Start();
        }

        private void ImportThread()
        {
            _timetable = _importer.ImportTimetable();
            Invoke(new MethodInvoker(delegate
            {
                if (_timetable != null)
                {
                    // build relational data
                    _timetable.BuildEquivalency();
                    _timetable.BuildCompatibility();

                    Next();
                }
                else
                {
                    MessageBox.Show("Failed to import timetable data.", "Import",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                }
            }));
        }

        private void SetPanel(Control control)
        {
            tableLayoutPanelImportContainer.Controls.Clear();
            control.Dock = DockStyle.Fill;
            control.Visible = true;
            tableLayoutPanelImportContainer.Controls.Add(control, 0, 0);
        }

        public new Timetable ShowDialog()
        {
            return base.ShowDialog() != DialogResult.OK ? null : _timetable;
        }

        private void ButtonNextClick(object sender, EventArgs e)
        {
            Next();
        }

        private void Next()
        {
            switch (_currentPanel)
            {
                case -1:
                {
                    buttonNext.Text = NextButtonText;
                    buttonNext.Enabled = false;
                    buttonCancel.Text = CancelButtonText;
                    buttonCancel.Enabled = false;
                    SetPanel(new ImportingPanel());
                    break;
                }
                case 0:
                {
                    buttonNext.Enabled = true;
                    buttonCancel.Enabled = true;
                    SetPanel(new PreviewPanel(_timetable));
                    break;
                }
                case 1:
                {
                    buttonNext.Text = FinishButtonText;
                    SetPanel(new StreamsPanel(_timetable));
                    break;
                }
                case 2:
                {
                    _timetable.Update();
                    DialogResult = DialogResult.OK;
                    Close();
                    break;
                }
            }
            _currentPanel++;
        }
    }
}
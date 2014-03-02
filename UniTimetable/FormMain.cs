using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using UniTimetable.Properties;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Drawing.Imaging;

namespace UniTimetable
{
    partial class FormMain : Form
    {
        TimeOfWeek _clickTime;
        Session _clickSession;
        Unavailability ClickUnavail_;

        Timetable Timetable_;
        Solver Solver_;

        Settings Settings_ = new Settings();

        FormStyle FormStyle_ = new FormStyle();
        FormUnavailability FormUnavail_ = new FormUnavailability();
        private FormImport FormImport_;
        FormSettings FormSettings_ = new FormSettings();
        AboutBox AboutBox_ = new AboutBox();

        History<Timetable> History_ = new History<Timetable>(50);
        int Changes_ = 0;
        TimetableControl Export_ = new TimetableControl();
        readonly Size ImageSize_ = new Size(1024, 768);

        SaveFileDialog SaveDialogXML_ = new SaveFileDialog();
        OpenFileDialog OpenDialogXML_ = new OpenFileDialog();
        SaveFileDialog SaveDialogRaster_ = new SaveFileDialog();
        SaveFileDialog SaveDialogVector_ = new SaveFileDialog();
        SaveFileDialog SaveDialogWallpaper_ = new SaveFileDialog();

        int egg = 0;
        Keys[] easter = new Keys[] {Keys.Up, Keys.Up, Keys.Down, Keys.Down, Keys.Left, Keys.Right, Keys.Left, Keys.Right, Keys.B, Keys.A, Keys.Enter};

        const int ListBoxMargin_ = 2;
        Cursor DragCursor_ = null;
        readonly Size DefaultSize_;
        bool SidePaneEnabled_ = true;

        Random Random_ = new Random((int)DateTime.Now.Ticks);


        public FormMain()
        {
            Timetable_ = null;
            Solver_ = new Solver(null);

            InitializeComponent();

            DefaultSize_ = Size;

            // do anchoring here to make form design easier
            tableLayoutPanel2.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            timetableControl1.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            tableLayoutPanel1.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            //tableLayoutPanel1.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            Export_.Width = 800;
            Export_.Height = 600;
            Export_.ShowAll = false;
            Export_.ShowDays = true;
            Export_.ShowGrayArea = false;
            Export_.ShowLocation = true;
            Export_.ShowText = true;
            Export_.ShowTimes = true;
            Export_.Grayscale = false;

            SaveDialogXML_.AddExtension = true;
            SaveDialogXML_.CheckFileExists = false;
            SaveDialogXML_.OverwritePrompt = true;
            SaveDialogXML_.Title = "Save Timetable";
            SaveDialogXML_.Filter = "XML File (*.xml)|*.xml";

            OpenDialogXML_.Multiselect = false;
            OpenDialogXML_.AddExtension = true;
            OpenDialogXML_.CheckFileExists = true;
            OpenDialogXML_.ShowReadOnly = false;
            OpenDialogXML_.Title = "Load Timetable";
            OpenDialogXML_.Filter = "XML File (*.xml)|*.xml";

            SaveDialogRaster_.AddExtension = true;
            SaveDialogRaster_.CheckFileExists = false;
            SaveDialogRaster_.OverwritePrompt = true;
            SaveDialogRaster_.Title = "Save Image";
            SaveDialogRaster_.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg, *.jpeg)|*.jpg;*.jpeg|GIF Image (*.gif)|*.gif";

            SaveDialogVector_.AddExtension = true;
            SaveDialogVector_.CheckFileExists = false;
            SaveDialogVector_.OverwritePrompt = true;
            SaveDialogVector_.Title = "Save Image";
            SaveDialogVector_.Filter = "Enhanced Metafile (*.emf)|*.emf";

            SaveDialogWallpaper_.AddExtension = true;
            SaveDialogWallpaper_.CheckFileExists = false;
            SaveDialogWallpaper_.OverwritePrompt = true;
            SaveDialogWallpaper_.Title = "Save Image";
            SaveDialogWallpaper_.Filter = "Bitmap Image (*.bmp)|*.bmp";
            SaveDialogWallpaper_.FileName = "wallpaper";

            //pageSetupDialog1.PageSettings = printDocument1.DefaultPageSettings;
            pageSetupDialog.PageSettings.Landscape = true;
            pageSetupDialog.PageSettings.Margins = new Margins(10 * 1000 / 254, 10 * 1000 / 254, 10 * 1000 / 254, 10 * 1000 / 254);

            FitToScreen();

            EnableButtons(false);

            UpdateSettings();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AreYouSure("Close"))
                e.Cancel = true;
        }

        private void timetableControl1_TimetableChanged(object sender)
        {
            MadeChanges(true);
        }

        private bool TimetableLoaded()
        {
            return (Timetable_ != null && Timetable_.HasData());
        }

        #region Menu strip

        #region Saving and opening

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New();
        }

        private void New()
        {
            if (!AreYouSure("Clear Timetable"))
                return;
            Timetable_ = null;
            timetableControl1.Timetable = Timetable_;
            SaveDialogXML_.FileName = null;
            EnableButtons(false);
            ClearHistory();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private DialogResult Save()
        {
            if (Timetable_ == null)
                return DialogResult.Cancel;

            if (SaveDialogXML_.FileName == null || !File.Exists(SaveDialogXML_.FileName))
            {
                DialogResult result = SaveDialogXML_.ShowDialog();
                if (result != DialogResult.OK)
                    return result;
            }

            SaveToFile(SaveDialogXML_.FileName);
            return DialogResult.OK;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private DialogResult SaveAs()
        {
            if (Timetable_ == null)
                return DialogResult.Cancel;

            DialogResult result = SaveDialogXML_.ShowDialog();
            if (result != DialogResult.OK)
                return result;

            SaveToFile(SaveDialogXML_.FileName);
            return DialogResult.OK;
        }

        private void SaveToFile(string filename)
        {
            XmlSerializer s = new XmlSerializer(typeof(Timetable));
            TextWriter w = new StreamWriter(SaveDialogXML_.FileName, false);
            s.Serialize(w, Timetable_);
            w.Close();

            Changes_ = 0;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void Open()
        {
            if (!AreYouSure("Open Timetable"))
                return;

            if (OpenDialogXML_.ShowDialog() != DialogResult.OK)
                return;

            XmlSerializer s = new XmlSerializer(typeof(Timetable));
            TextReader r;
            try
            {
                r = new StreamReader(OpenDialogXML_.FileName);
            }
            catch
            {
                MessageBox.Show("Could not open specified file!", "Open Timetable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Timetable t;
            try
            {
                t = (Timetable)s.Deserialize(r);
            }
            catch
            {
                MessageBox.Show("Failed to load saved data!", "Open Timetable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                r.Close();
                return;
            }
            r.Close();
            SaveDialogXML_.FileName = OpenDialogXML_.FileName;

            // build relationships and translate tree to lists
            foreach (Subject subject in t.SubjectList)
            {
                t.TypeList.AddRange(subject.Types);
                foreach (Type type in subject.Types)
                {
                    t.StreamList.AddRange(type.Streams);
                    type.Subject = subject;
                    foreach (Stream stream in type.Streams)
                    {
                        t.ClassList.AddRange(stream.Classes);
                        stream.Type = type;
                        foreach (Session session in stream.Classes)
                        {
                            session.Stream = stream;
                        }
                    }
                }
            }

            t.BuildEquivalency();
            t.BuildCompatibility();

            Timetable_ = t;
            timetableControl1.Timetable = Timetable_;
            timetableControl1.MatchBounds();
            ClearHistory();
            EnableButtons(true);
        }

        #endregion

        #region Exporting images

        private void LoadExportSettings()
        {
            Export_.Timetable = Timetable_;
            Export_.HourStart = timetableControl1.HourStart;
            Export_.HourEnd = timetableControl1.HourEnd;
            Export_.ShowWeekend = timetableControl1.ShowWeekend;
        }

        private void colourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveDialogRaster_.ShowDialog() != DialogResult.OK)
                return;

            LoadExportSettings();
            Export_.Size = ImageSize_;
            Export_.Grayscale = false;
            Export_.SaveRaster(SaveDialogRaster_.FileName);
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveDialogRaster_.ShowDialog() != DialogResult.OK)
                return;

            LoadExportSettings();
            Export_.Size = ImageSize_;
            Export_.Grayscale = true;
            Export_.SaveRaster(SaveDialogRaster_.FileName);
        }

        private void wallpaperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*if (SaveDialogWallpaper_.ShowDialog() != DialogResult.OK)
                return;

            int end = SaveDialogWallpaper_.FileName.LastIndexOf('.');
            string fileName;
            if (end == -1)
                fileName = SaveDialogWallpaper_.FileName;
            else
                fileName = SaveDialogWallpaper_.FileName.Substring(0, end);
            fileName += ".bmp";*/

            const float opacity = 0.75f;

            Rectangle screen = Screen.GetBounds(this);
            Rectangle free = Screen.GetWorkingArea(this);

            LoadExportSettings();
            Export_.Grayscale = false;
            const int margin = 100;
            Export_.Size = free.Size - new Size(2 * margin, 2 * margin);
            free.Offset(margin, margin);

            Graphics g;
            Bitmap overlay = new Bitmap(screen.Width, screen.Height);
            g = Graphics.FromImage(overlay);
            Export_.DrawTimetable(g);
            g.Dispose();

            Bitmap background = GetWallpaperBitmap();
            g = Graphics.FromImage(background);
            // Drawing Transparent Images
            // http://www.vbdotnetheaven.com/UploadFile/mahesh/TransparentImagesShapes04212005052247AM/TransparentImagesShapes.aspx
            ImageAttributes attr = new ImageAttributes();
            attr.SetColorMatrix(new ColorMatrix(new float[][] {
                new float[] {1, 0, 0, 0,       0},
                new float[] {0, 1, 0, 0,       0},
                new float[] {0, 0, 1, 0,       0},
                new float[] {0, 0, 0, opacity, 0},
                new float[] {0, 0, 0, 0,       1}}),
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
            g.TranslateTransform(free.X, free.Y);
            g.DrawImage(overlay, screen, 0, 0, screen.Width, screen.Height, GraphicsUnit.Pixel, attr);
            g.Dispose();

            string dir = GetUserDirectory();
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string fileName = Path.Combine(GetUserDirectory(), "Wallpaper1.bmp");
            try
            {
                Export_.SaveRaster(fileName, background);
            }
            catch
            {
                fileName = Path.Combine(GetUserDirectory(), "Wallpaper2.bmp");
                try
                {
                    Export_.SaveRaster(fileName, background);
                }
                catch
                {
                    MessageBox.Show("Sorry, an error occurred when trying to save the image.", "Save Image", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            SetWallpaper(fileName, 2, 0);
        }

        private string GetUserDirectory()
        {
            return Application.CommonAppDataPath;
        }

        // http://www.geekpedia.com/tutorial209_Setting-and-Retrieving-the-Desktop-Wallpaper.html

        private string GetWallpaperPath()
        {
            RegistryKey rkWallpaper = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);
            string wallpaper = rkWallpaper.GetValue("Wallpaper").ToString();
            rkWallpaper.Close();
            return wallpaper;
        }

        private Bitmap GetWallpaperBitmap()
        {
            string wallpaper = GetWallpaperPath();
            Rectangle screen = Screen.GetBounds(this);

            // have to get around indexed images, pieced together from:
            // http://www.c-sharpcorner.com/UploadFile/rrraman/graphicsObject08232007102733AM/graphicsObject.aspx
            // http://www.eggheadcafe.com/PrintSearchContent.asp?LINKID=799
            Bitmap b = new Bitmap(wallpaper);
            MemoryStream stream = new MemoryStream();
            b.Save(stream, ImageFormat.Bmp);
            Image image = Image.FromStream(stream);
            b = new Bitmap(image);

            Bitmap ret = new Bitmap(screen.Width, screen.Height);
            Graphics g = Graphics.FromImage(ret);
            g.DrawImage(b, screen, new Rectangle(0, 0, b.Width, b.Height), GraphicsUnit.Pixel);
            g.Dispose();

            return ret;
        }

        private void SetWallpaper(string path, int style, int tile)
        {
            WinAPI.SystemParametersInfo(20, 0, path, 0x01 | 0x02);

            RegistryKey rkWallPaper = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
            rkWallPaper.SetValue("WallpaperStyle", style);
            rkWallPaper.SetValue("TileWallpaper", tile);
            rkWallPaper.Close();
        }

        private void colourVectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveDialogVector_.ShowDialog() != DialogResult.OK)
                return;

            LoadExportSettings();
            Export_.Size = ImageSize_;
            Export_.Grayscale = false;
            Export_.SaveVector(SaveDialogVector_.FileName);
        }

        private void greyscaleVectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveDialogVector_.ShowDialog() != DialogResult.OK)
                return;

            LoadExportSettings();
            Export_.Size = ImageSize_;
            Export_.Grayscale = true;
            Export_.SaveVector(SaveDialogVector_.FileName);
        }

        #endregion

        #region Printing

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void Print()
        {
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                bool keepTrying = true;
                while (keepTrying)
                {
                    keepTrying = false;
                    try
                    {
                        printDocument.Print();
                    }
                    catch (Exception exception)
                    {
                        keepTrying = MessageBox.Show("Error occured in printing:\n\n" + exception.Message, "Print Timetable", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry;
                    }
                }
            }
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                printPreviewDialog.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error occured in printing:\n\n" + exception.Message, "Print Timetable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pageSetupDialog.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle marginBounds = e.MarginBounds;
            if (!printDocument.PrintController.IsPreview)
                marginBounds.Offset(-(int)e.PageSettings.HardMarginX, -(int)e.PageSettings.HardMarginY);

            LoadExportSettings();
            Export_.Size = marginBounds.Size;
            Export_.Grayscale = (!e.PageSettings.PrinterSettings.SupportsColor || MessageBox.Show("Print in colour?", "Print Timetable", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No);
            Export_.Timetable = Timetable_;
            g.TranslateTransform(marginBounds.X, marginBounds.Y);
            Export_.DrawTimetable(g);
        }

        #endregion

        #region Importing

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Import();
        }

        private void ImportNew()
        {
            if (!AreYouSure("Import Timetable"))
                return;

            var importForm = new FormImport();
            if (importForm.DialogResult == DialogResult.Cancel)
            {
                return;
            }

            var t = importForm.ShowDialog();
            if (t == null)
                return;

            Timetable_ = t;
            timetableControl1.Timetable = Timetable_;
            timetableControl1.MatchBounds();
            SaveDialogXML_.FileName = null;
            EnableButtons(true);
            ClearHistory();
        }

        private void Import()
        {
            if (!AreYouSure("Import Timetable"))
                return;

            // run the import wizard
            Timetable t = FormImport_.ShowDialog();
            if (t == null)
                return;

            Timetable_ = t;
            timetableControl1.Timetable = Timetable_;
            timetableControl1.MatchBounds();
            SaveDialogXML_.FileName = null;
            EnableButtons(true);
            ClearHistory();
        }

        private void importAndMergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // run the import wizard
            Timetable t = FormImport_.ShowDialog();
            if (t == null)
                return;

            Timetable_.MergeWith(t);
            timetableControl1.Invalidate();
            EnableButtons(true);
        }

        #endregion

        #region Undo and redo

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void Undo()
        {
            Timetable t = History_.Back();
            if (t == null)
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }

            Changes_--;
            Timetable_ = t.DeepCopy();
            timetableControl1.Timetable = Timetable_;
            Timetable_.RecomputeSolutions = true;
            UpdateRemaining();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void Redo()
        {
            Timetable t = History_.Forward();
            if (t == null)
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }

            Changes_++;
            Timetable_ = t.DeepCopy();
            timetableControl1.Timetable = Timetable_;
            UpdateRemaining();
        }

        private void ClearHistory()
        {
            Changes_ = 0;
            History_.Clear();
            if (Timetable_ != null)
                History_.Add(Timetable_.DeepCopy());

            UpdateRemaining();
        }

        private void MadeChanges(bool recompute)
        {
            Changes_++;
            History_.Add(Timetable_.DeepCopy());
            timetableControl1.NoCrazy();
            timetableControl1.Invalidate();
            if (recompute)
                Timetable_.RecomputeSolutions = true;
            UpdateRemaining();
        }

        #endregion

        #region Timetable config

        private void coloursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Timetable_ == null)
                return;

            if (FormStyle_.ShowDialog(Timetable_) == DialogResult.Cancel)
                return;

            MadeChanges(false);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings_.ResetWindow = false;
            Settings result = FormSettings_.ShowDialog(Settings_);
            if (result == null)
                return;
            Settings_ = result;
            Settings_.Save();
            UpdateSettings();
        }

        private void timetableControl1_BoundsClipped(object sender)
        {
            Settings_.HourStart = timetableControl1.HourStart;
            Settings_.HourEnd = timetableControl1.HourEnd;
            Settings_.Save();
        }

        private void UpdateSettings()
        {
            UpdateToolstrip();
            timetableControl1.ShowDragGhost = Settings_.ShowGhost;
            timetableControl1.ShowWeekend = Settings_.ShowWeekend;
            timetableControl1.ShowGrayArea = Settings_.ShowGray;
            timetableControl1.ShowLocation = Settings_.ShowLocation;
            timetableControl1.SetBounds(Settings_.HourStart, Settings_.HourEnd);

            if (Settings_.ResetWindow)
            {
                ResetWindow();
            }
        }

        #endregion

        #region Solver

        private void findSolutionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindSolutions();
        }

        private void FindSolutions()
        {
            if (Timetable_ == null)
                return;

            if (Timetable_.IsFull())
            {
                MessageBox.Show("There are no remaining streams to solve for!", "Find Solutions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Solver_.Timetable = Timetable_;
            FormProgress formProgress = new FormProgress();
            if (formProgress.ShowDialog(Solver_) != DialogResult.OK)
                return;

            FormSolution formSolution = new FormSolution();
            if (formSolution.ShowDialog(Solver_) != DialogResult.OK)
                return;

            MadeChanges(true);
        }

        private void editSolutionCriteriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditCriteria();
        }

        private void EditCriteria()
        {
            if (Timetable_ == null)
                return;

            FormCriteria formCriteria = new FormCriteria();
            if (formCriteria.ShowDialog(Solver_) != DialogResult.OK)
                return;
            Timetable_.RecomputeSolutions = true;
        }

        #endregion

        #region Help

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox_.ShowDialog();
        }

        private void utmHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string address = "http://jack.valmadre.net/timetable/";
            try
            {
                System.Diagnostics.Process.Start(address);
            }
            catch
            {
                MessageBox.Show(
                    "Unable to open web page.\n\nPlease point your browser to " + address,
                    "Online Help",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        #endregion

        #region Drop down disablers

        private void timetableToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = (History_.PeekBack() != null);
            redoToolStripMenuItem.Enabled = (History_.PeekForward() != null);

            importAndMergeToolStripMenuItem.Enabled = TimetableLoaded();
            coloursToolStripMenuItem.Enabled = TimetableLoaded();
        }

        private void timetableToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = true;
            importAndMergeToolStripMenuItem.Enabled = true;
            coloursToolStripMenuItem.Enabled = true;
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            saveToolStripMenuItem.Enabled = TimetableLoaded();
            saveAsToolStripMenuItem.Enabled = TimetableLoaded();
        }

        private void fileToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            saveToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;
        }

        private void optimiseToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            findSolutionsToolStripMenuItem.Enabled = TimetableLoaded();
            editSolutionCriteriaToolStripMenuItem.Enabled = TimetableLoaded();
        }

        private void optimiseToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            findSolutionsToolStripMenuItem.Enabled = true;
            editSolutionCriteriaToolStripMenuItem.Enabled = true;
        }

        #endregion

        #endregion

        #region Tool strip

        private void EnableButtons(bool enable)
        {
            btnCriteria.Enabled = enable;
            btnSolver.Enabled = enable;
            btnLucky.Enabled = enable;
            btnClear.Enabled = enable;
        }

        private void UpdateToolstrip()
        {
            bool large = Settings_.UseLargeIcons;

            btnNew.Image = (large ? Resources.Document : Resources.Document16);
            btnImport.Image = (large ? Resources.Download : Resources.Download16);
            btnOpen.Image = (large ? Resources.My_Documents : Resources.My_Documents16);
            btnSave.Image = (large ? Resources.Save : Resources.Save16);
            btnPrint.Image = (large ? Resources.Printer : Resources.Printer16);
            btnCriteria.Image = (large ? Resources.Config_Tools : Resources.Config_Tools16);
            btnSolver.Image = (large ? Resources.Find : Resources.Find16);
            btnLucky.Image = (large ? Resources.Dice : Resources.Dice16);
            btnAddByClass.Image = (large ? Resources.Symbol_Add_Stream : Resources.Symbol_Add_Stream16);
            btnClear.Image = (large ? Resources.Trashcan_full : Resources.Trashcan_full16);

            for (var i = 0; i < toolStrip1.Items.Count; i++)
            {
                ToolStripButton item;
                try
                {
                    item = (ToolStripButton)toolStrip1.Items[i];
                }
                catch
                {
                    continue;
                }

                if (large)
                {
                    item.Padding = new Padding(3, item.Padding.Top, 3, item.Padding.Bottom);
                    item.TextImageRelation = TextImageRelation.ImageAboveText;
                }
                else
                {
                    item.Padding = new Padding(0, item.Padding.Top, 0, item.Padding.Bottom);
                    item.TextImageRelation = TextImageRelation.ImageBeforeText;
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            New();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Import();
        }

        private void btnImportNew_Click(object sender, EventArgs e)
        {
            ImportNew();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void btnCriteria_Click(object sender, EventArgs e)
        {
            EditCriteria();
        }

        private void btnSolver_Click(object sender, EventArgs e)
        {
            FindSolutions();
        }

        private void btnLucky_Click(object sender, EventArgs e)
        {
            FeelingLucky();
        }

        private void FeelingLucky()
        {
            if (Timetable_ == null)
                return;

            if (Timetable_.IsFull())
                Timetable_.RevertToBaseStreams();

            Solver_.Timetable = Timetable_;
            FormProgress formProgress = new FormProgress();
            if (formProgress.ShowDialog(Solver_) != DialogResult.OK)
                return;

            int index = Random_.Next(Math.Min(100, Solver_.Solutions.Count));
            Timetable_.LoadSolution(Solver_.Solutions[index]);
            MadeChanges(true);
        }

        private void btnAddByClass_Click(object sender, EventArgs e)
        {
            /*ContextMenuStrip contextMenu = new ContextMenuStrip();
            List<ToolStripMenuItem> required = new List<ToolStripMenuItem>();
            List<ToolStripMenuItem> ignored = new List<ToolStripMenuItem>();
            foreach (Subject subject in Timetable_.SubjectList)
            {
                foreach (Type type in subject.Types)
                {
                    ToolStripMenuItem typeItem = new ToolStripMenuItem(subject.ToString() + " " + type.Name);
                    typeItem.Checked = (type.SelectedStream != null);
                    if (type.Required)
                        required.Add(typeItem);
                    else
                        ignored.Add(typeItem);

                    //typeItem.DropDownOpening +=

                    for (int i = 0; i < type.Streams.Count; i++)
                    {
                        Stream stream = type.Streams[i];
                        ToolStripMenuItem streamItem = new ToolStripMenuItem(stream.ToString());
                        streamItem.Checked = stream.Selected;
                        if (!m_Timetable.Fits(stream))
                            streamItem.ForeColor = Color.Gray;

                        typeItem.DropDownItems.Add(streamItem);
                        streamItem.MouseEnter += delegate
                        {
                            timetableControl1.PreviewAlt(stream);
                        };
                        streamItem.MouseLeave += delegate
                        {
                            timetableControl1.EndPreviewStream();
                        };
                        streamItem.Click += delegate
                        {
                            Timetable_.SelectStream(stream);
                            MadeChanges();
                        };
                    }
                }
            }

            if (required.Count == 0 && ignored.Count == 0)
                return;
            contextMenu.Items.AddRange(required.ToArray());
            if (required.Count != 0 && ignored.Count != 0)
                contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.AddRange(ignored.ToArray());
            contextMenu.Show(toolStrip1, new Point(0, btnAddByClass.Height));*/

            ToggleSidePane();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (Timetable_.RevertToBaseStreams())
            {
                timetableControl1.EndPreviewStream();
                timetableControl1.EndPreviewOptions();
                MadeChanges(true);
            }
        }

        #endregion

        #region Keyboard input

        private void EasterEggStep(Keys key)
        {
            if (key == easter[egg])
            {
                egg++;
                if (egg == easter.Length)
                {
                    timetableControl1.GoCrazy();
                    egg = 0;
                    return;
                }
            }
            else
            {
                egg = 0;
            }
            timetableControl1.NoCrazy();
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            /*if ((int)m.WParam >= (int)'a' && (int)m.WParam <= (int)'z')
                return base.ProcessKeyPreview(ref m);*/

            Keys keyData = (Keys)(int)m.WParam;
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    EasterEggStep(keyData);
                    break;
            }
            return base.ProcessKeyPreview(ref m);
        }

        // override required to process arrow keys for easter egg
        protected override bool ProcessDialogKey(Keys keyData)
        {
            EasterEggStep(keyData);
            return base.ProcessDialogKey(keyData);
        }

        #endregion

        #region Right click menus

        private void FindClickDetails(TimetableEventArgs e)
        {
            _clickTime = e.Time;
            if (Timetable_ == null)
            {
                _clickSession = null;
                ClickUnavail_ = null;
            }
            else
            {
                _clickSession = Timetable_.FindClassAt(_clickTime, true);
                ClickUnavail_ = Timetable_.FindUnavailableAt(_clickTime);
            }
        }

        private void timetableControl1_TimetableMouseClick(object sender, TimetableEventArgs e)
        {
            FindClickDetails(e);
            
            if (e.Button == MouseButtons.Right)
            {
                // no current timetable
                if (Timetable_ == null)
                {
                    findClassHereToolStripMenuItem.Enabled = false;
                    unavailabilityToolStripMenuItem.Enabled = false;
                    timeMenu.Show(timetableControl1, e.Location);
                }

                // right clicked empty space?
                else if (_clickSession == null && ClickUnavail_ == null)
                {
                    findClassHereToolStripMenuItem.DropDownItems.Clear();

                    // populate list of options at this time
                    List<ToolStripMenuItem> required = new List<ToolStripMenuItem>();
                    List<ToolStripMenuItem> ignored = new List<ToolStripMenuItem>();
                    foreach (Subject subject in Timetable_.SubjectList)
                    {
                        foreach (Type type in subject.Types)
                        {
                            for (int i = 0; i < type.UniqueStreams.Count; i++)
                            {
                                Stream stream = type.UniqueStreams[i];
                                bool atTime = false;
                                foreach (Session session in stream.Classes)
                                {
                                    if (_clickTime >= session.Start && _clickTime <= session.End)
                                    {
                                        atTime = true;
                                        break;
                                    }
                                }
                                if (!atTime)
                                    continue;

                                ToolStripMenuItem item = new ToolStripMenuItem(stream.Type.Subject.ToString() + " " + stream.ToString());
                                item.MouseEnter += delegate { timetableControl1.PreviewAlt(stream); };
                                item.MouseLeave += delegate { timetableControl1.EndPreviewStream(); };
                                item.Click += delegate
                                {
                                    if (!Timetable_.SelectStream(stream))
                                        return;
                                    MadeChanges(true);
                                };
                                if (!Timetable_.Fits(stream))
                                    item.ForeColor = Color.Gray;

                                if (type.Required)
                                    required.Add(item);
                                else
                                    ignored.Add(item);
                            }
                        }
                    }
                    if (required.Count > 0)
                    {
                        findClassHereToolStripMenuItem.DropDownItems.AddRange(required.ToArray());
                        if (ignored.Count > 0)
                            findClassHereToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
                    }
                    if (ignored.Count > 0)
                    {
                        findClassHereToolStripMenuItem.DropDownItems.AddRange(ignored.ToArray());
                    }
                    findClassHereToolStripMenuItem.Enabled = (required.Count > 0 || ignored.Count > 0);
                    unavailabilityToolStripMenuItem.Enabled = true;

                    timeMenu.Show(timetableControl1, e.Location);
                }

                // right clicked a session
                else if (_clickSession != null)
                {
                    alternativeToolStripMenuItem.DropDownItems.Clear();
                    equivalentToolStripMenuItem.DropDownItems.Clear();

                    timetableControl1.SetActive(_clickSession.Stream);

                    // populate alternative options menu
                    // NOTE: lambda-style delegates do not work with foreach
                    int n = 0;
                    for (int i = 0; i < _clickSession.Stream.Type.UniqueStreams.Count; i++)
                    {
                        Stream alt = _clickSession.Stream.Type.UniqueStreams[i];
                        if (alt == _clickSession.Stream || _clickSession.Stream.Equivalent.Contains(alt))
                            continue;
                        ToolStripMenuItem item = new ToolStripMenuItem(alt.ToString());
                        item.MouseEnter += delegate { timetableControl1.PreviewAlt(alt); };
                        item.MouseLeave += delegate { timetableControl1.EndPreviewStream(); };
                        item.Click += delegate
                        {
                            if (!Timetable_.SelectStream(alt))
                                return;
                            MadeChanges(true);
                        };
                        if (!Timetable_.Fits(alt))
                            item.ForeColor = Color.Gray;

                        alternativeToolStripMenuItem.DropDownItems.Add(item);
                        n++;
                    }
                    alternativeToolStripMenuItem.Enabled = (n != 0);

                    // populate equivalent options menu
                    n = 0;
                    for (int i = 0; i < _clickSession.Stream.Equivalent.Count; i++)
                    {
                        Stream equiv = _clickSession.Stream.Equivalent[i];
                        ToolStripMenuItem item = new ToolStripMenuItem(equiv.ToString());
                        item.MouseEnter += delegate { timetableControl1.PreviewEquiv(equiv); };
                        item.MouseLeave += delegate { timetableControl1.EndPreviewStream(); };
                        item.Click += delegate
                        {
                            if (!Timetable_.SelectStream(equiv))
                                return;
                            MadeChanges(true);
                        };
                        equivalentToolStripMenuItem.DropDownItems.Add(item);
                        n++;
                    }
                    equivalentToolStripMenuItem.Enabled = (n != 0);
                    streamMenu.Show(timetableControl1, e.Location);
                }

                // right clicked an unavailability
                else
                {
                    timetableControl1.SetActive(ClickUnavail_);

                    unavailableMenu.Show(timetableControl1, e.Location);
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_clickSession == null)
                return;
            _clickSession.Stream.Selected = false;
            MadeChanges(true);
        }

        private void unavailabilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormUnavail_.ShowDialog(Timetable_, new Timeslot(_clickTime.Day, _clickTime.Hour, 0, _clickTime.Hour + 1, 0), timetableControl1.HourStart, timetableControl1.HourEnd) == DialogResult.Cancel)
                return;
            MadeChanges(true);
        }

        private void streamMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            timetableControl1.ClearActive();
        }

        private void timetableControl1_TimetableMouseDoubleClick(object sender, TimetableEventArgs e)
        {
            // disable doucle click edit for now - conflicts with internal mouseup for dragging
            //FindClickDetails(e);
            //EditUnavailable();
        }

        private void editUnavailableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // click details already established by right click
            EditUnavailable();
        }

        private void EditUnavailable()
        {
            if (ClickUnavail_ != null)
            {
                if (FormUnavail_.ShowDialog(Timetable_, ClickUnavail_, timetableControl1.HourStart, timetableControl1.HourEnd) == DialogResult.Cancel)
                    return;
                MadeChanges(true);
            }
        }

        private void removeUnavailableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClickUnavail_ != null)
            {
                Timetable_.UnavailableList.Remove(ClickUnavail_);
                MadeChanges(true);
            }
        }

        private void unavailableMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            timetableControl1.ClearActive();
        }

        #endregion

        private bool AreYouSure(string caption)
        {
            if (Timetable_ == null || Changes_ == 0)
                return true;
            DialogResult choice = MessageBox.Show(
                "Would you like to save the changes?",
                caption,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3);
            if (choice == DialogResult.Cancel)
                return false;
            if (choice == DialogResult.No)
                return true;

            choice = Save();
            return (choice == DialogResult.OK);
        }

        #region Listboxes

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            e.DrawBackground();
            e.DrawFocusRectangle();

            ListBox listBox = (ListBox)sender;
            Graphics g = e.Graphics;
            Type type = (Type)listBox.Items[e.Index];

            Rectangle r = new Rectangle(e.Bounds.X + ListBoxMargin_, e.Bounds.Y + ListBoxMargin_, e.Bounds.Width - 2 * ListBoxMargin_ - 1, e.Bounds.Height - 2 * ListBoxMargin_ - 1);
            g.FillRectangle(TimetableControl.LinearGradient(r.Location, r.Width, r.Height, type.Subject.Color), r);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            g.DrawString(type.Subject.ToString() + " " + type.Code, listBox1.Font, Brushes.Black, r, format);

            g.DrawRectangle(Pens.Black, r);
        }

        private void listBox1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (timetableControl1.ShowDragGhost && DragCursor_ != null)
            {
                e.UseDefaultCursors = false;
                Cursor.Current = DragCursor_;
            }
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            ListBox listBox = (ListBox)sender;
            int index = listBox.IndexFromPoint(e.Location);
            if (index == -1)
            {
                listBox.SelectedIndex = -1;
                return;
            }

            Type type = (Type)listBox.SelectedItem;
            DragCursor_ = timetableControl1.DragCursor(type.FindShortestSession());
            timetableControl1.BeginDrag(type);
            DragDropEffects result = listBox.DoDragDrop(type, DragDropEffects.Move);
            timetableControl1.EndDrag();

            if (result != DragDropEffects.Move || listBox.SelectedItem == type)
                timetableControl1.PreviewOptions(type);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedIndex == -1)
            {
                timetableControl1.EndPreviewOptions();
                return;
            }
            Type type = (Type)listBox.SelectedItem;
            timetableControl1.PreviewOptions(type);
        }

        private void listBox1_Leave(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            listBox.SelectedIndex = -1;
            timetableControl1.EndPreviewOptions();
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Type)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            Type type = (Type)e.Data.GetData(typeof(Type));
            if (type.Required != true || type.SelectedStream != null)
            {
                type.Required = true;
                if (type.SelectedStream != null)
                    type.SelectedStream.Selected = false;

                MadeChanges(true);
                UpdateRemaining();
            }
        }

        private void listBox2_DragDrop(object sender, DragEventArgs e)
        {
            Type type = (Type)e.Data.GetData(typeof(Type));
            if (type.Required != false || type.SelectedStream != null)
            {
                type.Required = false;
                if (type.SelectedStream != null)
                    type.SelectedStream.Selected = false;

                MadeChanges(true);
                UpdateRemaining();
            }
        }

        private void timetableControl1_DragOver(object sender, DragEventArgs e)
        {
            Type type = (Type)e.Data.GetData(typeof(Type));
            TimeOfWeek time = timetableControl1.FindClickTime(timetableControl1.PointToClient(new Point(e.X, e.Y)));
            Stream hoverStream = FindStream(type, time);

            if (hoverStream != null)
            {
                timetableControl1.PreviewEquiv(hoverStream);
            }
            else
            {
                timetableControl1.EndPreviewStream();
            }
        }

        private void timetableControl1_DragDrop(object sender, DragEventArgs e)
        {
            Type type = (Type)e.Data.GetData(typeof(Type));
            TimeOfWeek time = timetableControl1.FindClickTime(timetableControl1.PointToClient(new Point(e.X, e.Y)));
            Stream hoverStream = FindStream(type, time);

            timetableControl1.EndPreviewStream();
            if (hoverStream != null)
            {
                if (Timetable_.SelectStream(hoverStream))
                    MadeChanges(true);
            }
        }

        private Stream FindStream(Type type, TimeOfWeek time)
        {
            return Timetable.From(type).FindClassAt(time, false).Stream;
        }

        private void UpdateRemaining()
        {
            if (Timetable_ == null)
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                //groupBox2.Text = "Remaining";
                //groupBox3.Text = "Ignored";
                return;
            }

            List<Type> remaining = new List<Type>();
            List<Type> ignored = new List<Type>();
            foreach (Subject subject in Timetable_.SubjectList)
            {
                foreach (Type type in subject.Types)
                {
                    if (type.SelectedStream != null)
                        continue;
                    if (!type.Required)
                    {
                        ignored.Add(type);
                    }
                    else
                    {
                        remaining.Add(type);
                    }
                }
            }
            listBox1.Items.Clear();
            listBox1.Items.AddRange(remaining.ToArray());
            listBox2.Items.Clear();
            listBox2.Items.AddRange(ignored.ToArray());

            timetableControl1.EndPreviewStream();
            timetableControl1.EndPreviewOptions();

            //groupBox2.Text = "Remaining (" + remaining.Count.ToString() + ")";
            //groupBox3.Text = "Ignored (" + ignored.Count.ToString() + ")";
        }

        #endregion

        private void timetableControl1_ResizeCell(object sender)
        {
            if (timetableControl1.CellSize.Height != 0)
            {
                listBox1.ItemHeight = listBox2.ItemHeight = Math.Min(timetableControl1.CellSize.Height + 2 * ListBoxMargin_, 255);
                listBox1.Font = listBox2.Font = timetableControl1.Font;
                listBox1.Invalidate();
                listBox2.Invalidate();
            }
        }

        #region Side pane

        private void btnShowHide_Click(object sender, EventArgs e)
        {
            btnAddByClass.Checked = !btnAddByClass.Checked;
            ToggleSidePane();
        }

        private void ToggleSidePane()
        {
            if (SidePaneEnabled_)
            {
                btnShowHide.Text = "»";
                btnAddByClass.Checked = false;
                HideSidePane();
            }
            else
            {
                btnShowHide.Text = "«";
                btnAddByClass.Checked = true;
                ShowSidePane();
            }
        }

        private void ShowSidePane()
        {
            if (SidePaneEnabled_)
                return;
            SidePaneEnabled_ = true;

            if (WindowState == FormWindowState.Maximized)
            {
                tableLayoutPanel2.ColumnCount = 3;
                return;
            }

            int prev = groupBox1.Width;

            int middle = tableLayoutPanel2.Width + tableLayoutPanel1.Margin.Horizontal;
            int left = groupBox1.Width + groupBox1.Margin.Horizontal;
            int old = middle + left;
            
            float fraction = tableLayoutPanel2.ColumnStyles[0].Width / 100f;
            int next = (int)Math.Ceiling((float)left / fraction) + middle;
            Width = Width - old + next;
            tableLayoutPanel2.ColumnCount = 3;

            listBox1.TabStop = true;
            listBox2.TabStop = true;
        }

        private void HideSidePane()
        {
            if (!SidePaneEnabled_)
                return;
            SidePaneEnabled_ = false;

            if (WindowState == FormWindowState.Maximized)
            {
                tableLayoutPanel2.ColumnCount = 2;
                return;
            }

            int panel = tableLayoutPanel3.Width + tableLayoutPanel3.Margin.Horizontal;
            tableLayoutPanel2.ColumnCount = 2;
            Width = Width - panel;

            listBox1.TabStop = false;
            listBox2.TabStop = false;
        }

        #endregion

        private void ResetWindow()
        {
            ShowSidePane();
            Size = DefaultSize_;
            FitToScreen();
        }

        private void FitToScreen()
        {
            Size screen = Screen.GetWorkingArea(this).Size;
            if (Height > screen.Height)
            {
                Properties.Settings.Default.LargeIcons = false;
                Properties.Settings.Default.Save();
                UpdateSettings();
                //if (Height > screen.Height)
                    Height = screen.Height;
            }
            if (Width > screen.Width)
                Width = screen.Width;
        }

        private void BtnAcceptClick(object sender, EventArgs e)
        {
            var setStreams = new FormSetStreams(ref Timetable_);
            setStreams.ShowDialog();
        }
    }

    static class WinAPI
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_SENDCHANGE = 0x2;
    }
}
#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Win32;
using UniTimetable.Model;
using UniTimetable.Model.Solver;
using UniTimetable.Model.Time;
using UniTimetable.Model.Timetable;
using UniTimetable.ViewControllers.CriteriaFilters;
using Stream = UniTimetable.Model.Timetable.Stream;
using Type = UniTimetable.Model.Timetable.Type;

#endregion

namespace UniTimetable.ViewControllers
{
    public partial class FormMain : Form
    {
        private const int ListBoxMargin = 2;
        private readonly Size _defaultSize;
        private readonly TimetableControl _export = new TimetableControl();
        private readonly FormSettings _formSettings = new FormSettings();

        private readonly History<Timetable> _history = new History<Timetable>(50);
        private readonly Size _imageSize = new Size(1024, 768);
        private readonly OpenFileDialog _openDialogXml = new OpenFileDialog();
        private readonly Random _random = new Random((int) DateTime.Now.Ticks);
        private readonly SaveFileDialog _saveDialogRaster = new SaveFileDialog();
        private readonly SaveFileDialog _saveDialogVector = new SaveFileDialog();
        private readonly SaveFileDialog _saveDialogWallpaper = new SaveFileDialog();
        private readonly SaveFileDialog _saveDialogXml = new SaveFileDialog();
        private int _changes;
        private Session _clickSession;
        private TimeOfWeek _clickTime;
        private Unavailability _clickUnavail;
        private Cursor _dragCursor;
        private Settings _settings = new Settings();
        private bool _sidePaneEnabled = true;
        public Timetable Timetable;
        public readonly Solver Solver;

        public FormMain()
        {
            Timetable = null;
            Solver = new Solver(null);

            InitializeComponent();

            _defaultSize = Size;

            // do anchoring here to make form design easier
            tableLayoutPanel2.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            timetableControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            tableLayoutPanel1.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            //tableLayoutPanel1.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;

            _export.Width = 800;
            _export.Height = 600;
            _export.ShowAll = false;
            _export.ShowDays = true;
            _export.ShowGrayArea = false;
            _export.ShowLocation = true;
            _export.ShowText = true;
            _export.ShowTimes = true;
            _export.Grayscale = false;

            _saveDialogXml.AddExtension = true;
            _saveDialogXml.CheckFileExists = false;
            _saveDialogXml.OverwritePrompt = true;
            _saveDialogXml.Title = "Save Timetable";
            _saveDialogXml.Filter = "XML File (*.xml)|*.xml";

            _openDialogXml.Multiselect = false;
            _openDialogXml.AddExtension = true;
            _openDialogXml.CheckFileExists = true;
            _openDialogXml.ShowReadOnly = false;
            _openDialogXml.Title = "Load Timetable";
            _openDialogXml.Filter = "XML File (*.xml)|*.xml";

            _saveDialogRaster.AddExtension = true;
            _saveDialogRaster.CheckFileExists = false;
            _saveDialogRaster.OverwritePrompt = true;
            _saveDialogRaster.Title = "Save Image";
            _saveDialogRaster.Filter =
                "PNG Image (*.png)|*.png|JPEG Image (*.jpg, *.jpeg)|*.jpg;*.jpeg|GIF Image (*.gif)|*.gif";

            _saveDialogVector.AddExtension = true;
            _saveDialogVector.CheckFileExists = false;
            _saveDialogVector.OverwritePrompt = true;
            _saveDialogVector.Title = "Save Image";
            _saveDialogVector.Filter = "Enhanced Metafile (*.emf)|*.emf";

            _saveDialogWallpaper.AddExtension = true;
            _saveDialogWallpaper.CheckFileExists = false;
            _saveDialogWallpaper.OverwritePrompt = true;
            _saveDialogWallpaper.Title = "Save Image";
            _saveDialogWallpaper.Filter = "Bitmap Image (*.bmp)|*.bmp";
            _saveDialogWallpaper.FileName = "wallpaper";

            //pageSetupDialog1.PageSettings = printDocument1.DefaultPageSettings;
            pageSetupDialog.PageSettings.Landscape = true;
            pageSetupDialog.PageSettings.Margins = new Margins(10*1000/254, 10*1000/254, 10*1000/254, 10*1000/254);

            FitToScreen();

            EnableButtons(false);

            UpdateSettings();
        }

        private void FormMainFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AreYouSure("Close"))
                e.Cancel = true;
        }

        private void TimetableControlTimetableChanged(object sender)
        {
            MadeChanges(true);
        }

        private bool AreYouSure(string caption)
        {
            if (Timetable == null || _changes == 0)
            {
                return true;
            }
            var choice = MessageBox.Show("Would you like to save the changes?", caption,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            switch (choice)
            {
                case DialogResult.Cancel: return false;
                case DialogResult.No: return true;
            }
            choice = Save();
            return (choice == DialogResult.OK);
        }

        private void TimetableControlResizeCell(object sender)
        {
            if (timetableControl.CellSize.Height == 0) return;
            listBox1.ItemHeight =
                listBox2.ItemHeight = Math.Min(timetableControl.CellSize.Height + 2*ListBoxMargin, 255);
            listBox1.Font = listBox2.Font = timetableControl.Font;
            listBox1.Invalidate();
            listBox2.Invalidate();
        }

        private void ResetWindow()
        {
            ShowSidePane();
            Size = _defaultSize;
            FitToScreen();
        }

        private void FitToScreen()
        {
            var screen = Screen.GetWorkingArea(this).Size;
            if (Height > screen.Height)
            {
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
            var setStreams = new FormSetStreams(ref Timetable);
            setStreams.ShowDialog();
        }

        #region Side pane

        private void BtnShowHideClick(object sender, EventArgs e)
        {
            btnAddByClass.Checked = !btnAddByClass.Checked;
            ToggleSidePane();
        }

        private void ToggleSidePane()
        {
            if (_sidePaneEnabled)
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
            if (_sidePaneEnabled)
                return;
            _sidePaneEnabled = true;

            if (WindowState == FormWindowState.Maximized)
            {
                tableLayoutPanel2.ColumnCount = 3;
                return;
            }

            var middle = tableLayoutPanel2.Width + tableLayoutPanel1.Margin.Horizontal;
            var left = timetableControl.Width + timetableControl.Margin.Horizontal;
            var old = middle + left;

            var fraction = tableLayoutPanel2.ColumnStyles[0].Width/100f;
            var next = (int) Math.Ceiling(left/fraction) + middle;
            Width = Width - old + next;
            tableLayoutPanel2.ColumnCount = 3;

            listBox1.TabStop = true;
            listBox2.TabStop = true;
        }

        private void HideSidePane()
        {
            if (!_sidePaneEnabled)
                return;
            _sidePaneEnabled = false;

            if (WindowState == FormWindowState.Maximized)
            {
                tableLayoutPanel2.ColumnCount = 2;
                return;
            }

            var panel = tableLayoutPanel3.Width + tableLayoutPanel3.Margin.Horizontal;
            tableLayoutPanel2.ColumnCount = 2;
            Width = Width - panel;

            listBox1.TabStop = false;
            listBox2.TabStop = false;
        }

        #endregion

        #region Listboxes

        private void ListBox1GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (!timetableControl.ShowDragGhost || _dragCursor == null) return;
            e.UseDefaultCursors = false;
            Cursor.Current = _dragCursor;
        }

        private void ListBox1MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var listBox = (ListBox) sender;
            var index = listBox.IndexFromPoint(e.Location);
            if (index == -1)
            {
                listBox.SelectedIndex = -1;
                return;
            }

            var type = (Type) listBox.SelectedItem;
            _dragCursor = timetableControl.DragCursor(type.FindShortestSession());
            timetableControl.BeginDrag(type);
            var result = listBox.DoDragDrop(type, DragDropEffects.Move);
            timetableControl.EndDrag();

            if (result != DragDropEffects.Move || listBox.SelectedItem == type)
                timetableControl.PreviewOptions(type);
        }

        private void ListBox1SelectedIndexChanged(object sender, EventArgs e)
        {
            var listBox = (ListBox) sender;
            if (listBox.SelectedIndex == -1)
            {
                timetableControl.EndPreviewOptions();
                return;
            }
            var type = (Type) listBox.SelectedItem;
            timetableControl.PreviewOptions(type);
        }

        private void ListBox1Leave(object sender, EventArgs e)
        {
            var listBox = (ListBox) sender;
            listBox.SelectedIndex = -1;
            timetableControl.EndPreviewOptions();
        }

        private void ListBox1DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(typeof (Type)) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void ListBox1DragDrop(object sender, DragEventArgs e)
        {
            var type = (Type) e.Data.GetData(typeof (Type));
            if (type.Required && type.SelectedStream == null) return;
            type.Required = true;
            if (type.SelectedStream != null)
                type.SelectedStream.Selected = false;

            MadeChanges(true);
            UpdateRemaining();
        }

        private void ListBox2DragDrop(object sender, DragEventArgs e)
        {
            var type = (Type) e.Data.GetData(typeof (Type));
            if (type.Required == false && type.SelectedStream == null) return;
            type.Required = false;
            if (type.SelectedStream != null)
                type.SelectedStream.Selected = false;

            MadeChanges(true);
            UpdateRemaining();
        }

        private void TimetableControlDragOver(object sender, DragEventArgs e)
        {
            var type = (Type) e.Data.GetData(typeof (Type));
            var time = timetableControl.FindClickTime(timetableControl.PointToClient(new Point(e.X, e.Y)));
            var hoverStream = FindStream(type, time);

            if (hoverStream != null)
            {
                timetableControl.PreviewEquiv(hoverStream);
            }
            else
            {
                timetableControl.EndPreviewStream();
            }
        }

        private void TimetableControlDragDrop(object sender, DragEventArgs e)
        {
            var type = (Type) e.Data.GetData(typeof (Type));
            var time = timetableControl.FindClickTime(timetableControl.PointToClient(new Point(e.X, e.Y)));
            var hoverStream = FindStream(type, time);

            timetableControl.EndPreviewStream();
            if (hoverStream == null) return;
            if (Timetable.SelectStream(hoverStream))
                MadeChanges(true);
        }

        private static Stream FindStream(Type type, TimeOfWeek time)
        {
            return Timetable.From(type).FindClassAt(time, false).Stream;
        }

        private void UpdateRemaining()
        {
            if (Timetable == null)
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                return;
            }

            var remaining = new List<object>();
            var ignored = new List<object>();
            foreach (
                var type in
                    Timetable.SubjectList.SelectMany(subject => subject.Types.Where(type => type.SelectedStream == null))
                )
            {
                if (!type.Required)
                {
                    ignored.Add(type);
                }
                else
                {
                    remaining.Add(type);
                }
            }
            listBox1.Items.Clear();
            listBox1.Items.AddRange(remaining.ToArray());
            listBox2.Items.Clear();
            listBox2.Items.AddRange(ignored.ToArray());

            timetableControl.EndPreviewStream();
            timetableControl.EndPreviewOptions();
        }

        #endregion

        #region Menu strip

        #region Saving and opening

        private void New(object sender, EventArgs e)
        {
            if (!AreYouSure("Clear Timetable"))
                return;
            Timetable = null;
            timetableControl.Timetable = Timetable;
            _saveDialogXml.FileName = null;
            EnableButtons(false);
            ClearHistory();
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            Save();
        }

        private DialogResult Save()
        {
            if (Timetable == null)
                return DialogResult.Cancel;

            if (_saveDialogXml.FileName == null || !File.Exists(_saveDialogXml.FileName))
            {
                var result = _saveDialogXml.ShowDialog();
                if (result != DialogResult.OK)
                    return result;
            }

            SaveToFile(_saveDialogXml.FileName);
            return DialogResult.OK;
        }

        private void SaveAs(object sender, EventArgs e)
        {
            if (Timetable == null)
                return;

            var result = _saveDialogXml.ShowDialog();
            if (result != DialogResult.OK)
                return;

            SaveToFile(_saveDialogXml.FileName);
        }

        private void SaveToFile(string filename)
        {
            var s = new XmlSerializer(typeof (Timetable));
            TextWriter w = new StreamWriter(string.IsNullOrWhiteSpace(filename) ? _saveDialogXml.FileName : filename,
                false);
            s.Serialize(w, Timetable);
            w.Close();

            _changes = 0;
        }

        private void Open(object sender, EventArgs e)
        {
            if (!AreYouSure("Open Timetable"))
                return;

            if (_openDialogXml.ShowDialog() != DialogResult.OK)
                return;

            var s = new XmlSerializer(typeof (Timetable));
            TextReader r;
            try
            {
                r = new StreamReader(_openDialogXml.FileName);
            }
            catch
            {
                MessageBox.Show("Could not open specified file!", "Open Timetable", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            Timetable t;
            try
            {
                t = (Timetable) s.Deserialize(r);
            }
            catch
            {
                MessageBox.Show("Failed to load saved data!", "Open Timetable", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                r.Close();
                return;
            }
            r.Close();
            _saveDialogXml.FileName = _openDialogXml.FileName;

            // build relationships and translate tree to lists
            foreach (var subject in t.SubjectList)
            {
                t.TypeList.AddRange(subject.Types);
                foreach (var type in subject.Types)
                {
                    t.StreamList.AddRange(type.Streams);
                    type.Subject = subject;
                    foreach (var stream in type.Streams)
                    {
                        t.ClassList.AddRange(stream.Classes);
                        stream.Type = type;
                        foreach (var session in stream.Classes)
                        {
                            session.Stream = stream;
                        }
                    }
                }
            }

            t.BuildEquivalency();
            t.BuildCompatibility();

            Timetable = t;
            timetableControl.Timetable = Timetable;
            timetableControl.MatchBounds();
            ClearHistory();
            EnableButtons(true);
        }

        #endregion

        #region Exporting images

        private void LoadExportSettings()
        {
            _export.Timetable = Timetable;
            _export.HourStart = timetableControl.HourStart;
            _export.HourEnd = timetableControl.HourEnd;
            _export.ShowWeekend = timetableControl.ShowWeekend;
        }

        private void ColourToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_saveDialogRaster.ShowDialog() != DialogResult.OK)
                return;

            LoadExportSettings();
            _export.Size = _imageSize;
            _export.Grayscale = false;
            _export.SaveRaster(_saveDialogRaster.FileName);
        }

        private void GreyscaleToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_saveDialogRaster.ShowDialog() != DialogResult.OK)
                return;

            LoadExportSettings();
            _export.Size = _imageSize;
            _export.Grayscale = true;
            _export.SaveRaster(_saveDialogRaster.FileName);
        }

        private void WallpaperToolStripMenuItemClick(object sender, EventArgs e)
        {
            const float opacity = 0.75f;

            var screen = Screen.GetBounds(this);
            var free = Screen.GetWorkingArea(this);

            LoadExportSettings();
            _export.Grayscale = false;
            const int margin = 100;
            _export.Size = free.Size - new Size(2*margin, 2*margin);
            free.Offset(margin, margin);

            var overlay = new Bitmap(screen.Width, screen.Height);
            var g = Graphics.FromImage(overlay);
            _export.DrawTimetable(g);
            g.Dispose();

            var background = GetWallpaperBitmap();
            g = Graphics.FromImage(background);
            // Drawing Transparent Images
            // http://www.vbdotnetheaven.com/UploadFile/mahesh/TransparentImagesShapes04212005052247AM/TransparentImagesShapes.aspx
            var attr = new ImageAttributes();
            attr.SetColorMatrix(new ColorMatrix(new[]
                                                {
                                                    new float[] {1, 0, 0, 0, 0},
                                                    new float[] {0, 1, 0, 0, 0},
                                                    new float[] {0, 0, 1, 0, 0},
                                                    new[] {0, 0, 0, opacity, 0},
                                                    new float[] {0, 0, 0, 0, 1}
                                                }),
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
            g.TranslateTransform(free.X, free.Y);
            g.DrawImage(overlay, screen, 0, 0, screen.Width, screen.Height, GraphicsUnit.Pixel, attr);
            g.Dispose();

            var dir = GetUserDirectory();
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var fileName = Path.Combine(GetUserDirectory(), "Wallpaper1.bmp");
            try
            {
                _export.SaveRaster(fileName, background);
            }
            catch
            {
                fileName = Path.Combine(GetUserDirectory(), "Wallpaper2.bmp");
                try
                {
                    _export.SaveRaster(fileName, background);
                }
                catch
                {
                    MessageBox.Show("Sorry, an error occurred when trying to save the image.", "Save Image",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            var rkWallpaper = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);
            if (rkWallpaper == null) return "";
            var wallpaper = rkWallpaper.GetValue("Wallpaper").ToString();
            rkWallpaper.Close();
            return wallpaper;
        }

        private Bitmap GetWallpaperBitmap()
        {
            var wallpaper = GetWallpaperPath();
            var screen = Screen.GetBounds(this);

            // have to get around indexed images, pieced together from:
            // http://www.c-sharpcorner.com/UploadFile/rrraman/graphicsObject08232007102733AM/graphicsObject.aspx
            // http://www.eggheadcafe.com/PrintSearchContent.asp?LINKID=799
            var b = new Bitmap(wallpaper);
            var stream = new MemoryStream();
            b.Save(stream, ImageFormat.Bmp);
            var image = Image.FromStream(stream);
            b = new Bitmap(image);

            var ret = new Bitmap(screen.Width, screen.Height);
            var g = Graphics.FromImage(ret);
            g.DrawImage(b, screen, new Rectangle(0, 0, b.Width, b.Height), GraphicsUnit.Pixel);
            g.Dispose();

            return ret;
        }

        private void SetWallpaper(string path, int style, int tile)
        {
            WinApi.SystemParametersInfo(20, 0, path, 0x01 | 0x02);

            var rkWallPaper = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
            if (rkWallPaper == null) return;
            rkWallPaper.SetValue("WallpaperStyle", style);
            rkWallPaper.SetValue("TileWallpaper", tile);
            rkWallPaper.Close();
        }

        private void ColourVectorToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_saveDialogVector.ShowDialog() != DialogResult.OK)
                return;

            LoadExportSettings();
            _export.Size = _imageSize;
            _export.Grayscale = false;
            _export.SaveVector(_saveDialogVector.FileName);
        }

        private void GreyscaleVectorToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_saveDialogVector.ShowDialog() != DialogResult.OK)
                return;

            LoadExportSettings();
            _export.Size = _imageSize;
            _export.Grayscale = true;
            _export.SaveVector(_saveDialogVector.FileName);
        }

        #endregion

        #region Printing

        private void Print(object sender, EventArgs e)
        {
            if (printDialog.ShowDialog() != DialogResult.OK) return;
            var keepTrying = true;
            while (keepTrying)
            {
                keepTrying = false;
                try
                {
                    printDocument.Print();
                }
                catch (Exception exception)
                {
                    keepTrying =
                        MessageBox.Show("Error occured in printing:\n\n" + exception.Message, "Print Timetable",
                            MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Retry;
                }
            }
        }

        private void PrintPreviewToolStripMenuItemClick(object sender, EventArgs e)
        {
            try
            {
                printPreviewDialog.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error occured in printing:\n\n" + exception.Message, "Print Timetable",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void PageSetupToolStripMenuItemClick(object sender, EventArgs e)
        {
            pageSetupDialog.ShowDialog();
        }

        private void PrintDocument1PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            var marginBounds = e.MarginBounds;
            if (!printDocument.PrintController.IsPreview)
                marginBounds.Offset(-(int) e.PageSettings.HardMarginX, -(int) e.PageSettings.HardMarginY);

            LoadExportSettings();
            _export.Size = marginBounds.Size;
            _export.Grayscale = (!e.PageSettings.PrinterSettings.SupportsColor ||
                                 MessageBox.Show("Print in colour?", "Print Timetable", MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question) == DialogResult.No);
            _export.Timetable = Timetable;
            g.TranslateTransform(marginBounds.X, marginBounds.Y);
            _export.DrawTimetable(g);
        }

        #endregion

        #region Importing

        private void Import(object sender, EventArgs e)
        {
            if (!AreYouSure("Import Timetable"))
                return;

            var importForm = new FormImport(_settings.ImportUnselectable);
            if (importForm.DialogResult == DialogResult.Cancel)
            {
                return;
            }

            var t = importForm.ShowDialog();
            if (t == null)
                return;

            Timetable = t;
            timetableControl.Timetable = Timetable;
            timetableControl.MatchBounds();
            _saveDialogXml.FileName = null;
            EnableButtons(true);
            ClearHistory();
        }

        private void ImportAndMergeToolStripMenuItemClick(object sender, EventArgs e)
        {
            // run the import wizard
            var importForm = new FormImport(_settings.ImportUnselectable);
            if (importForm.DialogResult == DialogResult.Cancel)
            {
                return;
            }

            var t = importForm.ShowDialog();
            if (t == null)
                return;

            Timetable.MergeWith(t);
            timetableControl.Invalidate();
            EnableButtons(true);
        }

        #endregion

        #region Undo and redo
        private void ClearHistory()
        {
            _changes = 0;
            _history.Clear();
            if (Timetable != null)
                _history.Add(Timetable.DeepCopy());

            UpdateRemaining();
        }

        public void MadeChanges(bool recompute)
        {
            _changes++;
            _history.Add(Timetable.DeepCopy());
            timetableControl.Invalidate();
            if (recompute)
                Timetable.RecomputeSolutions = true;
            UpdateRemaining();
        }

        #endregion

        #region Timetable config

        private void SettingsToolStripMenuItemClick(object sender, EventArgs e)
        {
            _settings.ResetWindow = false;
            var result = _formSettings.ShowDialog(_settings, this);
            if (result == null)
                return;
            _settings = result;
            _settings.Save();
            UpdateSettings();
        }

        private void TimetableControl1BoundsClipped(object sender)
        {
            _settings.HourStart = timetableControl.HourStart;
            _settings.HourEnd = timetableControl.HourEnd;
            _settings.Save();
        }

        private void UpdateSettings()
        {
            timetableControl.ShowDragGhost = _settings.ShowGhost;
            timetableControl.ShowWeekend = _settings.ShowWeekend;
            timetableControl.ShowGrayArea = _settings.ShowGray;
            timetableControl.ShowLocation = _settings.ShowLocation;
            timetableControl.SetBounds(_settings.HourStart, _settings.HourEnd);

            if (_settings.ResetWindow)
            {
                ResetWindow();
            }
        }

        #endregion

        #region Solver

        private void FindSolutions(object sender, EventArgs e)
        {
            if (Timetable == null)
                return;

            if (Timetable.IsFull())
            {
                MessageBox.Show("There are no remaining streams to solve for!", "Find Solutions", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            Solver.Timetable = Timetable;
            var formProgress = new FormProgress();
            if (formProgress.ShowDialog(Solver) != DialogResult.OK)
                return;

            var formSolution = new FormSolution();
            if (formSolution.ShowDialog(Solver) != DialogResult.OK)
                return;

            MadeChanges(true);
        }

        private void EditCriteria(object sender, EventArgs e)
        {
            if (Timetable == null)
            {
                return;
            }

            var formCriteria = new FormCriteria();
            if (formCriteria.ShowDialog(Solver) != DialogResult.OK)
            {
                return;
            }
            Timetable.RecomputeSolutions = true;
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
            btnAccept.Enabled = enable;
        }

        private void FeelingLucky(object sender, EventArgs e)
        {
            if (Timetable == null)
                return;

            if (Timetable.IsFull())
                Timetable.RevertToBaseStreams();

            Solver.Timetable = Timetable;
            var formProgress = new FormProgress();
            if (formProgress.ShowDialog(Solver) != DialogResult.OK)
                return;

            var index = _random.Next(Math.Min(100, Solver.Solutions.Count));
            Timetable.LoadSolution(Solver.Solutions[index]);
            MadeChanges(true);
        }

        private void AddByClass(object sender, EventArgs e)
        {
            ToggleSidePane();
        }

        private void Clear(object sender, EventArgs e)
        {
            if (!Timetable.RevertToBaseStreams()) return;
            timetableControl.EndPreviewStream();
            timetableControl.EndPreviewOptions();
            MadeChanges(true);
        }

        #endregion

        #region Right click menus

        private void FindClickDetails(TimetableEventArgs e)
        {
            _clickTime = e.Time;
            if (Timetable == null)
            {
                _clickSession = null;
                _clickUnavail = null;
            }
            else
            {
                _clickSession = Timetable.FindClassAt(_clickTime, true);
                _clickUnavail = Timetable.FindUnavailableAt(_clickTime);
            }
        }

        private void TimetableControl1TimetableMouseClick(object sender, TimetableEventArgs e)
        {
            FindClickDetails(e);

            switch (e.Button)
            {
                case MouseButtons.Right:
                    if (Timetable == null)
                    {
                        findClassHereToolStripMenuItem.Enabled = false;
                        unavailabilityToolStripMenuItem.Enabled = false;
                        timeMenu.Show(timetableControl, e.Location);
                    }

                    // right clicked empty space?
                    else if (_clickSession == null && _clickUnavail == null)
                    {
                        findClassHereToolStripMenuItem.DropDownItems.Clear();

                        // populate list of options at this time
                        var required = new List<ToolStripItem>();
                        var ignored = new List<ToolStripItem>();
                        foreach (var subject in Timetable.SubjectList)
                        {
                            foreach (var type in subject.Types)
                            {
                                foreach (var stream in type.UniqueStreams)
                                {
                                    var atTime =
                                        stream.Classes.Any(
                                            session => _clickTime >= session.Start && _clickTime <= session.End);
                                    if (!atTime)
                                        continue;

                                    var item =
                                        new ToolStripMenuItem(stream.Type.Subject + " " + stream);
                                    var stream1 = stream;
                                    item.MouseEnter += delegate { timetableControl.PreviewAlt(stream1); };
                                    item.MouseLeave += delegate { timetableControl.EndPreviewStream(); };
                                    var stream2 = stream;
                                    item.Click += delegate
                                                  {
                                                      if (!Timetable.SelectStream(stream2))
                                                          return;
                                                      MadeChanges(true);
                                                  };
                                    if (!Timetable.Fits(stream))
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

                        timeMenu.Show(timetableControl, e.Location);
                    }

                    // right clicked a session
                    else if (_clickSession != null)
                    {
                        alternativeToolStripMenuItem.DropDownItems.Clear();
                        equivalentToolStripMenuItem.DropDownItems.Clear();

                        timetableControl.SetActive(_clickSession.Stream);

                        // populate alternative options menu
                        // NOTE: lambda-style delegates do not work with foreach
                        var n = 0;
                        foreach (var alt in _clickSession.Stream.Type.UniqueStreams)
                        {
                            if (alt == _clickSession.Stream || _clickSession.Stream.Equivalent.Contains(alt))
                                continue;
                            var item = new ToolStripMenuItem(alt.ToString());
                            var alt1 = alt;
                            item.MouseEnter += delegate { timetableControl.PreviewAlt(alt1); };
                            item.MouseLeave += delegate { timetableControl.EndPreviewStream(); };
                            var alt2 = alt;
                            item.Click += delegate
                                          {
                                              if (!Timetable.SelectStream(alt2))
                                                  return;
                                              MadeChanges(true);
                                          };
                            if (!Timetable.Fits(alt))
                                item.ForeColor = Color.Gray;

                            alternativeToolStripMenuItem.DropDownItems.Add(item);
                            n++;
                        }
                        alternativeToolStripMenuItem.Enabled = (n != 0);

                        // populate equivalent options menu
                        n = 0;
                        foreach (var equiv in _clickSession.Stream.Equivalent)
                        {
                            var item = new ToolStripMenuItem(equiv.ToString());
                            var equiv1 = equiv;
                            item.MouseEnter += delegate { timetableControl.PreviewEquiv(equiv1); };
                            item.MouseLeave += delegate { timetableControl.EndPreviewStream(); };
                            var equiv2 = equiv;
                            item.Click += delegate
                                          {
                                              if (!Timetable.SelectStream(equiv2))
                                                  return;
                                              MadeChanges(true);
                                          };
                            equivalentToolStripMenuItem.DropDownItems.Add(item);
                            n++;
                        }
                        equivalentToolStripMenuItem.Enabled = (n != 0);
                        streamMenu.Show(timetableControl, e.Location);
                    }

                    // right clicked an unavailability
                    else
                    {
                        timetableControl.SetActive(_clickUnavail);

                        unavailableMenu.Show(timetableControl, e.Location);
                    }
                    break;
                case MouseButtons.Middle:
                    break;
            }
        }

        private void RemoveToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_clickSession == null)
                return;
            _clickSession.Stream.Selected = false;
            MadeChanges(true);
        }

        private void UnavailabilityToolStripMenuItemClick(object sender, EventArgs e)
        {
            var formUnavail = new FormUnavailability();
            var result = formUnavail.ShowDialog(Timetable,
                new Timeslot(_clickTime.Day, -1, _clickTime.Hour, 0, _clickTime.Hour + 1, 0),
                timetableControl.HourStart, timetableControl.HourEnd);
            if (result == DialogResult.Cancel) return;
            MadeChanges(true);
        }

        private void StreamMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            timetableControl.ClearActive();
        }

        private void EditUnavailableToolStripMenuItemClick(object sender, EventArgs e)
        {
            // click details already established by right click
            EditUnavailable();
        }

        private void EditUnavailable()
        {
            if (_clickUnavail == null) return;
            var formUnavail = new FormUnavailability();
            if (formUnavail.ShowDialog(Timetable, _clickUnavail, timetableControl.HourStart,
                    timetableControl.HourEnd) == DialogResult.Cancel)
                return;
            MadeChanges(true);
        }

        private void RemoveUnavailableToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_clickUnavail == null) return;
            Timetable.UnavailableList.Remove(_clickUnavail);
            MadeChanges(true);
        }

        private void UnavailableMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            timetableControl.ClearActive();
        }

        #endregion
    }

    internal static class WinApi
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
}
using System.Drawing;
using System.Windows.Forms;
using UniTimetable.Model.Timetable;

namespace UniTimetable.ViewControllers
{
    public partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.findClassAtThisTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findAlternativeStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewEquivalentStreamsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.addUnavailableTimeslotToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editUnavToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeUnavailableTimeslotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.streamMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.alternativeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.equivalentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.findClassHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unavailabilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unavailableMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editUnavailableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeUnavailableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnImport = new System.Windows.Forms.ToolStripSplitButton();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importAndMergeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAccept = new System.Windows.Forms.ToolStripSplitButton();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemColour = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGreyscale = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemColourEmf = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGreyscaleEmf = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemBackground = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripSplitButton();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPrint = new System.Windows.Forms.ToolStripSplitButton();
            this.sToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.pageSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddByClass = new System.Windows.Forms.ToolStripButton();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.btnPreferences = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSolver = new System.Windows.Forms.ToolStripButton();
            this.btnCriteria = new System.Windows.Forms.ToolStripButton();
            this.btnLucky = new System.Windows.Forms.ToolStripButton();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.timetableControl = new UniTimetable.ViewControllers.TimetableControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnShowHide = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.listBox2 = new UniTimetable.ViewControllers.ListBoxBuffered();
            this.listBox1 = new UniTimetable.ViewControllers.ListBoxBuffered();
            this.lblRemaining = new System.Windows.Forms.Label();
            this.lblIgnored = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.streamMenu.SuspendLayout();
            this.timeMenu.SuspendLayout();
            this.unavailableMenu.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findClassAtThisTimeToolStripMenuItem,
            this.findAlternativeStreamToolStripMenuItem,
            this.viewEquivalentStreamsToolStripMenuItem,
            this.deleteClassToolStripMenuItem,
            this.toolStripSeparator6,
            this.addUnavailableTimeslotToolStripMenuItem1,
            this.editUnavToolStripMenuItem,
            this.removeUnavailableTimeslotToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStrip.Size = new System.Drawing.Size(231, 186);
            // 
            // findClassAtThisTimeToolStripMenuItem
            // 
            this.findClassAtThisTimeToolStripMenuItem.Name = "findClassAtThisTimeToolStripMenuItem";
            this.findClassAtThisTimeToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.findClassAtThisTimeToolStripMenuItem.Text = "Find Class at this &Time...";
            // 
            // findAlternativeStreamToolStripMenuItem
            // 
            this.findAlternativeStreamToolStripMenuItem.Name = "findAlternativeStreamToolStripMenuItem";
            this.findAlternativeStreamToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.findAlternativeStreamToolStripMenuItem.Text = "&Alternative Streams...";
            // 
            // viewEquivalentStreamsToolStripMenuItem
            // 
            this.viewEquivalentStreamsToolStripMenuItem.Name = "viewEquivalentStreamsToolStripMenuItem";
            this.viewEquivalentStreamsToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.viewEquivalentStreamsToolStripMenuItem.Text = "E&quivalent Streams..";
            // 
            // deleteClassToolStripMenuItem
            // 
            this.deleteClassToolStripMenuItem.Name = "deleteClassToolStripMenuItem";
            this.deleteClassToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.deleteClassToolStripMenuItem.Text = "&Remove Stream";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(227, 6);
            // 
            // addUnavailableTimeslotToolStripMenuItem1
            // 
            this.addUnavailableTimeslotToolStripMenuItem1.Name = "addUnavailableTimeslotToolStripMenuItem1";
            this.addUnavailableTimeslotToolStripMenuItem1.Size = new System.Drawing.Size(230, 22);
            this.addUnavailableTimeslotToolStripMenuItem1.Text = "&Add Unavailable Timeslot...";
            // 
            // editUnavToolStripMenuItem
            // 
            this.editUnavToolStripMenuItem.Name = "editUnavToolStripMenuItem";
            this.editUnavToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.editUnavToolStripMenuItem.Text = "&Edit Unavailable Timeslot...";
            // 
            // removeUnavailableTimeslotToolStripMenuItem
            // 
            this.removeUnavailableTimeslotToolStripMenuItem.Name = "removeUnavailableTimeslotToolStripMenuItem";
            this.removeUnavailableTimeslotToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.removeUnavailableTimeslotToolStripMenuItem.Text = "&Remove Unavailable Timeslot";
            // 
            // printDocument
            // 
            this.printDocument.DocumentName = "Timetable";
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocument1PrintPage);
            // 
            // printDialog
            // 
            this.printDialog.Document = this.printDocument;
            this.printDialog.UseEXDialog = true;
            // 
            // pageSetupDialog
            // 
            this.pageSetupDialog.Document = this.printDocument;
            this.pageSetupDialog.EnableMetric = true;
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog.Document = this.printDocument;
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
            this.printPreviewDialog.Name = "printPreviewDialog1";
            this.printPreviewDialog.ShowIcon = false;
            this.printPreviewDialog.Visible = false;
            // 
            // streamMenu
            // 
            this.streamMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alternativeToolStripMenuItem,
            this.equivalentToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.streamMenu.Name = "rightClickMenu";
            this.streamMenu.Size = new System.Drawing.Size(132, 70);
            this.streamMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.StreamMenuClosed);
            // 
            // alternativeToolStripMenuItem
            // 
            this.alternativeToolStripMenuItem.Name = "alternativeToolStripMenuItem";
            this.alternativeToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.alternativeToolStripMenuItem.Text = "Alternative";
            // 
            // equivalentToolStripMenuItem
            // 
            this.equivalentToolStripMenuItem.Name = "equivalentToolStripMenuItem";
            this.equivalentToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.equivalentToolStripMenuItem.Text = "Equivalent";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.RemoveToolStripMenuItemClick);
            // 
            // timeMenu
            // 
            this.timeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findClassHereToolStripMenuItem,
            this.unavailabilityToolStripMenuItem});
            this.timeMenu.Name = "timeMenu";
            this.timeMenu.Size = new System.Drawing.Size(173, 48);
            // 
            // findClassHereToolStripMenuItem
            // 
            this.findClassHereToolStripMenuItem.Name = "findClassHereToolStripMenuItem";
            this.findClassHereToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.findClassHereToolStripMenuItem.Text = "Find Class Here";
            // 
            // unavailabilityToolStripMenuItem
            // 
            this.unavailabilityToolStripMenuItem.Name = "unavailabilityToolStripMenuItem";
            this.unavailabilityToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.unavailabilityToolStripMenuItem.Text = "Unavailable Here...";
            this.unavailabilityToolStripMenuItem.Click += new System.EventHandler(this.UnavailabilityToolStripMenuItemClick);
            // 
            // unavailableMenu
            // 
            this.unavailableMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editUnavailableToolStripMenuItem,
            this.removeUnavailableToolStripMenuItem});
            this.unavailableMenu.Name = "unavailableMenu";
            this.unavailableMenu.Size = new System.Drawing.Size(118, 48);
            this.unavailableMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.UnavailableMenuClosed);
            // 
            // editUnavailableToolStripMenuItem
            // 
            this.editUnavailableToolStripMenuItem.Name = "editUnavailableToolStripMenuItem";
            this.editUnavailableToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.editUnavailableToolStripMenuItem.Text = "&Edit...";
            this.editUnavailableToolStripMenuItem.Click += new System.EventHandler(this.EditUnavailableToolStripMenuItemClick);
            // 
            // removeUnavailableToolStripMenuItem
            // 
            this.removeUnavailableToolStripMenuItem.Name = "removeUnavailableToolStripMenuItem";
            this.removeUnavailableToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeUnavailableToolStripMenuItem.Text = "&Remove";
            this.removeUnavailableToolStripMenuItem.Click += new System.EventHandler(this.RemoveUnavailableToolStripMenuItemClick);
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripMargin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImport,
            this.btnAccept,
            this.toolStripSeparator2,
            this.btnOpen,
            this.btnSave,
            this.btnPrint,
            this.toolStripSeparator4,
            this.btnAddByClass,
            this.btnNew,
            this.btnClear,
            this.btnPreferences,
            this.toolStripSeparator10,
            this.btnSolver,
            this.btnCriteria,
            this.btnLucky});
            this.toolStrip.Location = new System.Drawing.Point(3, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(663, 52);
            this.toolStrip.TabIndex = 10;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnImport
            // 
            this.btnImport.DropDownButtonWidth = 7;
            this.btnImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.importAndMergeToolStripMenuItem});
            this.btnImport.Image = global::UniTimetable.Properties.Resources.Import;
            this.btnImport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImport.Name = "btnImport";
            this.btnImport.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnImport.Size = new System.Drawing.Size(58, 49);
            this.btnImport.Text = "Import";
            this.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnImport.ToolTipText = "Import from MyTimetable";
            this.btnImport.ButtonClick += new System.EventHandler(this.Import);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.importToolStripMenuItem.Text = "&Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.Import);
            // 
            // importAndMergeToolStripMenuItem
            // 
            this.importAndMergeToolStripMenuItem.Name = "importAndMergeToolStripMenuItem";
            this.importAndMergeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.importAndMergeToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.importAndMergeToolStripMenuItem.Text = "Import and &Merge";
            this.importAndMergeToolStripMenuItem.ToolTipText = "Import and merge into current timetable";
            this.importAndMergeToolStripMenuItem.Click += new System.EventHandler(this.ImportAndMergeToolStripMenuItemClick);
            // 
            // btnAccept
            // 
            this.btnAccept.DropDownButtonWidth = 7;
            this.btnAccept.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.toolStripSeparator3,
            this.toolStripMenuItemColour,
            this.toolStripMenuItemGreyscale,
            this.toolStripSeparator5,
            this.toolStripMenuItemColourEmf,
            this.toolStripMenuItemGreyscaleEmf,
            this.toolStripSeparator8,
            this.toolStripMenuItemBackground});
            this.btnAccept.Image = global::UniTimetable.Properties.Resources.Export;
            this.btnAccept.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAccept.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnAccept.Size = new System.Drawing.Size(55, 49);
            this.btnAccept.Text = "Export";
            this.btnAccept.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAccept.ToolTipText = "Export to MyTimetable (set your classes to those selected)";
            this.btnAccept.ButtonClick += new System.EventHandler(this.BtnAcceptClick);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.exportToolStripMenuItem.Text = "&Export to MyTimetable";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.BtnAcceptClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(231, 6);
            // 
            // toolStripMenuItemColour
            // 
            this.toolStripMenuItemColour.Name = "toolStripMenuItemColour";
            this.toolStripMenuItemColour.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItemColour.Text = "&Colour...";
            this.toolStripMenuItemColour.Click += new System.EventHandler(this.ColourToolStripMenuItemClick);
            // 
            // toolStripMenuItemGreyscale
            // 
            this.toolStripMenuItemGreyscale.Name = "toolStripMenuItemGreyscale";
            this.toolStripMenuItemGreyscale.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItemGreyscale.Text = "&Greyscale...";
            this.toolStripMenuItemGreyscale.Click += new System.EventHandler(this.GreyscaleToolStripMenuItemClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(231, 6);
            // 
            // toolStripMenuItemColourEmf
            // 
            this.toolStripMenuItemColourEmf.Name = "toolStripMenuItemColourEmf";
            this.toolStripMenuItemColourEmf.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItemColourEmf.Text = "C&olour EMF...";
            this.toolStripMenuItemColourEmf.Click += new System.EventHandler(this.ColourVectorToolStripMenuItemClick);
            // 
            // toolStripMenuItemGreyscaleEmf
            // 
            this.toolStripMenuItemGreyscaleEmf.Name = "toolStripMenuItemGreyscaleEmf";
            this.toolStripMenuItemGreyscaleEmf.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItemGreyscaleEmf.Text = "G&reyscale EMF...";
            this.toolStripMenuItemGreyscaleEmf.Click += new System.EventHandler(this.GreyscaleVectorToolStripMenuItemClick);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(231, 6);
            // 
            // toolStripMenuItemBackground
            // 
            this.toolStripMenuItemBackground.Name = "toolStripMenuItemBackground";
            this.toolStripMenuItemBackground.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItemBackground.Text = "&Desktop Background...";
            this.toolStripMenuItemBackground.Click += new System.EventHandler(this.WallpaperToolStripMenuItemClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 52);
            // 
            // btnOpen
            // 
            this.btnOpen.Image = global::UniTimetable.Properties.Resources.Open;
            this.btnOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnOpen.Size = new System.Drawing.Size(43, 49);
            this.btnOpen.Text = "Open";
            this.btnOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnOpen.ToolTipText = "Open saved timetable";
            this.btnOpen.Click += new System.EventHandler(this.Open);
            // 
            // btnSave
            // 
            this.btnSave.DropDownButtonWidth = 7;
            this.btnSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.btnSave.Image = global::UniTimetable.Properties.Resources.Save;
            this.btnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnSave.Size = new System.Drawing.Size(46, 49);
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSave.ToolTipText = "Save timetable to file";
            this.btnSave.ButtonClick += new System.EventHandler(this.BtnSaveClick);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAs);
            // 
            // btnPrint
            // 
            this.btnPrint.DropDownButtonWidth = 7;
            this.btnPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sToolStripMenuItem,
            this.toolStripSeparator7,
            this.pageSetupToolStripMenuItem,
            this.printPreviewToolStripMenuItem});
            this.btnPrint.Image = global::UniTimetable.Properties.Resources.Printer;
            this.btnPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnPrint.Size = new System.Drawing.Size(47, 49);
            this.btnPrint.Text = "Print";
            this.btnPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPrint.ToolTipText = "Print timetable";
            this.btnPrint.ButtonClick += new System.EventHandler(this.Print);
            // 
            // sToolStripMenuItem
            // 
            this.sToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.sToolStripMenuItem.Name = "sToolStripMenuItem";
            this.sToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.sToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sToolStripMenuItem.Text = "&Print";
            this.sToolStripMenuItem.ToolTipText = "Print timetable";
            this.sToolStripMenuItem.Click += new System.EventHandler(this.Print);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(149, 6);
            // 
            // pageSetupToolStripMenuItem
            // 
            this.pageSetupToolStripMenuItem.Name = "pageSetupToolStripMenuItem";
            this.pageSetupToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pageSetupToolStripMenuItem.Text = "Page Set&up...";
            this.pageSetupToolStripMenuItem.Click += new System.EventHandler(this.PageSetupToolStripMenuItemClick);
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.printPreviewToolStripMenuItem.Text = "Print Pre&view...";
            this.printPreviewToolStripMenuItem.Click += new System.EventHandler(this.PrintPreviewToolStripMenuItemClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 52);
            // 
            // btnAddByClass
            // 
            this.btnAddByClass.Image = global::UniTimetable.Properties.Resources.ShowClasses;
            this.btnAddByClass.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnAddByClass.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddByClass.Name = "btnAddByClass";
            this.btnAddByClass.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnAddByClass.Size = new System.Drawing.Size(52, 49);
            this.btnAddByClass.Text = "Classes";
            this.btnAddByClass.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAddByClass.ToolTipText = "Show/hide avalible classes";
            this.btnAddByClass.Click += new System.EventHandler(this.AddByClass);
            // 
            // btnNew
            // 
            this.btnNew.Image = global::UniTimetable.Properties.Resources.Clear;
            this.btnNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnNew.Size = new System.Drawing.Size(41, 49);
            this.btnNew.Text = "Clear";
            this.btnNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnNew.ToolTipText = "Clear timetable";
            this.btnNew.Click += new System.EventHandler(this.New);
            // 
            // btnClear
            // 
            this.btnClear.Image = global::UniTimetable.Properties.Resources.Reset;
            this.btnClear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnClear.Size = new System.Drawing.Size(42, 49);
            this.btnClear.Text = "Reset";
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnClear.ToolTipText = "Reset Canterbury Timetable Manager";
            this.btnClear.Click += new System.EventHandler(this.Clear);
            // 
            // btnPreferences
            // 
            this.btnPreferences.Image = global::UniTimetable.Properties.Resources.Preferences;
            this.btnPreferences.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPreferences.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPreferences.Name = "btnPreferences";
            this.btnPreferences.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnPreferences.Size = new System.Drawing.Size(75, 49);
            this.btnPreferences.Text = "Preferences";
            this.btnPreferences.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPreferences.Click += new System.EventHandler(this.SettingsToolStripMenuItemClick);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 52);
            // 
            // btnSolver
            // 
            this.btnSolver.Image = global::UniTimetable.Properties.Resources.Find;
            this.btnSolver.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSolver.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSolver.Name = "btnSolver";
            this.btnSolver.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnSolver.Size = new System.Drawing.Size(63, 49);
            this.btnSolver.Text = "Solutions";
            this.btnSolver.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSolver.Click += new System.EventHandler(this.FindSolutions);
            // 
            // btnCriteria
            // 
            this.btnCriteria.Image = global::UniTimetable.Properties.Resources.Criteria;
            this.btnCriteria.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnCriteria.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCriteria.Name = "btnCriteria";
            this.btnCriteria.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnCriteria.Size = new System.Drawing.Size(52, 49);
            this.btnCriteria.Text = "Criteria";
            this.btnCriteria.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCriteria.Click += new System.EventHandler(this.EditCriteria);
            // 
            // btnLucky
            // 
            this.btnLucky.Image = global::UniTimetable.Properties.Resources.Random;
            this.btnLucky.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnLucky.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLucky.Name = "btnLucky";
            this.btnLucky.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnLucky.Size = new System.Drawing.Size(59, 49);
            this.btnLucky.Text = "Random";
            this.btnLucky.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnLucky.Click += new System.EventHandler(this.FeelingLucky);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(828, 623);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tableLayoutPanel2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(992, 599);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolStripContainer1.LeftToolStripPanel
            // 
            this.toolStripContainer1.LeftToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            this.toolStripContainer1.RightToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripContainer1.Size = new System.Drawing.Size(992, 651);
            this.toolStripContainer1.TabIndex = 10;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip);
            this.toolStripContainer1.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.Controls.Add(this.timetableControl, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(6, 0, 6, 6);
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(992, 599);
            this.tableLayoutPanel2.TabIndex = 13;
            // 
            // timetableControl
            // 
            this.timetableControl.AllowDrop = true;
            this.timetableControl.BackColor = System.Drawing.Color.Transparent;
            this.timetableControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timetableControl.EnableDrag = true;
            this.timetableControl.Font = new System.Drawing.Font("Arial Narrow", 9.38927F);
            this.timetableControl.Grayscale = false;
            this.timetableControl.HourEnd = 21;
            this.timetableControl.HourStart = 8;
            this.timetableControl.Location = new System.Drawing.Point(8, 0);
            this.timetableControl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 3);
            this.timetableControl.Name = "timetableControl";
            this.timetableControl.OutlineColour = System.Drawing.Color.LightGray;
            this.timetableControl.ShowAll = false;
            this.timetableControl.ShowDays = true;
            this.timetableControl.ShowDragGhost = true;
            this.timetableControl.ShowGrayArea = true;
            this.timetableControl.ShowLocation = true;
            this.timetableControl.ShowText = true;
            this.timetableControl.ShowTimes = true;
            this.timetableControl.ShowWeekend = true;
            this.timetableControl.Size = new System.Drawing.Size(812, 590);
            this.timetableControl.TabIndex = 17;
            this.timetableControl.TimeslotUnavalibleColour = System.Drawing.Color.LightGray;
            this.timetableControl.Timetable = null;
            this.timetableControl.TimetableMouseClick += new UniTimetable.ViewControllers.TimetableEventHandler(this.TimetableControl1TimetableMouseClick);
            this.timetableControl.TimetableMouseDoubleClick += new UniTimetable.ViewControllers.TimetableEventHandler(this.TimetableControl1TimetableMouseClick);
            this.timetableControl.TimetableChanged += new UniTimetable.ViewControllers.TimetableChangedEventHandler(this.TimetableControlTimetableChanged);
            this.timetableControl.ResizeCell += new UniTimetable.ViewControllers.ResizeCellEventHandler(this.TimetableControlResizeCell);
            this.timetableControl.BoundsClipped += new UniTimetable.ViewControllers.BoundsClippedEventHandler(this.TimetableControl1BoundsClipped);
            this.timetableControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.TimetableControlDragDrop);
            this.timetableControl.DragOver += new System.Windows.Forms.DragEventHandler(this.TimetableControlDragOver);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnShowHide, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(822, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(20, 593);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // btnShowHide
            // 
            this.btnShowHide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowHide.Location = new System.Drawing.Point(0, 246);
            this.btnShowHide.Margin = new System.Windows.Forms.Padding(0);
            this.btnShowHide.Name = "btnShowHide";
            this.btnShowHide.Size = new System.Drawing.Size(20, 100);
            this.btnShowHide.TabIndex = 15;
            this.btnShowHide.Text = "«";
            this.btnShowHide.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowHide.UseVisualStyleBackColor = true;
            this.btnShowHide.Click += new System.EventHandler(this.BtnShowHideClick);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.listBox2, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.listBox1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblRemaining, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblIgnored, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(848, 6);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(132, 581);
            this.tableLayoutPanel3.TabIndex = 14;
            // 
            // listBox2
            // 
            this.listBox2.AllowDrop = true;
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.IntegralHeight = false;
            this.listBox2.ItemHeight = 40;
            this.listBox2.Location = new System.Drawing.Point(0, 418);
            this.listBox2.Margin = new System.Windows.Forms.Padding(0);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(132, 163);
            this.listBox2.TabIndex = 14;
            this.listBox2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.ListBox1SelectedIndexChanged);
            this.listBox2.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListBox2DragDrop);
            this.listBox2.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListBox1DragEnter);
            this.listBox2.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.ListBox1GiveFeedback);
            this.listBox2.Leave += new System.EventHandler(this.ListBox1Leave);
            this.listBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBox1MouseDown);
            // 
            // listBox1
            // 
            this.listBox1.AllowDrop = true;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.ItemHeight = 40;
            this.listBox1.Location = new System.Drawing.Point(0, 20);
            this.listBox1.Margin = new System.Windows.Forms.Padding(0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(132, 378);
            this.listBox1.TabIndex = 14;
            this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1SelectedIndexChanged);
            this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListBox1DragDrop);
            this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListBox1DragEnter);
            this.listBox1.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.ListBox1GiveFeedback);
            this.listBox1.Leave += new System.EventHandler(this.ListBox1Leave);
            this.listBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListBox1MouseDown);
            // 
            // lblRemaining
            // 
            this.lblRemaining.AutoSize = true;
            this.lblRemaining.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRemaining.Location = new System.Drawing.Point(3, 0);
            this.lblRemaining.Name = "lblRemaining";
            this.lblRemaining.Size = new System.Drawing.Size(126, 20);
            this.lblRemaining.TabIndex = 15;
            this.lblRemaining.Text = "Remaining";
            this.lblRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblIgnored
            // 
            this.lblIgnored.AutoSize = true;
            this.lblIgnored.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIgnored.Location = new System.Drawing.Point(3, 398);
            this.lblIgnored.Name = "lblIgnored";
            this.lblIgnored.Size = new System.Drawing.Size(126, 20);
            this.lblIgnored.TabIndex = 16;
            this.lblIgnored.Text = "Ignored";
            this.lblIgnored.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 651);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "FormMain";
            this.Text = "Canterbury Timetable Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMainFormClosing);
            this.contextMenuStrip.ResumeLayout(false);
            this.streamMenu.ResumeLayout(false);
            this.timeMenu.ResumeLayout(false);
            this.unavailableMenu.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteClassToolStripMenuItem;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
        private System.Windows.Forms.ToolStripMenuItem findClassAtThisTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findAlternativeStreamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewEquivalentStreamsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem addUnavailableTimeslotToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeUnavailableTimeslotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editUnavToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip streamMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alternativeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem equivalentToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip timeMenu;
        private System.Windows.Forms.ToolStripMenuItem findClassHereToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unavailabilityToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip unavailableMenu;
        private System.Windows.Forms.ToolStripMenuItem editUnavailableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeUnavailableToolStripMenuItem;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnCriteria;
        private System.Windows.Forms.ToolStripButton btnSolver;
        private System.Windows.Forms.ToolStripButton btnLucky;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton btnAddByClass;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private ListBoxBuffered listBox1;
        private ListBoxBuffered listBox2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button btnShowHide;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblRemaining;
        private System.Windows.Forms.Label lblIgnored;
        private System.Windows.Forms.ToolStripSplitButton btnSave;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton btnImport;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importAndMergeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton btnAccept;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemColour;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGreyscale;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemColourEmf;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGreyscaleEmf;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemBackground;
        private System.Windows.Forms.ToolStripSplitButton btnPrint;
        private System.Windows.Forms.ToolStripMenuItem sToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem pageSetupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnPreferences;
        private TimetableControl timetableControl;

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            e.DrawBackground();
            e.DrawFocusRectangle();

            var listBox = (ListBox)sender;
            var g = e.Graphics;
            var type = (Type)listBox.Items[e.Index];

            var r = new Rectangle(e.Bounds.X + ListBoxMargin, e.Bounds.Y + ListBoxMargin, e.Bounds.Width - 2 * ListBoxMargin - 1, e.Bounds.Height - 2 * ListBoxMargin - 1);
            g.FillRectangle(TimetableControl.LinearGradient(r.Location, r.Width, r.Height, type.Subject.Color), r);

            var format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            g.DrawString(type.Subject + " " + type.Code, listBox1.Font, Brushes.Black, r, format);

            g.DrawRectangle(Pens.Black, r);
        }
    }
}


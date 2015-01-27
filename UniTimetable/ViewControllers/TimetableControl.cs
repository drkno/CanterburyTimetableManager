#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using UniTimetable.Model.Time;
using UniTimetable.Model.Timetable;
using Type = UniTimetable.Model.Timetable.Type;

#endregion

namespace UniTimetable.ViewControllers
{
    public sealed partial class TimetableControl : UserControl
    {
        private const float TextAspect = 2.5f; // rough width:height ratio of text

        private static readonly string[] Days =
        {
            "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday",
            "Saturday"
        };

        private readonly List<AnimatedSession> _animation = new List<AnimatedSession>();
        private readonly Timer _timer = new Timer();
        private Stream _activeStream;
        private Unavailability _activeUnavail;
        private Stream _altStream;
        private Cursor _dragCursor;
        private Session _dragSession;
        private bool _easterEgg;
        private bool _enableDrag = true;
        private Stream _equivStream;
        private bool _grayscale;
        private int _hourEnd = 21;
        private int _hourStart = 8;
        private Timeslot _hoverUnavail;
        private Type _optionsType;
        private bool _showAll;
        private bool _showDays = true;
        private bool _showDragGhost = true;
        private bool _showGrayArea = true;
        private bool _showLocation = true;
        private bool _showText = true;
        private bool _showTimes = true;
        private bool _showWeekend = true;
        private Rectangle _table;
        private Timetable _timetable;

        public TimetableControl()
        {
            InitializeComponent();
            DoubleBuffered = true;

            _timer.Stop();
            _timer.Interval = 10;
            _timer.Tick += delegate { Timestep(); };
        }

        #region Accessors

        public Timetable Timetable
        {
            get { return _timetable; }
            set
            {
                _timetable = value;
                ValidateBounds();
                EndPreviewStream();
                EndPreviewOptions();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int HourStart
        {
            get { return _hourStart; }
            set
            {
                if (value > 23)
                    _hourStart = 23;
                else if (value < 0)
                    _hourStart = 0;
                else
                    _hourStart = value;
                ValidateBounds();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int HourEnd
        {
            get { return _hourEnd; }
            set
            {
                if (value > 24)
                    _hourEnd = 24;
                else if (value < 1)
                    _hourEnd = 1;
                else
                    _hourEnd = value;
                ValidateBounds();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowAll
        {
            get { return _showAll; }
            set
            {
                _showAll = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowDays
        {
            get { return _showDays; }
            set
            {
                _showDays = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowWeekend
        {
            get { return _showWeekend; }
            set
            {
                _showWeekend = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowTimes
        {
            get { return _showTimes; }
            set
            {
                _showTimes = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowText
        {
            get { return _showText; }
            set
            {
                _showText = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowLocation
        {
            get { return _showLocation; }
            set
            {
                _showLocation = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowGrayArea
        {
            get { return _showGrayArea; }
            set
            {
                _showGrayArea = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool Grayscale
        {
            get { return _grayscale; }
            set
            {
                _grayscale = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool EnableDrag
        {
            get { return _enableDrag; }
            set { _enableDrag = value; }
        }

        [Category("Appearance")]
        public bool ShowDragGhost
        {
            get { return _showDragGhost; }
            set { _showDragGhost = value; }
        }

        public Size CellSize { get; private set; }

        public Rectangle Table
        {
            get { return _table; }
        }

        private Size Cell
        {
            set
            {
                if (CellSize != value)
                {
                    CellSize = value;
                    if (ResizeCell != null)
                        ResizeCell(this);
                }
            }
        }

        public void Clear()
        {
            Timetable = null;
        }

        public void SetBounds(int hourStart, int hourEnd)
        {
            if (hourStart >= hourEnd)
            {
                throw new Exception("Start time must be before end time!");
            }
            _hourStart = hourStart;
            _hourEnd = hourEnd;
            ValidateBounds();
        }

        public void ValidateBounds()
        {
            if (_timetable == null)
                return;
            var maxStart = _timetable.EarlyBound();
            var minEnd = _timetable.LateBound();
            var maxHourStart = maxStart.Hour;
            var minHourEnd = minEnd.Hour;
            if (minEnd.Minute > 0)
                minHourEnd++;
            if (_hourStart > maxHourStart || _hourEnd < minHourEnd)
            {
                _hourStart = Math.Min(maxHourStart, _hourStart);
                _hourEnd = Math.Max(minHourEnd, _hourEnd);
                if (BoundsClipped != null)
                {
                    BoundsClipped(this);
                }
            }
        }

        public void MatchBounds()
        {
            if (_timetable == null)
                return;
            var maxStart = _timetable.EarlyBound();
            var minEnd = _timetable.LateBound();
            var maxHourStart = maxStart.Hour;
            var minHourEnd = minEnd.Hour;
            if (minEnd.Minute > 0)
                minHourEnd++;
            _hourStart = Math.Max(maxHourStart - 1, 0);
            _hourEnd = Math.Min(minHourEnd + 1, 23);

            const int minHours = 8;
            if (_hourEnd - _hourStart < minHours)
            {
                var before = (minHours - (_hourEnd - _hourStart))/2;
                var after = (minHours - (_hourEnd - _hourStart)) - before;
                if (_hourStart - before < 0)
                {
                    _hourStart = 0;
                    _hourEnd = _hourStart + minHours;
                }
                else if (_hourEnd + after > 23)
                {
                    _hourEnd = 23;
                    HourStart = _hourEnd - minHours;
                }
                else
                {
                    _hourStart = _hourStart - before;
                    _hourEnd = _hourStart + minHours;
                }
            }
            if (BoundsClipped != null)
            {
                BoundsClipped(this);
            }
        }

        #endregion

        #region Activate interaction

        public void SetActive(Stream stream)
        {
            if (stream == null || _activeStream == stream)
                return;
            _activeStream = stream;
            _activeUnavail = null;
            Invalidate();
        }

        public void SetActive(Unavailability unavail)
        {
            if (unavail == null || _activeUnavail == unavail)
                return;
            _activeUnavail = unavail;
            _activeStream = null;
            Invalidate();
        }

        public void ClearActive()
        {
            _activeStream = null;
            _activeUnavail = null;
            Invalidate();
        }

        public void PreviewAlt(Stream stream)
        {
            if (stream == null || _altStream == stream)
                return;
            _altStream = stream;
            Invalidate();
        }

        public void PreviewEquiv(Stream stream)
        {
            if (stream == null || _equivStream == stream)
                return;
            _equivStream = stream;
            Invalidate();
        }

        public void EndPreviewStream()
        {
            if (_altStream == null && _equivStream == null)
                return;
            _altStream = null;
            _equivStream = null;
            Invalidate();
        }

        public void PreviewOptions(Type type)
        {
            if (type == null)
                return;
            if (_optionsType == type)
                return;
            _optionsType = type;
            Invalidate();
        }

        public void EndPreviewOptions()
        {
            if (_optionsType == null)
                return;
            _optionsType = null;
            Invalidate();
        }

        #endregion

        #region Events

        [Category("Action")]
        public event TimetableEventHandler TimetableMouseClick;

        [Category("Action")]
        public event TimetableEventHandler TimetableMouseDoubleClick;

        [Category("Mouse")]
        public event TimetableEventHandler TimetableMouseDown;

        [Category("Action")]
        public event TimetableChangedEventHandler TimetableChanged;

        [Category("Layout")]
        public event ResizeCellEventHandler ResizeCell;

        [Category("Layout")]
        public event BoundsClippedEventHandler BoundsClipped;

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            var time = FindClickTime(e);
            if (ReferenceEquals(time, null))
                return;

            if (TimetableMouseClick != null)
                TimetableMouseClick(this, new TimetableEventArgs(e, time));
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            var time = FindClickTime(e);
            if (ReferenceEquals(time, null))
                return;

            if (TimetableMouseDoubleClick != null)
                TimetableMouseDoubleClick(this, new TimetableEventArgs(e, time));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            var time = FindClickTime(e);
            if (ReferenceEquals(time, null))
                return;

            if (_enableDrag && e.Button == MouseButtons.Left && _timetable != null)
            {
                _dragSession = _timetable.FindClassAt(time, !_showAll);
                var dragUnavail = _timetable.FindUnavailableAt(time);
                if (_dragSession != null)
                {
                    BeginDrag(_dragSession.Stream.Type);
                    _dragCursor = DragCursor(_dragSession);

                    DoDragDrop(_dragSession.Stream.Type, DragDropEffects.Move);

                    EndDrag();
                    _dragCursor = null;
                }
                else if (dragUnavail != null)
                {
                    _hoverUnavail = null;
                    Invalidate();
                    _dragCursor = DragCursor(dragUnavail);

                    DoDragDrop(dragUnavail, DragDropEffects.Move);

                    _hoverUnavail = null;
                    Invalidate();
                    _dragCursor = null;
                }
            }

            if (TimetableMouseDown != null)
                TimetableMouseDown(this, new TimetableEventArgs(e, time));
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (drgevent.Data.GetDataPresent(typeof (Session)) || drgevent.Data.GetDataPresent(typeof (Type)) ||
                drgevent.Data.GetDataPresent(typeof (Unavailability)))
            {
                // hijack the drag/drop event
                drgevent.Effect = DragDropEffects.Move;
            }
            else
            {
                base.OnDragEnter(drgevent);
            }
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            var time = FindClickTime(PointToClient(new Point(drgevent.X, drgevent.Y)));
            // outside of table bounds?
            if (ReferenceEquals(time, null))
            {
                // clear current preview (at edge of timetable)
                EndPreviewStream();
                // cannot drag outside of the actual table
                drgevent.Effect = DragDropEffects.None;
                return;
            }

            // dragging a class
            if (drgevent.Data.GetDataPresent(typeof (Session)) || drgevent.Data.GetDataPresent(typeof (Type)))
            {
                drgevent.Effect = DragDropEffects.Move;
                Type dragType;
                if (drgevent.Data.GetDataPresent(typeof (Session)))
                    dragType = ((Session) drgevent.Data.GetData(typeof (Session))).Stream.Type;
                else
                    dragType = (Type) drgevent.Data.GetData(typeof (Type));

                var session = Timetable.From(dragType).FindClassAt(time, false);
                if (session == null)
                {
                    EndPreviewStream();
                }
                else
                {
                    PreviewEquiv(session.Stream);
                }
            }
            // dragging an unavailability
            else if (drgevent.Data.GetDataPresent(typeof (Unavailability)))
            {
                var dragUnavail = (Unavailability) drgevent.Data.GetData(typeof (Unavailability));
                var offset = new TimeLength(dragUnavail.StartMinute);
                var start = time - dragUnavail.Length/2;
                start -= offset;
                start.RoundToNearestHour();
                start += offset;

                _hoverUnavail = new Timeslot(start.Day, start, (TimeOfDay) start + dragUnavail.Length);
                if (_hoverUnavail.StartTime < new TimeOfDay(_hourStart, 0) ||
                    _hoverUnavail.EndTime > new TimeOfDay(_hourEnd, 0))
                {
                    drgevent.Effect = DragDropEffects.None;
                    _hoverUnavail = null;
                }
                else
                {
                    drgevent.Effect = DragDropEffects.Move;
                }
                Invalidate();
            }
            else
            {
                base.OnDragOver(drgevent);
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            var time = FindClickTime(PointToClient(new Point(drgevent.X, drgevent.Y)));

            if (drgevent.Data.GetDataPresent(typeof (Stream)))
            {
                var dragSession = (Session) drgevent.Data.GetData(typeof (Session));
                var dropSession = Timetable.From(dragSession.Stream.Type).FindClassAt(time, false);
                if (dropSession != null && dropSession.Stream != dragSession.Stream)
                {
                    if (_timetable.SelectStream(dropSession.Stream))
                        TimetableChanged(this);
                }
            }
            if (drgevent.Data.GetDataPresent(typeof (Type)))
            {
                var dragType = (Type) drgevent.Data.GetData(typeof (Type));
                var dropSession = Timetable.From(dragType).FindClassAt(time, false);
                if (dropSession != null)
                {
                    if (_timetable.SelectStream(dropSession.Stream))
                        TimetableChanged(this);
                }
            }
            else if (drgevent.Data.GetDataPresent(typeof (Unavailability)))
            {
                var dragUnavail = (Unavailability) drgevent.Data.GetData(typeof (Unavailability));
                _timetable.UnavailableList.Remove(dragUnavail);
                if (_hoverUnavail != null && _timetable.FreeDuring(_hoverUnavail, true))
                {
                    _timetable.UnavailableList.Add(new Unavailability(dragUnavail.Name, _hoverUnavail));
                    TimetableChanged(this);
                }
                else
                {
                    _timetable.UnavailableList.Add(dragUnavail);
                }
            }
            else
            {
                base.OnDragDrop(drgevent);
            }
        }

        public void BeginDrag(Type type)
        {
            PreviewOptions(type);
        }

        public void EndDrag()
        {
            _dragSession = null;
            EndPreviewOptions();
            EndPreviewStream();
        }

        public Cursor DragCursor(Session s)
        {
            return DragCursor(s.Length, s.Stream.Type.Subject.Color);
        }

        public Cursor DragCursor(Unavailability u)
        {
            return DragCursor(u.Length, Color.DarkGray);
        }

        public Cursor DragCursor(TimeLength length, Color color)
        {
            // draw cursor bitmap
            var r = TimeLengthRectangle(length);
            var b = new Bitmap(r.Width + 1, r.Height + 1);
            var g = Graphics.FromImage(b);
            g.FillRectangle(
                new SolidBrush(Color.FromArgb(200, color)),
                new Rectangle(0, 0, r.Width, r.Height));
            return new Cursor(b.GetHicon());
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
        {
            if (_showDragGhost && _dragCursor != null)
            {
                gfbevent.UseDefaultCursors = false;
                Cursor.Current = _dragCursor;
            }
            base.OnGiveFeedback(gfbevent);
        }

        public TimeOfWeek FindClickTime(MouseEventArgs e)
        {
            return FindClickTime(e.Location);
        }

        public TimeOfWeek FindClickTime(Point location)
        {
            var x = location.X - _table.X;
            var y = location.Y - _table.Y;
            if (x < 0 || x >= _table.Width)
                return null;
            if (y < 0 || y > _table.Height)
                return null;

            var hour = y/CellSize.Height + _hourStart;
            var minute = (y%CellSize.Height)*60/CellSize.Height;
            var day = x/CellSize.Width;
            if (!_showWeekend)
                day++;

            return new TimeOfWeek(day, hour, minute);
        }

        #endregion

        #region Rendering

        public override Font Font
        {
            get
            {
                var scale = Math.Min(CellSize.Width, CellSize.Height*TextAspect)/100f;
                if (scale == 0f)
                    scale = 1f;
                return new Font(base.Font.FontFamily, base.Font.Size*scale);
            }
            set { base.Font = value; }
        }

        public Font FontHeading
        {
            get { return new Font(base.Font.FontFamily, Font.Size*1.25f, FontStyle.Bold); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!_easterEgg)
                DrawTimetable(e.Graphics);
            else
                DrawCrazytable(e.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        public void DrawTimetable(Graphics g)
        {
            FindDimensions();

            DrawBackground(g);

            if (_showDays)
                DrawDays(g);
            if (_showTimes)
                DrawTimes(g);
            if (_showGrayArea)
                DrawGrayArea(g);

            DrawOutline(g);

            DrawUnavailable(g);
            DrawClasses(g);

            DrawUnavailableTarget(g);
            DrawPreview(g);
            DrawOptions(g);
        }

        private void DrawCrazytable(Graphics g)
        {
            FindDimensions();

            DrawBackground(g);

            if (_showDays)
                DrawDays(g);
            if (_showTimes)
                DrawTimes(g);
            if (_showGrayArea)
                DrawGrayArea(g);

            DrawOutline(g);

            DrawAnimation(g);
        }

        private void FindDimensions()
        {
            var w = Bounds.Width - 1;
            var h = Bounds.Height - 1;
            var nx = (_showWeekend ? 7 : 5) + (_showTimes ? 1 : 0);
            var ny = (_hourEnd - _hourStart) + (_showDays ? 1 : 0);

            Cell = new Size(w/nx, h/ny);

            var outer = new Rectangle {Width = CellSize.Width*nx, Height = CellSize.Height*ny};
            outer.X = (w - outer.Width)/2;
            outer.Y = (h - outer.Height)/2;

            _table.Width = CellSize.Width*(_showWeekend ? 7 : 5);
            _table.Height = CellSize.Height*(_hourEnd - _hourStart);
            _table.X = outer.X;
            _table.Y = outer.Y;
            if (_showTimes)
                _table.X += CellSize.Width/2;
            if (_showDays)
                _table.Y += CellSize.Height;
        }

        private void DrawBackground(Graphics g)
        {
            g.FillRectangle(Brushes.White, _table);
        }

        private Pen _outlinePen = Pens.LightGray;

        public Color OutlineColour
        {
            get { return _outlinePen.Color; }
            set { _outlinePen = new Pen(value); }
        }

        private void DrawOutline(Graphics g)
        {
            var x = _table.X;
            for (var i = 0; i <= (_showWeekend ? 7 : 5); i++)
            {
                g.DrawLine(_outlinePen, x, _table.Top, x, _table.Bottom);
                x += CellSize.Width;
            }
            var y = _table.Y;
            for (var i = 0; i <= (_hourEnd - _hourStart); i++)
            {
                g.DrawLine(_outlinePen, _table.Left, y, _table.Right, y);
                y += CellSize.Height;
            }
        }

        private void DrawDays(Graphics g)
        {
            var format = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
            var cell = CellSize;
            cell.Height /= 2;
            var r = new Rectangle(_table.Location, cell);
            r.Offset(0, -cell.Height);

            for (var i = (_showWeekend ? 0 : 1); i < (_showWeekend ? 7 : 6); i++)
            {
                g.DrawString(Days[i], FontHeading, Brushes.Black, r, format);
                r.Offset(cell.Width, 0);
            }
        }

        private void DrawTimes(Graphics g)
        {
            var format = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near};

            var l = new Rectangle(_table.Location, CellSize);
            var r = new Rectangle(_table.Location, CellSize);
            l.Offset(-CellSize.Width/2, 0);
            r.Offset(_table.Width, 0);
            l.Width /= 2;
            r.Width /= 2;

            for (var i = _hourStart; i < _hourEnd; i++)
            {
                var hr = i%24;
                var time = (hr < 12 ? "am" : "pm");
                hr = i%12;
                time = (hr == 0 ? 12 : hr) + time;

                g.DrawString(time, Font, Brushes.Black, l, format);
                g.DrawString(time, Font, Brushes.Black, r, format);

                l.Offset(0, CellSize.Height);
                r.Offset(0, CellSize.Height);
            }
        }

        private void DrawGrayArea(Graphics g)
        {
            if (_timetable == null)
                return;

            for (var day = (_showWeekend ? 0 : 1); day < (_showWeekend ? 7 : 6); day++)
            {
                for (var hour = _hourStart; hour < _hourEnd; hour++)
                {
                    var time = new Timeslot(day, hour, 0, hour + 1, 0);
                    if (!_timetable.ClassDuring(time, false))
                    {
                        DrawTimeslot(g, time, _timeslotUnavalibleColor);
                    }
                }
            }
        }

        private Color _timeslotUnavalibleColor = Color.LightGray;

        public Color TimeslotUnavalibleColour
        {
            get { return _timeslotUnavalibleColor; }
            set { _timeslotUnavalibleColor = value; }
        }

        private void DrawUnavailable(Graphics g)
        {
            if (_timetable == null)
                return;

            foreach (var u in _timetable.UnavailableList)
            {
                if (_activeUnavail == u)
                {
                    DrawTimeslotActive(g, u, Color.DarkGray);
                }
                else
                {
                    DrawTimeslot(g, u, Color.DarkGray);
                }
                if (_showText)
                {
                    DrawTimeslotText(g, u);
                }
            }
        }

        private void DrawClasses(Graphics g)
        {
            if (_timetable == null)
                return;

            foreach (var s in _timetable.ClassList)
            {
                if (!_showAll && !s.Stream.Selected)
                    continue;
                if (_enableDrag && _dragSession != null && s.Stream.Type == _dragSession.Stream.Type)
                    continue;
                if (_altStream != null && s.Stream.Type == _altStream.Type)
                    continue;
                if (_equivStream != null && s.Stream.Type == _equivStream.Type)
                    continue;

                DrawSession(g, s);
            }
        }

        private void DrawUnavailableTarget(Graphics g)
        {
            if (_hoverUnavail != null)
            {
                DrawTransparentTimeslot(g, _hoverUnavail, Color.DarkGray);
            }
        }

        private void DrawPreview(Graphics g)
        {
            if (_altStream != null)
            {
                foreach (var session in _altStream.Classes)
                {
                    DrawTransparentTimeslot(g, session, session.Stream.Type.Subject.Color);
                }
            }
            if (_equivStream != null)
            {
                foreach (var session in _equivStream.Classes)
                {
                    DrawSession(g, session);
                }
            }
        }

        private void DrawOptions(Graphics g)
        {
            if (_optionsType != null)
            {
                foreach (var stream in _optionsType.UniqueStreams)
                {
                    foreach (var session in stream.Classes)
                    {
                        DrawTransparentTimeslot(g, session, _optionsType.Subject.Color);
                    }
                }
            }
        }

        private void DrawSession(Graphics g, Session session)
        {
            if (_activeStream == session.Stream)
            {
                DrawTimeslotActive(g, session, (_grayscale ? Color.DarkGray : session.Stream.Type.Subject.Color));
            }
            else
            {
                DrawTimeslot(g, session, (_grayscale ? Color.DarkGray : session.Stream.Type.Subject.Color));
            }

            if (_showText)
            {
                DrawTimeslotText(g, session);
            }
        }

        private void DrawTimeslot(Graphics g, Timeslot t, Color color)
        {
            var r = TimeslotRectangle(t);

            // solid color
            g.FillRectangle(new SolidBrush(color), r);
            // gradient
            var brush = LinearGradient(r.Location, CellSize.Width, CellSize.Height, color);

            var q = new Rectangle(r.X, r.Y, r.Width, r.Height);

            if (r.Height > CellSize.Height*2)
                r.Height = CellSize.Height*2;
            g.FillRectangle(brush, r);

            g.DrawRectangle(Pens.Black, q);
        }

        private void DrawTimeslotActive(Graphics g, Timeslot t, Color color)
        {
            var r = TimeslotRectangle(t);

            // solid color
            g.FillRectangle(new SolidBrush(color), r);
            // gradient
            var brush = LinearGradientActive(r.Location, CellSize.Width, CellSize.Height, color);

            var q = new Rectangle(r.X, r.Y, r.Width, r.Height);

            if (r.Height > CellSize.Height*2)
                r.Height = CellSize.Height*2;
            g.FillRectangle(brush, r);

            g.DrawRectangle(Pens.Black, q);
        }

        public static LinearGradientBrush LinearGradient(Point offset, int unitWidth, int unitHeight, Color color)
        {
            return new LinearGradientBrush(
                new Point(offset.X, offset.Y - unitHeight*4),
                new Point(offset.X, offset.Y + unitHeight*2),
                Color.White,
                color);
        }

        public static LinearGradientBrush LinearGradientActive(Point offset, int unitWidth, int unitHeight, Color color)
        {
            return new LinearGradientBrush(
                new Point(offset.X, offset.Y - unitHeight*4),
                new Point(offset.X, offset.Y + unitHeight*2),
                Color.Black,
                color);
        }

        private void DrawTimeslotText(Graphics g, Session s)
        {
            var text = s.Stream.Type.Subject + " " + s.Stream;
            if (_showLocation)
                text += "\n" + s.Location;
            DrawTimeslotText(g, s, text);
        }

        private void DrawTimeslotText(Graphics g, Unavailability u)
        {
            DrawTimeslotText(g, u, u.Name);
        }

        private void DrawTimeslotText(Graphics g, Timeslot t, String text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var format = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};

            var r = TimeslotRectangle(t);
            g.DrawString(text, Font, Brushes.Black, r, format);
        }

        private void DrawTransparentTimeslot(Graphics g, Timeslot t, Color color)
        {
            var r = TimeslotRectangle(t);
            g.FillRectangle(new SolidBrush(Color.FromArgb(80, color)), r);
            g.DrawRectangle(new Pen(color, 3f), r);
        }

        public Rectangle TimeslotRectangle(Timeslot t)
        {
            var r = new Rectangle(_table.Location, CellSize);
            r.Offset(CellSize.Width*(t.Day - (_showWeekend ? 0 : 1)),
                CellSize.Height*(t.Start.DayMinutes - HourStart*60)/60);
            r.Height = (int) Math.Ceiling(t.TotalMinutes/60f*CellSize.Height);
            return r;
        }

        public Rectangle TimeLengthRectangle(TimeLength t)
        {
            return new Rectangle(0, 0, CellSize.Width, (int) (t.TotalMinutes/60.0f*CellSize.Height));
        }

        #endregion

        #region Export image

        public void SaveRaster(string fileName)
        {
            SaveRaster(fileName, Size, new Point(0, 0));
        }

        public void SaveRaster(string fileName, Size size, Point offset)
        {
            var b = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(b);

            // ensure text renders OK on transparent background
            // http://weblogs.asp.net/israelio/archive/2006/05/06/445428.aspx
            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

            if (!fileName.EndsWith(".png"))
                g.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);
            g.TranslateTransform(offset.X, offset.Y);
            DrawTimetable(g);
            g.Dispose();
            SaveRaster(fileName, b);
        }

        public void SaveRaster(string fileName, Bitmap b)
        {
            ImageFormat format;
            if (fileName.EndsWith(".png"))
                format = ImageFormat.Png;
            else if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
                format = ImageFormat.Jpeg;
            else if (fileName.EndsWith(".gif"))
                format = ImageFormat.Gif;
            else if (fileName.EndsWith(".bmp"))
                format = ImageFormat.Bmp;
            else
            {
                MessageBox.Show("Invalid file extension provided.", "Save Image", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            if (format != ImageFormat.Jpeg)
            {
                b.Save(fileName, format);
                return;
            }

            // save jpeg at higher-than-default quality
            // http://msdn.microsoft.com/en-au/magazine/cc164121.aspx
            var encoder = new EncoderParameters(1);
            encoder.Param[0] = new EncoderParameter(Encoder.Quality, 95L);

            var codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegCodec = null;
            foreach (var codec in codecs.Where(codec => codec.FormatID == ImageFormat.Jpeg.Guid))
            {
                MessageBox.Show(codec.CodecName);
                jpegCodec = codec;
                break;
            }
            try
            {
                if (jpegCodec == null)
                {
                    throw new NullReferenceException("jpegCodec cannot be null.");
                }
                b.Save(fileName, jpegCodec, encoder);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void SaveVector(string fileName)
        {
            if (!fileName.EndsWith(".emf"))
            {
                MessageBox.Show("Invalid file extension provided.", "Save Image", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            var g = CreateGraphics();
            var hdc = g.GetHdc();
            var mf = new Metafile(fileName, hdc, new Rectangle(0, 0, Width, Height), MetafileFrameUnit.Pixel,
                EmfType.EmfOnly);
            g.ReleaseHdc(hdc);

            var mfg = Graphics.FromImage(mf);
            DrawTimetable(mfg);
            mfg.Dispose();
            mf.Dispose();
        }

        #endregion

        #region Easter egg

        public void GoCrazy()
        {
            if (_timetable == null)
                return;

            _easterEgg = true;
            _animation.Clear();
            foreach (var stream in _timetable.StreamList)
            {
                if (!stream.Selected) continue;
                foreach (var session in stream.Classes)
                {
                    var r = TimeslotRectangle(session);
                    var b = new Bitmap(r.Width + 1, r.Height + 1, PixelFormat.Format24bppRgb);
                    var g = Graphics.FromImage(b);
                    g.TranslateTransform(-r.X, -r.Y);
                    DrawSession(g, session);
                    _animation.Add(new AnimatedSession(b, r.Location, r.Size));
                }
            }
            Invalidate();
            _timer.Start();
        }

        public void NoCrazy()
        {
            _timer.Stop();
            _easterEgg = false;
            Invalidate();
        }

        private void Timestep()
        {
            const float g = 0.0005f;

            var area = _animation.Sum(session => session.Area);
            var centre = new PointF(Width/2f, Height/2f);

            foreach (var session1 in _animation)
            {
                // keep everything centred
                var b = UnitGravitationOn(session1.Centroid, centre);
                var a = new PointF(0f, 0f);
                a.X += b.X*area;
                a.Y += b.Y*area;
                // find contribution of each other object
                foreach (var session2 in _animation)
                {
                    b = UnitGravitationOn(session1.Centroid, session2.Centroid);
                    a.X += b.X*session2.Area;
                    a.Y += b.Y*session2.Area;
                }
                a.X *= g;
                a.Y *= g;
                session1.Timestep(a);
            }

            Invalidate();
        }

        private PointF UnitGravitationOn(PointF p, PointF q)
        {
            var dx = q.X - p.X;
            var dy = q.Y - p.Y;
            var r2 = dx*dx + dy*dy;
            if (r2 < 50)
                r2 = 50;
            //if (r2 == 0)
            //    return new PointF(0, 0);
            return new PointF(dx/r2, dy/r2);
        }

        private void DrawAnimation(Graphics g)
        {
            var offScreen = 0;
            foreach (var session in _animation)
            {
                if (session.Rectangle.IntersectsWith(new Rectangle(0, 0, Width, Height)))
                {
                    try
                    {
                        g.DrawImageUnscaled(session.Bitmap, session.IntegerPosition);
                    }
                    catch
                    {
                        NoCrazy();
                        return;
                    }
                }
                else
                {
                    offScreen++;
                }
            }
            if (offScreen == _animation.Count)
            {
                NoCrazy();
            }
        }

        public class AnimatedSession
        {
            private readonly Bitmap _bitmap;
            private PointF _position = new PointF(0, 0);
            private Size _size = new Size(0, 0);
            private PointF _velocity = new PointF(0, 0);

            public AnimatedSession(Bitmap b, Point pos, Size size)
            {
                _bitmap = b;
                _position = pos;
                _size = size;
            }

            public PointF Position
            {
                get { return _position; }
            }

            public Point IntegerPosition
            {
                get { return new Point((int) _position.X, (int) _position.Y); }
            }

            public PointF Centroid
            {
                get { return new PointF(_position.X + (float) _size.Width/2, _position.Y + (float) _size.Height/2); }
            }

            public Size Size
            {
                get { return _size; }
            }

            public float Area
            {
                get { return _size.Width*_size.Height; }
            }

            public Rectangle Rectangle
            {
                get { return new Rectangle(IntegerPosition, _size); }
            }

            public Bitmap Bitmap
            {
                get { return _bitmap; }
            }

            public void Timestep(PointF acceleration)
            {
                _velocity.X += acceleration.X;
                _velocity.Y += acceleration.Y;
                _position.X += _velocity.X;
                _position.Y += _velocity.Y;
            }
        }

        #endregion
    }

    public delegate void TimetableEventHandler(object sender, TimetableEventArgs e);

    public delegate void TimetableChangedEventHandler(object sender);

    public delegate void ResizeCellEventHandler(object sender);

    public delegate void BoundsClippedEventHandler(object sender);

    public class TimetableEventArgs : MouseEventArgs
    {
        private readonly TimeOfWeek _time;

        #region Constructors

        public TimetableEventArgs(MouseButtons button, int clicks, int x, int y, int delta, TimeOfWeek time)
            : base(button, clicks, x, y, delta)
        {
            _time = time;
        }

        public TimetableEventArgs(MouseEventArgs e, TimeOfWeek time)
            : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            _time = time;
        }

        #endregion

        #region Accessors

        public TimeOfWeek Time
        {
            get { return _time; }
        }

        public int Day
        {
            get { return _time.Day; }
        }

        public int Hour
        {
            get { return _time.Hour; }
        }

        public int Minute
        {
            get { return _time.Minute; }
        }

        #endregion
    }
}
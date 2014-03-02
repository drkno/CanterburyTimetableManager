using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace UniTimetable
{
    public partial class TimetableControl : UserControl
    {
        Timetable Timetable_ = null;
        int HourStart_ = 8;
        int HourEnd_ = 21;
        bool ShowAll_ = false;
        bool ShowDays_ = true;
        bool ShowWeekend_ = true;
        bool ShowTimes_ = true;
        bool ShowText_ = true;
        bool ShowLocation_ = true;
        bool ShowGrayArea_ = true;
        bool Grayscale_ = false;
        bool EnableDrag_ = true;
        bool ShowDragGhost_ = true;

        Size Cell_ = new Size();
        Rectangle Table_ = new Rectangle();
        static readonly string[] Days_ = {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
        const float TextAspect_ = 2.5f;   // rough width:height ratio of text

        // interaction
        Session DragSession_ = null;
        Cursor DragCursor_ = null;
        Timeslot HoverUnavail_ = null;

        Type OptionsType_ = null;
        Stream AltStream_ = null;
        Stream EquivStream_ = null;

        Stream ActiveStream_ = null;
        Unavailability ActiveUnavail_ = null;

        // animation
        bool EasterEgg_ = false;
        List<AnimatedSession> Animation_ = new List<AnimatedSession>();
        Timer Timer_ = new Timer();

        public TimetableControl()
        {
            InitializeComponent();
            DoubleBuffered = true;

            Timer_.Stop();
            Timer_.Interval = 10;
            Timer_.Tick += delegate { Timestep(); };
        }

        #region Accessors

        public Timetable Timetable
        {
            get
            {
                return Timetable_;
            }
            set
            {
                Timetable_ = value;
                ValidateBounds();
                EndPreviewStream();
                EndPreviewOptions();
                Invalidate();
            }
        }

        public void Clear()
        {
            Timetable = null;
        }

        [Category("Appearance")]
        public int HourStart
        {
            get
            {
                return HourStart_;
            }
            set
            {
                if (value > 23)
                    HourStart_ = 23;
                else if (value < 0)
                    HourStart_ = 0;
                else
                    HourStart_ = value;
                ValidateBounds();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public int HourEnd
        {
            get
            {
                return HourEnd_;
            }
            set
            {
                if (value > 24)
                    HourEnd_ = 24;
                else if (value < 1)
                    HourEnd_ = 1;
                else
                    HourEnd_ = value;
                ValidateBounds();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowAll
        {
            get
            {
                return ShowAll_;
            }
            set
            {
                ShowAll_ = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowDays
        {
            get
            {
                return ShowDays_;
            }
            set
            {
                ShowDays_ = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowWeekend
        {
            get
            {
                return ShowWeekend_;
            }
            set
            {
                ShowWeekend_ = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowTimes
        {
            get
            {
                return ShowTimes_;
            }
            set
            {
                ShowTimes_ = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowText
        {
            get
            {
                return ShowText_;
            }
            set
            {
                ShowText_ = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowLocation
        {
            get
            {
                return ShowLocation_;
            }
            set
            {
                ShowLocation_ = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool ShowGrayArea
        {
            get
            {
                return ShowGrayArea_;
            }
            set
            {
                ShowGrayArea_ = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool Grayscale
        {
            get
            {
                return Grayscale_;
            }
            set
            {
                Grayscale_ = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool EnableDrag
        {
            get
            {
                return EnableDrag_;
            }
            set
            {
                EnableDrag_ = value;
            }
        }

        [Category("Appearance")]
        public bool ShowDragGhost
        {
            get
            {
                return ShowDragGhost_;
            }
            set
            {
                ShowDragGhost_ = value;
            }
        }

        public Size CellSize { get { return Cell_; } }
        public Rectangle Table { get { return Table_; } }
        private Size Cell
        {
            set
            {
                if (Cell_ != value)
                {
                    Cell_ = value;
                    if (ResizeCell != null)
                        ResizeCell(this);
                }
            }
        }

        public void SetBounds(int hourStart, int hourEnd)
        {
            if (hourStart >= hourEnd)
            {
                throw new Exception("Start time must be before end time!");
            }
            HourStart_ = hourStart;
            HourEnd_ = hourEnd;
            ValidateBounds();
        }

        public void ValidateBounds()
        {
            if (Timetable_ == null)
                return;
            TimeOfDay maxStart = Timetable_.EarlyBound();
            TimeOfDay minEnd = Timetable_.LateBound();
            int maxHourStart = maxStart.Hour;
            int minHourEnd = minEnd.Hour;
            if (minEnd.Minute > 0)
                minHourEnd++;
            if (HourStart_ > maxHourStart || HourEnd_ < minHourEnd)
            {
                HourStart_ = Math.Min(maxHourStart, HourStart_);
                HourEnd_ = Math.Max(minHourEnd, HourEnd_);
                if (BoundsClipped != null)
                {
                    BoundsClipped(this);
                }
            }
        }

        public void MatchBounds()
        {
            if (Timetable_ == null)
                return;
            TimeOfDay maxStart = Timetable_.EarlyBound();
            TimeOfDay minEnd = Timetable_.LateBound();
            int maxHourStart = maxStart.Hour;
            int minHourEnd = minEnd.Hour;
            if (minEnd.Minute > 0)
                minHourEnd++;
            HourStart_ = Math.Max(maxHourStart - 1, 0);
            HourEnd_ = Math.Min(minHourEnd + 1, 23);

            const int minHours = 8;
            if (HourEnd_ - HourStart_ < minHours)
            {
                int before = (minHours - (HourEnd_ - HourStart_)) / 2;
                int after = (minHours - (HourEnd_ - HourStart_)) - before;
                if (HourStart_ - before < 0)
                {
                    HourStart_ = 0;
                    HourEnd_ = HourStart_ + minHours;
                }
                else if (HourEnd_ + after > 23)
                {
                    HourEnd_ = 23;
                    HourStart = HourEnd_ - minHours;
                }
                else
                {
                    HourStart_ = HourStart_ - before;
                    HourEnd_ = HourStart_ + minHours;
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
            if (stream == null || ActiveStream_ == stream)
                return;
            ActiveStream_ = stream;
            ActiveUnavail_ = null;
            Invalidate();
        }

        public void SetActive(Unavailability unavail)
        {
            if (unavail == null || ActiveUnavail_ == unavail)
                return;
            ActiveUnavail_ = unavail;
            ActiveStream_ = null;
            Invalidate();
        }

        public void ClearActive()
        {
            ActiveStream_ = null;
            ActiveUnavail_ = null;
            Invalidate();
        }

        public void PreviewAlt(Stream stream)
        {
            if (stream == null || AltStream_ == stream)
                return;
            AltStream_ = stream;
            Invalidate();
        }

        public void PreviewEquiv(Stream stream)
        {
            if (stream == null || EquivStream_ == stream)
                return;
            EquivStream_ = stream;
            Invalidate();
        }

        public void EndPreviewStream()
        {
            if (AltStream_ == null && EquivStream_ == null)
                return;
            AltStream_ = null;
            EquivStream_ = null;
            Invalidate();
        }

        public void PreviewOptions(Type type)
        {
            if (type == null)
                return;
            if (OptionsType_ == type)
                return;
            OptionsType_ = type;
            Invalidate();
        }

        public void EndPreviewOptions()
        {
            if (OptionsType_ == null)
                return;
            OptionsType_ = null;
            Invalidate();
        }

        #endregion

        #region Events

        [Category("Action")]
        public event TimetableEventHandler TimetableMouseClick = null;

        [Category("Action")]
        public event TimetableEventHandler TimetableMouseDoubleClick = null;

        [Category("Mouse")]
        public event TimetableEventHandler TimetableMouseDown = null;

        [Category("Action")]
        public event TimetableChangedEventHandler TimetableChanged = null;

        [Category("Layout")]
        public event ResizeCellEventHandler ResizeCell = null;

        [Category("Layout")]
        public event BoundsClippedEventHandler BoundsClipped = null;

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            TimeOfWeek time = FindClickTime(e);
            if (TimeOfWeek.ReferenceEquals(time, null))
                return;

            if (TimetableMouseClick != null)
                TimetableMouseClick(this, new TimetableEventArgs(e, time));
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            TimeOfWeek time = FindClickTime(e);
            if (TimeOfWeek.ReferenceEquals(time, null))
                return;

            if (TimetableMouseDoubleClick != null)
                TimetableMouseDoubleClick(this, new TimetableEventArgs(e, time));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            TimeOfWeek time = FindClickTime(e);
            if (TimeOfWeek.ReferenceEquals(time, null))
                return;

            if (EnableDrag_ && e.Button == MouseButtons.Left && Timetable_ != null)
            {
                DragSession_ = Timetable_.FindClassAt(time, !ShowAll_);
                Unavailability dragUnavail = Timetable_.FindUnavailableAt(time);
                if (DragSession_ != null)
                {
                    BeginDrag(DragSession_.Stream.Type);
                    DragCursor_ = DragCursor(DragSession_);

                    DoDragDrop(DragSession_.Stream.Type, DragDropEffects.Move);

                    EndDrag();
                    DragCursor_ = null;
                }
                else if (dragUnavail != null)
                {
                    HoverUnavail_ = null;
                    Invalidate();
                    DragCursor_ = DragCursor(dragUnavail);

                    DoDragDrop(dragUnavail, DragDropEffects.Move);

                    HoverUnavail_ = null;
                    Invalidate();
                    DragCursor_ = null;
                }
            }

            if (TimetableMouseDown != null)
                TimetableMouseDown(this, new TimetableEventArgs(e, time));
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (drgevent.Data.GetDataPresent(typeof(Session)) || drgevent.Data.GetDataPresent(typeof(Type)) || drgevent.Data.GetDataPresent(typeof(Unavailability)))
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
            TimeOfWeek time = FindClickTime(PointToClient(new Point(drgevent.X, drgevent.Y)));
            // outside of table bounds?
            if (TimeOfWeek.ReferenceEquals(time, null))
            {
                // clear current preview (at edge of timetable)
                EndPreviewStream();
                // cannot drag outside of the actual table
                drgevent.Effect = DragDropEffects.None;
                return;
            }

            // dragging a class
            if (drgevent.Data.GetDataPresent(typeof(Session)) || drgevent.Data.GetDataPresent(typeof(Type)))
            {
                drgevent.Effect = DragDropEffects.Move;
                Type dragType;
                if (drgevent.Data.GetDataPresent(typeof(Session)))
                    dragType = ((Session)drgevent.Data.GetData(typeof(Session))).Stream.Type;
                else
                    dragType = (Type)drgevent.Data.GetData(typeof(Type));

                Session session = Timetable.From(dragType).FindClassAt(time, false);
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
            else if (drgevent.Data.GetDataPresent(typeof(Unavailability)))
            {
                Unavailability dragUnavail = (Unavailability)drgevent.Data.GetData(typeof(Unavailability));
                TimeLength offset = new TimeLength(dragUnavail.StartMinute);
                TimeOfWeek start = time - dragUnavail.Length / 2;
                start -= offset;
                start.RoundToNearestHour();
                start += offset;

                HoverUnavail_ = new Timeslot(start.Day, (TimeOfDay)start, (TimeOfDay)start + dragUnavail.Length);
                if (HoverUnavail_.StartTime < new TimeOfDay(HourStart_, 0) || HoverUnavail_.EndTime > new TimeOfDay(HourEnd_, 0))
                {
                    drgevent.Effect = DragDropEffects.None;
                    HoverUnavail_ = null;
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
            TimeOfWeek time = FindClickTime(PointToClient(new Point(drgevent.X, drgevent.Y)));

            if (drgevent.Data.GetDataPresent(typeof(Stream)))
            {
                Session dragSession = (Session)drgevent.Data.GetData(typeof(Session));
                Session dropSession = Timetable.From(dragSession.Stream.Type).FindClassAt(time, false);
                if (dropSession != null && dropSession.Stream != dragSession.Stream)
                {
                    if (Timetable_.SelectStream(dropSession.Stream))
                        TimetableChanged(this);
                }
            }
            if (drgevent.Data.GetDataPresent(typeof(Type)))
            {
                Type dragType = (Type)drgevent.Data.GetData(typeof(Type));
                Session dropSession = Timetable.From(dragType).FindClassAt(time, false);
                if (dropSession != null)
                {
                    if (Timetable_.SelectStream(dropSession.Stream))
                        TimetableChanged(this);
                }
            }
            else if (drgevent.Data.GetDataPresent(typeof(Unavailability)))
            {
                Unavailability dragUnavail = (Unavailability)drgevent.Data.GetData(typeof(Unavailability));
                Timetable_.UnavailableList.Remove(dragUnavail);
                if (HoverUnavail_ != null && Timetable_.FreeDuring(HoverUnavail_, true))
                {
                    Timetable_.UnavailableList.Add(new Unavailability(dragUnavail.Name, HoverUnavail_));
                    TimetableChanged(this);
                }
                else
                {
                    Timetable_.UnavailableList.Add(dragUnavail);
                }
                dragUnavail = null;
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
            DragSession_ = null;
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
            Rectangle r = TimeLengthRectangle(length);
            Bitmap b = new Bitmap(r.Width + 1, r.Height + 1);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(
                new SolidBrush(Color.FromArgb(200, color)),
                new Rectangle(0, 0, r.Width, r.Height));
            return new Cursor(b.GetHicon());
        }

        /*public Cursor DragCursorDisabled(Timeslot timeslot)
        {
            return DragCursorDisabled(timeslot.Length);
        }

        public Cursor DragCursorDisabled(TimeLength length)
        {
            // draw cursor bitmap
            Rectangle r = TimeLengthRectangle(length);
            Bitmap b = new Bitmap(r.Width, r.Height);
            Graphics g = Graphics.FromImage(b);
            Pen p = new Pen(Color.Black, 1f);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawRectangle(
                p,
                new Rectangle(0, 0, r.Width - 1, r.Height - 1));
            return new Cursor(b.GetHicon());
        }*/

        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
        {
            if (ShowDragGhost_ && DragCursor_ != null)
            {
                gfbevent.UseDefaultCursors = false;
                Cursor.Current = DragCursor_;
            }
            base.OnGiveFeedback(gfbevent);
        }

        public TimeOfWeek FindClickTime(MouseEventArgs e)
        {
            return FindClickTime(e.Location);
        }

        public TimeOfWeek FindClickTime(Point location)
        {
            int x = location.X - Table_.X;
            int y = location.Y - Table_.Y;
            if (x < 0 || x >= Table_.Width)
                return null;
            if (y < 0 || y > Table_.Height)
                return null;

            int hour = y / Cell_.Height + HourStart_;
            int minute = (y % Cell_.Height) * 60 / Cell_.Height;
            int day = x / Cell_.Width;
            if (!ShowWeekend_)
                day++;

            return new TimeOfWeek(day, hour, minute);
        }

        #endregion

        #region Rendering

        public override Font Font
        {
            get
            {
                float scale = Math.Min((float)Cell_.Width, (float)Cell_.Height * TextAspect_) / 100f;
                if (scale == 0)
                    scale = 1f;
                return new Font(base.Font.FontFamily, base.Font.Size * scale);
            }
            set
            {
                base.Font = value;
            }
        }

        public Font FontHeading
        {
            get
            {
                return new Font(base.Font.FontFamily, this.Font.Size * 1.25f, FontStyle.Bold);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!EasterEgg_)
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

            if (ShowDays_)
                DrawDays(g);
            if (ShowTimes_)
                DrawTimes(g);
            if (ShowGrayArea_)
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

            if (ShowDays_)
                DrawDays(g);
            if (ShowTimes_)
                DrawTimes(g);
            if (ShowGrayArea_)
                DrawGrayArea(g);

            DrawOutline(g);

            DrawAnimation(g);
        }

        private void FindDimensions()
        {
            int w = Bounds.Width - 1;
            int h = Bounds.Height - 1;
            int nx = (ShowWeekend_ ? 7 : 5) + (ShowTimes_ ? 1 : 0);
            int ny = (HourEnd_ - HourStart_) + (ShowDays_ ? 1 : 0);

            Cell = new Size(w / nx, h / ny);

            Rectangle outer = new Rectangle();
            outer.Width = Cell_.Width * nx;
            outer.Height = Cell_.Height * ny;
            outer.X = (w - outer.Width) / 2;
            outer.Y = (h - outer.Height) / 2;

            Table_.Width = Cell_.Width * (ShowWeekend_ ? 7 : 5);
            Table_.Height = Cell_.Height * (HourEnd_ - HourStart_);
            Table_.X = outer.X;
            Table_.Y = outer.Y;
            if (ShowTimes_)
                Table_.X += Cell_.Width / 2;
            if (ShowDays_)
                Table_.Y += Cell_.Height;
        }

        private void DrawBackground(Graphics g)
        {
            g.FillRectangle(Brushes.White, Table_);
        }

        private void DrawOutline(Graphics g)
        {
            int x = Table_.X;
            for (int i = 0; i <= (ShowWeekend_ ? 7 : 5); i++)
            {
                g.DrawLine(Pens.Black, x, Table_.Top, x, Table_.Bottom);
                x += Cell_.Width;
            }
            int y = Table_.Y;
            for (int i = 0; i <= (HourEnd_ - HourStart_); i++)
            {
                g.DrawLine(Pens.Black, Table_.Left, y, Table_.Right, y);
                y += Cell_.Height;
            }
        }

        private void DrawDays(Graphics g)
        {
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            Rectangle r = new Rectangle(Table_.Location, Cell_);
            r.Offset(0, -Cell_.Height);

            for (int i = (ShowWeekend_ ? 0 : 1); i < (ShowWeekend_ ? 7 : 6); i++)
            {
                g.DrawString(Days_[i], FontHeading, Brushes.Black, r, format);
                r.Offset(Cell_.Width, 0);
            }
        }

        private void DrawTimes(Graphics g)
        {
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Near;

            Rectangle l = new Rectangle(Table_.Location, Cell_);
            Rectangle r = new Rectangle(Table_.Location, Cell_);
            l.Offset(-Cell_.Width / 2, 0);
            r.Offset(Table_.Width, 0);
            l.Width /= 2;
            r.Width /= 2;

            for (int i = HourStart_; i < HourEnd_; i++)
            {
                int hr = i % 24;
                string time = (hr < 12 ? "am" : "pm");
                hr = i % 12;
                time = (hr == 0 ? 12 : hr).ToString() + time;

                g.DrawString(time, Font, Brushes.Black, l, format);
                g.DrawString(time, Font, Brushes.Black, r, format);

                l.Offset(0, Cell_.Height);
                r.Offset(0, Cell_.Height);
            }
        }

        private void DrawGrayArea(Graphics g)
        {
            if (Timetable_ == null)
                return;

            for (int day = (ShowWeekend_ ? 0 : 1); day < (ShowWeekend_ ? 7 : 6); day++)
            {
                for (int hour = HourStart_; hour < HourEnd_; hour++)
                {
                    Timeslot time = new Timeslot(day, hour, 0, hour+1, 0);
                    if (!Timetable_.ClassDuring(time, false))
                        DrawTimeslot(g, time, Color.LightGray);
                }
            }
        }

        private void DrawUnavailable(Graphics g)
        {
            if (Timetable_ == null)
                return;

            foreach (Unavailability u in Timetable_.UnavailableList)
            {
                if (ActiveUnavail_ == u)
                {
                    DrawTimeslotActive(g, u, Color.DarkGray);
                }
                else
                {
                    DrawTimeslot(g, u, Color.DarkGray);
                }
                if (ShowText_)
                {
                    DrawTimeslotText(g, u);
                }
            }
        }

        private void DrawClasses(Graphics g)
        {
            if (Timetable_ == null)
                return;

            foreach (Session s in Timetable_.ClassList)
            {
                if (!ShowAll_ && !s.Stream.Selected)
                    continue;
                if (EnableDrag_ && DragSession_ != null && s.Stream.Type == DragSession_.Stream.Type)
                    continue;
                if (AltStream_ != null && s.Stream.Type == AltStream_.Type)
                    continue;
                if (EquivStream_ != null && s.Stream.Type == EquivStream_.Type)
                    continue;

                DrawSession(g, s);
            }
        }

        private void DrawActive(Graphics g, Timeslot timeslot, Color color)
        {
            Rectangle r = TimeslotRectangle(timeslot);
            r.X++; r.Y++; r.Width--; r.Height--;
            g.FillRectangle(LinearGradientActive(r.Location, Cell_.Width, Cell_.Height, color), r);
        }

        private void DrawUnavailableTarget(Graphics g)
        {
            if (HoverUnavail_ != null)
            {
                DrawTransparentTimeslot(g, HoverUnavail_, Color.DarkGray);
            }
        }

        private void DrawPreview(Graphics g)
        {
            if (AltStream_ != null)
            {
                foreach (Session session in AltStream_.Classes)
                {
                    DrawTransparentTimeslot(g, session, session.Stream.Type.Subject.Color);
                }
            }
            if (EquivStream_ != null)
            {
                foreach (Session session in EquivStream_.Classes)
                {
                    DrawSession(g, session);
                }
            }
        }

        private void DrawOptions(Graphics g)
        {
            if (OptionsType_ != null)
            {
                foreach (Stream stream in OptionsType_.UniqueStreams)
                {
                    foreach (Session session in stream.Classes)
                    {
                        DrawTransparentTimeslot(g, session, OptionsType_.Subject.Color);
                    }
                }
            }
        }

        private void DrawSession(Graphics g, Session session)
        {
            if (ActiveStream_ == session.Stream)
            {
                DrawTimeslotActive(g, session, (Grayscale_ ? Color.DarkGray : session.Stream.Type.Subject.Color));
            }
            else
            {
                DrawTimeslot(g, session, (Grayscale_ ? Color.DarkGray : session.Stream.Type.Subject.Color));
            }

            if (ShowText_)
            {
                DrawTimeslotText(g, session);
            }
        }

        private void DrawTimeslot(Graphics g, Timeslot t, Color color)
        {
            Rectangle r = TimeslotRectangle(t);

            // solid color
            g.FillRectangle(new SolidBrush(color), r);
            // gradient
            System.Drawing.Drawing2D.LinearGradientBrush brush = LinearGradient(r.Location, Cell_.Width, Cell_.Height, color);

            Rectangle q = new Rectangle(r.X, r.Y, r.Width, r.Height);

            if (r.Height > Cell_.Height * 2)
                r.Height = Cell_.Height * 2;
            g.FillRectangle(brush, r);

            g.DrawRectangle(Pens.Black, q);
        }

        private void DrawTimeslotActive(Graphics g, Timeslot t, Color color)
        {
            Rectangle r = TimeslotRectangle(t);

            // solid color
            g.FillRectangle(new SolidBrush(color), r);
            // gradient
            System.Drawing.Drawing2D.LinearGradientBrush brush = LinearGradientActive(r.Location, Cell_.Width, Cell_.Height, color);

            Rectangle q = new Rectangle(r.X, r.Y, r.Width, r.Height);

            if (r.Height > Cell_.Height * 2)
                r.Height = Cell_.Height * 2;
            g.FillRectangle(brush, r);

            g.DrawRectangle(Pens.Black, q);
        }

        public static System.Drawing.Drawing2D.LinearGradientBrush LinearGradient(Point offset, int unitWidth, int unitHeight, Color color)
        {
            return new System.Drawing.Drawing2D.LinearGradientBrush(
                new Point(offset.X, offset.Y - unitHeight * 4),
                new Point(offset.X, offset.Y + unitHeight * 2),
                Color.White,
                color);
        }

        public static System.Drawing.Drawing2D.LinearGradientBrush LinearGradientActive(Point offset, int unitWidth, int unitHeight, Color color)
        {
            return new System.Drawing.Drawing2D.LinearGradientBrush(
                new Point(offset.X, offset.Y - unitHeight * 4),
                new Point(offset.X, offset.Y + unitHeight * 2),
                Color.Black,
                color);
        }

        private void DrawTimeslotText(Graphics g, Session s)
        {
            string text = s.Stream.Type.Subject + " " + s.Stream.ToString();
            if (ShowLocation_)
                text += "\n" + s.Location;
            DrawTimeslotText(g, s, text);
        }

        private void DrawTimeslotText(Graphics g, Unavailability u)
        {
            DrawTimeslotText(g, u, u.Name);
        }

        private void DrawTimeslotText(Graphics g, Timeslot t, String text)
        {
            if (text == null || text.Length == 0)
                return;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            Rectangle r = TimeslotRectangle(t);
            g.DrawString(text, Font, Brushes.Black, r, format);
        }

        private void DrawTransparentTimeslot(Graphics g, Timeslot t, Color color)
        {
            Rectangle r = TimeslotRectangle(t);
            g.FillRectangle(new SolidBrush(Color.FromArgb(80, color)), r);
            g.DrawRectangle(new Pen(color, 3f), r);
        }

        public Rectangle TimeslotRectangle(Timeslot t)
        {
            Rectangle r = new Rectangle(Table_.Location, Cell_);
            r.Offset(Cell_.Width * (t.Day - (ShowWeekend_ ? 0 : 1)), Cell_.Height * (t.Start.DayMinutes - HourStart * 60) / 60);
            r.Height = (int)Math.Ceiling(t.TotalMinutes / 60f * Cell_.Height);
            return r;
        }

        public Rectangle TimeLengthRectangle(TimeLength t)
        {
            return new Rectangle(0, 0, Cell_.Width, (int)(t.TotalMinutes / 60.0f * Cell_.Height));
        }

        #endregion

        #region Export image

        public void SaveRaster(string fileName)
        {
            SaveRaster(fileName, Size, new Point(0, 0));
        }

        public void SaveRaster(string fileName, Size size, Point offset)
        {
            Bitmap b = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(b);

            // ensure text renders OK on transparent background
            // http://weblogs.asp.net/israelio/archive/2006/05/06/445428.aspx
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

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
                MessageBox.Show("Invalid file extension provided.", "Save Image", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (format == ImageFormat.Jpeg)
            {
                // save jpeg at higher-than-default quality
                // http://msdn.microsoft.com/en-au/magazine/cc164121.aspx
                EncoderParameters encoder = new EncoderParameters(1);
                encoder.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L);

                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegCodec = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.FormatID == ImageFormat.Jpeg.Guid)
                    {
                        MessageBox.Show(codec.CodecName);
                        jpegCodec = codec;
                        break;
                    }
                }
                try
                {
                    b.Save(fileName, jpegCodec, encoder);
                    return;
                }
                catch { }
            }
            b.Save(fileName, format);
        }

        public void SaveVector(string fileName)
        {
            if (!fileName.EndsWith(".emf"))
            {
                MessageBox.Show("Invalid file extension provided.", "Save Image", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Graphics g = CreateGraphics();
            IntPtr hdc = g.GetHdc();
            Metafile mf = new Metafile(fileName, hdc, new Rectangle(0, 0, Width, Height), MetafileFrameUnit.Pixel, EmfType.EmfOnly);
            g.ReleaseHdc(hdc);

            Graphics mfg = Graphics.FromImage(mf);
            DrawTimetable(mfg);
            mfg.Dispose();
            mf.Dispose();
        }

        #endregion

        #region Easter egg

        public void GoCrazy()
        {
            if (Timetable_ == null)
                return;

            EasterEgg_ = true;
            Animation_.Clear();
            foreach (Stream stream in Timetable_.StreamList)
            {
                if (stream.Selected)
                {
                    foreach (Session session in stream.Classes)
                    {
                        Rectangle r = TimeslotRectangle(session);
                        Bitmap b = new Bitmap(r.Width + 1, r.Height + 1, PixelFormat.Format24bppRgb);
                        Graphics g = Graphics.FromImage(b);
                        g.TranslateTransform(-r.X, -r.Y);
                        DrawSession(g, session);
                        Animation_.Add(new AnimatedSession(b, r.Location, r.Size));
                    }
                }
            }
            Invalidate();
            Timer_.Start();
        }

        public void NoCrazy()
        {
            Timer_.Stop();
            EasterEgg_ = false;
            Invalidate();
        }

        private void Timestep()
        {
            const float G = 0.0005f;

            float area = 0f;
            foreach (AnimatedSession session in Animation_)
                area += session.Area;
            PointF centre = new PointF((float)Width / 2f, (float)Height / 2f);

            foreach (AnimatedSession session1 in Animation_)
            {
                // keep everything centred
                PointF b = UnitGravitationOn(session1.Centroid, centre);
                PointF a = new PointF(0f, 0f);
                a.X += b.X * area;
                a.Y += b.Y * area;
                // find contribution of each other object
                foreach (AnimatedSession session2 in Animation_)
                {
                    b = UnitGravitationOn(session1.Centroid, session2.Centroid);
                    a.X += b.X * session2.Area;
                    a.Y += b.Y * session2.Area;
                }
                a.X *= G;
                a.Y *= G;
                session1.Timestep(a);
            }

            Invalidate();
        }

        private PointF UnitGravitationOn(PointF p, PointF q)
        {
            float dx = q.X - p.X;
            float dy = q.Y - p.Y;
            float r2 = dx * dx + dy * dy;
            if (r2 < 50)
                r2 = 50;
            //if (r2 == 0)
            //    return new PointF(0, 0);
            return new PointF(dx / r2, dy / r2);
        }

        private void DrawAnimation(Graphics g)
        {
            int offScreen = 0;
            foreach (AnimatedSession session in Animation_)
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
            if (offScreen == Animation_.Count)
            {
                NoCrazy();
            }
        }

        public class AnimatedSession
        {
            PointF Position_ = new PointF(0, 0);
            PointF Velocity_ = new PointF(0, 0);
            Bitmap Bitmap_ = null;
            Size Size_ = new Size(0, 0);

            public PointF Position { get { return Position_; } }
            public Point IntegerPosition { get { return new Point((int)Position_.X, (int)Position_.Y);}}
            public PointF Centroid { get { return new PointF(Position_.X + (float)Size_.Width / 2, Position_.Y + (float)Size_.Height / 2); } }
            public Size Size { get { return Size_; } }
            public float Area { get { return (float)(Size_.Width * Size_.Height); } }
            public Rectangle Rectangle { get { return new Rectangle(IntegerPosition, Size_); } }
            public Bitmap Bitmap { get { return Bitmap_; } }

            public AnimatedSession(Bitmap b, Point pos, Size size)
            {
                Bitmap_ = b;
                Position_ = pos;
                Size_ = size;
            }

            public void Timestep(PointF acceleration)
            {
                Velocity_.X += acceleration.X;
                Velocity_.Y += acceleration.Y;
                Position_.X += Velocity_.X;
                Position_.Y += Velocity_.Y;
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
        TimeOfWeek Time_;

        #region Constructors

        public TimetableEventArgs(MouseButtons button, int clicks, int x, int y, int delta, TimeOfWeek time)
            : base(button, clicks, x, y, delta)
        {
            Time_ = time;
        }

        public TimetableEventArgs(MouseEventArgs e, TimeOfWeek time)
            : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            Time_ = time;
        }

        #endregion

        #region Accessors

        public TimeOfWeek Time { get { return Time_; } }
        public int Day { get { return Time_.Day; } }
        public int Hour { get { return Time_.Hour; } }
        public int Minute { get { return Time_.Minute; } }

        #endregion
    }
}

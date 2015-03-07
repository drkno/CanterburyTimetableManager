#region

using System;
using System.Xml.Serialization;
using UniTimetable.Model.Time;

#endregion

namespace UniTimetable.Model.Timetable
{
    public class Timeslot : IComparable<Timeslot>
    {
        private readonly int _startYearDay, _endYearDay;
        private TimeOfDay _end, _start;
        private readonly string _weekPattern;
        private const ushort WeekLength = 7;
        private const char WeekInstance = '1';

        #region IComparable<Timeslot> Members

        public int CompareTo(Timeslot other)
        {
            int result;
            // first compare days
            if ((result = Day.CompareTo(other.Day)) != 0)
                return result;
            // same day - compare starts
            if ((result = _start.CompareTo(other._start)) != 0)
                return result;
            // same start - compare end
            return _end.CompareTo(other._end);
        }

        #endregion

        public bool ClashesWith(Timeslot other)
        {
            // if on the same day
            var initialCheck = ((Day == other.Day)
                // and object's start time is within the other's period
                    && ((_start >= other._start && _start < other._end)
                        // or object's end time is within the other's period
                        || (_end > other._start && _end <= other._end)
                        // or start/end times are either side
                        || (_start <= other._start && End >= other._end)));

            if (_startYearDay == -1 || string.IsNullOrWhiteSpace(_weekPattern) || !initialCheck || 
                other._endYearDay < _startYearDay || _endYearDay < other._startYearDay)
            {
                return initialCheck;
            }

            var difference = (other._startYearDay - _startYearDay)/WeekLength;
            int i = 0, j = 0;
            if (difference < 0)
            {
                i = difference * -1;
            }
            else
            {
                j = difference;
            }

            for (; i < _weekPattern.Length && j < other._weekPattern.Length; i++, j++)
            {
                if (_weekPattern[i] == WeekInstance && other._weekPattern[j] == WeekInstance)
                {
                    return true;
                }
            }
            return false;
        }

        public bool EquivalentTo(Timeslot other)
        {
            // return true if they're at the same time
            return (Day == other.Day
                    && _start == other._start
                    && _end == other._end);
        }

        #region Base methods

        public override string ToString()
        {
            return DayOfWeek + " " + _start + "-" + _end;
        }

        #endregion

        #region Constructors

        public Timeslot()
        {
            Day = -1;
            _start = TimeOfDay.Minimum;
            _end = TimeOfDay.Maximum;
        }

        public Timeslot(Timeslot other)
        {
            Day = other.Day;
            _start = new TimeOfDay(other._start);
            _end = new TimeOfDay(other._end);
        }

        public Timeslot(int day, TimeOfDay start, TimeOfDay end)
        {
            Day = day;
            _start = new TimeOfDay(start);
            _end = new TimeOfDay(end);
        }

        public Timeslot(int day, int startYearDay, int startHour, int startMinute, int endHour, int endMinute, string weekPattern = "")
        {
            Day = day;
            _startYearDay = startYearDay;
            _endYearDay = startYearDay + WeekLength * (weekPattern.Length - 1);
            _start = new TimeOfDay(startHour, startMinute);
            _end = new TimeOfDay(endHour, endMinute);
            _weekPattern = weekPattern;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        [XmlIgnore]
        public TimeOfDay StartTime
        {
            get { return _start; }
            set
            {
                if (value > _end)
                {
                    throw new Exception("Start time cannot be before end time.");
                }
                _start = value;
            }
        }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        [XmlIgnore]
        public TimeOfDay EndTime
        {
            get { return _end; }
            set
            {
                if (value < _start)
                {
                    throw new Exception("End time cannot be before start time.");
                }
                _end = value;
            }
        }

        /// <summary>
        /// Gets the start time with day.
        /// </summary>
        [XmlIgnore]
        public TimeOfWeek Start
        {
            get { return new TimeOfWeek(Day, _start); }
        }

        /// <summary>
        /// Gets the end time with day.
        /// </summary>
        [XmlIgnore]
        public TimeOfWeek End
        {
            get { return new TimeOfWeek(Day, _end); }
        }

        /// <summary>
        /// Gets or sets the day as an integer.
        /// </summary>
        [XmlAttribute("day")]
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the day of the week.
        /// </summary>
        [XmlIgnore]
        public DayOfWeek DayOfWeek
        {
            get { return (DayOfWeek) Day; }
            set { Day = (int) value; }
        }

        /// <summary>
        /// Gets the length of the timeslot in minutes.
        /// </summary>
        [XmlIgnore]
        public TimeLength Length
        {
            get { return _end - _start; }
        }

        /// <summary>
        /// Gets the length of the timeslot in minutes.
        /// </summary>
        [XmlIgnore]
        public int TotalMinutes
        {
            get { return _end.DayMinutes - _start.DayMinutes; }
        }

        /// <summary>
        /// Gets or sets the hour of the starting time.
        /// </summary>
        [XmlIgnore]
        public int StartHour
        {
            get { return _start.Hour; }
            set
            {
                TimeOfDay newStart = new TimeOfDay(_start);
                newStart.Hour = value;
                /*if (newStart > End_)
                {
                    throw new Exception("Start time cannot be after end time.");
                }*/
                _start.Hour = value;
            }
        }

        /// <summary>
        /// Gets or sets the minute of the starting time.
        /// </summary>
        [XmlIgnore]
        public int StartMinute
        {
            get { return _start.Minute; }
            set
            {
                TimeOfDay newStart = new TimeOfDay(_start);
                newStart.Minute = value;
                /*if (newStart > End_)
                {
                    throw new Exception("Start time cannot be after end time.");
                }*/
                _start.Minute = value;
            }
        }

        /// <summary>
        /// Gets or sets the total minutes in the starting time.
        /// </summary>
        [XmlAttribute("start")]
        public int StartTotalMinutes
        {
            get { return _start.DayMinutes; }
            set
            {
                TimeOfDay newStart = new TimeOfDay();
                newStart.DayMinutes = value;
                /*if (newStart > End_)
                {
                    throw new Exception("Start time cannot be after end time.");
                }*/
                _start.DayMinutes = value;
            }
        }

        /// <summary>
        /// Gets or sets the hour of the ending time.
        /// </summary>
        [XmlIgnore]
        public int EndHour
        {
            get { return _end.Hour; }
            set
            {
                TimeOfDay newEnd = new TimeOfDay(_end);
                newEnd.Hour = value;
                /*if (newEnd < Start_)
                {
                    throw new Exception("End time cannot be before start time.");
                }*/
                _end.Hour = value;
            }
        }

        /// <summary>
        /// Gets or sets the minute of the ending time.
        /// </summary>
        [XmlIgnore]
        public int EndMinute
        {
            get { return _end.Minute; }
            set
            {
                TimeOfDay newEnd = new TimeOfDay(_end);
                newEnd.Minute = value;
                /*if (newEnd < Start_)
                {
                    throw new Exception("End time cannot be before start time.");
                }*/
                _end.Minute = value;
            }
        }

        /// <summary>
        /// Gets or sets the total minutes in the ending time.
        /// </summary>
        [XmlAttribute("end")]
        public int EndTotalMinutes
        {
            get { return _end.DayMinutes; }
            set
            {
                TimeOfDay newEnd = new TimeOfDay();
                newEnd.DayMinutes = value;
                /*if (newEnd < Start_)
                {
                    throw new Exception("End time cannot be before start time.");
                }*/
                _end.DayMinutes = value;
            }
        }

        #endregion
    }
}
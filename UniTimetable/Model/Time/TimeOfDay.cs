#region

using System;
using System.Xml.Serialization;

#endregion

namespace UniTimetable.Model.Time
{
    public class TimeOfDay : IComparable<TimeOfDay>
    {
        protected int _hour;
        protected int _minute;

        public static TimeOfDay Minimum
        {
            get { return new TimeOfDay(0, 0); }
        }

        public static TimeOfDay Maximum
        {
            get { return new TimeOfDay(23, 59); }
        }

        #region IComparable<TimeOfDay> Members

        public int CompareTo(TimeOfDay other)
        {
            return DayMinutes - other.DayMinutes;
        }

        #endregion

        public static TimeOfDay FromDateTime(DateTime dateTime)
        {
            return new TimeOfDay(dateTime.Hour, dateTime.Minute);
        }

        public static explicit operator int(TimeOfDay time)
        {
            return time.DayMinutes;
        }

        #region Constructors

        public TimeOfDay()
        {
            _hour = 0;
            _minute = 0;
        }

        public TimeOfDay(TimeOfDay other)
        {
            _hour = other._hour;
            _minute = other._minute;
        }

        public TimeOfDay(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
        }

        public TimeOfDay(int totalMinutes)
        {
            DayMinutes = totalMinutes;
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        [XmlIgnore]
        public int Hour
        {
            get { return _hour; }
            set
            {
                if (value < 0 || value > 23)
                {
                    throw new Exception("Hour specified outside range (0-23).");
                }
                _hour = value;
            }
        }

        /// <summary>
        /// Gets or sets the minute.
        /// </summary>
        [XmlIgnore]
        public int Minute
        {
            get { return _minute; }
            set
            {
                if (value < 0 || value > 59)
                {
                    throw new Exception("Minute specified outside range (0-59).");
                }
                _minute = value;
            }
        }

        /// <summary>
        /// Gets or sets the total number of minutes.
        /// </summary>
        [XmlAttribute("time")]
        public int DayMinutes
        {
            get { return 60*_hour + _minute; }
            set
            {
                if (value < 0 || value >= 60*24)
                {
                    throw new Exception("Number of minutes is not within range of a single day.");
                }
                _hour = value/60;
                Minute = value%60;
            }
        }

        /// <summary>
        ///     Adds a number of minutes.
        /// </summary>
        /// <param name="minutes">Number of minutes.</param>
        public void AddMinutes(int minutes)
        {
            int newTotalMinutes = DayMinutes + minutes;
            if (newTotalMinutes < 0 || newTotalMinutes >= 60*24)
            {
                throw new Exception("Time is not within range of a single day.");
            }
            DayMinutes = newTotalMinutes;
        }

        /// <summary>
        ///     Adds a number of hours.
        /// </summary>
        /// <param name="hours">Number of hours.</param>
        public void AddHours(int hours)
        {
            AddMinutes(hours*60);
        }

        public void RoundToNearestHour()
        {
            if (_minute >= 30) _hour++;
            _minute = 0;
        }

        #endregion

        #region Overloaded comparison operators

        public static bool operator <(TimeOfDay time1, TimeOfDay time2)
        {
            return time1.CompareTo(time2) < 0;
        }

        public static bool operator >(TimeOfDay time1, TimeOfDay time2)
        {
            return time1.CompareTo(time2) > 0;
        }

        public static bool operator <=(TimeOfDay time1, TimeOfDay time2)
        {
            return time1.CompareTo(time2) <= 0;
        }

        public static bool operator >=(TimeOfDay time1, TimeOfDay time2)
        {
            return time1.CompareTo(time2) >= 0;
        }

        public static bool operator ==(TimeOfDay time1, TimeOfDay time2)
        {
            return time1.CompareTo(time2) == 0;
        }

        public static bool operator !=(TimeOfDay time1, TimeOfDay time2)
        {
            return time1.CompareTo(time2) != 0;
        }

        #endregion

        #region Overloaded arithmetic operators

        public static TimeLength operator -(TimeOfDay time1, TimeOfDay time2)
        {
            return new TimeLength(time1.DayMinutes - time2.DayMinutes);
        }

        public static TimeOfDay operator +(TimeOfDay time, TimeLength length)
        {
            return new TimeOfDay(time.DayMinutes + length.TotalMinutes);
        }

        public static TimeOfDay operator -(TimeOfDay time, TimeLength length)
        {
            return new TimeOfDay(time.DayMinutes - length.TotalMinutes);
        }

        #endregion

        #region Base methods

        public override bool Equals(object obj)
        {
            return CompareTo((TimeOfDay) obj) == 0;
        }

        public override int GetHashCode()
        {
            return _minute;
        }

        public override string ToString()
        {
            var time = _hour <= 12 ? _hour.ToString() : (_hour - 12).ToString();
            time += ":" + _minute.ToString("00");
            time += _hour >= 12 ? "pm" : "am";
            return time;
        }

        #endregion
    }
}
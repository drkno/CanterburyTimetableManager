#region

using System;

#endregion

namespace UniTimetable.Model.Time
{
    public class TimeOfWeek : TimeOfDay, IComparable<TimeOfWeek>
    {
        private int _day;

        #region IComparable<TimeOfWeek> Members

        public int CompareTo(TimeOfWeek other)
        {
            return WeekMinutes - other.WeekMinutes;
        }

        #endregion

        #region Constructors

        public TimeOfWeek()
        {
            _day = 0;
        }

        public TimeOfWeek(TimeOfWeek other)
            : base(other)
        {
            _day = other._day;
        }

        public TimeOfWeek(int day, TimeOfDay time)
            : base(time)
        {
            _day = day;
        }

        public TimeOfWeek(int day, int hour, int minute)
            : base(hour, minute)
        {
            _day = day;
        }

        public TimeOfWeek(int weekMinutes)
            : base(weekMinutes%(24*60))
        {
            _day = (weekMinutes/(24*60))%7;
        }

        #endregion

        #region Accessors

        public int Day
        {
            get { return _day; }
            set
            {
                _day = value;
                if (_day < 0)
                    _day = 0;
                if (_day > 6)
                    _day = 6;
            }
        }

        public DayOfWeek DayOfWeek
        {
            get { return (DayOfWeek) _day; }
            set { _day = (int) value; }
        }

        public int WeekMinutes
        {
            get { return _day*24*60 + DayMinutes; }
        }

        #endregion

        #region Overloaded comparison operators

        public static bool operator <(TimeOfWeek time1, TimeOfWeek time2)
        {
            return time1.CompareTo(time2) < 0;
        }

        public static bool operator >(TimeOfWeek time1, TimeOfWeek time2)
        {
            return time1.CompareTo(time2) > 0;
        }

        public static bool operator <=(TimeOfWeek time1, TimeOfWeek time2)
        {
            return time1.CompareTo(time2) <= 0;
        }

        public static bool operator >=(TimeOfWeek time1, TimeOfWeek time2)
        {
            return time1.CompareTo(time2) >= 0;
        }

        public static bool operator ==(TimeOfWeek time1, TimeOfWeek time2)
        {
            return time1.CompareTo(time2) == 0;
        }

        public static bool operator !=(TimeOfWeek time1, TimeOfWeek time2)
        {
            return time1.CompareTo(time2) != 0;
        }

        #endregion

        #region Overloaded arithmetic operators

        public static TimeLength operator -(TimeOfWeek time1, TimeOfWeek time2)
        {
            return new TimeLength(time1.WeekMinutes - time2.WeekMinutes);
        }

        public static TimeOfWeek operator +(TimeOfWeek time, TimeLength length)
        {
            return new TimeOfWeek(time.WeekMinutes + length.TotalMinutes);
        }

        public static TimeOfWeek operator -(TimeOfWeek time, TimeLength length)
        {
            return new TimeOfWeek(time.WeekMinutes - length.TotalMinutes);
        }

        #endregion

        #region Base methods

        public override bool Equals(object obj)
        {
            return CompareTo((TimeOfWeek) obj) == 0;
        }

        public override int GetHashCode()
        {
            return _minute;
        }

        public override string ToString()
        {
            return DayOfWeek + " " + base.ToString();
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace UniTimetable
{
    public class TimeOfWeek : TimeOfDay, IComparable<TimeOfWeek>
    {
        protected int Day_;

        #region Constructors

        public TimeOfWeek()
            : base()
        {
            Day_ = 0;
        }

        public TimeOfWeek(TimeOfWeek other)
            : base(other)
        {
            this.Day_ = other.Day_;
        }

        public TimeOfWeek(int day, TimeOfDay time)
            : base(time)
        {
            Day_ = day;
        }

        public TimeOfWeek(int day, int hour, int minute)
            : base(hour, minute)
        {
            Day_ = day;
        }

        public TimeOfWeek(int weekMinutes)
            : base(weekMinutes % (24 * 60))
        {
            Day_ = (weekMinutes / (24 * 60)) % 7;
        }

        #endregion

        #region Accessors

        public int Day
        {
            get
            {
                return Day_;
            }
            set
            {
                Day_ = value;
                if (Day_ < 0)
                    Day_ = 0;
                if (Day_ > 6)
                    Day_ = 6;
            }
        }

        public DayOfWeek DayOfWeek
        {
            get
            {
                return (DayOfWeek)Day_;
            }
            set
            {
                Day_ = (int)value;
            }
        }

        public int WeekMinutes
        {
            get
            {
                return Day_ * 24 * 60 + DayMinutes;
            }
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

        #region IComparable<TimeOfWeek> Members

        public int CompareTo(TimeOfWeek other)
        {
            return this.WeekMinutes - other.WeekMinutes;
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
            return this.CompareTo((TimeOfWeek)obj) == 0;
        }

        public override int GetHashCode()
        {
            return Minute_;
        }

        public override string ToString()
        {
            return DayOfWeek.ToString() + " " + base.ToString();
        }

        #endregion
    }
}

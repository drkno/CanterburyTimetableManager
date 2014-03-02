using System;
using System.Collections.Generic;
using System.Text;

namespace UniTimetable
{
    public class TimeLength : IComparable<TimeLength>
    {
        int Hours_;
        int Minutes_;

        #region Constructors

        public TimeLength()
        {
            Hours_ = 0;
            Minutes_ = 0;
        }

        public TimeLength(TimeLength other)
        {
            this.Hours_ = other.Hours_;
            this.Minutes_ = other.Minutes_;
        }

        public TimeLength(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }

        public TimeLength(int totalMinutes)
        {
            TotalMinutes = totalMinutes;
        }

        #endregion

        public static TimeLength FromTimeSpan(TimeSpan timeSpan)
        {
            return new TimeLength(timeSpan.Hours, timeSpan.Minutes);
        }

        #region Accessors

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        public int Hours
        {
            get
            {
                return Hours_;
            }
            set
            {
                Hours_ = value;
            }
        }

        /// <summary>
        /// Gets or sets the minute.
        /// </summary>
        public int Minutes
        {
            get
            {
                return Minutes_;
            }
            set
            {
                Minutes_ = value;
            }
        }

        /// <summary>
        /// Gets or sets the total number of minutes.
        /// </summary>
        public int TotalMinutes
        {
            get
            {
                return 60 * Hours_ + Minutes_;
            }
            set
            {
                Hours_ = value / 60;
                Minutes_ = value % 60;
            }
        }

        /// <summary>
        /// Adds a number of minutes.
        /// </summary>
        /// <param name="minutes">Number of minutes.</param>
        public void AddMinutes(int minutes)
        {
            TotalMinutes += minutes;
        }

        /// <summary>
        /// Adds a number of hours.
        /// </summary>
        /// <param name="hours">Number of hours.</param>
        public void AddHours(int hours)
        {
            Hours_ += hours;
        }

        #endregion

        #region Overloaded comparison operators

        public static bool operator <(TimeLength time1, TimeLength time2)
        {
            return time1.CompareTo(time2) < 0;
        }

        public static bool operator >(TimeLength time1, TimeLength time2)
        {
            return time1.CompareTo(time2) > 0;
        }

        public static bool operator <=(TimeLength time1, TimeLength time2)
        {
            return time1.CompareTo(time2) <= 0;
        }

        public static bool operator >=(TimeLength time1, TimeLength time2)
        {
            return time1.CompareTo(time2) >= 0;
        }

        public static bool operator ==(TimeLength time1, TimeLength time2)
        {
            return time1.CompareTo(time2) == 0;
        }

        public static bool operator !=(TimeLength time1, TimeLength time2)
        {
            return time1.CompareTo(time2) != 0;
        }

        #endregion

        #region Overloaded arithmetic operators

        public static TimeLength operator +(TimeLength length1, TimeLength length2)
        {
            return new TimeLength(length1.TotalMinutes + length2.TotalMinutes);
        }

        public static TimeLength operator -(TimeLength length1, TimeLength length2)
        {
            return new TimeLength(length1.TotalMinutes - length2.TotalMinutes);
        }

        public static TimeLength operator /(TimeLength length, int divisor)
        {
            return new TimeLength(length.TotalMinutes / divisor);
        }

        public static TimeLength operator *(TimeLength length, int scalar)
        {
            return new TimeLength(length.TotalMinutes * scalar);
        }

        #endregion

        public static explicit operator int(TimeLength time)
        {
            return time.TotalMinutes;
        }

        #region IComparable<TimeLength> Members

        public int CompareTo(TimeLength other)
        {
            return this.TotalMinutes - other.TotalMinutes;
        }

        #endregion

        #region Base methods

        public override bool Equals(object obj)
        {
            return this.CompareTo((TimeLength)obj) == 0;
        }

        public override int GetHashCode()
        {
            return Minutes_;
        }

        public override string ToString()
        {
            return Hours_.ToString() +  ":" + Minutes_.ToString("00");
        }

        #endregion
    }
}

#region

using System;

#endregion

namespace UniTimetable.Model.Time
{
    public class TimeLength : IComparable<TimeLength>
    {
        #region IComparable<TimeLength> Members

        public int CompareTo(TimeLength other)
        {
            return TotalMinutes - other.TotalMinutes;
        }

        #endregion

        public static TimeLength FromTimeSpan(TimeSpan timeSpan)
        {
            return new TimeLength(timeSpan.Hours, timeSpan.Minutes);
        }

        public static explicit operator int(TimeLength time)
        {
            return time.TotalMinutes;
        }

        #region Constructors

        public TimeLength()
        {
            Hours = 0;
            Minutes = 0;
        }

        public TimeLength(TimeLength other)
        {
            Hours = other.Hours;
            Minutes = other.Minutes;
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

        #region Accessors

        /// <summary>
        ///     Gets or sets the hour.
        /// </summary>
        public int Hours { get; set; }

        /// <summary>
        ///     Gets or sets the minute.
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        ///     Gets or sets the total number of minutes.
        /// </summary>
        public int TotalMinutes
        {
            get { return 60*Hours + Minutes; }
            set
            {
                Hours = value/60;
                Minutes = value%60;
            }
        }

        /// <summary>
        ///     Adds a number of minutes.
        /// </summary>
        /// <param name="minutes">Number of minutes.</param>
        public void AddMinutes(int minutes)
        {
            TotalMinutes += minutes;
        }

        /// <summary>
        ///     Adds a number of hours.
        /// </summary>
        /// <param name="hours">Number of hours.</param>
        public void AddHours(int hours)
        {
            Hours += hours;
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
            return new TimeLength(length.TotalMinutes/divisor);
        }

        public static TimeLength operator *(TimeLength length, int scalar)
        {
            return new TimeLength(length.TotalMinutes*scalar);
        }

        #endregion

        #region Base methods

        public override bool Equals(object obj)
        {
            return CompareTo((TimeLength) obj) == 0;
        }

        public override int GetHashCode()
        {
            return Minutes;
        }

        public override string ToString()
        {
            return Hours + ":" + Minutes.ToString("00");
        }

        #endregion
    }
}
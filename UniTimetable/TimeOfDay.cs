using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace UniTimetable
{
    public class TimeOfDay : IComparable<TimeOfDay>
    {
        protected int Hour_;
        protected int Minute_;

        #region Constructors

        public TimeOfDay()
        {
            Hour_ = 0;
            Minute_ = 0;
        }

        public TimeOfDay(TimeOfDay other)
        {
            this.Hour_ = other.Hour_;
            this.Minute_ = other.Minute_;
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

        public static TimeOfDay Minimum
        {
            get
            {
                return new TimeOfDay(0, 0);
            }
        }

        public static TimeOfDay Maximum
        {
            get
            {
                return new TimeOfDay(23, 59);
            }
        }

        public static TimeOfDay FromDateTime(DateTime dateTime)
        {
            return new TimeOfDay(dateTime.Hour, dateTime.Minute);
        }

        #region Accessors

        [XmlIgnore]
        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        public int Hour
        {
            get
            {
                return Hour_;
            }
            set
            {
                if (value < 0 || value > 23)
                {
                    throw new Exception("Hour specified outside range (0-23).");
                }
                Hour_ = value;
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets or sets the minute.
        /// </summary>
        public int Minute
        {
            get
            {
                return Minute_;
            }
            set
            {
                if (value < 0 || value > 59)
                {
                    throw new Exception("Minute specified outside range (0-59).");
                }
                Minute_ = value;
            }
        }

        [XmlAttribute("time")]
        /// <summary>
        /// Gets or sets the total number of minutes.
        /// </summary>
        public int DayMinutes
        {
            get
            {
                return 60 * Hour_ + Minute_;
            }
            set
            {
                if (value < 0 || value >= 60 * 24)
                {
                    throw new Exception("Number of minutes is not within range of a single day.");
                }
                Hour_ = value / 60;
                Minute = value % 60;
            }
        }

        /// <summary>
        /// Adds a number of minutes.
        /// </summary>
        /// <param name="minutes">Number of minutes.</param>
        public void AddMinutes(int minutes)
        {
            int newTotalMinutes = DayMinutes + minutes;
            if (newTotalMinutes < 0 || newTotalMinutes >= 60 * 24)
            {
                throw new Exception("Time is not within range of a single day.");
            }
            DayMinutes = newTotalMinutes;
        }

        /// <summary>
        /// Adds a number of hours.
        /// </summary>
        /// <param name="hours">Number of hours.</param>
        public void AddHours(int hours)
        {
            AddMinutes(hours * 60);
        }

        public void RoundToNearestHour()
        {
            if (Minute_ >= 30)
                Hour_++;
            Minute_ = 0;
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

        public static explicit operator int(TimeOfDay time)
        {
            return time.DayMinutes;
        }

        #region IComparable<TimeOfDay> Members

        public int CompareTo(TimeOfDay other)
        {
            return this.DayMinutes - other.DayMinutes;
        }

        #endregion

        #region Base methods

        public override bool Equals(object obj)
        {
            return this.CompareTo((TimeOfDay)obj) == 0;
        }

        public override int GetHashCode()
        {
            return Minute_;
        }

        public override string ToString()
        {
            string time;
            if (Hour_ <= 12)
                time = Hour_.ToString();
            else
                time = (Hour_ - 12).ToString();
            time += ":" + Minute_.ToString("00");
            if (Hour_ >= 12)
                time += "pm";
            else
                time += "am";
            return time;
        }

        #endregion
    }
}

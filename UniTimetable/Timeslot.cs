using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace UniTimetable
{
    public class Timeslot : IComparable<Timeslot>
    {
        protected int Day_;
        protected TimeOfDay Start_;
        protected TimeOfDay End_;

        #region Constructors

        public Timeslot()
        {
            Day_ = -1;
            Start_ = TimeOfDay.Minimum;
            End_ = TimeOfDay.Maximum;
        }

        public Timeslot(Timeslot other)
        {
            this.Day_ = other.Day_;
            this.Start_ = new TimeOfDay(other.Start_);
            this.End_ = new TimeOfDay(other.End_);
        }

        public Timeslot(int day, TimeOfDay start, TimeOfDay end)
        {
            Day_ = day;
            Start_ = new TimeOfDay(start);
            End_ = new TimeOfDay(end);
        }

        public Timeslot(int day, int startHour, int startMinute, int endHour, int endMinute)
        {
            Day_ = day;
            Start_ = new TimeOfDay(startHour, startMinute);
            End_ = new TimeOfDay(endHour, endMinute);
        }

        #endregion

        #region Accessors

        [XmlIgnore]
        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public TimeOfDay StartTime
        {
            get
            {
                return Start_;
            }
            set
            {
                if (value > End_)
                {
                    throw new Exception("Start time cannot be before end time.");
                }
                Start_ = value;
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        public TimeOfDay EndTime
        {
            get
            {
                return End_;
            }
            set
            {
                if (value < Start_)
                {
                    throw new Exception("End time cannot be before start time.");
                }
                End_ = value;
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets the start time with day.
        /// </summary>
        public TimeOfWeek Start
        {
            get
            {
                return new TimeOfWeek(Day_, Start_);
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets the end time with day.
        /// </summary>
        public TimeOfWeek End
        {
            get
            {
                return new TimeOfWeek(Day_, End_);
            }
        }

        [XmlAttribute("day")]
        /// <summary>
        /// Gets or sets the day as an integer.
        /// </summary>
        public int Day
        {
            get
            {
                return Day_;
            }
            set
            {
                Day_ = value;
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets or sets the day of the week.
        /// </summary>
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

        [XmlIgnore]
        /// <summary>
        /// Gets the length of the timeslot in minutes.
        /// </summary>
        public TimeLength Length
        {
            get
            {
                return End_ - Start_;
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets the length of the timeslot in minutes.
        /// </summary>
        public int TotalMinutes
        {
            get
            {
                return End_.DayMinutes - Start_.DayMinutes;
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets or sets the hour of the starting time.
        /// </summary>
        public int StartHour
        {
            get
            {
                return Start_.Hour;
            }
            set
            {
                TimeOfDay newStart = new TimeOfDay(Start_);
                newStart.Hour = value;
                /*if (newStart > End_)
                {
                    throw new Exception("Start time cannot be after end time.");
                }*/
                Start_.Hour = value;
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets or sets the minute of the starting time.
        /// </summary>
        public int StartMinute
        {
            get
            {
                return Start_.Minute;
            }
            set
            {
                TimeOfDay newStart = new TimeOfDay(Start_);
                newStart.Minute = value;
                /*if (newStart > End_)
                {
                    throw new Exception("Start time cannot be after end time.");
                }*/
                Start_.Minute = value;
            }
        }

        [XmlAttribute("start")]
        /// <summary>
        /// Gets or sets the total minutes in the starting time.
        /// </summary>
        public int StartTotalMinutes
        {
            get
            {
                return Start_.DayMinutes;
            }
            set
            {
                TimeOfDay newStart = new TimeOfDay();
                newStart.DayMinutes = value;
                /*if (newStart > End_)
                {
                    throw new Exception("Start time cannot be after end time.");
                }*/
                Start_.DayMinutes = value;
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets or sets the hour of the ending time.
        /// </summary>
        public int EndHour
        {
            get
            {
                return End_.Hour;
            }
            set
            {
                TimeOfDay newEnd = new TimeOfDay(End_);
                newEnd.Hour = value;
                /*if (newEnd < Start_)
                {
                    throw new Exception("End time cannot be before start time.");
                }*/
                End_.Hour = value;
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets or sets the minute of the ending time.
        /// </summary>
        public int EndMinute
        {
            get
            {
                return End_.Minute;
            }
            set
            {
                TimeOfDay newEnd = new TimeOfDay(End_);
                newEnd.Minute = value;
                /*if (newEnd < Start_)
                {
                    throw new Exception("End time cannot be before start time.");
                }*/
                End_.Minute = value;
            }
        }

        [XmlAttribute("end")]
        /// <summary>
        /// Gets or sets the total minutes in the ending time.
        /// </summary>
        public int EndTotalMinutes
        {
            get
            {
                return End_.DayMinutes;
            }
            set
            {
                TimeOfDay newEnd = new TimeOfDay();
                newEnd.DayMinutes = value;
                /*if (newEnd < Start_)
                {
                    throw new Exception("End time cannot be before start time.");
                }*/
                End_.DayMinutes = value;
            }
        }

        #endregion

        public bool ClashesWith(Timeslot other)
        {
            // return true if on the same day
            return ((this.Day_ == other.Day_)
                // and object's start time is within the other's period
                && ((this.Start_ >= other.Start_ && this.Start_ < other.End_)
                // or object's end time is within the other's period
                    || (this.End_ > other.Start_ && this.End_ <= other.End_)
                // or start/end times are either side
                    || (this.Start_ <= other.Start_ && this.End >= other.End_)));
        }

        public bool EquivalentTo(Timeslot other)
        {
            // return true if they're at the same time
            return (this.Day_ == other.Day_
                && this.Start_ == other.Start_
                && this.End_ == other.End_);
        }

        #region IComparable<Timeslot> Members

        public int CompareTo(Timeslot other)
        {
            int result;
            // first compare days
            if ((result = this.Day_.CompareTo(other.Day_)) != 0)
                return result;
            // same day - compare starts
            if ((result = this.Start_.CompareTo(other.Start_)) != 0)
                return result;
            // same start - compare end
            return this.End_.CompareTo(other.End_);
        }

        #endregion

        #region Base methods

        public override string ToString()
        {
            return DayOfWeek.ToString() + " " + Start_.ToString() + "-" + End_.ToString();
        }

        #endregion
    }
}

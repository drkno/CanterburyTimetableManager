#region

using System;
using System.Xml.Serialization;

#endregion

namespace UniTimetable.Model.Timetable
{
    public class Session : Timeslot, IComparable<Session>
    {
        private string Location_ = "";

        #region IComparable<Session> Members

        public int CompareTo(Session other)
        {
            return base.CompareTo(other);
        }

        #endregion

        #region Constructors

        public Session()
        {
            Stream = null;
        }

        public Session(Timeslot time, string location)
            : base(time)
        {
            Stream = null;
            Location_ = location;
        }

        public Session(Timeslot time, string location, Stream stream)
            : base(time)
        {
            Location_ = location;
            Stream = stream;
        }

        public Session(int day, int startHour, int startMinute, int endHour, int endMinute, string location)
            : base(day, startHour, startMinute, endHour, endMinute)
        {
            Location_ = location;
            Stream = null;
        }

        public Session(int day, int startHour, int startMinute, int endHour, int endMinute, string location,
            Stream stream)
            : base(day, startHour, startMinute, endHour, endMinute)
        {
            Location_ = location;
            Stream = stream;
        }

        public Session(Session other)
            : base(other)
        {
            Location_ = other.Location_;
            Stream = other.Stream;
        }

        public Session Clone()
        {
            return new Session(this);
        }

        #endregion

        #region Accessors

        [XmlAttribute("location")]
        public string Location
        {
            get { return Location_; }
            set { Location_ = value; }
        }

        [XmlIgnore]
        public Stream Stream { get; set; }

        /*
        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        public int Day
        {
            get
            {
                return Day;
            }
            set
            {
                Day = value;
            }
        }

        /// <summary>
        /// Gets or sets the starting hour.
        /// </summary>
        public int StartHour
        {
            get
            {
                return StartHour;
            }
            set
            {
                StartHour = value;
            }
        }

        /// <summary>
        /// Gets or sets the starting minute.
        /// </summary>
        public int StartMinute
        {
            get
            {
                return StartMinute;
            }
            set
            {
                StartMinute = value;
            }
        }

        /// <summary>
        /// Gets or sets the ending hour.
        /// </summary>
        public int EndHour
        {
            get
            {
                return EndHour;
            }
            set
            {
                EndHour = value;
            }
        }

        /// <summary>
        /// Gets or sets the ending minute.
        /// </summary>
        public int EndMinute
        {
            get
            {
                return EndMinute;
            }
            set
            {
                EndMinute = value;
            }
        }

        /// <summary>
        /// Gets or sets the starting time.
        /// </summary>
        public TimeOfDay Start
        {
            get
            {
                return Start;
            }
            set
            {
                Start = value;
            }
        }

        /// <summary>
        /// Gets or sets the ending time.
        /// </summary>
        public TimeOfDay End
        {
            get
            {
                return End;
            }
            set
            {
                End = value;
            }
        }
        */

        #endregion
    }
}
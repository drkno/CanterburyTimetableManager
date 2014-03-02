using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace UniTimetable
{
    public class Session : Timeslot, IComparable<Session>
    {
        string Location_ = "";

        Stream Stream_ = null;

        #region Constructors

        public Session()
            : base() { }

        public Session(Timeslot time, string location)
            : base(time)
        {
            Location_ = location;
        }

        public Session(Timeslot time, string location, Stream stream)
            : base(time)
        {
            Location_ = location;
            Stream_ = stream;
        }

        public Session(int day, int startHour, int startMinute, int endHour, int endMinute, string location)
            : base(day, startHour, startMinute, endHour, endMinute)
        {
            Location_ = location;
            Stream_ = null;
        }

        public Session(int day, int startHour, int startMinute, int endHour, int endMinute, string location, Stream stream)
            : base(day, startHour, startMinute, endHour, endMinute)
        {
            Location_ = location;
            Stream_ = stream;
        }

        public Session(Session other)
            : base(other)
        {
            Location_ = other.Location_;
            Stream_ = other.Stream_;
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
            get
            {
                return Location_;
            }
            set
            {
                Location_ = value;
            }
        }

        [XmlIgnore]
        public Stream Stream
        {
            get
            {
                return Stream_;
            }
            set
            {
                Stream_ = value;
            }
        }
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

        #region IComparable<Session> Members

        public int CompareTo(Session other)
        {
            return base.CompareTo(other);
        }

        #endregion
    }
}

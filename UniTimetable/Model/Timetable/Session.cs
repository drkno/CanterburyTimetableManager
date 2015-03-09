#region

using System;
using System.Xml.Serialization;

#endregion

namespace UniTimetable.Model.Timetable
{
    public class Session : Timeslot, IComparable<Session>
    {
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
            Location = "";
        }

        public Session(int day, int startYearDay, int startHour, int startMinute, int endHour, int endMinute, string location, string weekPattern = "")
            : base(day, startYearDay, startHour, startMinute, endHour, endMinute, weekPattern)
        {
            Location = location;
            Stream = null;
        }

        public Session(Session other)
            : base(other)
        {
            Location = other.Location;
            Stream = other.Stream;
        }

        public Session Clone()
        {
            return new Session(this);
        }

        #endregion

        #region Accessors

        [XmlAttribute("location")]
        public string Location { get; set; }

        [XmlIgnore]
        public Stream Stream { get; set; }

        #endregion
    }
}
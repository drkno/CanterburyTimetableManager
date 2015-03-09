#region

using System.Xml.Serialization;
using UniTimetable.Model.Time;

#endregion

namespace UniTimetable.Model.Timetable
{
    public class Unavailability : Timeslot
    {
        #region Accessors

        [XmlAttribute("name")]
        public string Name { get; set; }

        #endregion

        public Unavailability Clone()
        {
            return new Unavailability(this);
        }

        #region Constructors

        public Unavailability()
        {
            Name = "";
        }

        public Unavailability(Unavailability other)
            : base(other)
        {
            Name = other.Name;
        }

        public Unavailability(string name, Timeslot time)
            : base(time)
        {
            Name = name;
        }

        public Unavailability(string name, int day, TimeOfDay start, TimeOfDay end)
            : base(day, start, end)
        {
            Name = name;
        }

        public Unavailability(string name, int day, int startYearDay, int startHour, int startMinute, int endHour, int endMinute)
            : base(day, startYearDay, startHour, startMinute, endHour, endMinute)
        {
            Name = name;
        }

        #endregion
    }
}
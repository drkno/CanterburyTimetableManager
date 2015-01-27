using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace UniTimetable
{
    public class Unavailability : Timeslot
    {
        string Name_;

        #region Constructors

        public Unavailability()
            : base()
        {
            Name_ = "";
        }

        public Unavailability(Unavailability other)
            : base(other)
        {
            Name_ = other.Name_;
        }

        public Unavailability(string name, Timeslot time)
            : base(time)
        {
            Name_ = name;
        }

        public Unavailability(string name, int day, TimeOfDay start, TimeOfDay end)
            : base(day, start, end)
        {
            Name_ = name;
        }

        public Unavailability(string name, int day, int startHour, int startMinute, int endHour, int endMinute)
            : base(day, startHour, startMinute, endHour, endMinute)
        {
            Name_ = name;
        }

        #endregion

        #region Accessors

        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return Name_;
            }
            set
            {
                Name_ = value;
            }
        }

        #endregion

        public Unavailability Clone()
        {
            return new Unavailability(this);
        }
    }
}

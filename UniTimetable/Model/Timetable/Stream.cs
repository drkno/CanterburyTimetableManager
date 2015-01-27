#region

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace UniTimetable.Model.Timetable
{
    public class Stream : IComparable<Stream>
    {
        private List<Session> Classes_ = new List<Session>();
        private List<Stream> Equivalent_ = new List<Stream>();
        private int ID_ = -1;
        private List<Stream> Incompatible_ = new List<Stream>();
        private int Number_ = -1;

        #region IComparable<Stream> Members

        public int CompareTo(Stream other)
        {
            // sort by name
            int result;
            // compare type codes
            if ((result = Type.Code.CompareTo(other.Type.Code)) != 0)
                return result;
            // then compare stream numbers
            return Number.CompareTo(other.Number);
        }

        #endregion

        #region Base methods

        // TODO: provide some way of handling null => ""?
        public override string ToString()
        {
            string text = Type.Code;
            if (Number_ > 0)
                text += Number_.ToString();
            return text;
        }

        #endregion

        #region Constructors

        public Stream()
        {
            ClashTable = null;
            Type = null;
            Selected = false;
            Classes_ = new List<Session>();
            //State_ = StreamState.Null;
            Incompatible_ = new List<Stream>();
            Equivalent_ = new List<Stream>();
        }

        public Stream(int number)
        {
            ClashTable = null;
            Type = null;
            Selected = false;
            Number_ = number;
        }

        public Stream(int number, Type type)
        {
            ClashTable = null;
            Selected = false;
            Number_ = number;
            Type = type;
        }

        public Stream(Stream other)
        {
            ClashTable = null;
            ID_ = other.ID_;
            Number_ = other.Number_;
            Selected = other.Selected;
            Type = other.Type;
            Classes_ = new List<Session>(other.Classes_);
            Incompatible_ = new List<Stream>(other.Incompatible_);
            Equivalent_ = new List<Stream>(other.Equivalent_);
            //this.State_ = other.State_;
        }

        public Stream Clone()
        {
            return new Stream(this);
        }

        #endregion

        #region Accessors

        [XmlIgnore]
        public int ID
        {
            get { return ID_; }
            set { ID_ = value; }
        }

        [XmlAttribute("number")]
        public int Number
        {
            get { return Number_; }
            set { Number_ = value; }
        }

        /*public StreamState State
        {
            get
            {
                return State_;
            }
            set
            {
                State_ = value;
            }
        }*/

        [XmlAttribute("selected")]
        public bool Selected { get; set; }

        [XmlIgnore]
        public Type Type { get; set; }

        [XmlArray("sessions"), XmlArrayItem("session", typeof (Session))]
        public List<Session> Classes
        {
            get { return Classes_; }
            set { Classes_ = value; }
        }

        [XmlIgnore]
        public bool[] ClashTable { get; set; }

        [XmlIgnore]
        public List<Stream> Incompatible
        {
            get { return Incompatible_; }
            set { Incompatible_ = value; }
        }

        [XmlIgnore]
        public List<Stream> Equivalent
        {
            get { return Equivalent_; }
            set { Equivalent_ = value; }
        }

        #endregion

        #region Processing data

        public bool ClashesWith(Stream other)
        {
            if (ClashTable != null)
                return ClashTable[other.ID_];

            if (other == this)
                return false;

            foreach (Session a in Classes)
            {
                foreach (Session b in other.Classes)
                {
                    if (a.ClashesWith(b))
                        return true;
                }
            }
            return false;
        }

        public bool EquivalentTo(Stream other)
        {
            // streams can't be equivalent if they don't have the same number of classes
            if (Classes.Count != other.Classes.Count)
                return false;

            int matches = 0;
            // compare each class in stream 1
            for (int i = 0; i == matches && i < Classes.Count; i++)
            {
                // against each class in stream 2
                for (int j = 0; j < other.Classes.Count; j++)
                {
                    if (Classes[i].EquivalentTo(other.Classes[j]))
                    {
                        matches++;
                        break;
                    }
                }
            }

            // return true if the number of class matches equals the number of classes
            return (matches == Classes.Count);
            // TODO: this method may fail if one stream contains equivalent classes
        }

        #endregion
    }
}
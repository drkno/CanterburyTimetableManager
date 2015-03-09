#region

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace UniTimetable.Model.Timetable
{
    public class Stream : IComparable<Stream>
    {
        private List<Stream> _equivalent = new List<Stream>();
        private int _id = -1;
        private List<Stream> _incompatible = new List<Stream>();
        private string _number;

        #region IComparable<Stream> Members

        public int CompareTo(Stream other)
        {
            // sort by name
            int result;
            // compare type codes
            return (result = String.Compare(Type.Code, other.Type.Code, StringComparison.Ordinal)) != 0 ? result : String.Compare(Number, other.Number, StringComparison.Ordinal);
            // then compare stream numbers
        }

        #endregion

        #region Base methods

        // TODO: provide some way of handling null => ""?
        public override string ToString()
        {
            var text = Type.Code;
            if (string.IsNullOrWhiteSpace(_number))
                text += _number;
            return text;
        }

        #endregion

        #region Constructors

        public Stream()
        {
            ClashTable = null;
            Type = null;
            Selected = false;
            Classes = new List<Session>();
            _incompatible = new List<Stream>();
            _equivalent = new List<Stream>();
        }

        public Stream(string number) : this()
        {
            _number = number;
        }

        public Stream(Stream other)
        {
            ClashTable = null;
            _id = other._id;
            _number = other._number;
            Selected = other.Selected;
            Type = other.Type;
            Classes = new List<Session>(other.Classes);
            _incompatible = new List<Stream>(other._incompatible);
            _equivalent = new List<Stream>(other._equivalent);
        }

        public Stream Clone()
        {
            return new Stream(this);
        }

        #endregion

        #region Accessors

        [XmlIgnore]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [XmlAttribute("number")]
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        [XmlAttribute("selected")]
        public bool Selected { get; set; }

        [XmlIgnore]
        public Type Type { get; set; }

        [XmlArray("sessions"), XmlArrayItem("session", typeof (Session))]
        public List<Session> Classes { get; set; }

        [XmlIgnore]
        public bool[] ClashTable { get; set; }

        [XmlIgnore]
        public List<Stream> Incompatible
        {
            get { return _incompatible; }
            set { _incompatible = value; }
        }

        [XmlIgnore]
        public List<Stream> Equivalent
        {
            get { return _equivalent; }
            set { _equivalent = value; }
        }

        #endregion

        #region Processing data

        public bool ClashesWith(Stream other)
        {
            if (ClashTable != null)
                return ClashTable[other._id];

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
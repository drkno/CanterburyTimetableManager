using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace UniTimetable
{
    public class Stream : IComparable<Stream>
    {
        int ID_ = -1;
        int Number_ = -1;
        bool Selected_ = false;
        Type Type_ = null;
        List<Session> Classes_ = new List<Session>();

        bool[] ClashTable_ = null;
        List<Stream> Incompatible_ = new List<Stream>();
        List<Stream> Equivalent_ = new List<Stream>();

        #region Constructors

        public Stream()
        {
            Classes_ = new List<Session>();
            //State_ = StreamState.Null;
            Incompatible_ = new List<Stream>();
            Equivalent_ = new List<Stream>();
        }

        public Stream(int number)
        {
            Number_ = number;
        }

        public Stream(int number, Type type)
        {
            Number_ = number;
            Type_ = type;
        }

        public Stream(Stream other)
        {
            this.ID_ = other.ID_;
            this.Number_ = other.Number_;
            this.Selected_ = other.Selected_;
            this.Type_ = other.Type_;
            this.Classes_ = new List<Session>(other.Classes_);
            this.Incompatible_ = new List<Stream>(other.Incompatible_);
            this.Equivalent_ = new List<Stream>(other.Equivalent_);
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
            get
            {
                return ID_;
            }
            set
            {
                ID_ = value;
            }
        }

        [XmlAttribute("number")]
        public int Number
        {
            get
            {
                return Number_;
            }
            set
            {
                Number_ = value;
            }
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
        public bool Selected
        {
            get
            {
                return Selected_;
            }
            set
            {
                Selected_ = value;
            }
        }

        [XmlIgnore]
        public Type Type
        {
            get
            {
                return Type_;
            }
            set
            {
                Type_ = value;
            }
        }

        [XmlArray("sessions"), XmlArrayItem("session", typeof(Session))]
        public List<Session> Classes
        {
            get
            {
                return Classes_;
            }
            set
            {
                Classes_ = value;
            }
        }

        [XmlIgnore]
        public bool[] ClashTable
        {
            get
            {
                return ClashTable_;
            }
            set
            {
                ClashTable_ = value;
            }
        }

        [XmlIgnore]
        public List<Stream> Incompatible
        {
            get
            {
                return Incompatible_;
            }
            set
            {
                Incompatible_ = value;
            }
        }

        [XmlIgnore]
        public List<Stream> Equivalent
        {
            get
            {
                return Equivalent_;
            }
            set
            {
                Equivalent_ = value;
            }
        }

        #endregion

        #region IComparable<Stream> Members

        public int CompareTo(Stream other)
        {
            // sort by name
            int result;
            // compare type codes
            if ((result = this.Type.Code.CompareTo(other.Type.Code)) != 0)
                return result;
            // then compare stream numbers
            return this.Number.CompareTo(other.Number);
        }

        #endregion

        #region Processing data

        public bool ClashesWith(Stream other)
        {
            if (ClashTable_ != null)
                return ClashTable_[other.ID_];

            if (other == this)
                return false;

            foreach (Session a in this.Classes)
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
            if (this.Classes.Count != other.Classes.Count)
                return false;

            int matches = 0;
            // compare each class in stream 1
            for (int i = 0; i == matches && i < this.Classes.Count; i++)
            {
                // against each class in stream 2
                for (int j = 0; j < other.Classes.Count; j++)
                {
                    if (this.Classes[i].EquivalentTo(other.Classes[j]))
                    {
                        matches++;
                        break;
                    }
                }
            }

            // return true if the number of class matches equals the number of classes
            return (matches == this.Classes.Count);
            // TODO: this method may fail if one stream contains equivalent classes
        }

        #endregion

        #region Base methods

        // TODO: provide some way of handling null => ""?
        public override string ToString()
        {
            string text = Type_.Code;
            if (Number_ > 0)
                text += Number_.ToString();
            return text;
        }

        #endregion
    }
}

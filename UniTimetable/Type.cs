using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace UniTimetable
{
    public class Type : IComparable<Type>
    {
        int ID_ = -1;
        string Name_ = "";
        string Code_ = "?";
        bool Required_ = true;

        Subject Subject_ = null;
        List<Stream> Streams_ = new List<Stream>();

        bool[] ClashTable_ = null;
        List<Stream> UniqueStreams_ = new List<Stream>();

        #region Constructors

        public Type()
        {
        }

        public Type(string name, string code)
        {
            Name_ = name;
            Code_ = code;
        }

        public Type(string name, string code, Subject subject)
        {
            Name_ = name;
            Code_ = code;
            Subject_ = subject;
        }

        public Type(Type other)
        {
            this.Name_ = other.Name_;
            this.Code_ = other.Code_;
            this.Required_ = other.Required_;
            this.Subject_ = other.Subject_;
            this.Streams_ = new List<Stream>(other.Streams_);
            //this.State_ = other.State_;
            this.UniqueStreams_ = new List<Stream>(other.UniqueStreams_);
        }

        public Type Clone()
        {
            return new Type(this);
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

        [XmlAttribute("code")]
        public string Code
        {
            get
            {
                return Code_;
            }
            set
            {
                Code_ = value;
            }
        }

        [XmlAttribute("required")]
        public bool Required
        {
            get
            {
                return Required_;
            }
            set
            {
                Required_ = value;
            }
        }

        /*public TypeState State
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

        [XmlIgnore]
        public Subject Subject
        {
            get
            {
                return Subject_;
            }
            set
            {
                Subject_ = value;
            }
        }

        [XmlArray("streams"), XmlArrayItem("stream", typeof(Stream))]
        public List<Stream> Streams
        {
            get
            {
                return Streams_;
            }
            set
            {
                Streams_ = value;
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
        public List<Stream> UniqueStreams
        {
            get
            {
                return UniqueStreams_;
            }
            set
            {
                UniqueStreams_ = value;
            }
        }
        
        [XmlIgnore]
        /// <summary>
        /// Gets the enabled stream in the type.
        /// </summary>
        public Stream SelectedStream
        {
            get
            {
                foreach (Stream stream in Streams_)
                {
                    if (stream.Selected)
                        return stream;
                }
                return null;
            }
        }

        [XmlIgnore]
        /// <summary>
        /// Gets the number of available streams in the type.
        /// </summary>
        public int NumberAvailable
        {
            get
            {
                /*int count = 0;
                foreach (Stream stream in Streams_)
                {
                    // increment counter for each available stream
                    if (stream.IsAvailable())
                    {
                        count++;
                    }
                }
                return count;*/
                return Streams_.Count;
            }
        }

        public Session FindShortestSession()
        {
            Session min = null;
            foreach (Stream stream in Streams_)
            {
                foreach (Session session in stream.Classes)
                {
                    if (min == null || session.Length < min.Length)
                        min = session;
                }
            }
            return min;
        }

        #endregion

        #region Processing data

        public bool ClashesWith(Type other)
        {
            if (ClashTable_ != null)
                return ClashTable_[other.ID_];
            
            // if there exists a stream in type a and a stream in type b that don't clash, the types are compatible
            foreach (Stream a in this.Streams)
            {
                foreach (Stream b in other.Streams)
                {
                    if (!a.ClashesWith(b))
                        return false;
                }
            }
            return true;
        }

        public void BuildEquivalency()
        {
            // clear unique streams list
            UniqueStreams_.Clear();

            // fill the lookup list with -1
            int[] lookup = new int[Streams.Count];
            for (int i = 0; i < lookup.Length; i++)
            {
                lookup[i] = -1;
            }
            // for each stream in the current type
            for (int i = 0; i < lookup.Length; i++)
            {
                // if the lookup entry isn't set
                if (lookup[i] == -1)
                {
                    // first stream to be examined in the set of equivalents
                    UniqueStreams_.Add(Streams_[i]);
                    // for all streams not yet examined in the current type
                    for (int j = i + 1; j < lookup.Length; j++)
                    {
                        // stream is equivalent to a previous, different stream
                        if (lookup[j] > -1)
                            continue;
                        // check if the streams are equivalent
                        if (Streams[i].EquivalentTo(Streams[j]))
                        {
                            // add the stream to its equivalent list
                            Streams[i].Equivalent.Add(Streams[j]);
                            // refer the stream's lookup back to the earlier stream
                            lookup[j] = i;
                        }
                    }
                }
                // if the lookup entry is set (already identified as equivalent)
                else
                {
                    // add the earliest stream in the equivalent set
                    Streams[i].Equivalent.Add(Streams[lookup[i]]);
                    // add all the other equivalent streams
                    Streams[i].Equivalent.AddRange(Streams[lookup[i]].Equivalent);
                    // remove the reference to itself
                    Streams[i].Equivalent.Remove(Streams[i]);
                }
            }
        }

        #endregion

        #region IComparable<Type> Members

        public int CompareTo(Type other)
        {
            return this.Name_.CompareTo(other.Name_);
        }

        #endregion

        #region Base methods

        public override string ToString()
        {
            return Name_ + " (" + Code_.ToString() + ")";
        }

        #endregion
    }
}

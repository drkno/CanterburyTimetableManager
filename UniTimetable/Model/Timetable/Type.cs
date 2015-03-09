#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

#endregion

namespace UniTimetable.Model.Timetable
{
    public class Type : IComparable<Type>
    {
        #region IComparable<Type> Members

        public int CompareTo(Type other)
        {
            return string.CompareOrdinal(Name, other.Name);
        }

        #endregion

        #region Base methods

        public override string ToString()
        {
            return Name + " (" + Code + ")";
        }

        #endregion

        #region Constructors

        private Type()
        {
            UniqueStreams = new List<Stream>();
            Streams = new List<Stream>();
            Required = true;
            Code = "?";
            Id = -1;
            ClashTable = null;
        }

        public Type(string name, string code, Subject subject) : this()
        {
            Name = name;
            Code = code;
            Subject = subject;
            Required = true;
            Streams = new List<Stream>();
            UniqueStreams = new List<Stream>();
        }

        public Type(Type other) : this()
        {
            Name = other.Name;
            Code = other.Code;
            Required = other.Required;
            Subject = other.Subject;
            Streams = new List<Stream>(other.Streams);
            UniqueStreams = new List<Stream>(other.UniqueStreams);
        }

        public Type Clone()
        {
            return new Type(this);
        }

        #endregion

        #region Accessors

        [XmlIgnore]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("code")]
        public string Code { get; set; }

        [XmlAttribute("required")]
        public bool Required { get; set; }

        [XmlIgnore]
        public Subject Subject { get; set; }

        [XmlArray("streams"), XmlArrayItem("stream", typeof (Stream))]
        public List<Stream> Streams { get; set; }

        [XmlIgnore]
        public bool[] ClashTable { get; set; }

        [XmlIgnore]
        public List<Stream> UniqueStreams { get; set; }

        /// <summary>
        /// Gets the enabled stream in the type.
        /// </summary>
        [XmlIgnore]
        public Stream SelectedStream
        {
            get
            {
                return Streams.FirstOrDefault(stream => stream.Selected);
            }
        }

        public Session FindShortestSession()
        {
            Session min = null;
            foreach (Stream stream in Streams)
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
            if (ClashTable != null)
                return ClashTable[other.Id];

            // if there exists a stream in type a and a stream in type b that don't clash, the types are compatible
            foreach (Stream a in Streams)
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
            UniqueStreams.Clear();

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
                    UniqueStreams.Add(Streams[i]);
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
    }
}
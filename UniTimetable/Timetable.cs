using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Drawing.Imaging;
using System.Xml.Serialization;

namespace UniTimetable
{
    [XmlRoot("timetable")]
    public class Timetable
    {
        [XmlArray("subjects"), XmlArrayItem("subject", typeof(Subject))]
        public List<Subject> SubjectList = new List<Subject>();

        [XmlIgnore]
        public List<Type> TypeList = new List<Type>();

        [XmlIgnore]
        public List<Stream> StreamList = new List<Stream>();

        [XmlIgnore]
        public List<Session> ClassList = new List<Session>();

        [XmlArray("unavailabilities"), XmlArrayItem("unavailability")]
        public List<Unavailability> UnavailableList = new List<Unavailability>();

        public static List<string> Days = new List<string>(new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" });
        [XmlIgnore]

        public bool RecomputeSolutions = true;

        private bool[][] StreamClashTable_ = null;
        private bool[][] TypeClashTable_ = null;

        #region Constructors

        public Timetable()
        {
        }

        public static Timetable From(Subject subject)
        {
            Timetable t = new Timetable();
            t.SubjectList.Add(subject);
            t.TypeList.AddRange(subject.Types);
            foreach (Type type in subject.Types)
            {
                t.StreamList.AddRange(type.Streams);
                foreach (Stream stream in type.Streams)
                {
                    t.ClassList.AddRange(stream.Classes);
                }
            }
            return t;
        }

        public static Timetable From(Type type)
        {
            Timetable t = new Timetable();
            t.SubjectList.Add(type.Subject);
            t.TypeList.Add(type);
            t.StreamList.AddRange(type.Streams);
            foreach (Stream stream in type.Streams)
            {
                t.ClassList.AddRange(stream.Classes);
            }
            return t;
        }

        public static Timetable From(Stream stream)
        {
            Timetable t = new Timetable();
            t.SubjectList.Add(stream.Type.Subject);
            t.TypeList.Add(stream.Type);
            t.StreamList.Add(stream);
            t.ClassList.AddRange(stream.Classes);
            return t;
        }

        /*public Timetable ShallowCopy()
        {
            Timetable t = new Timetable();
            t.SubjectList.AddRange(this.SubjectList);
            t.TypeList.AddRange(this.TypeList);
            t.StreamList.AddRange(this.StreamList);
            t.ClassList.AddRange(this.ClassList);
        }*/

        // construct a new timetable object with enough information to render a preview
        public Timetable PreviewSolution(Solver.Solution solution)
        {
            Timetable t = new Timetable();
            for (int i = 0; i < StreamList.Count; i++)
            {
                Stream stream = new Stream(StreamList[i]);
                if (solution.Streams.Contains(StreamList[i]))
                    stream.Selected = true;
                t.StreamList.Add(stream);

                stream.Classes.Clear();
                stream.Incompatible.Clear();
                stream.Equivalent.Clear();

                for (int j = 0; j < StreamList[i].Classes.Count; j++)
                {
                    Session session = new Session(StreamList[i].Classes[j]);
                    session.Stream = stream;
                    stream.Classes.Add(session);
                    t.ClassList.Add(session);
                }
            }
            t.UnavailableList = this.UnavailableList;

            return t;
        }

        public Timetable(Timetable other)
        {
            #region Clone subject/type/stream/class data

            // copy across all the subject data
            this.SubjectList = new List<Subject>();
            foreach (Subject subject in other.SubjectList)
                this.SubjectList.Add(subject.Clone());
            // copy all the type data
            this.TypeList = new List<Type>();
            foreach (Type type in other.TypeList)
                this.TypeList.Add(type.Clone());
            // copy all the stream data
            this.StreamList = new List<Stream>();
            foreach (Stream stream in other.StreamList)
                this.StreamList.Add(stream.Clone());
            // copy all the session data
            this.ClassList = new List<Session>();
            foreach (Session session in other.ClassList)
                this.ClassList.Add(session.Clone());

            // update reference to subjects
            for (int i = 0; i < this.SubjectList.Count; i++)
            {
                foreach (Type type in this.TypeList)
                {
                    if (type.Subject == other.SubjectList[i])
                        type.Subject = this.SubjectList[i];
                }
            }
            // update references to types
            for (int i = 0; i < this.TypeList.Count; i++)
            {
                // parent references
                foreach (Stream stream in this.StreamList)
                {
                    if (stream.Type == other.TypeList[i])
                        stream.Type = this.TypeList[i];
                }
                // child references
                for (int j = 0; j < this.TypeList[i].Subject.Types.Count; j++)
                {
                    if (this.TypeList[i].Subject.Types[j] == other.TypeList[i])
                    {
                        this.TypeList[i].Subject.Types[j] = this.TypeList[i];
                        break;
                    }
                }
                // unique streams
                for (int j = 0; j < this.TypeList[i].UniqueStreams.Count; j++)
                {
                    if (this.StreamList[i].Type.Streams[j] == other.StreamList[i])
                    {
                        this.StreamList[i].Type.Streams[j] = this.StreamList[i];
                        break;
                    }
                }
            }
            // update references to streams
            for (int i = 0; i < this.StreamList.Count; i++)
            {
                // parent references
                foreach (Session session in this.ClassList)
                {
                    if (session.Stream == other.StreamList[i])
                        session.Stream = this.StreamList[i];
                }
                // child references
                for (int j = 0; j < this.StreamList[i].Type.Streams.Count; j++)
                {
                    if (this.StreamList[i].Type.Streams[j] == other.StreamList[i])
                    {
                        this.StreamList[i].Type.Streams[j] = this.StreamList[i];
                        break;
                    }
                }
                // find all equivalent streams set to other.StreamList[i]
                for (int j = 0; j < this.StreamList.Count; j++)
                {
                    for (int k = 0; k < this.StreamList[j].Equivalent.Count; k++)
                    {
                        if (this.StreamList[j].Equivalent[k] == other.StreamList[i])
                        {
                            this.StreamList[j].Equivalent[k] = this.StreamList[i];
                            break;
                        }
                    }
                }
                // find all incompatible streams set to other.StreamList[i]
                for (int j = 0; j < this.StreamList.Count; j++)
                {
                    for (int k = 0; k < this.StreamList[j].Incompatible.Count; k++)
                    {
                        if (this.StreamList[j].Incompatible[k] == other.StreamList[i])
                        {
                            this.StreamList[j].Incompatible[k] = this.StreamList[i];
                            break;
                        }
                    }
                }
                // find all unique streams set to other.StreamList[i]
                foreach (Type type in this.TypeList)
                {
                    for (int j = 0; j < type.UniqueStreams.Count; j++)
                    {
                        if (type.UniqueStreams[j] == other.StreamList[i])
                        {
                            type.UniqueStreams[j] = this.StreamList[i];
                            break;
                        }
                    }
                }
            }
            // update references to the sessions
            for (int i = 0; i < this.ClassList.Count; i++)
            {
                // child references
                for (int j = 0; j < this.ClassList[i].Stream.Classes.Count; j++)
                {
                    if (this.ClassList[i].Stream.Classes[j] == other.ClassList[i])
                    {
                        this.ClassList[i].Stream.Classes[j] = this.ClassList[i];
                        break;
                    }
                }
            }

            #endregion

            // a shallow copy will do for boolean (value type)
            // TODO: copies both levels or just first level?
            this.StreamClashTable_ = (bool[][])other.StreamClashTable_.Clone();

            // clone unavailable data
            this.UnavailableList = new List<Unavailability>();
            foreach (Unavailability unavail in other.UnavailableList)
            {
                this.UnavailableList.Add(unavail.Clone());
            }

            // copy status of solution recomputation required
            this.RecomputeSolutions = other.RecomputeSolutions;
        }

        public void MergeWith(Timetable t)
        {
            SubjectList.AddRange(t.SubjectList);
            TypeList.AddRange(t.TypeList);
            StreamList.AddRange(t.StreamList);
            ClassList.AddRange(t.ClassList);
            UnavailableList.AddRange(t.UnavailableList);

            BuildEquivalency();
            BuildCompatibility();
            Update();
        }

        public Timetable Clone()
        {
            return new Timetable(this);
        }

        public Timetable DeepCopy()
        {
            return new Timetable(this);
        }

        #endregion

        public bool StreamClashTable(Stream stream1, Stream stream2)
        {
            return StreamClashTable_[stream1.ID][stream2.ID];
        }

        public bool TypeClashTable(Type type1, Type type2)
        {
            return TypeClashTable_[type1.ID][type2.ID];
        }

        /// <summary>
        /// Checks if a full set of timetable data is loaded.
        /// </summary>
        public bool HasData()
        {
            return (SubjectList.Count > 0 && TypeList.Count > 0 && StreamList.Count > 0 && ClassList.Count > 0);
        }

        /// <summary>
        /// Checks if an option has been selected for each required set of streams.
        /// </summary>
        public bool IsFull()
        {
            foreach (Type type in TypeList)
            {
                // if type is required and has not been chosen
                if (type.Required && type.SelectedStream == null)
                    return false;
            }
            return true;
        }

        public TimeOfDay EarlyBound()
        {
            TimeOfDay min = null;
            foreach (Session session in ClassList)
            {
                if (TimeOfDay.ReferenceEquals(min, null) || session.StartTime < min)
                {
                    min = session.StartTime;
                }
            }
            return min;
        }

        public TimeOfDay LateBound()
        {
            TimeOfDay max = null;
            foreach (Session session in ClassList)
            {
                if (TimeOfDay.ReferenceEquals(max, null) || session.EndTime > max)
                {
                    max = session.EndTime;
                }
            }
            return max;
        }

        #region Modifying timetable

        public void Update()
        {
            BuildCompatibility();

            bool modified = true;
            while (modified)
            {
                modified = false;
                foreach (Type type in TypeList)
                {
                    if (!type.Required)
                        continue;
                    if (type.SelectedStream != null)
                        continue;

                    int n = 0;
                    Stream driven = null;
                    foreach (Stream stream in type.Streams)
                    {
                        if (Fits(stream))
                        {
                            driven = stream;
                            n++;
                        }
                    }
                    if (n == 1)
                    {
                        modified = true;
                        this.SelectStream(driven);
                        BuildCompatibility();
                    }
                }

                /*if (modified)
                    continue;
                foreach (Type type in TypeList)
                {
                    if (!type.Required)
                        continue;
                    if (type.ChosenStream != null)
                        continue;
                    foreach (Stream stream in type.Streams)
                    {
                        if (this.StreamClash(stream, true))
                            continue;
                        modified = true;
                        this.SelectStream(stream);
                        BuildCompatibility();
                        break;
                    }
                    if (modified)
                        break;
                }*/
            }
        }

        /// <summary>
        /// Adds a stream to the active timetable.
        /// </summary>
        /// <returns>True if the operation succeeded.</returns>
        public bool SelectStream(Stream stream)
        {
            if (stream.Selected)
                return true;

            if (!Fits(stream))
            {
                MessageBox.Show("The stream you are attempting to select does not fit in the timetable.", "Add Stream to Timetable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            // if an alternative stream is selected, remove it
            Stream current = stream.Type.SelectedStream;
            if (current != null)
                current.Selected = false;
            // add the new stream
            stream.Selected = true;
            RecomputeSolutions = true;
            
            return true;
        }

        public List<Stream> FindStreamClashes(Stream stream)
        {
            List<Stream> clashes = new List<Stream>();

            Stream current = stream.Type.SelectedStream;
            if (current != null)
                current.Selected = false;

            foreach (Stream other in StreamList)
            {
                if (!other.Selected)
                    continue;
                if (stream.ClashesWith(other))
                    clashes.Add(other);
            }

            if (current != null)
                current.Selected = true;
            return clashes;
        }

        public bool SwapStreams(Stream stream1, Stream stream2)
        {
            if (stream1.Type.SelectedStream == null || stream2.Type.SelectedStream == null || stream1.Type == stream2.Type)
                return false;

            stream1.Selected = false;
            stream2.Selected = false;

            List<Stream> alt1 = new List<Stream>();
            foreach (Stream alt in stream1.Type.UniqueStreams)
            {
                if (alt.ClashesWith(stream2))
                    alt1.Add(alt);
            }
            
            List<Stream> alt2 = new List<Stream>();
            foreach (Stream alt in stream2.Type.UniqueStreams)
            {
                if (alt.ClashesWith(stream1))
                    alt2.Add(alt);
            }

            foreach (Stream select1 in alt1)
            {
                select1.Selected = true;
                foreach (Stream select2 in alt2)
                {
                    // TODO: use !StreamClash() or Fits() here?
                    if (!StreamClash(select2, true))
                    {
                        select2.Selected = true;
                        return true;
                    }
                }
                select1.Selected = false;
            }

            stream1.Selected = true;
            stream2.Selected = true;
            return false;
        }

        public bool LoadSolution(Solver.Solution solution)
        {
            bool result = true;
            foreach (Stream stream in solution.Streams)
            {
                // attempt to add it, return false if failed
                if (!SelectStream(stream))
                    result = false;
            }
            Update();
            return result;
        }

        /*public void Clear()
        {
            SubjectList.Clear();
            TypeList.Clear();
            StreamList.Clear();
            ClassList.Clear();

            UnavailableList.Clear();
            StreamClashTable_ = null;
            RecomputeSolutions = true;
        }*/

        public bool RevertToBaseStreams()
        {
            bool[] prev = new bool[StreamList.Count];
            for (int i = 0; i < StreamList.Count; i++)
            {
                prev[i] = StreamList[i].Selected;
                StreamList[i].Selected = false;
            }
            Update();
            for (int i = 0; i < StreamList.Count; i++)
            {
                if (prev[i] != StreamList[i].Selected)
                    return true;
            }
            return false;
        }

        #endregion

        #region Check against timetable

        private int NumberStreamsThatFit(List<Stream> streams)
        {
            int n = 0;
            foreach (Stream stream in streams)
            {
                if (Fits(stream))
                    n++;
            }
            return n;
        }

        /// <summary>
        /// Check if the timetable is free at a given time.
        /// </summary>
        /// <returns>True if it's free.</returns>
        public bool FreeAt(TimeOfWeek time, bool selected)
        {
            return (!ClassAt(time, selected) && !UnavailableAt(time));
        }

        /// <summary>
        /// Check if a time is within an unavailable slot.
        /// </summary>
        public bool UnavailableAt(TimeOfWeek time)
        {
            return (FindUnavailableAt(time) != null);
        }

        /// <summary>
        /// Find an unavailable slot at a given time.
        /// </summary>
        /// <returns>The first unavailable slot found, or null if none were found.</returns>
        public Unavailability FindUnavailableAt(TimeOfWeek time)
        {
            foreach (Unavailability u in UnavailableList)
            {
                if (time >= u.Start && time <= u.End)
                    return u;
            }
            return null;
        }

        /// <summary>
        /// Find all unavailable slots at a given time.
        /// </summary>
        /// <returns>The list of unavailable slots found.</returns>
        public List<Unavailability> FindAllUnavailableAt(TimeOfWeek time)
        {
            List<Unavailability> us = new List<Unavailability>();
            foreach (Unavailability u in UnavailableList)
            {
                if (time >= u.Start && time <= u.End)
                    us.Add(u);
            }
            return us;
        }

        /// <summary>
        /// Check if there's a class on at a given time.
        /// </summary>
        public bool ClassAt(TimeOfWeek time, bool selected)
        {
            return FindClassAt(time, selected) != null;
        }

        /// <summary>
        /// Find a class at a given time.
        /// </summary>
        /// <returns>The first class found, or null if none were found.</returns>
        public Session FindClassAt(TimeOfWeek time, bool selected)
        {
            foreach (Session session in ClassList)
            {
                if (selected && !session.Stream.Selected)
                    continue;
                if (time >= session.Start && time <= session.End)
                    return session;
            }
            return null;
        }

        /// <summary>
        /// Find all classes at a given time.
        /// </summary>
        public List<Session> FindAllClassesAt(TimeOfWeek time, bool selected)
        {
            List<Session> sessions = new List<Session>();
            // examine every class
            foreach (Session session in ClassList)
            {
                if (selected && !session.Stream.Selected)
                    continue;
                if (time >= session.Start && time <= session.End)
                    sessions.Add(session);
            }
            return sessions;
        }

        /// <summary>
        /// Check if a given timeslot is free.
        /// </summary>
        /// <returns>True if there is nothing on during the timeslot.</returns>
        public bool FreeDuring(Timeslot timeslot, bool selected)
        {
            return !ClassDuring(timeslot, selected)
                && !UnavailableDuring(timeslot);
        }

        /// <summary>
        /// Check if there is an unavailable timeslot within the given timeslot.
        /// </summary>
        public bool UnavailableDuring(Timeslot timeslot)
        {
            return FindUnavailableDuring(timeslot) != null;
        }

        /// <summary>
        /// Find an unavailable timeslot within the given range.
        /// </summary>
        /// <returns>The first unavailable timeslot found within the range, or null if none were found.</returns>
        public Unavailability FindUnavailableDuring(Timeslot timeslot)
        {
            foreach (Unavailability u in UnavailableList)
            {
                if (u.ClashesWith(timeslot))
                    return u;
            }
            return null;
        }

        /// <summary>
        /// Finds an unavailable timeslot that clashes with a given stream.
        /// </summary>
        /// <returns>The first unavailable timeslot within the stream, or null if none were found.</returns>
        public Unavailability FindUnavailableDuringStream(Stream stream)
        {
            foreach (Session session in stream.Classes)
            {
                Unavailability u = FindUnavailableDuring(session);
                if (u != null)
                    return u;
            }
            return null;
        }

        /// <summary>
        /// Check is there is an unavailable timeslot that clashes with a given stream.
        /// </summary>
        public bool UnavailableDuringStream(Stream stream)
        {
            return (FindUnavailableDuringStream(stream) != null);
        }

        /// <summary>
        /// Check if there is a class within a given time range.
        /// </summary>
        public bool ClassDuring(Timeslot timeslot, bool selected)
        {
            return FindClassDuring(timeslot, selected) != null;
        }

        /// <summary>
        /// Finds a class within a given time range.
        /// </summary>
        /// <returns>The first class found, or null if none were found.</returns>
        public Session FindClassDuring(Timeslot timeslot, bool selected)
        {
            foreach (Session session in ClassList)
            {
                if (selected && !session.Stream.Selected)
                    continue;
                if (session.ClashesWith(timeslot))
                    return session;
            }
            return null;
        }

        /// <summary>
        /// Check if a class clashes with others.
        /// </summary>
        /// <returns>True if there is a clash.</returns>
        public bool ClassClash(Session session, bool classesEnabled)
        {
            return FindClassClash(session, classesEnabled) != null;
        }

        /// <summary>
        /// Finds a class which clashes with a given class.
        /// </summary>
        /// <returns>The first clashing class, or null if none were found.</returns>
        public Session FindClassClash(Session session, bool selected)
        {
            return FindClassDuring(session, selected);
        }

        /// <summary>
        /// Tests whether a stream will fit with the current timetable if selected.
        /// </summary>
        public bool Fits(Stream stream)
        {
            // disable current stream option
            Stream current = stream.Type.SelectedStream;
            if (current != null)
                current.Selected = false;

            bool fits = true;
            foreach (Session session in stream.Classes)
            {
                if (!FreeDuring(session, true))
                    fits = false;
            }

            if (current != null)
                current.Selected = true;
            return fits;
        }

        /// <summary>
        /// Check if a stream clashes with others.
        /// </summary>
        /// <returns>True if there is a clash.</returns>
        public bool StreamClash(Stream stream, bool enabled)
        {
            return FindStreamClash(stream, enabled) != null;
        }

        /// <summary>
        /// Find a stream which clashes with a given stream.
        /// </summary>
        /// <returns>The first clashing stream found, or null if none were found.</returns>
        public Stream FindStreamClash(Stream stream, bool enabled)
        {
            // check each class in the given stream
            foreach (Session session in stream.Classes)
            {
                Session other = FindClassClash(session, enabled);
                if (other != null)
                    return other.Stream;
            }
            return null;
        }

        /// <summary>
        /// Find the streams which clash with a given stream.
        /// </summary>
        /// <returns>A list of clashing streams.</returns>
        public List<Stream> FindStreamClashes(Stream stream, bool selected)
        {
            List<Stream> clashes = new List<Stream>();
            foreach (Stream other in StreamList)
            {
                if (selected && other.Selected)
                    continue;
                if (other.ClashesWith(stream))
                    clashes.Add(other);
            }
            return clashes;
        }

        #endregion

        #region Building relational lists/matrices

        /// <summary>
        /// Build lists of equivalent streams.
        /// </summary>
        public void BuildEquivalency()
        {
            // for each set of streams
            foreach (Type type in TypeList)
            {
                type.BuildEquivalency();
            }
        }

        /// <summary>
        /// Build stream (in)compatibility lists.
        /// </summary>
        public void BuildCompatibility()
        {
            StreamClashTable_ = new bool[StreamList.Count][];
            for (int i = 0; i < StreamList.Count; i++)
            {
                StreamList[i].ID = i;
                StreamClashTable_[i] = new bool[StreamList.Count];
                StreamList[i].ClashTable = null;
            }
            // compare every stream
            for (int i = 0; i < StreamList.Count; i++)
            {
                // against all streams after it
                for (int j = i + 1; j < StreamList.Count; j++)
                {
                    // matching types - no clash
                    if (StreamList[i].Type == StreamList[j].Type)
                        continue;
                    // if there is a clash
                    if (StreamList[i].ClashesWith(StreamList[j]))
                    {
                        // set the table value to true
                        StreamClashTable_[i][j] = StreamClashTable_[j][i] = true;
                        // add to lists
                        StreamList[i].Incompatible.Add(StreamList[j]);
                        StreamList[j].Incompatible.Add(StreamList[i]);
                    }
                    else
                    {
                        // set the table value to false
                        StreamClashTable_[i][j] = StreamClashTable_[j][i] = false;
                    }
                }
            }
            for (int i = 0; i < StreamList.Count; i++)
            {
                StreamList[i].ClashTable = StreamClashTable_[i];
            }

            TypeClashTable_ = new bool[TypeList.Count][];
            for (int i = 0; i < TypeList.Count; i++)
            {
                TypeList[i].ID = i;
                TypeClashTable_[i] = new bool[TypeList.Count];
                TypeList[i].ClashTable = null;
            }
            // compare each type
            for (int i = 0; i < TypeList.Count; i++)
            {
                for (int j = i + 1; j < TypeList.Count; j++)
                {
                    TypeClashTable_[i][j] = TypeClashTable_[j][i] = TypeList[i].ClashesWith(TypeList[j]);
                }
            }
            for (int i = 0; i < TypeList.Count; i++)
            {
                TypeList[i].ClashTable = TypeClashTable_[i];
            }
        }

        public bool CheckDirectClash(Type a)
        {
            if (!a.Required)
                return false;
            foreach (Type b in TypeList)
            {
                if (a == b)
                    continue;
                if (!b.Required)
                    continue;
                if (a.ClashesWith(b))
                    return true;
            }
            return false;
        }

        #endregion

        #region Tree-style output

        /// <summary>
        /// Display the tree generated by importing a file.
        /// </summary>
        /// <returns>A string representing the stream hierarchy.</returns>
        public string BuildTextTreeView()
        {
            string tree = "";
            foreach (Subject subject in SubjectList)
            {
                tree += subject.Name + "\n";
                foreach (Type type in subject.Types)
                {
                    tree += "  " + type.Name + " (" + type.Streams.Count.ToString() + ")\n";
                }
            }
            return tree;
        }

        /// <summary>
        /// Displays the subject/type/stream hierarchy in the timetable.
        /// </summary>
        /// <param name="treeView"></param>
        public void BuildTreeView(TreeView treeView)
        {
            treeView.Nodes.Clear();
            // do all subjects
            foreach (Subject subject in SubjectList)
            {
                TreeNode subjectNode = new TreeNode(subject.Name);
                subjectNode.Tag = subject;
                treeView.Nodes.Add(subjectNode);
                // do types in each subject
                foreach (Type type in subject.Types)
                {
                    TreeNode typeNode = new TreeNode(type.Name);
                    typeNode.Tag = type;
                    subjectNode.Nodes.Add(typeNode);
                    // do streams in each type
                    foreach (Stream stream in type.Streams)
                    {
                        TreeNode streamNode = new TreeNode(stream.ToString());
                        streamNode.Tag = stream;
                        typeNode.Nodes.Add(streamNode);
                    }
                }
                subjectNode.Expand();
            }
        }

        #endregion
    }
}
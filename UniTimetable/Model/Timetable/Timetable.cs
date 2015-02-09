#region

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using UniTimetable.Model.Time;

#endregion

namespace UniTimetable.Model.Timetable
{
    [XmlRoot("timetable")]
    public class Timetable
    {
        public static List<string> Days =
            new List<string>(new[] {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"});

        private bool[][] _streamClashTable;
        private bool[][] _typeClashTable;
        [XmlIgnore] public List<Session> ClassList = new List<Session>();
        [XmlIgnore] public bool RecomputeSolutions = true;
        [XmlIgnore] public List<Stream> StreamList = new List<Stream>();

        [XmlArray("subjects"), XmlArrayItem("subject", typeof (Subject))] public List<Subject> SubjectList =
            new List<Subject>();

        [XmlIgnore] public List<Type> TypeList = new List<Type>();

        [XmlArray("unavailabilities"), XmlArrayItem("unavailability")] public List<Unavailability> UnavailableList =
            new List<Unavailability>();

        public bool StreamClashTable(Stream stream1, Stream stream2)
        {
            return _streamClashTable[stream1.ID][stream2.ID];
        }

        public bool TypeClashTable(Type type1, Type type2)
        {
            return _typeClashTable[type1.ID][type2.ID];
        }

        /// <summary>
        ///     Checks if a full set of timetable data is loaded.
        /// </summary>
        public bool HasData()
        {
            return (SubjectList.Count > 0 && TypeList.Count > 0 && StreamList.Count > 0 && ClassList.Count > 0);
        }

        /// <summary>
        ///     Checks if an option has been selected for each required set of streams.
        /// </summary>
        public bool IsFull()
        {
            return TypeList.All(type => !type.Required || type.SelectedStream != null);
        }

        public TimeOfDay EarlyBound()
        {
            TimeOfDay min = null;
            foreach (var session in ClassList)
            {
                if (ReferenceEquals(min, null) || session.StartTime < min)
                {
                    min = session.StartTime;
                }
            }
            return min;
        }

        public TimeOfDay LateBound()
        {
            TimeOfDay max = null;
            foreach (var session in ClassList)
            {
                if (ReferenceEquals(max, null) || session.EndTime > max)
                {
                    max = session.EndTime;
                }
            }
            return max;
        }

        #region Constructors

        public Timetable()
        {
        }

        public Timetable(Timetable other)
        {
            #region Clone subject/type/stream/class data

            // copy across all the subject data
            SubjectList = new List<Subject>();
            foreach (var subject in other.SubjectList)
                SubjectList.Add(subject.Clone());
            // copy all the type data
            TypeList = new List<Type>();
            foreach (var type in other.TypeList)
                TypeList.Add(type.Clone());
            // copy all the stream data
            StreamList = new List<Stream>();
            foreach (var stream in other.StreamList)
                StreamList.Add(stream.Clone());
            // copy all the session data
            ClassList = new List<Session>();
            foreach (var session in other.ClassList)
                ClassList.Add(session.Clone());

            // update reference to subjects
            for (var i = 0; i < SubjectList.Count; i++)
            {
                foreach (var type in TypeList)
                {
                    if (type.Subject == other.SubjectList[i])
                        type.Subject = SubjectList[i];
                }
            }
            // update references to types
            for (var i = 0; i < TypeList.Count; i++)
            {
                // parent references
                foreach (var stream in StreamList)
                {
                    if (stream.Type == other.TypeList[i])
                        stream.Type = TypeList[i];
                }
                // child references
                for (var j = 0; j < TypeList[i].Subject.Types.Count; j++)
                {
                    if (TypeList[i].Subject.Types[j] == other.TypeList[i])
                    {
                        TypeList[i].Subject.Types[j] = TypeList[i];
                        break;
                    }
                }
                // unique streams
                for (var j = 0; j < TypeList[i].UniqueStreams.Count; j++)
                {
                    if (StreamList[i].Type.Streams[j] == other.StreamList[i])
                    {
                        StreamList[i].Type.Streams[j] = StreamList[i];
                        break;
                    }
                }
            }
            // update references to streams
            for (var i = 0; i < StreamList.Count; i++)
            {
                // parent references
                foreach (var session in ClassList)
                {
                    if (session.Stream == other.StreamList[i])
                        session.Stream = StreamList[i];
                }
                // child references
                for (var j = 0; j < StreamList[i].Type.Streams.Count; j++)
                {
                    if (StreamList[i].Type.Streams[j] == other.StreamList[i])
                    {
                        StreamList[i].Type.Streams[j] = StreamList[i];
                        break;
                    }
                }
                // find all equivalent streams set to other.StreamList[i]
                foreach (Stream t in StreamList)
                {
                    for (var k = 0; k < t.Equivalent.Count; k++)
                    {
                        if (t.Equivalent[k] == other.StreamList[i])
                        {
                            t.Equivalent[k] = StreamList[i];
                            break;
                        }
                    }
                }
                // find all incompatible streams set to other.StreamList[i]
                foreach (Stream t in StreamList)
                {
                    for (var k = 0; k < t.Incompatible.Count; k++)
                    {
                        if (t.Incompatible[k] == other.StreamList[i])
                        {
                            t.Incompatible[k] = StreamList[i];
                            break;
                        }
                    }
                }
                // find all unique streams set to other.StreamList[i]
                foreach (var type in TypeList)
                {
                    for (var j = 0; j < type.UniqueStreams.Count; j++)
                    {
                        if (type.UniqueStreams[j] == other.StreamList[i])
                        {
                            type.UniqueStreams[j] = StreamList[i];
                            break;
                        }
                    }
                }
            }
            // update references to the sessions
            for (var i = 0; i < ClassList.Count; i++)
            {
                // child references
                for (var j = 0; j < ClassList[i].Stream.Classes.Count; j++)
                {
                    if (ClassList[i].Stream.Classes[j] == other.ClassList[i])
                    {
                        ClassList[i].Stream.Classes[j] = ClassList[i];
                        break;
                    }
                }
            }

            #endregion

            // a shallow copy will do for boolean (value type)
            // TODO: copies both levels or just first level?
            _streamClashTable = (bool[][]) other._streamClashTable.Clone();

            // clone unavailable data
            UnavailableList = new List<Unavailability>();
            foreach (var unavail in other.UnavailableList)
            {
                UnavailableList.Add(unavail.Clone());
            }

            // copy status of solution recomputation required
            RecomputeSolutions = other.RecomputeSolutions;
        }

        public static Timetable From(Subject subject)
        {
            var t = new Timetable();
            t.SubjectList.Add(subject);
            t.TypeList.AddRange(subject.Types);
            foreach (var type in subject.Types)
            {
                t.StreamList.AddRange(type.Streams);
                foreach (var stream in type.Streams)
                {
                    t.ClassList.AddRange(stream.Classes);
                }
            }
            return t;
        }

        public static Timetable From(Type type)
        {
            var t = new Timetable();
            t.SubjectList.Add(type.Subject);
            t.TypeList.Add(type);
            t.StreamList.AddRange(type.Streams);
            foreach (var stream in type.Streams)
            {
                t.ClassList.AddRange(stream.Classes);
            }
            return t;
        }

        public static Timetable From(Stream stream)
        {
            var t = new Timetable();
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
        public Timetable PreviewSolution(Solver.Solver.Solution solution)
        {
            var t = new Timetable();
            for (var i = 0; i < StreamList.Count; i++)
            {
                var stream = new Stream(StreamList[i]);
                if (solution.Streams.Contains(StreamList[i]))
                    stream.Selected = true;
                t.StreamList.Add(stream);

                stream.Classes.Clear();
                stream.Incompatible.Clear();
                stream.Equivalent.Clear();

                for (var j = 0; j < StreamList[i].Classes.Count; j++)
                {
                    var session = new Session(StreamList[i].Classes[j]) {Stream = stream};
                    stream.Classes.Add(session);
                    t.ClassList.Add(session);
                }
            }
            t.UnavailableList = UnavailableList;

            return t;
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

        #region Modifying timetable

        public void Update()
        {
            BuildCompatibility();

            var modified = true;
            while (modified)
            {
                modified = false;
                foreach (var type in TypeList)
                {
                    if (!type.Required)
                        continue;
                    if (type.SelectedStream != null)
                        continue;

                    var n = 0;
                    Stream driven = null;
                    foreach (var stream in type.Streams)
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
                        SelectStream(driven);
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
        ///     Adds a stream to the active timetable.
        /// </summary>
        /// <returns>True if the operation succeeded.</returns>
        public bool SelectStream(Stream stream)
        {
            if (stream.Selected)
                return true;

            if (!Fits(stream))
            {
                MessageBox.Show("The stream you are attempting to select does not fit in the timetable.",
                    "Add Stream to Timetable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            // if an alternative stream is selected, remove it
            var current = stream.Type.SelectedStream;
            if (current != null)
                current.Selected = false;
            // add the new stream
            stream.Selected = true;
            RecomputeSolutions = true;

            return true;
        }

        public List<Stream> FindStreamClashes(Stream stream)
        {
            var current = stream.Type.SelectedStream;
            if (current != null)
                current.Selected = false;

            var clashes = StreamList.Where(other => other.Selected).Where(stream.ClashesWith).ToList();

            if (current != null)
                current.Selected = true;
            return clashes;
        }

        public bool SwapStreams(Stream stream1, Stream stream2)
        {
            if (stream1.Type.SelectedStream == null || stream2.Type.SelectedStream == null ||
                stream1.Type == stream2.Type)
                return false;

            stream1.Selected = false;
            stream2.Selected = false;

            var alt1 = stream1.Type.UniqueStreams.Where(alt => alt.ClashesWith(stream2)).ToList();

            var alt2 = stream2.Type.UniqueStreams.Where(alt => alt.ClashesWith(stream1)).ToList();

            foreach (var select1 in alt1)
            {
                select1.Selected = true;
                foreach (var select2 in alt2)
                {
                    // TODO: use !StreamClash() or Fits() here?
                    if (StreamClash(select2, true)) continue;
                    select2.Selected = true;
                    return true;
                }
                select1.Selected = false;
            }

            stream1.Selected = true;
            stream2.Selected = true;
            return false;
        }

        public bool LoadSolution(Solver.Solver.Solution solution)
        {
            var result = true;
            foreach (var stream in solution.Streams)
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
            var prev = new bool[StreamList.Count];
            for (var i = 0; i < StreamList.Count; i++)
            {
                prev[i] = StreamList[i].Selected;
                StreamList[i].Selected = false;
            }
            Update();
            return StreamList.Where((t, i) => prev[i] != t.Selected).Any();
        }

        #endregion

        #region Check against timetable

        private int NumberStreamsThatFit(List<Stream> streams)
        {
            return streams.Count(Fits);
        }

        /// <summary>
        ///     Check if the timetable is free at a given time.
        /// </summary>
        /// <returns>True if it's free.</returns>
        public bool FreeAt(TimeOfWeek time, bool selected)
        {
            return (!ClassAt(time, selected) && !UnavailableAt(time));
        }

        /// <summary>
        ///     Check if a time is within an unavailable slot.
        /// </summary>
        public bool UnavailableAt(TimeOfWeek time)
        {
            return (FindUnavailableAt(time) != null);
        }

        /// <summary>
        ///     Find an unavailable slot at a given time.
        /// </summary>
        /// <returns>The first unavailable slot found, or null if none were found.</returns>
        public Unavailability FindUnavailableAt(TimeOfWeek time)
        {
            return UnavailableList.FirstOrDefault(u => time >= u.Start && time <= u.End);
        }

        /// <summary>
        ///     Find all unavailable slots at a given time.
        /// </summary>
        /// <returns>The list of unavailable slots found.</returns>
        public List<Unavailability> FindAllUnavailableAt(TimeOfWeek time)
        {
            return UnavailableList.Where(u => time >= u.Start && time <= u.End).ToList();
        }

        /// <summary>
        ///     Check if there's a class on at a given time.
        /// </summary>
        public bool ClassAt(TimeOfWeek time, bool selected)
        {
            return FindClassAt(time, selected) != null;
        }

        /// <summary>
        ///     Find a class at a given time.
        /// </summary>
        /// <returns>The first class found, or null if none were found.</returns>
        public Session FindClassAt(TimeOfWeek time, bool selected)
        {
            return
                ClassList.Where(session => !selected || session.Stream.Selected)
                    .FirstOrDefault(session => time >= session.Start && time <= session.End);
        }

        /// <summary>
        ///     Find all classes at a given time.
        /// </summary>
        public List<Session> FindAllClassesAt(TimeOfWeek time, bool selected)
        {
            // examine every class
            return
                ClassList.Where(session => !selected || session.Stream.Selected)
                    .Where(session => time >= session.Start && time <= session.End)
                    .ToList();
        }

        /// <summary>
        ///     Check if a given timeslot is free.
        /// </summary>
        /// <returns>True if there is nothing on during the timeslot.</returns>
        public bool FreeDuring(Timeslot timeslot, bool selected)
        {
            return !ClassDuring(timeslot, selected)
                   && !UnavailableDuring(timeslot);
        }

        /// <summary>
        ///     Check if there is an unavailable timeslot within the given timeslot.
        /// </summary>
        public bool UnavailableDuring(Timeslot timeslot)
        {
            return FindUnavailableDuring(timeslot) != null;
        }

        /// <summary>
        ///     Find an unavailable timeslot within the given range.
        /// </summary>
        /// <returns>The first unavailable timeslot found within the range, or null if none were found.</returns>
        public Unavailability FindUnavailableDuring(Timeslot timeslot)
        {
            return UnavailableList.FirstOrDefault(u => u.ClashesWith(timeslot));
        }

        /// <summary>
        ///     Finds an unavailable timeslot that clashes with a given stream.
        /// </summary>
        /// <returns>The first unavailable timeslot within the stream, or null if none were found.</returns>
        public Unavailability FindUnavailableDuringStream(Stream stream)
        {
            return stream.Classes.Select(FindUnavailableDuring).FirstOrDefault(u => u != null);
        }

        /// <summary>
        ///     Check is there is an unavailable timeslot that clashes with a given stream.
        /// </summary>
        public bool UnavailableDuringStream(Stream stream)
        {
            return (FindUnavailableDuringStream(stream) != null);
        }

        /// <summary>
        ///     Check if there is a class within a given time range.
        /// </summary>
        public bool ClassDuring(Timeslot timeslot, bool selected)
        {
            return FindClassDuring(timeslot, selected) != null;
        }

        /// <summary>
        ///     Finds a class within a given time range.
        /// </summary>
        /// <returns>The first class found, or null if none were found.</returns>
        public Session FindClassDuring(Timeslot timeslot, bool selected)
        {
            return
                ClassList.Where(session => !selected || session.Stream.Selected)
                    .FirstOrDefault(session => session.ClashesWith(timeslot));
        }

        /// <summary>
        ///     Check if a class clashes with others.
        /// </summary>
        /// <returns>True if there is a clash.</returns>
        public bool ClassClash(Session session, bool classesEnabled)
        {
            return FindClassClash(session, classesEnabled) != null;
        }

        /// <summary>
        ///     Finds a class which clashes with a given class.
        /// </summary>
        /// <returns>The first clashing class, or null if none were found.</returns>
        public Session FindClassClash(Session session, bool selected)
        {
            return FindClassDuring(session, selected);
        }

        /// <summary>
        ///     Tests whether a stream will fit with the current timetable if selected.
        /// </summary>
        public bool Fits(Stream stream)
        {
            // disable current stream option
            var current = stream.Type.SelectedStream;
            if (current != null)
                current.Selected = false;

            var fits = true;
            foreach (var session in stream.Classes)
            {
                if (!FreeDuring(session, true))
                    fits = false;
            }

            if (current != null)
                current.Selected = true;
            return fits;
        }

        /// <summary>
        ///     Check if a stream clashes with others.
        /// </summary>
        /// <returns>True if there is a clash.</returns>
        public bool StreamClash(Stream stream, bool enabled)
        {
            return FindStreamClash(stream, enabled) != null;
        }

        /// <summary>
        ///     Find a stream which clashes with a given stream.
        /// </summary>
        /// <returns>The first clashing stream found, or null if none were found.</returns>
        public Stream FindStreamClash(Stream stream, bool enabled)
        {
            // check each class in the given stream
            return (from session in stream.Classes
                select FindClassClash(session, enabled)
                into other
                where other != null
                select other.Stream).FirstOrDefault();
        }

        /// <summary>
        ///     Find the streams which clash with a given stream.
        /// </summary>
        /// <returns>A list of clashing streams.</returns>
        public List<Stream> FindStreamClashes(Stream stream, bool selected)
        {
            return
                StreamList.Where(other => !selected || !other.Selected)
                    .Where(other => other.ClashesWith(stream))
                    .ToList();
        }

        #endregion

        #region Building relational lists/matrices

        /// <summary>
        ///     Build lists of equivalent streams.
        /// </summary>
        public void BuildEquivalency()
        {
            // for each set of streams
            foreach (var type in TypeList)
            {
                type.BuildEquivalency();
            }
        }

        /// <summary>
        ///     Build stream (in)compatibility lists.
        /// </summary>
        public void BuildCompatibility()
        {
            _streamClashTable = new bool[StreamList.Count][];
            for (var i = 0; i < StreamList.Count; i++)
            {
                StreamList[i].ID = i;
                _streamClashTable[i] = new bool[StreamList.Count];
                StreamList[i].ClashTable = null;
            }
            // compare every stream
            for (var i = 0; i < StreamList.Count; i++)
            {
                // against all streams after it
                for (var j = i + 1; j < StreamList.Count; j++)
                {
                    // matching types - no clash
                    if (StreamList[i].Type == StreamList[j].Type)
                        continue;
                    // if there is a clash
                    if (StreamList[i].ClashesWith(StreamList[j]))
                    {
                        // set the table value to true
                        _streamClashTable[i][j] = _streamClashTable[j][i] = true;
                        // add to lists
                        StreamList[i].Incompatible.Add(StreamList[j]);
                        StreamList[j].Incompatible.Add(StreamList[i]);
                    }
                    else
                    {
                        // set the table value to false
                        _streamClashTable[i][j] = _streamClashTable[j][i] = false;
                    }
                }
            }
            for (var i = 0; i < StreamList.Count; i++)
            {
                StreamList[i].ClashTable = _streamClashTable[i];
            }

            _typeClashTable = new bool[TypeList.Count][];
            for (var i = 0; i < TypeList.Count; i++)
            {
                TypeList[i].ID = i;
                _typeClashTable[i] = new bool[TypeList.Count];
                TypeList[i].ClashTable = null;
            }
            // compare each type
            for (var i = 0; i < TypeList.Count; i++)
            {
                for (var j = i + 1; j < TypeList.Count; j++)
                {
                    _typeClashTable[i][j] = _typeClashTable[j][i] = TypeList[i].ClashesWith(TypeList[j]);
                }
            }
            for (var i = 0; i < TypeList.Count; i++)
            {
                TypeList[i].ClashTable = _typeClashTable[i];
            }
        }

        public bool CheckDirectClash(Type a)
        {
            return a.Required && TypeList.Where(b => a != b).Where(b => b.Required).Any(a.ClashesWith);
        }

        #endregion

        #region Tree-style output

        /// <summary>
        ///     Display the tree generated by importing a file.
        /// </summary>
        /// <returns>A string representing the stream hierarchy.</returns>
        public string BuildTextTreeView()
        {
            var tree = "";
            foreach (var subject in SubjectList)
            {
                tree += subject.Name + "\n";
                tree = subject.Types.Aggregate(tree,
                    (current, type) =>
                        current +
                        ("  " + type.Name + " (" + type.Streams.Count.ToString(CultureInfo.InvariantCulture) + ")\n"));
            }
            return tree;
        }

        /// <summary>
        ///     Displays the subject/type/stream hierarchy in the timetable.
        /// </summary>
        /// <param name="treeView"></param>
        public void BuildTreeView(TreeView treeView)
        {
            treeView.Nodes.Clear();
            // do all subjects
            foreach (var subject in SubjectList)
            {
                var subjectNode = new TreeNode(subject.Name) {Tag = subject};
                treeView.Nodes.Add(subjectNode);
                // do types in each subject
                foreach (var type in subject.Types)
                {
                    var typeNode = new TreeNode(type.Name) {Tag = type};
                    subjectNode.Nodes.Add(typeNode);
                    // do streams in each type
                    foreach (
                        var streamNode in type.Streams.Select(stream => new TreeNode(stream.ToString()) {Tag = stream}))
                    {
                        typeNode.Nodes.Add(streamNode);
                    }
                }
                subjectNode.Expand();
            }
        }

        #endregion
    }
}
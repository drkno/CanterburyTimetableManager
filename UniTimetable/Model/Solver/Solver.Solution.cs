#region

using System.Collections.Generic;
using System.Linq;
using UniTimetable.Model.Time;
using UniTimetable.Model.Timetable;

#endregion

namespace UniTimetable.Model.Solver
{
    public partial class Solver
    {
        public class Solution
        {
            private const int MinBreak = 15;
            // list of streams
            // member variables for solution analysis
            private OrderedList<Session>[] _classesByDay;
            private TimeLength _totalEnd;
            // used in computation
            private TimeLength _totalStart;

            #region Accessors

            public List<Stream> Streams { get; private set; }

            #endregion

            private void Clear()
            {
                // list of streams
                Streams = new List<Stream>();
                // clear all the data for calculating statistics
                ClearComputation();
            }

            private void ClearComputation()
            {
                // classes indexed by day
                _classesByDay = new OrderedList<Session>[7];
                for (int i = 0; i < 7; i++)
                    _classesByDay[i] = new OrderedList<Session>();

                TimeAtUni = new TimeLength(0, 0);
                TimeInClasses = new TimeLength(0, 0);
                TimeInBreaks = new TimeLength(0, 0);
                Days = 0;

                MinDayLength = new TimeLength(24, 0);
                MaxDayLength = new TimeLength(0, 0);
                AverageDayLength = new TimeLength(0, 0);

                ShortBreak = new TimeLength(24, 0);
                LongBreak = new TimeLength(0, 0);
                AverageBreak = new TimeLength(0, 0);
                NumberBreaks = 0;

                ShortBlock = new TimeLength(24, 0);
                LongBlock = new TimeLength(0, 0);
                AverageBlock = new TimeLength(0, 0);
                NumberBlocks = 0;

                EarlyStart = TimeOfDay.Maximum;
                LateStart = TimeOfDay.Minimum;
                AverageStart = TimeOfDay.Minimum;

                EarlyEnd = TimeOfDay.Maximum;
                LateEnd = TimeOfDay.Minimum;
                AverageEnd = TimeOfDay.Minimum;

                _totalStart = new TimeLength(0);
                _totalEnd = new TimeLength(0);
            }

            public int CompareTo(Solution other, SolutionComparer solutionComparer)
            {
                return solutionComparer.Compare(this, other);
            }

            #region Solution criteria accessors

            public TimeLength TimeAtUni { get; private set; }
            public TimeLength TimeInClasses { get; private set; }
            public TimeLength TimeInBreaks { get; private set; }
            public int Days { get; private set; }

            public TimeLength MinDayLength { get; private set; }
            public TimeLength MaxDayLength { get; private set; }
            public TimeLength AverageDayLength { get; private set; }

            public TimeLength ShortBreak { get; private set; }
            public TimeLength LongBreak { get; private set; }
            public TimeLength AverageBreak { get; private set; }
            public int NumberBreaks { get; private set; }

            public TimeLength ShortBlock { get; private set; }
            public TimeLength LongBlock { get; private set; }
            public TimeLength AverageBlock { get; private set; }
            public int NumberBlocks { get; private set; }

            public TimeOfDay EarlyStart { get; private set; }
            public TimeOfDay LateStart { get; private set; }
            public TimeOfDay AverageStart { get; private set; }

            public TimeOfDay EarlyEnd { get; private set; }
            public TimeOfDay LateEnd { get; private set; }
            public TimeOfDay AverageEnd { get; private set; }

            #endregion

            #region Solution criteria

            #endregion

            #region Constructors

            public Solution()
            {
                Clear();
            }

            public Solution(Solution other)
            {
                Streams = new List<Stream>(other.Streams);
                _classesByDay = new OrderedList<Session>[7];
                for (int i = 0; i < 7; i++)
                {
                    _classesByDay[i] = new OrderedList<Session>(other._classesByDay[i]);
                }

                TimeAtUni = other.TimeAtUni;
                TimeInClasses = other.TimeInClasses;
                TimeInBreaks = other.TimeInBreaks;
                Days = other.Days;

                MinDayLength = other.MinDayLength;
                MaxDayLength = other.MaxDayLength;
                AverageDayLength = other.AverageDayLength;

                ShortBreak = other.ShortBreak;
                LongBreak = other.LongBreak;
                AverageBreak = other.AverageBreak;
                NumberBreaks = other.NumberBreaks;

                ShortBlock = other.ShortBlock;
                LongBlock = other.LongBlock;
                AverageBlock = other.AverageBlock;
                NumberBlocks = other.NumberBlocks;

                EarlyStart = other.EarlyStart;
                LateStart = other.LateStart;
                AverageStart = other.AverageStart;

                EarlyEnd = other.EarlyEnd;
                LateEnd = other.EarlyEnd;
                AverageEnd = other.AverageEnd;

                _totalStart = other._totalStart;
                _totalEnd = other._totalEnd;
            }

            #endregion

            #region Evaluation

            private bool Fits(Stream stream)
            {
                // check if stream fits with the current streams
                return Streams.All(other => !stream.ClashesWith(other));
            }

            public bool AddStream(Stream stream)
            {
                // if the stream clashes with another stream
                if (!Fits(stream))
                    return false;

                // add to list of streams
                Streams.Add(stream);

                // run through list of classes
                foreach (Session session in stream.Classes)
                {
                    // increase total time spent in classes
                    TimeInClasses += session.Length;

                    // if we're adding the class to an empty day
                    if (_classesByDay[session.Day].Count == 0)
                    {
                        // increment day count
                        Days++;
                        // add start and ending times to totals
                        _totalStart += new TimeLength(session.StartTime.DayMinutes);
                        _totalEnd += new TimeLength(session.EndTime.DayMinutes);

                        // add length to total time at uni
                        TimeAtUni += session.Length;
                    }
                    else
                    {
                        // if it's earlier than the earliest class of that day
                        if (session.StartTime < _classesByDay[session.Day][0].StartTime)
                        {
                            TimeLength difference = _classesByDay[session.Day][0].StartTime - session.StartTime;
                            // remove the difference for total start
                            _totalStart -= difference;
                            // add the difference for total time at uni
                            TimeAtUni += difference;
                        }
                        // if it's later than the latest class
                        if (session.EndTime > _classesByDay[session.Day][_classesByDay[session.Day].Count - 1].EndTime)
                        {
                            TimeLength difference = session.EndTime -
                                                    _classesByDay[session.Day][_classesByDay[session.Day].Count - 1]
                                                        .EndTime;
                            // add the difference for total end
                            _totalEnd += difference;
                            // add the difference for total time at uni
                            TimeAtUni += difference;
                        }
                    }
                    // update average day length
                    AverageDayLength = TimeAtUni/Days;

                    // add new classes to day-indexed list
                    _classesByDay[session.Day].Add(session);
                    //ClassesByDay_[session.Day].Sort();

                    // check class start/end times against maxima/minima
                    if (session.StartTime < EarlyStart)
                        EarlyStart = session.StartTime;
                    if (session.StartTime > LateStart)
                        LateStart = session.StartTime;
                    if (session.EndTime < EarlyEnd)
                        EarlyEnd = session.EndTime;
                    if (session.EndTime > LateEnd)
                        LateEnd = session.EndTime;

                    // check day length
                    TimeLength dayLength = _classesByDay[session.Day][_classesByDay[session.Day].Count - 1].EndTime -
                                           _classesByDay[session.Day][0].StartTime;
                    if (dayLength < MinDayLength)
                        MinDayLength = dayLength;
                    if (dayLength > MaxDayLength)
                        MaxDayLength = dayLength;
                }

                // clear break data
                NumberBreaks = 0;
                AverageBreak = new TimeLength(0, 0);
                ShortBreak = new TimeLength(24, 0);
                LongBreak = new TimeLength(0, 0);
                TimeInBreaks = new TimeLength(0, 0);
                // clear block data
                NumberBlocks = 0;
                AverageBlock = new TimeLength(0, 0);
                ShortBlock = new TimeLength(24, 0);
                LongBlock = new TimeLength(0, 0);
                // TODO: rewrite to avoid full sweep?
                // do a fresh sweep of all days to rebuild break/block data
                foreach (List<Session> daySessions in _classesByDay)
                {
                    // empty day - skip
                    if (daySessions.Count == 0)
                        continue;

                    int i;
                    CompareAdjacent(daySessions, out i);
                }

                if (NumberBreaks > 0)
                {
                    // divide the sum of breaks to find the mean
                    AverageBreak = TimeInBreaks/NumberBreaks;
                }

                // divide the sum of blocks to find the mean
                AverageBlock /= NumberBlocks;

                AverageStart = new TimeOfDay(_totalStart.TotalMinutes/Days);
                AverageEnd = new TimeOfDay(_totalEnd.TotalMinutes/Days);

                return true;
            }

            private void CompareAdjacent(List<Session> daySessions, out int i)
            {
                // set up data for the start of the block
                var blockStart = daySessions[0].StartTime;
                TimeLength blockLength;

                // compare adjacent classes
                for (i = 1; i < daySessions.Count; i++)
                {
                    var breakLength = daySessions[i].StartTime - daySessions[i - 1].EndTime;

                    // if there is at least ~15 minutes between the classes, call it a break, otherwise skip
                    if (breakLength.TotalMinutes < MinBreak)
                        continue;

                    // find block length
                    blockLength = daySessions[i - 1].EndTime - blockStart;

                    // increment number of blocks
                    NumberBlocks++;
                    // add block length to cumulative sum
                    AverageBlock += blockLength;
                    // set start of next block
                    blockStart = daySessions[i].StartTime;

                    // compare block against maxima/minima
                    if (blockLength < ShortBlock)
                        ShortBlock = blockLength;
                    if (blockLength > LongBlock)
                        LongBlock = blockLength;

                    // increment number of breaks
                    NumberBreaks++;
                    // add break to cumulative sum
                    TimeInBreaks += breakLength;

                    // compare break length against maxima/minima
                    if (breakLength < ShortBreak)
                        ShortBreak = breakLength;
                    // check if it's the longest break so far
                    if (breakLength > LongBreak)
                        LongBreak = breakLength;
                }

                // also create a block at the end
                // find block length
                blockLength = daySessions[i - 1].EndTime - blockStart;
                // compare block against maxima/minima
                if (blockLength < ShortBlock)
                    ShortBlock = blockLength;
                if (blockLength > LongBlock)
                    LongBlock = blockLength;
                // increment number of blocks
                NumberBlocks++;
                // add block length to cumulative sum
                AverageBlock += blockLength;
            }

            #endregion

            #region Field value accessors

            /*public IComparable Field(int field)
        {
            switch (field)
            {
                case 0:
                    return (IComparable)TimeAtUni_;
                case 1:
                    return (IComparable)TimeInClasses_;
                case 2:
                    return (IComparable)TimeInBreaks_;
                case 3:
                    return Days_;
                case 4:
                    return (IComparable)MinDayLength_;
                case 5:
                    return (IComparable)MaxDayLength_;
                case 6:
                    return (IComparable)AverageDayLength_;
                case 7:
                    return (IComparable)ShortBreak_;
                case 8:
                    return (IComparable)LongBreak_;
                case 9:
                    return (IComparable)AverageBreak_;
                case 10:
                    return NumberBreaks_;
                case 11:
                    return (IComparable)ShortBlock_;
                case 12:
                    return (IComparable)LongBlock_;
                case 13:
                    return (IComparable)AverageBlock_;
                case 14:
                    return NumberBlocks_;
                case 15:
                    return (IComparable)EarlyStart_;
                case 16:
                    return (IComparable)LateStart_;
                case 17:
                    return (IComparable)AverageStart_;
                case 18:
                    return (IComparable)EarlyEnd_;
                case 19:
                    return (IComparable)LateEnd_;
                case 20:
                    return (IComparable)AverageEnd_;
                default:
                    throw new Exception("Field index out of range.");
            }
        }*/

            public string FieldValueToString(FieldIndex field)
            {
                switch (field)
                {
                    case FieldIndex.TimeAtUni:
                        return TimeAtUni.ToString();

                    case FieldIndex.TimeInClasses:
                        return TimeInClasses.ToString();

                    case FieldIndex.TimeInBreaks:
                        return TimeInBreaks.ToString();

                    case FieldIndex.Days:
                        return Days.ToString();

                    case FieldIndex.MinDayLength:
                        return MinDayLength.ToString();

                    case FieldIndex.MaxDayLength:
                        return MaxDayLength.ToString();

                    case FieldIndex.AverageDayLength:
                        return AverageDayLength.ToString();

                    case FieldIndex.ShortBreak:
                        return ShortBreak.ToString();

                    case FieldIndex.LongBreak:
                        return LongBreak.ToString();

                    case FieldIndex.AverageBreak:
                        return AverageBreak.ToString();

                    case FieldIndex.NumberBreaks:
                        return NumberBreaks.ToString();

                    case FieldIndex.ShortBlock:
                        return ShortBlock.ToString();

                    case FieldIndex.LongBlock:
                        return LongBlock.ToString();

                    case FieldIndex.AverageBlock:
                        return AverageBlock.ToString();

                    case FieldIndex.NumberBlocks:
                        return NumberBlocks.ToString();

                    case FieldIndex.EarlyStart:
                        return EarlyStart.ToString();

                    case FieldIndex.LateStart:
                        return LateStart.ToString();

                    case FieldIndex.AverageStart:
                        return AverageStart.ToString();

                    case FieldIndex.EarlyEnd:
                        return EarlyEnd.ToString();

                    case FieldIndex.LateEnd:
                        return LateEnd.ToString();

                    case FieldIndex.AverageEnd:
                        return AverageEnd.ToString();

                    default:
                        return "";
                }
            }

            public int FieldValueToInt(FieldIndex field)
            {
                switch (field)
                {
                    case FieldIndex.TimeAtUni:
                        return TimeAtUni.TotalMinutes;

                    case FieldIndex.TimeInClasses:
                        return TimeInClasses.TotalMinutes;

                    case FieldIndex.TimeInBreaks:
                        return TimeInBreaks.TotalMinutes;

                    case FieldIndex.Days:
                        return Days;

                    case FieldIndex.MinDayLength:
                        return MinDayLength.TotalMinutes;

                    case FieldIndex.MaxDayLength:
                        return MaxDayLength.TotalMinutes;

                    case FieldIndex.AverageDayLength:
                        return AverageDayLength.TotalMinutes;

                    case FieldIndex.ShortBreak:
                        return ShortBreak.TotalMinutes;

                    case FieldIndex.LongBreak:
                        return LongBreak.TotalMinutes;

                    case FieldIndex.AverageBreak:
                        return AverageBreak.TotalMinutes;

                    case FieldIndex.NumberBreaks:
                        return NumberBreaks;

                    case FieldIndex.ShortBlock:
                        return ShortBlock.TotalMinutes;

                    case FieldIndex.LongBlock:
                        return LongBlock.TotalMinutes;

                    case FieldIndex.AverageBlock:
                        return AverageBlock.TotalMinutes;

                    case FieldIndex.NumberBlocks:
                        return NumberBlocks;

                    case FieldIndex.EarlyStart:
                        return EarlyStart.DayMinutes;

                    case FieldIndex.LateStart:
                        return LateStart.DayMinutes;

                    case FieldIndex.AverageStart:
                        return AverageStart.DayMinutes;

                    case FieldIndex.EarlyEnd:
                        return EarlyEnd.DayMinutes;

                    case FieldIndex.LateEnd:
                        return LateEnd.DayMinutes;

                    case FieldIndex.AverageEnd:
                        return AverageEnd.DayMinutes;

                    default:
                        return 0;
                }
            }

            public int CompareFieldValueTo(FieldIndex field, Solution other)
            {
                switch (field)
                {
                    case FieldIndex.TimeAtUni:
                        return TimeAtUni.CompareTo(other.TimeAtUni);

                    case FieldIndex.TimeInClasses:
                        return TimeInClasses.CompareTo(other.TimeInClasses);

                    case FieldIndex.TimeInBreaks:
                        return TimeInBreaks.CompareTo(other.TimeInBreaks);

                    case FieldIndex.Days:
                        return Days.CompareTo(other.Days);

                    case FieldIndex.MinDayLength:
                        return MinDayLength.CompareTo(other.MinDayLength);

                    case FieldIndex.MaxDayLength:
                        return MaxDayLength.CompareTo(other.MaxDayLength);

                    case FieldIndex.AverageDayLength:
                        return AverageDayLength.CompareTo(other.AverageDayLength);

                    case FieldIndex.ShortBreak:
                        return ShortBreak.CompareTo(other.ShortBreak);

                    case FieldIndex.LongBreak:
                        return LongBreak.CompareTo(other.LongBreak);

                    case FieldIndex.AverageBreak:
                        return AverageBreak.CompareTo(other.AverageBreak);

                    case FieldIndex.NumberBreaks:
                        return NumberBreaks.CompareTo(other.NumberBreaks);

                    case FieldIndex.ShortBlock:
                        return ShortBlock.CompareTo(other.ShortBlock);

                    case FieldIndex.LongBlock:
                        return LongBlock.CompareTo(other.LongBlock);

                    case FieldIndex.AverageBlock:
                        return AverageBlock.CompareTo(other.AverageBlock);

                    case FieldIndex.NumberBlocks:
                        return NumberBlocks.CompareTo(other.NumberBlocks);

                    case FieldIndex.EarlyStart:
                        return EarlyStart.CompareTo(other.EarlyStart);

                    case FieldIndex.LateStart:
                        return LateStart.CompareTo(other.LateStart);

                    case FieldIndex.AverageStart:
                        return AverageStart.CompareTo(other.AverageStart);

                    case FieldIndex.EarlyEnd:
                        return EarlyEnd.CompareTo(other.EarlyEnd);

                    case FieldIndex.LateEnd:
                        return LateEnd.CompareTo(other.LateEnd);

                    case FieldIndex.AverageEnd:
                        return AverageEnd.CompareTo(other.AverageEnd);

                    default:
                        return 0;
                }
            }

            #endregion
        }
    }
}
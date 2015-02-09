#region

using System.Collections.Generic;
using UniTimetable.Model.Time;
using UniTimetable.Model.Timetable;

#endregion

namespace UniTimetable.Model.Solver
{
    public partial class Solver
    {
        public class Solution
        {
            public static int MinBreak = 15;
            // list of streams
            // member variables for solution analysis
            private OrderedList<Session>[] ClassesByDay_;
            private TimeLength TotalEnd_;
            // used in computation
            private TimeLength TotalStart_;

            #region Accessors

            public List<Stream> Streams { get; private set; }

            #endregion

            public void Clear()
            {
                // list of streams
                Streams = new List<Stream>();
                // clear all the data for calculating statistics
                ClearComputation();
            }

            public void ClearComputation()
            {
                // classes indexed by day
                ClassesByDay_ = new OrderedList<Session>[7];
                for (int i = 0; i < 7; i++)
                    ClassesByDay_[i] = new OrderedList<Session>();

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

                TotalStart_ = new TimeLength(0);
                TotalEnd_ = new TimeLength(0);
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

            public Solution(IEnumerable<Stream> streams)
            {
                Streams = new List<Stream>(streams);
                ReCompute();
            }

            public Solution(Solution other)
            {
                Streams = new List<Stream>(other.Streams);
                ClassesByDay_ = new OrderedList<Session>[7];
                for (int i = 0; i < 7; i++)
                {
                    ClassesByDay_[i] = new OrderedList<Session>(other.ClassesByDay_[i]);
                    //ClassesByDay_[i] = other.ClassesByDay_[i].Clone();
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

                TotalStart_ = other.TotalStart_;
                TotalEnd_ = other.TotalEnd_;
            }

            #endregion

            #region Evaluation

            public bool ReCompute()
            {
                ClearComputation();

                // check if each stream fits with the other streams
                /*for (int i = 0; i < Combination_.Count; i++)
                {
                    for (int j = i + 1; j < Combination_.Count; j++)
                    {
                        if (Timetable_.LookupClashTable(Combination_[i], Combination_[j]))
                            return false;
                    }
                }*/

                // for each stream
                foreach (Stream stream in Streams)
                {
                    // for each session in each stream
                    foreach (Session session in stream.Classes)
                    {
                        // build day-indexed list of classes
                        ClassesByDay_[session.Day].Add(session);
                        // calculate total time spent in classes
                        TimeInClasses += session.Length;
                    }
                }

                // for each day of classes
                foreach (List<Session> daySessions in ClassesByDay_)
                {
                    // empty day - skip
                    if (daySessions.Count == 0)
                        continue;
                    // otherwise increment day count
                    Days++;

                    #region Breaks and blocks

                    // set up data for the start of the block
                    TimeOfDay blockStart = daySessions[0].StartTime;
                    TimeLength blockLength, breakLength;

                    // compare adjacent classes
                    int i;
                    for (i = 1; i < daySessions.Count; i++)
                    {
                        breakLength = daySessions[i].StartTime - daySessions[i - 1].EndTime;

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

                    #endregion

                    TimeOfDay dayStart = daySessions[0].StartTime;
                    TimeOfDay dayEnd = daySessions[i - 1].EndTime;

                    TimeLength dayLength = dayEnd - dayStart;
                    // add to total time at uni
                    TimeAtUni += dayLength;
                    // check max/min
                    if (dayLength > MaxDayLength)
                        MaxDayLength = dayLength;
                    if (dayLength < MinDayLength)
                        MinDayLength = dayLength;

                    // add to total time for start and end
                    TotalStart_.TotalMinutes += dayStart.DayMinutes;
                    TotalEnd_.TotalMinutes += dayEnd.DayMinutes;

                    // check start/end min/max
                    if (dayStart < EarlyStart)
                        EarlyStart = dayStart;
                    if (dayStart > LateStart)
                        LateStart = dayStart;
                    if (dayEnd < EarlyEnd)
                        EarlyEnd = dayEnd;
                    if (dayEnd > LateEnd)
                        LateEnd = dayEnd;
                }

                // calculate averages using totals and counts
                if (NumberBreaks > 0)
                    AverageBreak = TimeInBreaks/NumberBreaks;
                AverageBlock /= NumberBlocks;
                AverageStart = new TimeOfDay(TotalStart_.TotalMinutes/Days);
                AverageEnd = new TimeOfDay(TotalEnd_.TotalMinutes/Days);
                AverageDayLength = TimeAtUni/Days;


                /*
                // process all the streams
                foreach (Stream stream in Combination_)
                {
                    // run through list of classes
                    foreach (Session session in stream.Classes)
                    {
                        // increase total time spent in classes
                        TimeInClasses_ += session.Length;

                        // if we're adding the class to an empty day
                        if (ClassesByDay_[session.Day].Count == 0)
                        {
                            // increment day count
                            Days_++;
                            // add start and ending times to totals
                            TotalStart_ += new TimeLength(session.Start.TotalMinutes);
                            TotalEnd_ += new TimeLength(session.End.TotalMinutes);

                            // add length to total time at uni
                            TimeAtUni_ += session.Length;
                        }
                        else
                        {
                            // if it's earlier than the earliest class of that day
                            if (session.Start < ClassesByDay_[session.Day][0].Start)
                            {
                                TimeLength difference = ClassesByDay_[session.Day][0].Start - session.Start;
                                // remove the difference for total start
                                TotalStart_ -= difference;
                                // add the difference for total time at uni
                                TimeAtUni_ += difference;
                            }
                            // if it's later than the latest class
                            if (session.End > ClassesByDay_[session.Day][ClassesByDay_[session.Day].Count - 1].End)
                            {
                                TimeLength difference = session.End - ClassesByDay_[session.Day][ClassesByDay_[session.Day].Count - 1].End;
                                // add the difference for total end
                                TotalStart_ += difference;
                                // add the difference for total time at uni
                                TimeAtUni_ += difference;
                            }
                        }
                        // update average day length
                        AverageDayLength_ = TimeAtUni_ / Days_;

                        // add new classes to day-indexed list
                        ClassesByDay_[session.Day].Add(session);
                        ClassesByDay_[session.Day].Sort();

                        // check class start/end times against maxima/minima
                        if (session.Start < EarlyStart_)
                            EarlyStart_ = session.Start;
                        if (session.Start > LateStart_)
                            LateStart_ = session.Start;
                        if (session.End < EarlyEnd_)
                            EarlyEnd_ = session.End;
                        if (session.End > LateEnd_)
                            LateEnd_ = session.End;

                        // check day length
                        TimeLength dayLength = ClassesByDay_[session.Day][ClassesByDay_[session.Day].Count - 1].End -
                            ClassesByDay_[session.Day][0].Start;
                        if (dayLength < MinDayLength_)
                            MinDayLength_ = dayLength;
                        if (dayLength > MaxDayLength_)
                            MaxDayLength_ = dayLength;
                    }
                }

                // clear break data
                NumberBreaks_ = 0;
                AverageBreak_ = new TimeLength(0, 0);
                ShortBreak_ = new TimeLength(24, 0);
                LongBreak_ = new TimeLength(0, 0);
                TimeInBreaks_ = new TimeLength(0, 0);
                // clear block data
                NumberBlocks_ = 0;
                AverageBlock_ = new TimeLength(0, 0);
                ShortBlock_ = new TimeLength(24, 0);
                LongBlock_ = new TimeLength(0, 0);
                // TODO: rewrite to avoid full sweep?
                // do a fresh sweep of all days to rebuild break/block data
                foreach (List<Session> daySessions in ClassesByDay_)
                {
                    // set up data for the start of the block
                    TimeOfDay blockStart;
                    if (daySessions.Count > 0)
                        blockStart = daySessions[0].Start;
                    else
                        blockStart = new TimeOfDay();

                    // compare adjacent classes
                    for (int i = 1; i < daySessions.Count; i++)
                    {
                        TimeLength breakLength = daySessions[i].Start - daySessions[i - 1].End;

                        // if there's a break between classes, make a block
                        if (breakLength.TotalMinutes >= MinBreak)
                        {
                            // find block length
                            TimeLength blockLength = daySessions[i - 1].End - blockStart;
                            // compare block against maxima/minima
                            if (blockLength < ShortBlock_)
                                ShortBlock_ = blockLength;
                            if (blockLength > LongBlock_)
                                LongBlock_ = blockLength;

                            // increment number of blocks
                            NumberBlocks_++;
                            // add block length to cumulative sum
                            AverageBlock_ += blockLength;
                            // set start of next block
                            blockStart = daySessions[i].Start;
                        }
                        // also create a block at the end
                        if (i == daySessions.Count - 1)
                        {
                            // find block length
                            TimeLength blockLength = daySessions[i].End - blockStart;
                            // compare block against maxima/minima
                            if (blockLength < ShortBlock_)
                                ShortBlock_ = blockLength;
                            if (blockLength > LongBlock_)
                                LongBlock_ = blockLength;

                            // increment number of blocks
                            NumberBlocks_++;
                            // add block length to cumulative sum
                            AverageBlock_ += blockLength;
                        }

                        // if there is at least ~15 minutes between the classes, call it a break, otherwise skip
                        if (breakLength.TotalMinutes < MinBreak)
                            continue;

                        // increment number of breaks
                        NumberBreaks_++;
                        // add break to cumulative sum
                        TimeInBreaks_ += breakLength;

                        // compare break length against maxima/minima
                        if (breakLength < ShortBreak_)
                            ShortBreak_ = breakLength;
                        // check if it's the longest break so far
                        if (breakLength > LongBreak_)
                            LongBreak_ = breakLength;
                    }
                }
                // divide the sum of breaks to find the mean
                AverageBreak_ = TimeInBreaks_ / NumberBreaks_;
                // divide the sum of blocks to find the mean
                AverageBlock_ /= NumberBlocks_;*/

                return true;
            }

            public bool Fits(Stream stream)
            {
                // check if stream fits with the current streams
                foreach (Stream other in Streams)
                {
                    if (stream.ClashesWith(other))
                        return false;
                }
                return true;
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
                    if (ClassesByDay_[session.Day].Count == 0)
                    {
                        // increment day count
                        Days++;
                        // add start and ending times to totals
                        TotalStart_ += new TimeLength(session.StartTime.DayMinutes);
                        TotalEnd_ += new TimeLength(session.EndTime.DayMinutes);

                        // add length to total time at uni
                        TimeAtUni += session.Length;
                    }
                    else
                    {
                        // if it's earlier than the earliest class of that day
                        if (session.StartTime < ClassesByDay_[session.Day][0].StartTime)
                        {
                            TimeLength difference = ClassesByDay_[session.Day][0].StartTime - session.StartTime;
                            // remove the difference for total start
                            TotalStart_ -= difference;
                            // add the difference for total time at uni
                            TimeAtUni += difference;
                        }
                        // if it's later than the latest class
                        if (session.EndTime > ClassesByDay_[session.Day][ClassesByDay_[session.Day].Count - 1].EndTime)
                        {
                            TimeLength difference = session.EndTime -
                                                    ClassesByDay_[session.Day][ClassesByDay_[session.Day].Count - 1]
                                                        .EndTime;
                            // add the difference for total end
                            TotalEnd_ += difference;
                            // add the difference for total time at uni
                            TimeAtUni += difference;
                        }
                    }
                    // update average day length
                    AverageDayLength = TimeAtUni/Days;

                    // add new classes to day-indexed list
                    ClassesByDay_[session.Day].Add(session);
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
                    TimeLength dayLength = ClassesByDay_[session.Day][ClassesByDay_[session.Day].Count - 1].EndTime -
                                           ClassesByDay_[session.Day][0].StartTime;
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
                foreach (List<Session> daySessions in ClassesByDay_)
                {
                    // empty day - skip
                    if (daySessions.Count == 0)
                        continue;

                    // set up data for the start of the block
                    TimeOfDay blockStart = daySessions[0].StartTime;
                    TimeLength blockLength, breakLength;

                    // compare adjacent classes
                    int i;
                    for (i = 1; i < daySessions.Count; i++)
                    {
                        breakLength = daySessions[i].StartTime - daySessions[i - 1].EndTime;

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

                if (NumberBreaks > 0)
                {
                    // divide the sum of breaks to find the mean
                    AverageBreak = TimeInBreaks/NumberBreaks;
                }

                // divide the sum of blocks to find the mean
                AverageBlock /= NumberBlocks;

                AverageStart = new TimeOfDay(TotalStart_.TotalMinutes/Days);
                AverageEnd = new TimeOfDay(TotalEnd_.TotalMinutes/Days);

                return true;
            }

            public bool AddStreamWithoutCompute(Stream stream)
            {
                // if the stream clashes with another stream
                if (!Fits(stream))
                    return false;

                // add to list of streams
                Streams.Add(stream);
                return true;
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
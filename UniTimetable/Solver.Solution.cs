using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UniTimetable
{
    public partial class Solver
    {
        public class Solution
        {
            // list of streams
            List<Stream> Combination_;
            // member variables for solution analysis
            OrderedList<Session>[] ClassesByDay_;

            public static int MinBreak = 15;

            #region Accessors

            public List<Stream> Streams
            {
                get
                {
                    return Combination_;
                }
            }

            #endregion

            #region Solution criteria accessors

            public TimeLength TimeAtUni { get { return TimeAtUni_; } }
            public TimeLength TimeInClasses { get { return TimeInClasses_; } }
            public TimeLength TimeInBreaks { get { return TimeInBreaks_; } }
            public int Days { get { return Days_; } }

            public TimeLength MinDayLength { get { return MinDayLength_; } }
            public TimeLength MaxDayLength { get { return MaxDayLength_; } }
            public TimeLength AverageDayLength { get { return AverageDayLength_; } }

            public TimeLength ShortBreak { get { return ShortBreak_; } }
            public TimeLength LongBreak { get { return LongBreak_; } }
            public TimeLength AverageBreak { get { return AverageBreak_; } }
            public int NumberBreaks { get { return NumberBreaks_; } }

            public TimeLength ShortBlock { get { return ShortBlock_; } }
            public TimeLength LongBlock { get { return LongBlock_; } }
            public TimeLength AverageBlock { get { return AverageBlock_; } }
            public int NumberBlocks { get { return NumberBlocks_; } }

            public TimeOfDay EarlyStart { get { return EarlyStart_; } }
            public TimeOfDay LateStart { get { return LateStart_; } }
            public TimeOfDay AverageStart { get { return AverageStart_; } }

            public TimeOfDay EarlyEnd { get { return EarlyEnd_; } }
            public TimeOfDay LateEnd { get { return LateEnd_; } }
            public TimeOfDay AverageEnd { get { return AverageEnd_; } }

            #endregion

            #region Solution criteria

            TimeLength TimeAtUni_;          // 0
            TimeLength TimeInClasses_;      // 1
            TimeLength TimeInBreaks_;       // 2
            int Days_;                      // 3

            TimeLength MinDayLength_;       // 4
            TimeLength MaxDayLength_;       // 5
            TimeLength AverageDayLength_;   // 6

            TimeLength ShortBreak_;         // 7
            TimeLength LongBreak_;          // 8
            TimeLength AverageBreak_;       // 9
            int NumberBreaks_;              // 10

            TimeLength ShortBlock_;         // 11
            TimeLength LongBlock_;          // 12
            TimeLength AverageBlock_;       // 13
            int NumberBlocks_;              // 14

            TimeOfDay EarlyStart_;          // 15
            TimeOfDay LateStart_;           // 16
            TimeOfDay AverageStart_;        // 17

            TimeOfDay EarlyEnd_;            // 18
            TimeOfDay LateEnd_;             // 19
            TimeOfDay AverageEnd_;          // 20

            #endregion

            // used in computation
            TimeLength TotalStart_;
            TimeLength TotalEnd_;

            #region Constructors

            public Solution()
            {
                Clear();
            }

            public Solution(IEnumerable<Stream> streams)
            {
                Combination_ = new List<Stream>(streams);
                ReCompute();
            }

            public Solution(Solution other)
            {
                this.Combination_ = new List<Stream>(other.Combination_);
                this.ClassesByDay_ = new OrderedList<Session>[7];
                for (int i = 0; i < 7; i++)
                {
                    ClassesByDay_[i] = new OrderedList<Session>(other.ClassesByDay_[i]);
                    //ClassesByDay_[i] = other.ClassesByDay_[i].Clone();
                }

                this.TimeAtUni_ = other.TimeAtUni_;
                this.TimeInClasses_ = other.TimeInClasses_;
                this.TimeInBreaks_ = other.TimeInBreaks_;
                this.Days_ = other.Days_;

                this.MinDayLength_ = other.MinDayLength_;
                this.MaxDayLength_ = other.MaxDayLength_;
                this.AverageDayLength_ = other.AverageDayLength_;

                this.ShortBreak_ = other.ShortBreak_;
                this.LongBreak_ = other.LongBreak_;
                this.AverageBreak_ = other.AverageBreak_;
                this.NumberBreaks_ = other.NumberBreaks_;

                this.ShortBlock_ = other.ShortBlock_;
                this.LongBlock_ = other.LongBlock_;
                this.AverageBlock_ = other.AverageBlock_;
                this.NumberBlocks_ = other.NumberBlocks_;

                this.EarlyStart_ = other.EarlyStart_;
                this.LateStart_ = other.LateStart_;
                this.AverageStart_ = other.AverageStart_;

                this.EarlyEnd_ = other.EarlyEnd_;
                this.LateEnd_ = other.EarlyEnd_;
                this.AverageEnd_ = other.AverageEnd_;

                this.TotalStart_ = other.TotalStart_;
                this.TotalEnd_ = other.TotalEnd_;
            }

            #endregion

            public void Clear()
            {
                // list of streams
                Combination_ = new List<Stream>();
                // clear all the data for calculating statistics
                ClearComputation();
            }

            public void ClearComputation()
            {
                // classes indexed by day
                ClassesByDay_ = new OrderedList<Session>[7];
                for (int i = 0; i < 7; i++)
                    ClassesByDay_[i] = new OrderedList<Session>();

                TimeAtUni_ = new TimeLength(0, 0);
                TimeInClasses_ = new TimeLength(0, 0);
                TimeInBreaks_ = new TimeLength(0, 0);
                Days_ = 0;

                MinDayLength_ = new TimeLength(24, 0);
                MaxDayLength_ = new TimeLength(0, 0);
                AverageDayLength_ = new TimeLength(0, 0);

                ShortBreak_ = new TimeLength(24, 0);
                LongBreak_ = new TimeLength(0, 0);
                AverageBreak_ = new TimeLength(0, 0);
                NumberBreaks_ = 0;

                ShortBlock_ = new TimeLength(24, 0);
                LongBlock_ = new TimeLength(0, 0);
                AverageBlock_ = new TimeLength(0, 0);
                NumberBlocks_ = 0;

                EarlyStart_ = TimeOfDay.Maximum;
                LateStart_ = TimeOfDay.Minimum;
                AverageStart_ = TimeOfDay.Minimum;

                EarlyEnd_ = TimeOfDay.Maximum;
                LateEnd_ = TimeOfDay.Minimum;
                AverageEnd_ = TimeOfDay.Minimum;

                TotalStart_ = new TimeLength(0);
                TotalEnd_ = new TimeLength(0);
            }

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
                foreach (Stream stream in Combination_)
                {
                    // for each session in each stream
                    foreach (Session session in stream.Classes)
                    {
                        // build day-indexed list of classes
                        ClassesByDay_[session.Day].Add(session);
                        // calculate total time spent in classes
                        TimeInClasses_ += session.Length;
                    }
                }

                // for each day of classes
                foreach (List<Session> daySessions in ClassesByDay_)
                {
                    // empty day - skip
                    if (daySessions.Count == 0)
                        continue;
                    // otherwise increment day count
                    Days_++;

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
                        NumberBlocks_++;
                        // add block length to cumulative sum
                        AverageBlock_ += blockLength;
                        // set start of next block
                        blockStart = daySessions[i].StartTime;

                        // compare block against maxima/minima
                        if (blockLength < ShortBlock_)
                            ShortBlock_ = blockLength;
                        if (blockLength > LongBlock_)
                            LongBlock_ = blockLength;

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

                    // also create a block at the end
                    // find block length
                    blockLength = daySessions[i - 1].EndTime - blockStart;
                    // compare block against maxima/minima
                    if (blockLength < ShortBlock_)
                        ShortBlock_ = blockLength;
                    if (blockLength > LongBlock_)
                        LongBlock_ = blockLength;
                    // increment number of blocks
                    NumberBlocks_++;
                    // add block length to cumulative sum
                    AverageBlock_ += blockLength;

                    #endregion

                    TimeOfDay dayStart = daySessions[0].StartTime;
                    TimeOfDay dayEnd = daySessions[i - 1].EndTime;

                    TimeLength dayLength = dayEnd - dayStart;
                    // add to total time at uni
                    TimeAtUni_ += dayLength;
                    // check max/min
                    if (dayLength > MaxDayLength_)
                        MaxDayLength_ = dayLength;
                    if (dayLength < MinDayLength_)
                        MinDayLength_ = dayLength;

                    // add to total time for start and end
                    TotalStart_.TotalMinutes += dayStart.DayMinutes;
                    TotalEnd_.TotalMinutes += dayEnd.DayMinutes;

                    // check start/end min/max
                    if (dayStart < EarlyStart_)
                        EarlyStart_ = dayStart;
                    if (dayStart > LateStart_)
                        LateStart_ = dayStart;
                    if (dayEnd < EarlyEnd_)
                        EarlyEnd_ = dayEnd;
                    if (dayEnd > LateEnd_)
                        LateEnd_ = dayEnd;
                }

                // calculate averages using totals and counts
                if (NumberBreaks_ > 0)
                    AverageBreak_ = TimeInBreaks_ / NumberBreaks_;
                AverageBlock_ /= NumberBlocks_;
                AverageStart_ = new TimeOfDay(TotalStart_.TotalMinutes / Days_);
                AverageEnd_ = new TimeOfDay(TotalEnd_.TotalMinutes / Days_);
                AverageDayLength_ = TimeAtUni_ / Days_;



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
                foreach (Stream other in Combination_)
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
                Combination_.Add(stream);

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
                        TotalStart_ += new TimeLength(session.StartTime.DayMinutes);
                        TotalEnd_ += new TimeLength(session.EndTime.DayMinutes);

                        // add length to total time at uni
                        TimeAtUni_ += session.Length;
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
                            TimeAtUni_ += difference;
                        }
                        // if it's later than the latest class
                        if (session.EndTime > ClassesByDay_[session.Day][ClassesByDay_[session.Day].Count - 1].EndTime)
                        {
                            TimeLength difference = session.EndTime - ClassesByDay_[session.Day][ClassesByDay_[session.Day].Count - 1].EndTime;
                            // add the difference for total end
                            TotalEnd_ += difference;
                            // add the difference for total time at uni
                            TimeAtUni_ += difference;
                        }
                    }
                    // update average day length
                    AverageDayLength_ = TimeAtUni_ / Days_;

                    // add new classes to day-indexed list
                    ClassesByDay_[session.Day].Add(session);
                    //ClassesByDay_[session.Day].Sort();

                    // check class start/end times against maxima/minima
                    if (session.StartTime < EarlyStart_)
                        EarlyStart_ = session.StartTime;
                    if (session.StartTime > LateStart_)
                        LateStart_ = session.StartTime;
                    if (session.EndTime < EarlyEnd_)
                        EarlyEnd_ = session.EndTime;
                    if (session.EndTime > LateEnd_)
                        LateEnd_ = session.EndTime;

                    // check day length
                    TimeLength dayLength = ClassesByDay_[session.Day][ClassesByDay_[session.Day].Count - 1].EndTime -
                        ClassesByDay_[session.Day][0].StartTime;
                    if (dayLength < MinDayLength_)
                        MinDayLength_ = dayLength;
                    if (dayLength > MaxDayLength_)
                        MaxDayLength_ = dayLength;
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
                        NumberBlocks_++;
                        // add block length to cumulative sum
                        AverageBlock_ += blockLength;
                        // set start of next block
                        blockStart = daySessions[i].StartTime;

                        // compare block against maxima/minima
                        if (blockLength < ShortBlock_)
                            ShortBlock_ = blockLength;
                        if (blockLength > LongBlock_)
                            LongBlock_ = blockLength;

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

                    // also create a block at the end
                    // find block length
                    blockLength = daySessions[i - 1].EndTime - blockStart;
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

                if (NumberBreaks_ > 0)
                {
                    // divide the sum of breaks to find the mean
                    AverageBreak_ = TimeInBreaks_ / NumberBreaks_;
                }

                // divide the sum of blocks to find the mean
                AverageBlock_ /= NumberBlocks_;

                AverageStart_ = new TimeOfDay(TotalStart_.TotalMinutes / Days_);
                AverageEnd_ = new TimeOfDay(TotalEnd_.TotalMinutes / Days_);

                return true;
            }

            public bool AddStreamWithoutCompute(Stream stream)
            {
                // if the stream clashes with another stream
                if (!Fits(stream))
                    return false;

                // add to list of streams
                Combination_.Add(stream);
                return true;
            }

            #endregion

            public int CompareTo(Solution other, SolutionComparer solutionComparer)
            {
                return solutionComparer.Compare(this, other);
            }

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
                        return TimeAtUni_.ToString();

                    case FieldIndex.TimeInClasses:
                        return TimeInClasses_.ToString();

                    case FieldIndex.TimeInBreaks:
                        return TimeInBreaks_.ToString();

                    case FieldIndex.Days:
                        return Days_.ToString();

                    case FieldIndex.MinDayLength:
                        return MinDayLength_.ToString();

                    case FieldIndex.MaxDayLength:
                        return MaxDayLength_.ToString();

                    case FieldIndex.AverageDayLength:
                        return AverageDayLength_.ToString();

                    case FieldIndex.ShortBreak:
                        return ShortBreak_.ToString();

                    case FieldIndex.LongBreak:
                        return LongBreak_.ToString();

                    case FieldIndex.AverageBreak:
                        return AverageBreak_.ToString();

                    case FieldIndex.NumberBreaks:
                        return NumberBreaks_.ToString();

                    case FieldIndex.ShortBlock:
                        return ShortBlock_.ToString();

                    case FieldIndex.LongBlock:
                        return LongBlock_.ToString();

                    case FieldIndex.AverageBlock:
                        return AverageBlock_.ToString();

                    case FieldIndex.NumberBlocks:
                        return NumberBlocks_.ToString();

                    case FieldIndex.EarlyStart:
                        return EarlyStart_.ToString();

                    case FieldIndex.LateStart:
                        return LateStart_.ToString();

                    case FieldIndex.AverageStart:
                        return AverageStart_.ToString();

                    case FieldIndex.EarlyEnd:
                        return EarlyEnd_.ToString();

                    case FieldIndex.LateEnd:
                        return LateEnd_.ToString();

                    case FieldIndex.AverageEnd:
                        return AverageEnd_.ToString();

                    default:
                        return "";
                }
            }

            public int FieldValueToInt(FieldIndex field)
            {
                switch (field)
                {
                    case FieldIndex.TimeAtUni:
                        return TimeAtUni_.TotalMinutes;

                    case FieldIndex.TimeInClasses:
                        return TimeInClasses_.TotalMinutes;

                    case FieldIndex.TimeInBreaks:
                        return TimeInBreaks_.TotalMinutes;

                    case FieldIndex.Days:
                        return Days_;

                    case FieldIndex.MinDayLength:
                        return MinDayLength_.TotalMinutes;

                    case FieldIndex.MaxDayLength:
                        return MaxDayLength_.TotalMinutes;

                    case FieldIndex.AverageDayLength:
                        return AverageDayLength_.TotalMinutes;

                    case FieldIndex.ShortBreak:
                        return ShortBreak_.TotalMinutes;

                    case FieldIndex.LongBreak:
                        return LongBreak_.TotalMinutes;

                    case FieldIndex.AverageBreak:
                        return AverageBreak_.TotalMinutes;

                    case FieldIndex.NumberBreaks:
                        return NumberBreaks_;

                    case FieldIndex.ShortBlock:
                        return ShortBlock_.TotalMinutes;

                    case FieldIndex.LongBlock:
                        return LongBlock_.TotalMinutes;

                    case FieldIndex.AverageBlock:
                        return AverageBlock_.TotalMinutes;

                    case FieldIndex.NumberBlocks:
                        return NumberBlocks_;

                    case FieldIndex.EarlyStart:
                        return EarlyStart_.DayMinutes;

                    case FieldIndex.LateStart:
                        return LateStart_.DayMinutes;

                    case FieldIndex.AverageStart:
                        return AverageStart_.DayMinutes;

                    case FieldIndex.EarlyEnd:
                        return EarlyEnd_.DayMinutes;

                    case FieldIndex.LateEnd:
                        return LateEnd_.DayMinutes;

                    case FieldIndex.AverageEnd:
                        return AverageEnd_.DayMinutes;

                    default:
                        return 0;
                }
            }

            public int CompareFieldValueTo(FieldIndex field, Solution other)
            {
                switch (field)
                {
                    case FieldIndex.TimeAtUni:
                        return this.TimeAtUni_.CompareTo(other.TimeAtUni_);

                    case FieldIndex.TimeInClasses:
                        return this.TimeInClasses_.CompareTo(other.TimeInClasses_);

                    case FieldIndex.TimeInBreaks:
                        return this.TimeInBreaks_.CompareTo(other.TimeInBreaks_);

                    case FieldIndex.Days:
                        return this.Days_.CompareTo(other.Days_);

                    case FieldIndex.MinDayLength:
                        return this.MinDayLength_.CompareTo(other.MinDayLength_);

                    case FieldIndex.MaxDayLength:
                        return this.MaxDayLength_.CompareTo(other.MaxDayLength_);

                    case FieldIndex.AverageDayLength:
                        return this.AverageDayLength_.CompareTo(other.AverageDayLength_);

                    case FieldIndex.ShortBreak:
                        return this.ShortBreak_.CompareTo(other.ShortBreak_);

                    case FieldIndex.LongBreak:
                        return this.LongBreak_.CompareTo(other.LongBreak_);

                    case FieldIndex.AverageBreak:
                        return this.AverageBreak_.CompareTo(other.AverageBreak_);

                    case FieldIndex.NumberBreaks:
                        return this.NumberBreaks_.CompareTo(other.NumberBreaks_);

                    case FieldIndex.ShortBlock:
                        return this.ShortBlock_.CompareTo(other.ShortBlock_);

                    case FieldIndex.LongBlock:
                        return this.LongBlock_.CompareTo(other.LongBlock_);

                    case FieldIndex.AverageBlock:
                        return this.AverageBlock_.CompareTo(other.AverageBlock_);

                    case FieldIndex.NumberBlocks:
                        return this.NumberBlocks_.CompareTo(other.NumberBlocks_);

                    case FieldIndex.EarlyStart:
                        return this.EarlyStart_.CompareTo(other.EarlyStart_);

                    case FieldIndex.LateStart:
                        return this.LateStart_.CompareTo(other.LateStart_);

                    case FieldIndex.AverageStart:
                        return AverageStart_.CompareTo(other.AverageStart_);

                    case FieldIndex.EarlyEnd:
                        return EarlyEnd_.CompareTo(other.EarlyEnd_);

                    case FieldIndex.LateEnd:
                        return LateEnd_.CompareTo(other.LateEnd_);

                    case FieldIndex.AverageEnd:
                        return AverageEnd_.CompareTo(other.AverageEnd_);

                    default:
                        return 0;
                }
            }

            #endregion
        }
    }
}

#region

using System.Collections.Generic;
using System.ComponentModel;
using UniTimetable.Model.Timetable;

#endregion

namespace UniTimetable.Model.Solver
{
    public partial class Solver
    {
        private readonly int MaxResults_ = 250;
        private readonly List<Solution> Solutions_ = new List<Solution>();
        private int NumberValidSolutions_;
        private HeapWithComparer<Solution> SolutionHeap_;
        // mid-calculation stuff
        private List<List<Stream>> StreamSets_ = new List<List<Stream>>();
        private Solution[] UnorderedSolutions_;

        public void Default()
        {
            LoadPreset(Presets[0]);
        }

        public void LoadPreset(Preset preset)
        {
            Comparer.LoadCriteria(preset.Criteria);

            Filters.Clear();
            Filters.AddRange(preset.Filters);
        }

        #region Constructors

        public Solver(Timetable.Timetable timetable)
        {
            Timetable = timetable;

            Comparer = new SolutionComparer();
            Filters = new List<Filter>();
            Default();
        }

        public Solver(Timetable.Timetable timetable, Solver other)
        {
            Timetable = timetable;

            Comparer = other.Comparer.Clone();
            Filters = new List<Filter>(other.Filters);

            Solutions_ = new List<Solution>(Solutions_);
            MaxResults_ = other.MaxResults_;

            // other variables are initiated during solving
        }

        public Solver Clone(Timetable.Timetable timetable)
        {
            return new Solver(timetable, this);
        }

        public Solver Clone()
        {
            return new Solver(Timetable.DeepCopy(), this);
        }

        #endregion

        #region Accessors

        public Timetable.Timetable Timetable { get; set; }

        public List<Solution> Solutions
        {
            get { return Solutions_; }
        }

        public SolutionComparer Comparer { get; private set; }

        public List<Filter> Filters { get; set; }

        #endregion

        #region Actual solving

        public SolverResult Compute(BackgroundWorker worker, DoWorkEventArgs e)
        {
            if (Timetable == null || !Timetable.HasData())
            {
                return SolverResult.NoTimetable;
            }

            Solutions_.Clear();
            StreamSets_.Clear();

            long totalSolutions = 1;
            SolutionHeap_ = null;
            UnorderedSolutions_ = new Solution[MaxResults_];

            #region Build lists of stream options

            foreach (Type type in Timetable.TypeList)
            {
                // if the stream type is ignored, skip
                if (!type.Required)
                    continue;

                // create a list of streams for each type
                List<Stream> streams = new List<Stream>();

                // stream already enabled? only one option
                if (type.SelectedStream != null)
                {
                    streams.Add(type.SelectedStream);
                }
                else
                {
                    // for each stream in the current type
                    foreach (Stream stream in type.UniqueStreams)
                    {
                        // if stream can't be selected, skip it
                        if (!Timetable.Fits(stream))
                            continue;

                        streams.Add(stream);
                    }
                    // no streams made it - skip
                    if (streams.Count == 0)
                    {
                        return SolverResult.Clash;
                    }
                }

                // add streams to list
                StreamSets_.Add(streams);

                // update the complexity of the solution
                totalSolutions *= streams.Count;
            }

            #endregion

            #region Optimise unique stream list order

            // order determined greedily on basis of maximum clashes

            float[,] clashChance = new float[StreamSets_.Count, StreamSets_.Count];

            // build 2D clash probability table
            for (int i = 0; i < StreamSets_.Count; i++)
            {
                List<Stream> set = StreamSets_[i];
                for (int j = i + 1; j < StreamSets_.Count; j++)
                {
                    List<Stream> otherSet = StreamSets_[j];

                    int total = set.Count*otherSet.Count;
                    int clash = 0;
                    foreach (Stream stream in set)
                    {
                        foreach (Stream otherStream in otherSet)
                        {
                            if (stream.ClashesWith(otherStream))
                                clash++;
                        }
                    }
                    float chance = clash/(float) total;
                    clashChance[i, j] = chance;
                    clashChance[j, i] = chance;
                }
            }

            // build index lists, extract sets of only one stream
            List<int> ordered = new List<int>();
            List<int> remaining = new List<int>();
            for (int i = 0; i < StreamSets_.Count; i++)
            {
                if (StreamSets_[i].Count == 1)
                {
                    ordered.Add(i);
                }
                else
                {
                    remaining.Add(i);
                }
            }


            // get probability for single-option sets
            foreach (int i in ordered)
            {
                float chance = 1f;
                foreach (int j in ordered)
                {
                    // reached same index - compared to all preceding
                    if (j == i)
                        break;
                    chance *= 1f - clashChance[i, j];
                }
                chance = 1f - chance;
            }

            // order by probability of clash with previously selected items
            while (remaining.Count > 0)
            {
                float maxChance = 0;
                float maxFuture = 0;
                int maxIndex = -1;

                foreach (int i in remaining)
                {
                    float chance = 1f;
                    float future = 1f;
                    foreach (int j in ordered)
                    {
                        // find chance of not successively not clashing
                        chance *= 1f - clashChance[i, j];
                    }
                    foreach (int j in remaining)
                    {
                        if (i == j)
                            continue;
                        // chance of clashing with remaining options
                        future *= 1f - clashChance[i, j];
                    }
                    // chance of clashing
                    chance = 1f - chance;
                    future = 1f - future;

                    // check if there's a pair which is more likely to clash
                    float pairMax = 0f;
                    foreach (int j in remaining)
                    {
                        if (i == j)
                            continue;
                        if (clashChance[i, j] > pairMax)
                        {
                            pairMax = clashChance[i, j];
                        }
                    }
                    if (pairMax > chance)
                    {
                        chance = pairMax;
                        // give preference to any instant solution
                        future = -1;
                    }

                    if (maxIndex < 0 || chance > maxChance || (chance == maxChance && future > maxFuture))
                    {
                        maxIndex = i;
                        maxChance = chance;
                        maxFuture = future;
                    }
                }

                remaining.Remove(maxIndex);
                ordered.Add(maxIndex);
            }

            /*
            // do specialised selection sort to bring most incompatible streams to the front

            // first find single worst instance of incompatibility
            float maxValue = -1;
            int maxIndex = -1;
            // look for worst incompatibility between a set of streams
            foreach (int i in remaining)
            {
                if (StreamSets_[i].Count == 1)
                {
                    maxIndex = i;
                    break;
                }
                // and all sets of streams which follow
                foreach (int j in remaining)
                {
                    if (i == j)
                        continue;
                    if (clashChance[i, j] > maxValue)
                    {
                        maxValue = clashChance[i, j];
                        maxIndex = i;
                    }
                }
            }

            remaining.Remove(maxIndex);
            ordered.Add(maxIndex);

            // selection sort the rest of the list
            while (remaining.Count > 0)
            {
                // TODO: initialisation required?
                maxValue = -1;
                maxIndex = -1;
                // find maximum from all remaining sets of streams
                foreach (int j in remaining)
                {
                    if (StreamSets_[j].Count == 1)
                    {
                        maxIndex = j;
                        break;
                    }

                    float currentMaxValue = -1;
                    // to find the maximum for a set, find its worst match from the already selected streams
                    foreach (int k in ordered)
                    {
                        // if found new max for current stream list
                        if (clashChance[j, k] > currentMaxValue)
                        {
                            currentMaxValue = clashChance[j, k];
                        }
                    }
                    // if found new max for all stream lists
                    if (currentMaxValue > maxValue)
                    {
                        maxValue = currentMaxValue;
                        maxIndex = j;
                    }
                }

                remaining.Remove(maxIndex);
                ordered.Add(maxIndex);
            }*/

            List<List<Stream>> final = new List<List<Stream>>();
            foreach (int index in ordered)
            {
                final.Add(StreamSets_[index]);
            }
            StreamSets_ = final;

            #endregion

            #region Stack-based algorithm

            Stack<Solution> solutionStack = new Stack<Solution>(StreamSets_.Count);
            solutionStack.Push(new Solution());
            Solution solution = new Solution();

            NumberValidSolutions_ = 0;
            long progress = 0;
            int progressPercent = 0;
            long numLoops = 0;

            // create an array such that the value at an index gives the number
            // of complete solutions "down that alley" when processing is cut
            // short at that index into the unique streams list
            long[] cumulativeProduct = new long[StreamSets_.Count];
            cumulativeProduct[cumulativeProduct.Length - 1] = 1;
            for (int i = cumulativeProduct.Length - 2; i >= 0; i--)
            {
                cumulativeProduct[i] = cumulativeProduct[i + 1]*StreamSets_[i + 1].Count;
            }

            // setIndex represents from which set of streams the next stream is being selected
            int setIndex = 0;
            int[] streamIndices = new int[StreamSets_.Count];
            for (int i = 0; i < streamIndices.Length; i++)
                streamIndices[i] = 0;

            //while (true)
            while (true)
            {
                if (worker.CancellationPending)
                {
                    SortFinalResults();
                    e.Cancel = true;
                    return SolverResult.UserCancel;
                }

                numLoops++;

                // check if the stream index is out of range
                // (done all options for current set of streams)
                if (streamIndices[setIndex] == StreamSets_[setIndex].Count)
                {
                    // if we're back at the start
                    if (setIndex == 0)
                    {
                        // then we're spent! exit
                        break;
                    }
                    // tried all possibilities at the current level, backtrack a level
                    setIndex--;
                    // and go to the next possibility there
                    streamIndices[setIndex]++;
                    // pop off the saved solution from the previous level
                    //solution.Streams.RemoveAt(solution.Streams.Count - 1);
                    solutionStack.Pop();
                    solution = new Solution(solutionStack.Peek());

                    // try with the new indices!
                    continue;
                }

                // attempt to add the current stream to the solution
                if (!solution.AddStream(StreamSets_[setIndex][streamIndices[setIndex]]))
                {
                    // add to progress the number of solutions just skipped
                    progress += cumulativeProduct[setIndex];
                    int percent = (int) ((float) progress/totalSolutions*100);
                    if (percent > progressPercent)
                    {
                        worker.ReportProgress(percent);
                        progressPercent = percent;
                    }
                    // stream is incompatible, try adjacent possibility
                    streamIndices[setIndex]++;
                    // try now
                    continue;
                }

                // found stream that fits!

                // if we have a complete solution
                if (setIndex == StreamSets_.Count - 1)
                {
                    // increment progress
                    progress++;
                    int percent = (int) ((float) progress/totalSolutions*100);
                    if (percent > progressPercent)
                    {
                        worker.ReportProgress(percent);
                        progressPercent = percent;
                    }

                    bool filteredOut = false;
                    foreach (Filter filter in Filters)
                    {
                        // failed on a filter?
                        if (!filter.Pass(solution))
                        {
                            filteredOut = true;
                            break;
                        }
                    }

                    if (!filteredOut)
                    {
                        NumberValidSolutions_++;

                        // if the solution list is full to capacity
                        if (NumberValidSolutions_ > MaxResults_)
                        {
                            // if the solution is in the top so far, take the place of the worst solution
                            SolutionHeap_.CompareReplaceMaximum(solution);
                        }
                        else
                        {
                            // append the solution to the array
                            UnorderedSolutions_[NumberValidSolutions_ - 1] = solution;
                            // if the unordered list just filled up
                            if (NumberValidSolutions_ == MaxResults_)
                            {
                                // build the heap from the array
                                SolutionHeap_ = new HeapWithComparer<Solution>(MaxResults_, UnorderedSolutions_,
                                    Comparer);
                            }
                        }
                    }

                    // now revert current solution
                    solution = new Solution(solutionStack.Peek());
                    //solution.Streams.RemoveAt(solution.Streams.Count - 1);
                    // and move to next along
                    streamIndices[setIndex]++;
                    // try next
                    continue;
                }

                // no real gain observed
                // check if path is worth pursuing
                /*else if (NumberValidSolutions_ > MaxResults_)
                {
                    bool abort = false;
                    Solution worst = SolutionHeap_.FindMaximum();
                    foreach (Criteria critieria in Comparer_.Criteria)
                    {
                        // grows randomly
                        if (critieria.Field.Progression == FieldProgression.Unknown)
                        {
                            break;
                        }
                        // grows and trying to minimise
                        else if (critieria.Field.Progression == FieldProgression.Grow && critieria.Preference == Preference.Minimise)
                        {
                            if (solution.FieldValueToInt(critieria.Field.Index) > worst.FieldValueToInt(critieria.Field.Index))
                            {
                                abort = true;
                            }
                            break;
                        }
                        // shrinks and trying to maximise
                        else if (critieria.Field.Progression == FieldProgression.Shrink && critieria.Preference == Preference.Maximise)
                        {
                            if (solution.FieldValueToInt(critieria.Field.Index) < worst.FieldValueToInt(critieria.Field.Index))
                            {
                                abort = true;
                            }
                            break;
                        }
                    }
                    if (abort)
                    {
                        // stream won't work, restore and try adjacent
                        solution = new Solution(solutionStack.Peek());
                        streamIndices[setIndex]++;
                        continue;
                    }
                }*/

                // move to next level
                setIndex++;
                // and try first option there
                streamIndices[setIndex] = 0;
                // push the solution with the new stream added on to the stack
                solutionStack.Push(solution);
                // set the working solution to a copy of the previous solution
                solution = new Solution(solution);
            }

            //MessageBox.Show(numLoops.ToString() + " loops\n" + totalSolutions.ToString() + " combinations");
            //MessageBox.Show("Number of valid solutions found: " + numValid.ToString() +
            //    "\nExpected number of solutions: " + totalSolutions.ToString() +
            //    "\nNumber of solutions \"checked\": " + progress.ToString());

            SortFinalResults();

            #endregion

            Timetable.RecomputeSolutions = false;
            return SolverResult.Complete;
        }

        private void SortFinalResults()
        {
            // if the best list didn't fill up
            if (NumberValidSolutions_ < MaxResults_)
                SolutionHeap_ = new HeapWithComparer<Solution>(MaxResults_, UnorderedSolutions_, Comparer);

            // run heapsort on the top solutions, copy into solution list
            Solutions_.AddRange(SolutionHeap_.GetSorted());
        }

        #endregion
    }

    public enum SolverResult
    {
        Complete,
        UserCancel,
        Clash,
        NoTimetable
    }
}
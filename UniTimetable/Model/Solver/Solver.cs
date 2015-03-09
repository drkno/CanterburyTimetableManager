#region

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UniTimetable.Model.Timetable;

#endregion

namespace UniTimetable.Model.Solver
{
    public partial class Solver
    {
        private const int MaxResults = 250;
        private readonly List<Solution> _solutions = new List<Solution>();
        private int _numberValidSolutions;
        private HeapWithComparer<Solution> _solutionHeap;
        // mid-calculation stuff
        private List<List<Stream>> _streamSets = new List<List<Stream>>();
        private Solution[] _unorderedSolutions;

        public void Default()
        {
            LoadPreset((Preset)Presets[0]);
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

        #endregion

        #region Accessors

        public Timetable.Timetable Timetable { get; set; }

        public List<Solution> Solutions
        {
            get { return _solutions; }
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

            _solutions.Clear();
            _streamSets.Clear();

            long totalSolutions = 1;
            _solutionHeap = null;
            _unorderedSolutions = new Solution[MaxResults];

            #region Build lists of stream options

            foreach (var type in Timetable.TypeList)
            {
                // if the stream type is ignored, skip
                if (!type.Required)
                {
                    continue;
                }

                // create a list of streams for each type
                var streams = new List<Stream>();

                // stream already enabled? only one option
                if (type.SelectedStream != null)
                {
                    streams.Add(type.SelectedStream);
                }
                else
                {
                    // for each stream in the current type
                    // if stream can't be selected, skip it
                    streams.AddRange(type.UniqueStreams.Where(stream => Timetable.Fits(stream)));
                    // no streams made it - skip
                    if (streams.Count == 0)
                    {
                        return SolverResult.Clash;
                    }
                }

                // add streams to list
                _streamSets.Add(streams);

                // update the complexity of the solution
                totalSolutions *= streams.Count;
            }

            #endregion

            #region Optimise unique stream list order

            // order determined greedily on basis of maximum clashes

            var clashChance = new float[_streamSets.Count, _streamSets.Count];

            // build 2D clash probability table
            for (var i = 0; i < _streamSets.Count; i++)
            {
                var set = _streamSets[i];
                for (var j = i + 1; j < _streamSets.Count; j++)
                {
                    var otherSet = _streamSets[j];
                    var total = set.Count*otherSet.Count;
                    var clash = set.Sum(stream => otherSet.Count(stream.ClashesWith));
                    var chance = clash/(float) total;
                    clashChance[i, j] = chance;
                    clashChance[j, i] = chance;
                }
            }

            // build index lists, extract sets of only one stream
            var ordered = new List<int>();
            var remaining = new List<int>();
            for (var i = 0; i < _streamSets.Count; i++)
            {
                if (_streamSets[i].Count == 1)
                {
                    ordered.Add(i);
                }
                else
                {
                    remaining.Add(i);
                }
            }

            // order by probability of clash with previously selected items
            while (remaining.Count > 0)
            {
                float maxChance = 0;
                float maxFuture = 0;
                var maxIndex = -1;

                foreach (var i in remaining)
                {
                    var chance = ordered.Aggregate(1f, (current, j) => current*(1f - clashChance[i, j]));
                    // find chance of not successively not clashing
                    // chance of clashing with remaining options
                    var future = remaining.Where(j => i != j).Aggregate(1f, (current, j) => current*(1f - clashChance[i, j]));
                    // chance of clashing
                    chance = 1f - chance;
                    future = 1f - future;

                    // check if there's a pair which is more likely to clash
                    var pairMax = (from j in remaining where i != j select clashChance[i, j]).Concat(new[] {0f}).Max();
                    if (pairMax > chance)
                    {
                        chance = pairMax;
                        // give preference to any instant solution
                        future = -1;
                    }

                    if (maxIndex >= 0 && !(chance > maxChance) && (chance != maxChance || !(future > maxFuture)))
                    {
                        continue;
                    }
                    maxIndex = i;
                    maxChance = chance;
                    maxFuture = future;
                }

                remaining.Remove(maxIndex);
                ordered.Add(maxIndex);
            }

            var final = ordered.Select(index => _streamSets[index]).ToList();
            _streamSets = final;

            #endregion

            #region Stack-based algorithm

            var solutionStack = new Stack<Solution>(_streamSets.Count);
            solutionStack.Push(new Solution());
            var solution = new Solution();

            _numberValidSolutions = 0;
            long progress = 0;
            var progressPercent = 0;

            // create an array such that the value at an index gives the number
            // of complete solutions "down that alley" when processing is cut
            // short at that index into the unique streams list
            var cumulativeProduct = new long[_streamSets.Count];
            cumulativeProduct[cumulativeProduct.Length - 1] = 1;
            for (var i = cumulativeProduct.Length - 2; i >= 0; i--)
            {
                cumulativeProduct[i] = cumulativeProduct[i + 1]*_streamSets[i + 1].Count;
            }

            // setIndex represents from which set of streams the next stream is being selected
            var setIndex = 0;
            var streamIndices = new int[_streamSets.Count];
            for (var i = 0; i < streamIndices.Length; i++)
            {
                streamIndices[i] = 0;
            }

            //while (true)
            while (true)
            {
                if (worker.CancellationPending)
                {
                    SortFinalResults();
                    e.Cancel = true;
                    return SolverResult.UserCancel;
                }

                // check if the stream index is out of range
                // (done all options for current set of streams)
                if (streamIndices[setIndex] == _streamSets[setIndex].Count)
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
                if (!solution.AddStream(_streamSets[setIndex][streamIndices[setIndex]]))
                {
                    // add to progress the number of solutions just skipped
                    progress += cumulativeProduct[setIndex];
                    var percent = (int) ((float) progress/totalSolutions*100);
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
                if (setIndex == _streamSets.Count - 1)
                {
                    // increment progress
                    progress++;
                    var percent = (int) ((float) progress/totalSolutions*100);
                    if (percent > progressPercent)
                    {
                        worker.ReportProgress(percent);
                        progressPercent = percent;
                    }

                    var filteredOut = Filters.Any(filter => !filter.Pass(solution));

                    if (!filteredOut)
                    {
                        _numberValidSolutions++;

                        // if the solution list is full to capacity
                        if (_numberValidSolutions > MaxResults)
                        {
                            // if the solution is in the top so far, take the place of the worst solution
                            _solutionHeap.CompareReplaceMaximum(solution);
                        }
                        else
                        {
                            // append the solution to the array
                            _unorderedSolutions[_numberValidSolutions - 1] = solution;
                            // if the unordered list just filled up
                            if (_numberValidSolutions == MaxResults)
                            {
                                // build the heap from the array
                                _solutionHeap = new HeapWithComparer<Solution>(MaxResults, _unorderedSolutions,
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
            if (_numberValidSolutions < MaxResults)
                _solutionHeap = new HeapWithComparer<Solution>(MaxResults, _unorderedSolutions, Comparer);

            // run heapsort on the top solutions, copy into solution list
            _solutions.AddRange(_solutionHeap.GetSorted());
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
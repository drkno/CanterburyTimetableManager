#region

using System.Collections.Generic;

#endregion

namespace UniTimetable.Model.Solver
{
    partial class Solver
    {
        public class SolutionComparer : IComparer<Solution>
        {
            /*public void Default()
            {
                LoadCriteria(Presets[2]);
            }*/

            #region IComparer<Solution> Members

            public int Compare(Solution x, Solution y)
            {
                // for each field
                for (int i = 0; i < Criteria.Count; i++)
                {
                    // no sort order for current field - skip
                    if (Criteria[i].Preference == Preference.None)
                        continue;
                    // compare corresponding fields
                    int result = x.CompareFieldValueTo(Criteria[i].FieldIndex, y);
                    // no difference - go to next criteria
                    if (result == 0)
                        continue;
                    // descending order - negate result
                    if (Criteria[i].Preference == Preference.Maximise)
                        result = -result;
                    return result;
                }
                // couldn't find a difference in all the criteria
                return 0;
            }

            #endregion

            public void Add(FieldIndex field, Preference preference)
            {
                Criteria.Add(new Criteria(field, preference));
            }

            public void Clear()
            {
                Criteria.Clear();
            }

            public void RemoveAt(int index)
            {
                Criteria.RemoveAt(index);
            }

            public void Insert(int index, FieldIndex field, Preference preference)
            {
                Criteria.Insert(index, new Criteria(field, preference));
            }

            public void Swap(int index1, int index2)
            {
                Criteria temp = Criteria[index1];
                Criteria[index1] = Criteria[index2];
                Criteria[index2] = temp;
            }

            public SolutionComparer ShallowClone()
            {
                SolutionComparer clone = new SolutionComparer();
                clone.Criteria.AddRange(Criteria);
                return clone;
            }

            /*public void LoadCriteria(Preset preset)
            {
                LoadCriteria(preset.Criteria);
            }*/

            public void LoadCriteria(IEnumerable<Criteria> criteria)
            {
                Criteria.Clear();
                Criteria.AddRange(criteria);
            }

            #region Constructors

            public SolutionComparer()
            {
                Criteria = new List<Criteria>();
            }

            public SolutionComparer(SolutionComparer other)
            {
                // clone criteria list
                Criteria = new List<Criteria>();
                foreach (Criteria criteria in other.Criteria)
                {
                    Criteria.Add(criteria.Clone());
                }
            }

            public SolutionComparer Clone()
            {
                return new SolutionComparer(this);
            }

            #endregion

            #region Accessors

            public List<Criteria> Criteria { get; private set; }

            public bool ContainsField(FieldIndex fieldIndex)
            {
                foreach (Criteria criteria in Criteria)
                {
                    if (criteria.FieldIndex == fieldIndex)
                        return true;
                }
                return false;
            }

            #endregion
        }
    }
}
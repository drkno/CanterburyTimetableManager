using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UniTimetable
{
    partial class Solver
    {
        public class SolutionComparer : IComparer<Solution>
        {
            List<Criteria> Criteria_;

            #region Constructors

            public SolutionComparer()
            {
                Criteria_ = new List<Criteria>();
            }

            public SolutionComparer(SolutionComparer other)
            {
                // clone criteria list
                this.Criteria_ = new List<Criteria>();
                foreach (Criteria criteria in other.Criteria_)
                {
                    this.Criteria_.Add(criteria.Clone());
                }
            }

            public SolutionComparer Clone()
            {
                return new SolutionComparer(this);
            }

            #endregion

            #region Accessors

            public List<Criteria> Criteria
            {
                get
                {
                    return Criteria_;
                }
            }

            public bool ContainsField(FieldIndex fieldIndex)
            {
                foreach (Criteria criteria in Criteria_)
                {
                    if (criteria.FieldIndex == fieldIndex)
                        return true;
                }
                return false;
            }

            #endregion

            public void Add(FieldIndex field, Preference preference)
            {
                Criteria_.Add(new Criteria(field, preference));
            }

            public void Clear()
            {
                Criteria_.Clear();
            }

            public void RemoveAt(int index)
            {
                Criteria_.RemoveAt(index);
            }

            public void Insert(int index, FieldIndex field, Preference preference)
            {
                Criteria_.Insert(index, new Criteria(field, preference));
            }

            public void Swap(int index1, int index2)
            {
                Criteria temp = Criteria_[index1];
                Criteria_[index1] = Criteria_[index2];
                Criteria_[index2] = temp;
            }

            public SolutionComparer ShallowClone()
            {
                SolutionComparer clone = new SolutionComparer();
                clone.Criteria_.AddRange(Criteria_);
                return clone;
            }

            /*public void LoadCriteria(Preset preset)
            {
                LoadCriteria(preset.Criteria);
            }*/

            public void LoadCriteria(IEnumerable<Criteria> criteria)
            {
                Criteria_.Clear();
                Criteria_.AddRange(criteria);
            }

            /*public void Default()
            {
                LoadCriteria(Presets[2]);
            }*/

            #region IComparer<Solution> Members

            public int Compare(Solution x, Solution y)
            {
                // for each field
                for (int i = 0; i < Criteria_.Count; i++)
                {
                    // no sort order for current field - skip
                    if (Criteria_[i].Preference == Preference.None)
                        continue;
                    // compare corresponding fields
                    int result = x.CompareFieldValueTo(Criteria_[i].FieldIndex, y);
                    // no difference - go to next criteria
                    if (result == 0)
                        continue;
                    // descending order - negate result
                    if (Criteria_[i].Preference == Preference.Maximise)
                        result = -result;
                    return result;
                }
                // couldn't find a difference in all the criteria
                return 0;
            }

            #endregion
        }
    }
}

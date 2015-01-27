using System;
using System.Collections.Generic;
using System.Text;

namespace UniTimetable
{
    partial class Solver
    {
        public class Criteria
        {
            Solver.FieldIndex FieldIndex_;
            Solver.Preference Preference_;

            static readonly string[][] fieldPrefs = new string[][] {
                // TimeOfDay
                new string[] {"None", "Early", "Late"},
                // TimeLength
                new string[] {"None", "Short", "Long"},
                // Int
                new string[] {"None", "Less", "More"}
            };

            public static string FieldSpecificPreference(FieldType field, Preference preference)
            {
                if (field == FieldType.Unknown)
                    return "";
                return fieldPrefs[(int)field][(int)preference];
            }

            public static string FieldSpecificPreference(Criteria criteria)
            {
                return FieldSpecificPreference(criteria.Field.Type, criteria.Preference);
            }

            public static string[] FieldSpecificPreferences(FieldType field)
            {
                if (field == FieldType.Unknown)
                    return new string[] { };
                return fieldPrefs[(int)field];
            }

            public static string[] FieldSpecificPreferences(Criteria criteria)
            {
                return FieldSpecificPreferences(criteria.Field.Type);
            }

            #region Constructors

            public Criteria()
                : this(Solver.FieldIndex.Days) { }

            public Criteria(FieldIndex fieldIndex)
            {
                FieldIndex_ = fieldIndex;
                Preference_ = Fields[(int)FieldIndex_].DefaultPreference;
            }

            public Criteria(FieldIndex fieldIndex, Preference preference)
            {
                FieldIndex_ = fieldIndex;
                Preference_ = preference;
            }

            public Criteria(Criteria other)
            {
                this.FieldIndex_ = other.FieldIndex_;
                this.Preference_ = other.Preference_;
            }

            public Criteria Clone()
            {
                return new Criteria(this);
            }

            public Criteria DeepCopy()
            {
                return new Criteria(this);
            }

            #endregion

            #region Accessors

            /// <summary>
            /// Gets or sets the field index.
            /// </summary>
            public FieldIndex FieldIndex
            {
                get
                {
                    return FieldIndex_;
                }
                set
                {
                    FieldIndex_ = value;
                }
            }

            /// <summary>
            /// Gets the field.
            /// </summary>
            public Field Field
            {
                get
                {
                    return Fields[(int)FieldIndex_];
                }
            }

            /// <summary>
            /// Gets or sets the criteria preference.
            /// </summary>
            public Preference Preference
            {
                get
                {
                    return Preference_;
                }
                set
                {
                    Preference_ = value;
                }
            }

            #endregion

            public override string ToString()
            {
                return Field.ToString();
            }
        }

        // compatible with SortOrder (None, Ascending, Descending)
        public enum Preference
        {
            None = 0,
            Minimise = 1,
            Maximise = 2
        }
    }
}

namespace UniTimetable.Model.Solver
{
    partial class Solver
    {
        // compatible with SortOrder (None, Ascending, Descending)
        public enum Preference
        {
            None = 0,
            Minimise = 1,
            Maximise = 2
        }

        public class Criteria
        {
            private static readonly string[][] FieldPrefs =
            {
                // TimeOfDay
                new[] {"None", "Early", "Late"},
                // TimeLength
                new[] {"None", "Short", "Long"},
                // Int
                new[] {"None", "Less", "More"}
            };

            private static string FieldSpecificPreference(FieldType field, Preference preference)
            {
                return field == FieldType.Unknown ? "" : FieldPrefs[(int) field][(int) preference];
            }

            public static string FieldSpecificPreference(Criteria criteria)
            {
                return FieldSpecificPreference(criteria.Field.Type, criteria.Preference);
            }

            public static string[] FieldSpecificPreferences(FieldType field)
            {
                if (field == FieldType.Unknown)
                    return new string[] {};
                return FieldPrefs[(int) field];
            }

            public static string[] FieldSpecificPreferences(Criteria criteria)
            {
                return FieldSpecificPreferences(criteria.Field.Type);
            }

            public override string ToString()
            {
                return Field.ToString();
            }

            #region Constructors

            public Criteria()
                : this(FieldIndex.Days)
            {
            }

            public Criteria(FieldIndex fieldIndex)
            {
                FieldIndex = fieldIndex;
                Preference = Fields[(int) FieldIndex].DefaultPreference;
            }

            public Criteria(FieldIndex fieldIndex, Preference preference)
            {
                FieldIndex = fieldIndex;
                Preference = preference;
            }

            public Criteria(Criteria other)
            {
                FieldIndex = other.FieldIndex;
                Preference = other.Preference;
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
            ///     Gets or sets the field index.
            /// </summary>
            public FieldIndex FieldIndex { get; set; }

            /// <summary>
            ///     Gets the field.
            /// </summary>
            public Field Field
            {
                get { return Fields[(int) FieldIndex]; }
            }

            /// <summary>
            ///     Gets or sets the criteria preference.
            /// </summary>
            public Preference Preference { get; set; }

            #endregion
        }
    }
}
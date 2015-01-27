using UniTimetable.Model.Time;

namespace UniTimetable.Model.Solver
{
    partial class Solver
    {
        public enum FilterTest
        {
            LessThan,
            GreaterThan,
            EqualTo
        }

        public class Filter
        {
            public static readonly string[] TestNames = {"<", ">", "="};

            private static readonly string[][] fieldTest =
            {
                // TimeOfDay
                new[] {"earlier than", "later than", "exactly"},
                // TimeLength
                new[] {"shorter than", "longer than", "exactly"},
                // Int
                new[] {"less than", "more than", "exactly"}
            };

            public static string FieldSpecificTest(FieldType field, FilterTest test)
            {
                if (field == FieldType.Unknown)
                    return "";
                return fieldTest[(int) field][(int) test];
            }

            public static string FieldSpecificTest(Filter filter)
            {
                return FieldSpecificTest(filter.Field.Type, filter.Test);
            }

            public static string[] FieldSpecificTests(FieldType field)
            {
                if (field == FieldType.Unknown)
                    return new string[] {};
                return fieldTest[(int) field];
            }

            public static string[] FieldSpecificTests(Filter filter)
            {
                return FieldSpecificTests(filter.Field.Type);
            }

            public string ValueToString()
            {
                switch (Field.Type)
                {
                    case FieldType.Int:
                        return ValueAsInt.ToString();

                    case FieldType.TimeLength:
                        return ValueAsTimeLength.ToString();

                    case FieldType.TimeOfDay:
                        return ValueAsTimeOfDay.ToString();

                    default:
                        return "";
                }
            }

            public bool Pass(Solution solution)
            {
                bool result;
                switch (Test)
                {
                    case FilterTest.LessThan:
                        result = solution.FieldValueToInt(FieldIndex) < ValueAsInt;
                        break;

                    case FilterTest.GreaterThan:
                        result = solution.FieldValueToInt(FieldIndex) > ValueAsInt;
                        break;

                    case FilterTest.EqualTo:
                        result = solution.FieldValueToInt(FieldIndex) == ValueAsInt;
                        break;

                    default:
                        result = true;
                        break;
                }
                if (Exclude)
                    result = !result;
                return result;
            }

            public override string ToString()
            {
                return Field.ToString();
            }

            #region Constructors

            public Filter()
            {
                // defaults
                Exclude = true;
                FieldIndex = FieldIndex.Days;
                Test = FilterTest.GreaterThan;
                ValueAsInt = 5;
            }

            public Filter(Filter other)
            {
                Exclude = other.Exclude;
                FieldIndex = other.FieldIndex;
                ValueAsInt = other.ValueAsInt;
                Test = other.Test;
            }

            public Filter(bool exclude, FieldIndex fieldIndex, TimeLength value, FilterTest test)
            {
                Exclude = exclude;
                FieldIndex = fieldIndex;
                ValueAsInt = value.TotalMinutes;
                Test = test;
            }

            public Filter(bool exclude, FieldIndex fieldIndex, TimeOfDay value, FilterTest test)
            {
                Exclude = exclude;
                FieldIndex = fieldIndex;
                ValueAsInt = value.DayMinutes;
                Test = test;
            }

            public Filter(bool exclude, FieldIndex fieldIndex, int value, FilterTest test)
            {
                Exclude = exclude;
                FieldIndex = fieldIndex;
                ValueAsInt = value;
                Test = test;
            }

            public Filter DeepCopy()
            {
                return new Filter(this);
            }

            #endregion

            #region Accessors

            public bool Exclude { get; private set; }

            public FieldIndex FieldIndex { get; private set; }

            public Field Field
            {
                get { return Fields[(int) FieldIndex]; }
            }

            public int ValueAsInt { get; private set; }

            public TimeLength ValueAsTimeLength
            {
                get { return new TimeLength(ValueAsInt); }
            }

            public TimeOfDay ValueAsTimeOfDay
            {
                get { return new TimeOfDay(ValueAsInt); }
            }

            public FilterTest Test { get; private set; }

            #endregion
        }
    }
}
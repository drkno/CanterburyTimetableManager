using System;
using System.Collections.Generic;
using System.Text;

namespace UniTimetable
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
            bool Exclude_;
            FieldIndex FieldIndex_;
            FilterTest Test_;
            int Value_;
            
            public static readonly string[] TestNames = new string[] { "<", ">", "=" };

            static readonly string[][] fieldTest = new string[][] {
                // TimeOfDay
                new string[] {"earlier than", "later than", "exactly"},
                // TimeLength
                new string[] {"shorter than", "longer than", "exactly"},
                // Int
                new string[] {"less than", "more than", "exactly"}
            };

            public static string FieldSpecificTest(FieldType field, FilterTest test)
            {
                if (field == FieldType.Unknown)
                    return "";
                return fieldTest[(int)field][(int)test];
            }

            public static string FieldSpecificTest(Filter filter)
            {
                return FieldSpecificTest(filter.Field.Type, filter.Test);
            }

            public static string[] FieldSpecificTests(FieldType field)
            {
                if (field == FieldType.Unknown)
                    return new string[] { };
                return fieldTest[(int)field];
            }

            public static string[] FieldSpecificTests(Filter filter)
            {
                return FieldSpecificTests(filter.Field.Type);
            }

            #region Constructors

            public Filter()
            {
                // defaults
                Exclude_ = true;
                FieldIndex_ = Solver.FieldIndex.Days;
                Test_ = FilterTest.GreaterThan;
                Value_ = 5;
            }

            public Filter(Filter other)
            {
                this.Exclude_ = other.Exclude_;
                this.FieldIndex_ = other.FieldIndex_;
                this.Value_ = other.Value_;
                this.Test_ = other.Test_;
            }

            public Filter(bool exclude, FieldIndex fieldIndex, TimeLength value, FilterTest test)
            {
                Exclude_ = exclude;
                FieldIndex_ = fieldIndex;
                Value_ = value.TotalMinutes;
                Test_ = test;
            }

            public Filter(bool exclude, FieldIndex fieldIndex, TimeOfDay value, FilterTest test)
            {
                Exclude_ = exclude;
                FieldIndex_ = fieldIndex;
                Value_ = value.DayMinutes;
                Test_ = test;
            }

            public Filter(bool exclude, FieldIndex fieldIndex, int value, FilterTest test)
            {
                Exclude_ = exclude;
                FieldIndex_ = fieldIndex;
                Value_ = value;
                Test_ = test;
            }

            public Filter DeepCopy()
            {
                return new Filter(this);
            }

            #endregion

            #region Accessors

            public bool Exclude
            {
                get
                {
                    return Exclude_;
                }
            }

            public FieldIndex FieldIndex
            {
                get
                {
                    return FieldIndex_;
                }
            }

            public Field Field
            {
                get
                {
                    return Fields[(int)FieldIndex_];
                }
            }

            public int ValueAsInt
            {
                get
                {
                    return Value_;
                }
            }

            public TimeLength ValueAsTimeLength
            {
                get
                {
                    return new TimeLength(Value_);
                }
            }

            public TimeOfDay ValueAsTimeOfDay
            {
                get
                {
                    return new TimeOfDay(Value_);
                }
            }

            public FilterTest Test
            {
                get
                {
                    return Test_;
                }
            }

            #endregion

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
                switch (Test_)
                {
                    case FilterTest.LessThan:
                        result = solution.FieldValueToInt(FieldIndex_) < Value_;
                        break;

                    case FilterTest.GreaterThan:
                        result = solution.FieldValueToInt(FieldIndex_) > Value_;
                        break;

                    case FilterTest.EqualTo:
                        result = solution.FieldValueToInt(FieldIndex_) == Value_;
                        break;

                    default:
                        result = true;
                        break;
                }
                if (Exclude_)
                    result = !result;
                return result;
            }

            public override string ToString()
            {
                return Field.ToString();
            }
        }
    }
}

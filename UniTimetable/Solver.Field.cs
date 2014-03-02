using System;
using System.Collections.Generic;
using System.Text;

namespace UniTimetable
{
	partial class Solver
	{
        public static Field[] Fields = new Field[] {
            new Field("Time at Uni", FieldIndex.TimeAtUni, FieldType.TimeLength, Preference.Minimise, FieldProgression.Grow),
            new Field("Time in Classes", FieldIndex.TimeInClasses, FieldType.TimeLength, Preference.Minimise, FieldProgression.Grow),
            new Field("Time in Breaks", FieldIndex.TimeInBreaks, FieldType.TimeLength, Preference.Minimise, FieldProgression.Unknown),
            new Field("Number of Days", FieldIndex.Days, FieldType.Int, Preference.Minimise, FieldProgression.Grow),

            new Field("Shortest Day", FieldIndex.MinDayLength, FieldType.TimeLength, Preference.Maximise, FieldProgression.Shrink),
            new Field("Longest Day", FieldIndex.MaxDayLength, FieldType.TimeLength, Preference.Minimise, FieldProgression.Grow),
            new Field("Average Day", FieldIndex.AverageDayLength, FieldType.TimeLength, Preference.Minimise, FieldProgression.Unknown),

            new Field("Shortest Break", FieldIndex.ShortBreak, FieldType.TimeLength, Preference.None, FieldProgression.Shrink),
            new Field("Longest Break", FieldIndex.LongBreak, FieldType.TimeLength, Preference.Minimise, FieldProgression.Grow),
            new Field("Average Break", FieldIndex.AverageBreak, FieldType.TimeLength, Preference.Minimise, FieldProgression.Unknown),
            new Field("Number of Breaks", FieldIndex.NumberBreaks, FieldType.Int, Preference.Minimise, FieldProgression.Grow),

            new Field("Shortest Block", FieldIndex.ShortBlock, FieldType.TimeLength, Preference.Maximise, FieldProgression.Shrink),
            new Field("Longest Block", FieldIndex.LongBlock, FieldType.TimeLength, Preference.Minimise, FieldProgression.Grow),
            new Field("Average Block", FieldIndex.AverageBlock, FieldType.TimeLength, Preference.None, FieldProgression.Unknown),
            new Field("Number of Blocks", FieldIndex.NumberBlocks, FieldType.Int, Preference.Minimise, FieldProgression.Unknown),

            new Field("Earliest Start", FieldIndex.EarlyStart, FieldType.TimeOfDay, Preference.Maximise, FieldProgression.Shrink),
            new Field("Latest Start", FieldIndex.LateStart, FieldType.TimeOfDay, Preference.Maximise, FieldProgression.Grow),
            new Field("Average Start", FieldIndex.AverageStart, FieldType.TimeOfDay, Preference.Maximise, FieldProgression.Unknown),

            new Field("Earliest Finish", FieldIndex.EarlyEnd, FieldType.TimeOfDay, Preference.Minimise, FieldProgression.Shrink),
            new Field("Latest Finish", FieldIndex.LateEnd, FieldType.TimeOfDay, Preference.Minimise, FieldProgression.Grow),
            new Field("Average Finish", FieldIndex.AverageEnd, FieldType.TimeOfDay, Preference.Minimise, FieldProgression.Unknown)
        };

        public enum FieldIndex
        {
            TimeAtUni = 0,
            TimeInClasses = 1,
            TimeInBreaks = 2,
            Days = 3,

            MinDayLength = 4,
            MaxDayLength = 5,
            AverageDayLength = 6,

            ShortBreak = 7,
            LongBreak = 8,
            AverageBreak = 9,
            NumberBreaks = 10,

            ShortBlock = 11,
            LongBlock = 12,
            AverageBlock = 13,
            NumberBlocks = 14,

            EarlyStart = 15,
            LateStart = 16,
            AverageStart = 17,

            EarlyEnd = 18,
            LateEnd = 19,
            AverageEnd = 20
        }

        public class Field
        {
            /*public static string[] Names = new string[] {
                "Time at Uni", "Time in Classes", "Time in Breaks", "Days",
                "Shortest Day", "Longest Day", "Avg. Day Length",
                "Shortest Break", "Longest Break", "Average Break", "Number of Breaks",
                "Shortest Block", "Longest Block", "Average Block", "Number of Blocks",
                "Earliest Start", "Latest Start", "Average Start",
                "Earliest Finish", "Latest Finish", "Average Finish" };*/

            string Name_;
            FieldIndex Index_;
            FieldType Type_;
            FieldProgression Progression_;
            Preference DefaultPreference_;

            #region Constructors

            public Field(string name, FieldIndex index, FieldType type)
            {
                Name_ = name;
                Index_ = index;
                Type_ = type;
                DefaultPreference_ = Preference.None;
                Progression_ = FieldProgression.Unknown;
            }

            public Field(string name, FieldIndex index, FieldType type, Preference preference)
            {
                Name_ = name;
                Index_ = index;
                Type_ = type;
                DefaultPreference_ = preference;
                Progression_ = FieldProgression.Unknown;
            }

            public Field(string name, FieldIndex index, FieldType type, FieldProgression progression)
            {
                Name_ = name;
                Index_ = index;
                Type_ = type;
                DefaultPreference_ = Preference.None;
                Progression_ = progression;
            }

            public Field(string name, FieldIndex index, FieldType type, Preference preference, FieldProgression progression)
            {
                Name_ = name;
                Index_ = index;
                Type_ = type;
                DefaultPreference_ = preference;
                Progression_ = progression;
            }

            #endregion

            #region Accessors

            public string Name
            {
                get
                {
                    return Name_;
                }
            }

            public FieldIndex Index
            {
                get
                {
                    return Index_;
                }
            }

            public FieldType Type
            {
                get
                {
                    return Type_;
                }
            }

            public Preference DefaultPreference
            {
                get
                {
                    return DefaultPreference_;
                }
            }

            public FieldProgression Progression
            {
                get
                {
                    return Progression_;
                }
            }

            #endregion

            public override string ToString()
            {
                return Name;
            }
        }

        // to be used for part-way exclusion of solutions
        public enum FieldProgression
        {
            Unknown = 0,
            Grow = 1,
            Shrink = 2
        }

        public enum FieldType
        {
            Unknown = -1,
            TimeOfDay = 0,
            TimeLength = 1,
            Int = 2
        }
	}
}

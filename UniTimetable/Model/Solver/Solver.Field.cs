namespace UniTimetable.Model.Solver
{
    partial class Solver
    {
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

        public static Field[] Fields =
        {
            new Field("Time at Uni", FieldIndex.TimeAtUni, FieldType.TimeLength, Preference.Minimise,
                FieldProgression.Grow),
            new Field("Time in Classes", FieldIndex.TimeInClasses, FieldType.TimeLength, Preference.Minimise,
                FieldProgression.Grow),
            new Field("Time in Breaks", FieldIndex.TimeInBreaks, FieldType.TimeLength, Preference.Minimise,
                FieldProgression.Unknown),
            new Field("Number of Days", FieldIndex.Days, FieldType.Int, Preference.Minimise, FieldProgression.Grow),
            new Field("Shortest Day", FieldIndex.MinDayLength, FieldType.TimeLength, Preference.Maximise,
                FieldProgression.Shrink),
            new Field("Longest Day", FieldIndex.MaxDayLength, FieldType.TimeLength, Preference.Minimise,
                FieldProgression.Grow),
            new Field("Average Day", FieldIndex.AverageDayLength, FieldType.TimeLength, Preference.Minimise,
                FieldProgression.Unknown),
            new Field("Shortest Break", FieldIndex.ShortBreak, FieldType.TimeLength, Preference.None,
                FieldProgression.Shrink),
            new Field("Longest Break", FieldIndex.LongBreak, FieldType.TimeLength, Preference.Minimise,
                FieldProgression.Grow),
            new Field("Average Break", FieldIndex.AverageBreak, FieldType.TimeLength, Preference.Minimise,
                FieldProgression.Unknown),
            new Field("Number of Breaks", FieldIndex.NumberBreaks, FieldType.Int, Preference.Minimise,
                FieldProgression.Grow),
            new Field("Shortest Block", FieldIndex.ShortBlock, FieldType.TimeLength, Preference.Maximise,
                FieldProgression.Shrink),
            new Field("Longest Block", FieldIndex.LongBlock, FieldType.TimeLength, Preference.Minimise,
                FieldProgression.Grow),
            new Field("Average Block", FieldIndex.AverageBlock, FieldType.TimeLength, Preference.None,
                FieldProgression.Unknown),
            new Field("Number of Blocks", FieldIndex.NumberBlocks, FieldType.Int, Preference.Minimise,
                FieldProgression.Unknown),
            new Field("Earliest Start", FieldIndex.EarlyStart, FieldType.TimeOfDay, Preference.Maximise,
                FieldProgression.Shrink),
            new Field("Latest Start", FieldIndex.LateStart, FieldType.TimeOfDay, Preference.Maximise,
                FieldProgression.Grow),
            new Field("Average Start", FieldIndex.AverageStart, FieldType.TimeOfDay, Preference.Maximise,
                FieldProgression.Unknown),
            new Field("Earliest Finish", FieldIndex.EarlyEnd, FieldType.TimeOfDay, Preference.Minimise,
                FieldProgression.Shrink),
            new Field("Latest Finish", FieldIndex.LateEnd, FieldType.TimeOfDay, Preference.Minimise,
                FieldProgression.Grow),
            new Field("Average Finish", FieldIndex.AverageEnd, FieldType.TimeOfDay, Preference.Minimise,
                FieldProgression.Unknown)
        };

        public class Field
        {
            public override string ToString()
            {
                return Name;
            }

            /*public static string[] Names = new string[] {
                "Time at Uni", "Time in Classes", "Time in Breaks", "Days",
                "Shortest Day", "Longest Day", "Avg. Day Length",
                "Shortest Break", "Longest Break", "Average Break", "Number of Breaks",
                "Shortest Block", "Longest Block", "Average Block", "Number of Blocks",
                "Earliest Start", "Latest Start", "Average Start",
                "Earliest Finish", "Latest Finish", "Average Finish" };*/

            #region Constructors

            public Field(string name, FieldIndex index, FieldType type)
            {
                Name = name;
                Index = index;
                Type = type;
                DefaultPreference = Preference.None;
                Progression = FieldProgression.Unknown;
            }

            public Field(string name, FieldIndex index, FieldType type, Preference preference)
            {
                Name = name;
                Index = index;
                Type = type;
                DefaultPreference = preference;
                Progression = FieldProgression.Unknown;
            }

            public Field(string name, FieldIndex index, FieldType type, FieldProgression progression)
            {
                Name = name;
                Index = index;
                Type = type;
                DefaultPreference = Preference.None;
                Progression = progression;
            }

            public Field(string name, FieldIndex index, FieldType type, Preference preference,
                FieldProgression progression)
            {
                Name = name;
                Index = index;
                Type = type;
                DefaultPreference = preference;
                Progression = progression;
            }

            #endregion

            #region Accessors

            public string Name { get; private set; }

            public FieldIndex Index { get; private set; }

            public FieldType Type { get; private set; }

            public Preference DefaultPreference { get; private set; }

            public FieldProgression Progression { get; private set; }

            #endregion
        }
    }
}
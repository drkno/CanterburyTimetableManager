#region

using System.Collections.Generic;

#endregion

namespace UniTimetable.Model.Solver
{
    partial class Solver
    {
        public static readonly Preset[] Presets =
        {
            /*new Preset(
                    "Test",
                    new Criteria[] {
                        new Criteria(FieldIndex.Days, Preference.Minimise),
                        new Criteria(FieldIndex.LongBreak, Preference.Minimise),
                        new Criteria(FieldIndex.EarlyStart, Preference.Maximise),
                        new Criteria(FieldIndex.TimeAtUni, Preference.None),
                        new Criteria(FieldIndex.AverageStart, Preference.Maximise),
                        new Criteria(FieldIndex.MaxDayLength, Preference.Minimise) },
                    new Filter[] {
                        new Filter(false, FieldIndex.AverageStart, new TimeOfDay(10, 30), FilterTest.GreaterThan),
                        new Filter(true, FieldIndex.LongBreak, new TimeLength(3, 00), FilterTest.EqualTo) }),*/
            new Preset(
                "Default",
                new[]
                {
                    new Criteria(FieldIndex.Days, Preference.Minimise),
                    new Criteria(FieldIndex.EarlyStart, Preference.Maximise),
                    new Criteria(FieldIndex.TimeAtUni, Preference.Minimise),
                    new Criteria(FieldIndex.LongBreak, Preference.Minimise),
                    new Criteria(FieldIndex.AverageStart, Preference.Maximise),
                    new Criteria(FieldIndex.MaxDayLength, Preference.Minimise)
                },
                new Filter[] {}),
            new Preset(
                "No Early Mornings!",
                new[]
                {
                    new Criteria(FieldIndex.EarlyStart, Preference.Maximise),
                    new Criteria(FieldIndex.AverageStart, Preference.Maximise),
                    new Criteria(FieldIndex.Days, Preference.Minimise)
                },
                new Filter[] {})
        };

        public class Preset
        {
            private readonly string Name_;

            public override string ToString()
            {
                return Name_;
            }

            #region Constructors

            public Preset(string name)
            {
                Name_ = name;
                Criteria = new List<Criteria>();
                Filters = new List<Filter>();
            }

            public Preset(string name, IEnumerable<Criteria> criteria, IEnumerable<Filter> filters)
            {
                Name_ = name;
                // criteria list
                if (criteria == null)
                    Criteria = new List<Criteria>();
                else
                    Criteria = new List<Criteria>(criteria);
                // filter list
                if (filters == null)
                    Filters = new List<Filter>();
                else
                    Filters = new List<Filter>(filters);
            }

            #endregion

            #region Accessors

            public List<Criteria> Criteria { get; private set; }

            public List<Filter> Filters { get; private set; }

            #endregion
        }
    }
}
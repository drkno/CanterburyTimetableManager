#region

using System.Collections.Generic;

#endregion

namespace UniTimetable.Model.Solver
{
    partial class Solver
    {
        public static readonly object[] Presets =
        {
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
            private readonly string _name;

            public override string ToString()
            {
                return _name;
            }

            #region Constructors

            public Preset(string name, IEnumerable<Criteria> criteria, IEnumerable<Filter> filters)
            {
                _name = name;
                // criteria list
                Criteria = criteria == null ? new List<Criteria>() : new List<Criteria>(criteria);
                // filter list
                Filters = filters == null ? new List<Filter>() : new List<Filter>(filters);
            }

            #endregion

            #region Accessors

            public List<Criteria> Criteria { get; private set; }

            public List<Filter> Filters { get; private set; }

            #endregion
        }
    }
}
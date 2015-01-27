using System;
using System.Collections.Generic;
using System.Text;

namespace UniTimetable
{
    partial class Solver
    {
        public class Preset
        {
            string Name_;
            List<Criteria> Criteria_;
            List<Filter> Filters_;

            #region Constructors

            public Preset(string name)
            {
                Name_ = name;
                Criteria_ = new List<Criteria>();
                Filters_ = new List<Filter>();
            }

            public Preset(string name, IEnumerable<Criteria> criteria, IEnumerable<Filter> filters)
            {
                Name_ = name;
                // criteria list
                if (criteria == null)
                    Criteria_ = new List<Criteria>();
                else
                    Criteria_ = new List<Criteria>(criteria);
                // filter list
                if (filters == null)
                    Filters_ = new List<Filter>();
                else
                    Filters_ = new List<Filter>(filters);
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

            public List<Filter> Filters
            {
                get
                {
                    return Filters_;
                }
            }

            #endregion

            public override string ToString()
            {
                return Name_;
            }
        }

        public static readonly Preset[] Presets = new Preset[] {
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
                    new Criteria[] {
                        new Criteria(FieldIndex.Days, Preference.Minimise),
                        new Criteria(FieldIndex.EarlyStart, Preference.Maximise),
                        new Criteria(FieldIndex.TimeAtUni, Preference.Minimise),
                        new Criteria(FieldIndex.LongBreak, Preference.Minimise),
                        new Criteria(FieldIndex.AverageStart, Preference.Maximise),
                        new Criteria(FieldIndex.MaxDayLength, Preference.Minimise) },
                    new Filter[] {}),
                new Preset(
                    "No Early Mornings!",
                    new Criteria[] {
                        new Criteria(FieldIndex.EarlyStart, Preference.Maximise),
                        new Criteria(FieldIndex.AverageStart, Preference.Maximise),
                        new Criteria(FieldIndex.Days, Preference.Minimise) },
                    new Filter[] {})
            };
    }
}

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury.JsonObjects
{
    [DataContract]
    public class SubjectStream
    {
        [DataMember(Name = "subject_code")]
        public string SubjectCode { get;set; }

        [DataMember(Name = "activity_group_code")]
        public string ActivityGroupCode { get;set; }

        [DataMember(Name = "activity_code")]
        public string ActivityCode { get;set; }

        [DataMember(Name = "campus")]
        public string Campus { get;set; }

        [DataMember(Name = "day_of_week")]
        public string DayOfWeek { get;set; }

        [DataMember(Name = "start_time")]
        public string StartTime { get; set; }

        [DataMember(Name = "location")]
        public string Location { get;set; }

        [DataMember(Name = "staff")]
        public string Staff { get;set; }

        [DataMember(Name = "duration")]
        public string Duration { get;set; }

        [DataMember(Name = "selectable")]
        public string Selectable { get;set; }

        [DataMember(Name = "availability")]
        public int Availability { get;set; }

        [DataMember(Name = "week_pattern")]
        public string WeekPattern { get;set; }

        [DataMember(Name = "description")]
        public string Description { get;set; }

        [DataMember(Name = "zone")]
        public string Zone { get;set; }

        [DataMember(Name = "department")]
        public string Department { get;set; }

        [DataMember(Name = "semester")]
        public string Semester { get;set; }

        [DataMember(Name = "activity_type")]
        public string ActivityType { get;set; }

        [DataMember(Name = "message")]
        public string Message { get;set; }

        [DataMember(Name = "start_date")]
        public string StartDate { get; set; }

        [DataMember(Name = "color")]
        public string Colour { get;set; }

        [DataMember(Name = "lat")]
        public string Latitude { get;set; }

        [DataMember(Name = "lng")]
        public string Longitude { get;set; }

        public DateTime Date
        {
            get
            {
                return DateTime.ParseExact(StartDate + " " + StartTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
        }
    }
}

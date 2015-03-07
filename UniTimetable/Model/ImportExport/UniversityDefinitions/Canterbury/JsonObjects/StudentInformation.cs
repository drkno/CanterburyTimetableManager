using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury.JsonObjects
{
    [DataContract]
    public class Student
    {
        public static Student GetStudent(string data)
        {
            var index = data.IndexOf(':') + 1;
            var last = data.LastIndexOf('}');
            data = data.Substring(index, last - index);
            data = Regex.Replace(data, "\"[A-Z]{4}[0-9]{3}-[0-9]{2}[^\"]+\":", "");
            data = Regex.Replace(data, "\"[A-Z][a-z]+[A-Z]\":[^{]*", "");
            data = Regex.Replace(data, ":{\\s*{", ":[{");
            data = Regex.Replace(data, "}\\s*}(?!(\\s*($)|\"))", "}]");

            var memoryStream = new MemoryStream();
            memoryStream.Write(Encoding.UTF8.GetBytes(data), 0, data.Length);
            memoryStream.Position = 0;
            var serializer = new DataContractJsonSerializer(typeof (Student));
            return (Student)serializer.ReadObject(memoryStream);
        }

        [DataMember(Name = "student_code")]
        public uint StudentCode { get; set; }
        [DataMember(Name = "first_name")]
        public string FirstNames { get; set; }
        [DataMember(Name = "last_name")]
        public string LastName { get; set; }
        [DataMember(Name = "attend_type")]
        public string AttendType { get; set; }
        [DataMember(Name = "disability")]
        public string Disability { get; set; }
        [DataMember(Name = "course")]
        public string Course { get; set; }
        [DataMember(Name = "email_address")]
        public string EmailAddress { get; set; }
        [DataMember(Name = "course_type")]
        public string CourseType { get; set; }
        [DataMember(Name = "allocated")]
        public AllocatedStream[] AllocatedStreams { get; set; }
        [DataMember(Name = "student_enrolment")]
        public StudentEnrollment[] StudentEnrolments { get; set; }
        [DataMember(Name = "preferences")]
        public object Preferences { get; set; }
        [DataMember(Name = "section_preferences")]
        public object SectionPreferences { get; set; }
        [DataMember(Name = "external_activities")]
        public object ExternalActivities { get; set; }
    }

    [DataContract]
    public class AllocatedStream : SubjectStream
    {
        public override string ToString()
        {
            return SubjectCode + "|" + ActivityGroupCode + "|" + ActivityCode;
        }

        [DataMember(Name = "activity_size")]
        public int ActivitySize { get; set; }
        [DataMember(Name = "student_count")]
        public int StudentCount { get; set; }
        [DataMember(Name = "buffer")]
        public int Buffer { get; set; }
        [DataMember(Name = "capacity")]
        public int Capacity { get; set; }
        [DataMember(Name = "section_code")]
        public string SectionCode { get; set; }
        [DataMember(Name = "source")]
        public string Source { get; set; }
    }

    [DataContract]
    public class StudentEnrollment
    {
        [DataMember(Name = "subject_code")]
        public string SubjectCode { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "manager")]
        public string Manager { get; set; }
        [DataMember(Name = "email_address")]
        public string EmailAddress { get; set; }
        [DataMember(Name = "faculty")]
        public string Faculty { get; set; }
        [DataMember(Name = "semester")]
        public string Semester { get; set; }
        [DataMember(Name = "campus")]
        public string Campus { get; set; }
        [DataMember(Name = "showOnTT")]
        public string ShowOnTimetable { get; set; }
        [DataMember(Name = "display_subject_code")]
        public string DisplaySubjectCode { get; set; }
        [DataMember(Name = "groups")]
        public Group[] Groups { get; set; }
    }

    [DataContract]
    public class Group
    {
        [DataMember(Name = "subject_code")]
        public string SubjectCode { get; set; }
        [DataMember(Name = "activity_group_code")]
        public string ActivityGroupCode { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "num_flagged_timeslots")]
        public string NumFlaggedTimeslots { get; set; }
        [DataMember(Name = "status")]
        public string Status { get; set; }
        [DataMember(Name = "auto_single")]
        public string AutoSingle { get; set; }
        [DataMember(Name = "min_prefs")]
        public string MinPrefs { get; set; }
        [DataMember(Name = "allow_justification")]
        public string AllowJustification { get; set; }
        [DataMember(Name = "allow_waitlist")]
        public string AllowWaitlist { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "show_availability")]
        public string ShowAvailability { get; set; }
        [DataMember(Name = "act_cnt")]
        public int ActCnt { get; set; }
    }
}

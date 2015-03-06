using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury.JsonObjects;
using UniTimetable.Model.Timetable;
using Stream = System.IO.Stream;
using Type = UniTimetable.Model.Timetable.Type;

namespace UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury
{
    public class CanterburyImporter : Canterbury, IImporter
    {
        public bool ImportUnselectableStreams { get; set; }

        public Timetable.Timetable ImportTimetable()
        {
            try
            {
                if (!LoginHandle.LoggedIn)
                {
                    LoginHandle.Login();
                }

                var courseData = GetCourseData();
                var timetable = new Timetable.Timetable();

                foreach (var course in courseData)
                {
                    foreach (var subs in course.SubjectStreams)
                    {
                        ParseSubjectStream(ref timetable, subs.Value);
                    }
                }

                SetColors(timetable);

                return timetable;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region Parse Subject Stream
        private void ParseSubjectStream(ref Timetable.Timetable timetable, SubjectStream subs)
        {
            Console.WriteLine(subs.Selectable);
            Console.WriteLine(subs.Availability);
            if (!ImportUnselectableStreams && subs.Selectable != "available" && subs.Selectable != "clash")
            {
                return;
            }

            var endTime = subs.Date.AddMinutes(double.Parse(subs.Duration));
            int currentDay;
            switch (subs.DayOfWeek)
            {
                case "Sun":
                    currentDay = 0;
                    break;
                case "Mon":
                    currentDay = 1;
                    break;
                case "Tue":
                    currentDay = 2;
                    break;
                case "Wed":
                    currentDay = 3;
                    break;
                case "Thu":
                    currentDay = 4;
                    break;
                case "Fri":
                    currentDay = 5;
                    break;
                case "Sat":
                    currentDay = 6;
                    break;
                default:
                    return;
            }
            
            var session = new Session(currentDay, subs.Date.DayOfYear, subs.Date.Hour,
                subs.Date.Minute, endTime.Hour, endTime.Minute, subs.Location);

            Subject subject;
            if (timetable.SubjectList.Exists(element => element.Name == subs.SubjectCode))
            {
                subject = timetable.SubjectList.Find(element => element.Name == subs.SubjectCode);
            }
            else
            {
                subject = new Subject(subs.SubjectCode);
                timetable.SubjectList.Add(subject);
            }

            // Set the session type
            Type type;
            if (subject.Types.Exists(types => types.Code == subs.ActivityGroupCode))
            {
                type = subject.Types.Find(types => types.Code == subs.ActivityGroupCode);
            }
            else // The session type doesn't exist, create it.
            {
                type = new Type(subs.ActivityType, subs.ActivityGroupCode, subject);
                switch (subs.ActivityGroupCode)
                {
                    case "tes":
                        type.Required = false;
                        break;
                    default:
                        type.Required = true;
                        break;
                }
                timetable.TypeList.Add(type);
            }

            // Set the session
            var streamNumber = subs.ActivityCode;
            Timetable.Stream stream;
            if (type.Streams.Exists(x => x.Number == streamNumber))
            {
                stream = type.Streams.Find(x => x.Number == streamNumber);
            }
            else
            {
                stream = new Timetable.Stream(streamNumber);
                timetable.StreamList.Add(stream); // Add it to the stream list
            }

            // Link the subject and type
            if (!subject.Types.Contains(type))
            {
                subject.Types.Add(type);
                type.Subject = subject;
            }

            // Link the stream and type.
            if (!type.Streams.Contains(stream))
            {
                type.Streams.Add(stream);
                stream.Type = type;
            }

            // Link the stream and class together.
            // Add it to our list of classes.
            timetable.ClassList.Add(session);
            stream.Classes.Add(session);
            session.Stream = stream;
        }

        private static void SetColors(Timetable.Timetable timetable)
        {
            var scheme = ColorScheme.Schemes[0];
            for (var i = 0; i < timetable.SubjectList.Count; i++)
            {
                timetable.SubjectList[i].Color = ((ColorScheme)scheme).Colours[i % ((ColorScheme)scheme).Colours.Count];
            }
        }
        #endregion

        #region Get Course Information
        private List<EnrolledSubjectStreams> GetCourseData()
        {
            var subjectStreams = new List<EnrolledSubjectStreams>();
            foreach (var course in LoginHandle.Student.StudentEnrolments)
            {
                foreach (var group in course.Groups)
                {
                    var stream = GetCourse(group.SubjectCode, group.ActivityGroupCode);
                    if (!ImportUnselectableStreams)
                    {
                        for (var i = 0; i < stream.SubjectStreams.Count; i++)
                        {
                            if (stream.SubjectStreams[i].Value.Selectable != "full") continue;
                            stream.SubjectStreams.RemoveAt(i);
                            i--;
                        }
                    }
                    subjectStreams.Add(stream);
                }
            }
            foreach (var allocatedStream in LoginHandle.Student.AllocatedStreams)
            {
                var stream = GetCourse(allocatedStream.SubjectCode, allocatedStream.ActivityGroupCode);
                subjectStreams.Add(stream);
            }

            subjectStreams.AddRange(LoginHandle.Student.AllocatedStreams.Select(allocatedStream => GetCourse(allocatedStream.SubjectCode, allocatedStream.ActivityGroupCode)));
            return subjectStreams;
        }

        private EnrolledSubjectStreams GetCourse(string courseName, string activityName)
        {
            var webRequest =
                (HttpWebRequest)
                    WebRequest.Create("https://mytimetable.canterbury.ac.nz/aplus/rest/student/" + LoginHandle.Student.StudentCode +
                                      "/subject/" + courseName + "/group/" + activityName + "/activities/?ss=" +
                                      LoginHandle.LoginToken);
            webRequest.UserAgent = LoginHandle.UserAgent;
            webRequest.CookieContainer = LoginHandle.Cookies;
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            EnrolledSubjectStreams enrolledSubjectStreams;
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                var stream = webResponse.GetResponseStream();
                enrolledSubjectStreams = ParseCourseJson(ref stream);
            }
            return enrolledSubjectStreams;
        }

        private static EnrolledSubjectStreams ParseCourseJson(ref Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream", "Stream cannot be null.");
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            reader.Close();
            json = json.Replace(":{", ",\"Value\":{");
            json = json.Replace("},", "}},{\"Key\":");
            json += "]}";
            json = "{\"SubjectStreams\":[{\"Key\":" + json.Substring(1);
            json = json.Replace("\"Key\":\"success\":", "\"success\":");

            var streamN = new MemoryStream();
            streamN.Write(Encoding.ASCII.GetBytes(json), 0, json.Length);
            streamN.Position = 0;
            return (EnrolledSubjectStreams)CommonRequest.JsonParse(streamN, typeof(EnrolledSubjectStreams));
        }
        #endregion
    }
}

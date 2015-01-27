#region

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

#endregion

namespace UniTimetable.CanterburyData
{
    [DataContract]
    public class CanterburyData
    {
        [DataMember] public string msg;
        [DataMember] public bool success = true;

        [DataMember]
        public Subjectstream[] SubjectStreams { get; set; }

        public static CanterburyData GetCanterburyData(string courseName, string activityName, ref string studentCode,
            ref string uniqueToken, ref CookieContainer cookieContainer, ref string userAgent)
        {
#if DEBUG
            System.IO.Stream obj = null;
            return ParseJson(ref obj, courseName);
#else
            var webRequest =
                (HttpWebRequest)
                    WebRequest.Create("https://mytimetable.canterbury.ac.nz/aplus/rest/student/" + studentCode +
                                      "/subject/" + courseName + "/group/" + activityName + "/activities/?ss=" +
                                      uniqueToken);
            webRequest.UserAgent = userAgent;
            webRequest.CookieContainer = cookieContainer;
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            CanterburyData canterburyData;
            using (var webResponse = (HttpWebResponse) webRequest.GetResponse())
            {
                var stream = webResponse.GetResponseStream();
                canterburyData = ParseJson(ref stream);
                cookieContainer = webRequest.CookieContainer;
            }
            return canterburyData;
#endif
        }

        public static CanterburyData ParseJson(ref System.IO.Stream stream, string courseName = "")
        {
#if DEBUG
            stream = new FileStream(courseName + ".log", FileMode.Open);
#endif
            if (stream == null) throw new ArgumentNullException("stream", "Stream cannot be null.");
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            reader.Close();
            json = json.Replace(":{", ",\"Value\":{");
            json = json.Replace("},", "}},{\"Key\":");
            json += "]}";
            json = "{\"SubjectStreams\":[{\"Key\":" + json.Substring(1);
            json = json.Replace("\"Key\":\"success\":", "\"success\":");
            Debug.WriteLine("\n\n" + json + "\n\n");
            var ser = new DataContractJsonSerializer(typeof (CanterburyData));
            var streamN = new MemoryStream();
            streamN.Write(Encoding.ASCII.GetBytes(json), 0, json.Length);
            streamN.Position = 0;
            var jsonSs = (CanterburyData) ser.ReadObject(streamN);
            return jsonSs;
        }
    }

    [DataContract]
    public class Subjectstream
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public Value Value { get; set; }
    }

    [DataContract]
    public class Value
    {
        private string _start_date;
        private string _start_time;

        [DataMember]
        public string subject_code { get; set; }

        [DataMember]
        public string activity_group_code { get; set; }

        [DataMember]
        public string activity_code { get; set; }

        [DataMember]
        public string campus { get; set; }

        [DataMember]
        public string day_of_week { get; set; }

        [DataMember]
        public string start_time
        {
            get { return _start_time; }
            set
            {
                _start_time = value;
                if (!string.IsNullOrWhiteSpace(start_date))
                {
                    Date = DateTime.Parse(start_date + " " + value);
                }
            }
        }

        [DataMember]
        public string location { get; set; }

        [DataMember]
        public string staff { get; set; }

        [DataMember]
        public string duration { get; set; }

        [DataMember]
        public string selectable { get; set; }

        [DataMember]
        public int availability { get; set; }

        [DataMember]
        public string week_pattern { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string zone { get; set; }

        [DataMember]
        public string department { get; set; }

        [DataMember]
        public string semester { get; set; }

        [DataMember]
        public string activity_type { get; set; }

        [DataMember]
        public string message { get; set; }

        [DataMember]
        public string start_date
        {
            get { return _start_date; }
            set
            {
                _start_date = value;
                if (!string.IsNullOrWhiteSpace(start_time))
                {
                    Date = DateTime.Parse(value + " " + start_time);
                }
            }
        }

        [DataMember]
        public string color { get; set; }

        [DataMember]
        public string lat { get; set; }

        [DataMember]
        public string lng { get; set; }

        public DateTime Date { get; set; }
    }
}
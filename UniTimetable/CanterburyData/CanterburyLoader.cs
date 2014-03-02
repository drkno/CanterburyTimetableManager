using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;

namespace UniTimetable.CanterburyData
{
    class CanterburyLoader
    {
        public string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:27.0) Gecko/20100101 Firefox/27.0";
        private readonly string _username;
        private readonly string _password;
        private CookieContainer _cookieContainer;
        private string _loginToken;
        public string StudentCode;
        public readonly List<CanterburyData> CanterburyDatas = new List<CanterburyData>();
        private List<string> _coursesList;

        public CanterburyLoader(string username, string password)
        {
            _username = username;
            _password = password;
            _cookieContainer = new CookieContainer();
        }

        private void CanterburyLogin()
        {
            if (string.IsNullOrWhiteSpace(_username) || string.IsNullOrWhiteSpace(_password))
            {
                throw new AuthenticationException("Invalid username or password provided.");
            }

            var webRequest = (HttpWebRequest)WebRequest.Create("https://mytimetable.canterbury.ac.nz/aplus/rest/student/login");
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            webRequest.UserAgent = UserAgent;
            webRequest.Referer = "https://mytimetable.canterbury.ac.nz/aplus/student";
            var postData = "username=" + _username + "&password=" + _password;
            SetPostData(ref webRequest, postData);
            webRequest.CookieContainer = _cookieContainer;

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                var stream = webResponse.GetResponseStream();
                if (stream == null) throw new AuthenticationException("Login failed.");
                var response = LoginObject.Parse(stream);
                if (response == null || !response.success || string.IsNullOrWhiteSpace(response.token))
                {
                    throw new AuthenticationException("Login failed.");
                }
                _cookieContainer = webRequest.CookieContainer;
                Debug.Write("Login Success. token (ss) = \"" + response.token + "\"");
                _loginToken = response.token;
            }

            _coursesList = new List<string>();
            webRequest = (HttpWebRequest)WebRequest.Create("https://mytimetable.canterbury.ac.nz/aplus/student?ss=" + _loginToken);
            webRequest.UserAgent = UserAgent;
            webRequest.Referer = "https://mytimetable.canterbury.ac.nz/aplus/student";
            webRequest.CookieContainer = _cookieContainer;
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                var stream = webResponse.GetResponseStream();
                if (stream == null) throw new NullReferenceException("Initial data load failed.");
                var reader = new StreamReader(stream);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line.Contains("student_code") && string.IsNullOrWhiteSpace(StudentCode))
                    {
                        StudentCode = Regex.Match(line, "[0-9]+").Value;
                    }

                    if (!line.Contains("data={")) continue;
                    line = line.Substring(line.IndexOf("student_enrolment", StringComparison.Ordinal));
                    foreach (var match in Regex.Matches(line, "\"subject_code\":\"[a-zA-Z0-9\\(\\)\\-\\s]+\",\"activity_group_code\":\"[a-zA-Z0-9 -]+\""))
                    {
                        //"subject_code":"COSC261-14S1 (C)","activity_group_code":"Com"
                        var str = match.ToString();
                        str = str.Replace("\"subject_code\":\"", "");
                        str = str.Replace("\",\"activity_group_code\":\"", ",");
                        str = str.Replace("\"", "");
                        _coursesList.Add(str);
                    }
                }
                reader.Close();
                _cookieContainer = webRequest.CookieContainer;
            }
        }

        public void GetData()
        {
#if !DEBUG
            // Login and Get Courses
            if (string.IsNullOrWhiteSpace(_loginToken) || string.IsNullOrWhiteSpace(StudentCode))
            {
                CanterburyLogin();
            }
#endif

#if DEBUG
            var debugReader = new StreamReader("GetCourses.log");
            while (!debugReader.EndOfStream)
            {
                var line = debugReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.Contains("student_code") && string.IsNullOrWhiteSpace(StudentCode))
                {
                    StudentCode = Regex.Match(line, "[0-9]+").Value;
                }

                if (!line.Contains("data={")) continue;
                line = line.Substring(line.IndexOf("student_enrolment", StringComparison.Ordinal));
                foreach (var match in Regex.Matches(line, "\"subject_code\":\"[a-zA-Z0-9\\(\\)\\-\\s]+\",\"activity_group_code\":\"[a-zA-Z0-9 -]+\""))
                {
                    //"subject_code":"COSC261-14S1 (C)","activity_group_code":"Com"
                    var str = match.ToString();
                    str = str.Replace("\"subject_code\":\"", "");
                    str = str.Replace("\",\"activity_group_code\":\"", ",");
                    str = str.Replace("\"", "");
                    coursesList.Add(str);
                }
            }
            debugReader.Close();
            _loginToken = "DEBUG";
            StudentCode = "DEBUG";
#endif

            foreach (var course in _coursesList)
            {
                var split = course.Split(',');
                var canterburyData = CanterburyData.GetCanterburyData(split[0], split[1], ref StudentCode,
                    ref _loginToken, ref _cookieContainer, ref UserAgent);
                if (canterburyData != null)
                {
                    CanterburyDatas.Add(canterburyData);
                }
            }

            foreach (var course in CanterburyDatas)
            {
                foreach (var substr in course.SubjectStreams)
                {
                    Console.WriteLine(substr.Key + "\t: " + substr.Value.activity_group_code + ", " + substr.Value.start_date + " " + substr.Value.start_time + substr.Value.activity_code);
                }
            }
        }

        private static void SetPostData(ref HttpWebRequest a, string data)
        {
            a.Method = "POST";
            var st = a.GetRequestStream();
            var byteArray = Encoding.UTF8.GetBytes(data);
            st.Write(byteArray, 0, byteArray.Length);
            st.Close();
            a.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
        }

        [DataContract]
        public class LoginObject
        {
            public static LoginObject Parse(System.IO.Stream stream)
            {
                try
                {
                    if (stream.CanSeek)
                    {
                        stream.Position = 0;
                    }
                    var ser = new DataContractJsonSerializer(typeof(LoginObject));
                    var loginObj = (LoginObject)ser.ReadObject(stream);
                    return loginObj;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            [DataMember]
            public bool success { get; set; }
            [DataMember]
            public string token { get; set; }
        }

        public SetCourseResponse SetCourse(Subject subject, string code, int number)
        {
            try
            {
                // Login
                if (string.IsNullOrWhiteSpace(_loginToken) || string.IsNullOrWhiteSpace(StudentCode))
                {
                    CanterburyLogin();
                }

                // Create Set Course Request
                var webRequest = (HttpWebRequest)WebRequest.Create("https://mytimetable.canterbury.ac.nz/aplus/rest/student/changeActivity/?ss=" + _loginToken);
                webRequest.CookieContainer = _cookieContainer;
                webRequest.UserAgent = UserAgent;
                webRequest.AllowAutoRedirect = true;
                webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                var stream = webRequest.GetRequestStream();
                var postStr = "token=a" +
                    "&student_code=" + StudentCode +
                    "&subject_code=" + subject +
                    "&activity_group_code=" + code +
                    "&activity_code=" + number.ToString("00");
                stream.Write(Encoding.ASCII.GetBytes(postStr), 0, postStr.Length);
                stream.Close();

                SetCourseResponse courseResponse;
                using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    var reStream = webResponse.GetResponseStream();
                    if (reStream == null) return null;
                    var courseResponseSerializer = new DataContractJsonSerializer(typeof (SetCourseResponse));
                    courseResponse = (SetCourseResponse) courseResponseSerializer.ReadObject(reStream);
                }
                return courseResponse;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [DataContract]
        public class SetCourseResponse
        {
            [DataMember]
            public bool success { get; set; }
            [DataMember]
            public string msg { get; set; }
        }

    }
}

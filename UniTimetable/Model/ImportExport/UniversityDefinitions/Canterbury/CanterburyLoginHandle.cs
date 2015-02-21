using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using UniTimetable.Model.ImportExport.Login;
using UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury.JsonObjects;

namespace UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury
{
    public class CanterburyLoginHandle : ILoginHandle
    {
        private string _password;
        public CookieContainer Cookies { get; private set; }
        public string Username { get; private set; }
        public string LoginToken { get; private set; }
        public string StudentCode { get; private set; }
        public string[] Courses { get; private set; }
        public string UserAgent { get; set; }
        
        public CanterburyLoginHandle()
        {
            UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:27.0) Gecko/20100101 Firefox/27.0";
            Cookies = new CookieContainer();
        }

        public bool LoggedIn { get; private set; }

        public void Login()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                throw new FailedLoginException(Username, "Username cannot be blank.");
            }

            if (_password == null)
            {
                throw new FailedLoginException(Username, "Password cannot be null.");
            }

            var webRequest =
                (HttpWebRequest)WebRequest.Create("https://mytimetable.canterbury.ac.nz/aplus/rest/student/login");
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            webRequest.UserAgent = UserAgent;
            webRequest.Referer = "https://mytimetable.canterbury.ac.nz/aplus/student";
            var postData = "username=" + Username + "&password=" + _password;
            CommonRequest.SetPostData(ref webRequest, postData);
            webRequest.CookieContainer = Cookies;

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                var stream = webResponse.GetResponseStream();
                if (stream == null) throw new AuthenticationException("Login failed.");
                var response = (CommonRequest) CommonRequest.JsonParse(stream, typeof(CommonRequest));
                if (response == null || !response.ReturnedSuccess || string.IsNullOrWhiteSpace(response.ReturnedToken))
                {
                    LoggedIn = false;
                    var msg = (response != null) ? response.ReturnedMessage : String.Empty;
                    throw new FailedLoginException(Username, msg);
                }
                Cookies = webRequest.CookieContainer;
                LoginToken = response.ReturnedToken;
            }

            var coursesList = new List<string>();
            webRequest =
                (HttpWebRequest)
                    WebRequest.Create("https://mytimetable.canterbury.ac.nz/aplus/student?ss=" + LoginToken);
            webRequest.UserAgent = UserAgent;
            webRequest.Referer = "https://mytimetable.canterbury.ac.nz/aplus/student";
            webRequest.CookieContainer = Cookies;
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

                    coursesList.AddRange(from object course in Regex.Matches(line, "[A-Z]+[0-9]+-[0-9]{2}[^\\\"]+\",\"activity_group_code\":\"[^\\\"]+") select course.ToString());
                }
                reader.Close();
                Cookies = webRequest.CookieContainer;
            }
            Courses = coursesList.ToArray();
            LoggedIn = true;
        }

        public void Logout()
        {
            var webRequest =
                (HttpWebRequest)WebRequest.Create("https://mytimetable.canterbury.ac.nz/aplus/rest/student/logout");
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            webRequest.UserAgent = UserAgent;
            webRequest.Referer = "https://mytimetable.canterbury.ac.nz/aplus/student";
            var postData = "ss=" + LoginToken;
            CommonRequest.SetPostData(ref webRequest, postData);
            webRequest.CookieContainer = Cookies;
            using (webRequest.GetResponse()) {}
            Cookies = new CookieContainer();
            LoggedIn = false;
        }

        public string GetLoginPrompt()
        {
            return "Import Timetable";
        }

        public string GetLoginAction()
        {
            return "Login";
        }

        public string GetPrivacyPromise()
        {
            return "Your details will not be stored or used outside of this application session.";
        }

        public IEnumerable<LoginField> GetLoginFields()
        {
            return new[]
                   {
                       new LoginField(LoginFieldType.String, "Username"),
                       new LoginField(LoginFieldType.PasswordString, "Password") 
                   };
        }

        public void SetLoginFields(IEnumerable<LoginField> loginFields)
        {
            foreach (var loginField in loginFields)
            {
                switch (loginField.Name)
                {
                    case "Username":
                        Username = loginField.Value; break;
                    case "Password":
                        _password = loginField.Value; break;
                    default:
                        throw new InvalidLoginFieldException(loginField, "CanterburyLoginHandler");
                }
            }
        }
    }
}

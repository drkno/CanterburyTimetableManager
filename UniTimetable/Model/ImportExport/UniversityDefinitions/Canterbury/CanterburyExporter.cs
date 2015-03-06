using System;
using System.Linq;
using System.Net;
using UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury.JsonObjects;
using UniTimetable.Model.Timetable;

namespace UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury
{
    public class CanterburyExporter : Canterbury, IExporter
    {
        public bool Export(Timetable.Timetable timetable, Action<string, bool> modifyList)
        {
            var response = true;
            try
            {
                foreach (var stream in timetable.StreamList.Where(stream => stream.Selected))
                {
                    modifyList(
                        "Setting " + stream.Type.Subject + ": " + stream.Type.Code + " to stream " + stream.Number,
                        false);
                    var inrep = SetCourse(stream.Type.Subject, stream.Type.Code, stream.Number);

                    if (inrep != null)
                    {
                        response = response && inrep.ReturnedSuccess;
                    }

                    if (inrep == null)
                    {
                        modifyList(
                            "UNKO: Setting " + stream.Type.Subject + ": " + stream.Type.Code + " to stream " +
                            stream.Number, true);
                        modifyList("ERRO: No data was returned so the status is unknown", false);
                    }
                    else if (!inrep.ReturnedSuccess && !inrep.ReturnedMessage.EndsWith("is already in this activity."))
                    {
                        modifyList(
                            "FAIL: Setting " + stream.Type.Subject + ": " + stream.Type.Code + " to stream " +
                            stream.Number, true);
                        modifyList("ERRO: " + inrep.ReturnedMessage, false);
                    }
                    else
                    {
                        modifyList(
                            "DONE: Setting " + stream.Type.Subject + ": " + stream.Type.Code + " to stream " +
                            stream.Number, true);
                    }
                }
            }
            catch (Exception e)
            {
                modifyList("A critical error occurred: " + e.Message, false);
                response = false;
            }

            return response;
        }

        private CommonRequest SetCourse(Subject subject, string code, string streamNumber)
        {
            try
            {
                if (!LoginHandle.LoggedIn)
                {
                    LoginHandle.Login();
                }

                // Create Set Course Request
                var webRequest =
                    (HttpWebRequest)
                        WebRequest.Create(
                            "https://mytimetable.canterbury.ac.nz/aplus/rest/student/changeActivity/?ss=" + LoginHandle.LoginToken);
                webRequest.CookieContainer = LoginHandle.Cookies;
                webRequest.UserAgent = LoginHandle.UserAgent;
                webRequest.AllowAutoRedirect = true;
                webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
                
                var postStr = "token=a" +
                              "&student_code=" + LoginHandle.Student.StudentCode +
                              "&subject_code=" + subject +
                              "&activity_group_code=" + code +
                              "&activity_code=" + streamNumber;
                CommonRequest.SetPostData(ref webRequest, postStr);

                CommonRequest courseResponse;
                using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    var reStream = webResponse.GetResponseStream();
                    if (reStream == null) return null;
                    courseResponse = (CommonRequest)CommonRequest.JsonParse(reStream, typeof(CommonRequest));
                }
                return courseResponse;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

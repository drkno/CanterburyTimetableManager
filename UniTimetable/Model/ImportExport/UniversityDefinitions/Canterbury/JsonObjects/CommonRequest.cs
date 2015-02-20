using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury.JsonObjects
{
    [DataContract]
    public class CommonRequest
    {
        [DataMember(Name = "success")]
        public bool ReturnedSuccess { get; set; }

        [DataMember(Name = "token")]
        public string ReturnedToken { get; set; }

        [DataMember(Name = "msg")]
        public string ReturnedMessage { get; set; }

        public static object JsonParse(Stream stream, Type type)
        {
            try
            {
                if (stream.CanSeek)
                {
                    stream.Position = 0;
                }
                var ser = new DataContractJsonSerializer(type);
                return ser.ReadObject(stream);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetPostData(ref HttpWebRequest a, string data)
        {
            a.Method = "POST";
            var st = a.GetRequestStream();
            var byteArray = Encoding.UTF8.GetBytes(data);
            st.Write(byteArray, 0, byteArray.Length);
            st.Close();
            a.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
        }
    }
}

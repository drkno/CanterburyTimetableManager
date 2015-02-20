using System.Runtime.Serialization;

namespace UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury.JsonObjects
{
    [DataContract]
    public class EnrolledSubjectStreams : CommonRequest
    {
        [DataMember]
        public KeySubjectStreamPair[] SubjectStreams { get; set; }
    }
}

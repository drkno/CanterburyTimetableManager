using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury.JsonObjects
{
    [DataContract]
    public class EnrolledSubjectStreams : CommonRequest
    {
        [DataMember]
        public List<KeySubjectStreamPair> SubjectStreams { get; set; }
    }
}

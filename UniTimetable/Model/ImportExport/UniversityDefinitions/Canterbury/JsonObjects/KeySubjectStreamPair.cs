using System.Runtime.Serialization;

namespace UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury.JsonObjects
{
    [DataContract]
    public class KeySubjectStreamPair
    {
        private int _multipleAllocationIndex = -1;

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public SubjectStream Value { get; set; }

        public bool Allocated { get; set; }

        public int MultipleAllocationIndex
        {
            get { return _multipleAllocationIndex; }
            set { _multipleAllocationIndex = value; }
        }
    }
}

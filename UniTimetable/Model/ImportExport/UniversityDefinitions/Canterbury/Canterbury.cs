using System;
using System.Drawing;
using UniTimetable.Model.ImportExport.Login;

namespace UniTimetable.Model.ImportExport.UniversityDefinitions.Canterbury
{
    public class Canterbury : IUniversity, ILoginRequired
    {
        public Image UniversityLogo
        {
            get
            {
                var stream = Resources.Resources.GetEmbeddedResourceStream("Resources.UNOC.png");
                return Image.FromStream(stream);
            }
        }

        public string UniversityName { get { return "University of Canterbury"; } }
        public string CreatedBy { get { return "Matthew Knox"; } }
        public DateTime LastUpdated { get { return new DateTime(2015, 03, 06); } }

        protected CanterburyLoginHandle LoginHandle;
        public ILoginHandle CreateNewLoginHandle()
        {
            return new CanterburyLoginHandle();
        }

        public void SetLoginHandle(ref ILoginHandle handle)
        {
            if (handle.GetType() != typeof(CanterburyLoginHandle))
            {
                throw new InvalidLoginHandleException(true, "the wrong type of LoginHandle was provided");
            }
            LoginHandle = handle as CanterburyLoginHandle;
        }
    }
}

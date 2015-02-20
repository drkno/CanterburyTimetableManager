using System;
using System.Drawing;

namespace UniTimetable.Model.ImportExport
{
    public interface IUniversity
    {
        Image UniversityLogo { get; }
        string UniversityName { get; }
        string CreatedBy { get; }
        DateTime LastUpdated { get; }
    }
}

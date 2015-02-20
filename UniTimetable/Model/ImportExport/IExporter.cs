using System;

namespace UniTimetable.Model.ImportExport
{
    public interface IExporter : IUniversity
    {
        bool Export(Timetable.Timetable timetable, Action<string, bool> modifyList);
    }
}

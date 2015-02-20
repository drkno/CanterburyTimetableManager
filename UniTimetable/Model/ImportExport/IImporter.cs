namespace UniTimetable.Model.ImportExport
{
    public interface IImporter : IUniversity
    {
        Timetable.Timetable ImportTimetable();
    }
}

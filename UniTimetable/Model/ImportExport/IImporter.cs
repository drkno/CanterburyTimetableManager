namespace UniTimetable.Model.ImportExport
{
    public interface IImporter : IUniversity
    {
        bool ImportUnselectableStreams { get; set; }
        Timetable.Timetable ImportTimetable();
    }
}

namespace UniTimetable.Model.ImportExport.Login
{
    public interface ILoginRequired
    {
        ILoginHandle CreateNewLoginHandle();
        void SetLoginHandle(ref ILoginHandle handle);
    }
}
